
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using System.Numerics;
using AgvantageAPI.Models;
using AgvantageAPI.Services;
using AgvantageAPI.DTO;


namespace AgvantageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsApiController : ControllerBase
    {
        private readonly dbContext _ctx;

        private readonly ILog _log = new Services.Log();
        private readonly IConfiguration _config;

        public AccountsApiController(dbContext ctx ,  ILog log, IConfiguration config)
        {
            _ctx = ctx;
           
            _log = log;
            _config = config;
        }

    


        [HttpGet("GetAccountsFromAgvantage")]
        public async Task<IActionResult> GetAccountsFromAgvantage()
        {
           List<LogDTO> logDTOs = new List<LogDTO>();   
           await SyncAccountsFromAgvantage(logDTOs);
           return Ok(logDTOs);
        }


        private async Task<IActionResult> SyncAccountsFromAgvantage(List<LogDTO> logDTOs)
        {
            try
            {
                string? connectionString = _config.GetConnectionString("AgvantageConnectionString");
                if (string.IsNullOrEmpty(connectionString))
                {
                    await _log.LogError(logDTOs, new Exception("Connection string 'AgvantageConnectionString' is null or empty."), "Connection string 'AgvantageConnectionString' is null or empty.");
                    return new ObjectResult("Connection string is missing.") { StatusCode = 500 };
                }

                string sql = @"
                SELECT
                    CSCNO   AS AccountId,
                    CSCONM  AS EntityName,
                    CSLKNM  AS LookupName,
                    CSFRNM  AS OwnerFirstName,
                    CSLSNM  AS OwnerLastName,
                    CSAD1   AS Address1,
                    CSAD2   AS Address2,
                    CSCITY  AS City,
                    CSSTAT  AS State,
                    CSZIP   AS Zip,
                    CSMPHN  AS Phone1,
                    CSWPHN  AS Phone2,
                    CSHPHN  AS Phone3,
                    CSEADR  AS Email,
                    CSTEXD  AS TaxExemptDate,
                    CASE WHEN CSMBER  = 'P' THEN 1 ELSE 0 END AS IsProducer,
                    CASE WHEN CSACT   = 'I' THEN 0 ELSE 1 END AS Active,
                    CASE WHEN CSESTMT = 'Y' THEN 1 ELSE 0 END AS EmailStatement,
                    CASE WHEN CSPSTA  = 'Y' THEN 1 ELSE 0 END AS PrintStatement,
                    CASE WHEN CSTEXD  = 0  THEN 0 ELSE 1 END AS TaxExempt,
                    0 AS WHOLESALE,
                    1 AS CustomerPaysRoyalties,
                    0 AS AutoPrice,
                    '' AS Contact,
                    COALESCE(CSMEMO, '') ||
                    COALESCE(CSNOT1, '') ||
                    COALESCE(CSNOT2, '') AS Notes
                FROM COMDATA.U4CSTMR";

                var accountList = new List<Account>();

                using (var conn = new OdbcConnection(connectionString))
                using (var cmd = new OdbcCommand(sql, conn))
                {
                    await conn.OpenAsync();
                    using (var rdr = await cmd.ExecuteReaderAsync())
                    {
                        while (await rdr.ReadAsync())
                        {
                            var newRow = new Account
                            {
                                AccountId = rdr.GetInt64(0),
                                EntityName = rdr.GetString(1),
                                LookupName = rdr.GetString(2),
                                OwnerFirstName = rdr.GetString(3),
                                OwnerLastName = rdr.GetString(4),
                                Address1 = rdr.GetString(5),
                                Address2 = rdr.GetString(6),
                                City = rdr.GetString(7),
                                State = rdr.GetString(8),
                                Zip = rdr.GetString(9),
                                Phone1 = rdr.GetString(10),
                                Phone2 = rdr.GetString(11),
                                Phone3 = rdr.GetString(12),
                                Email = rdr.GetString(13),
                                TaxExemptDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-100)),
                                IsProducer = rdr.GetBoolean(15),
                                Active = rdr.GetBoolean(16),
                                EmailStatement = rdr.GetBoolean(17),
                                PrintStatement = rdr.GetBoolean(18),
                                TaxExempt = rdr.GetBoolean(19),
                                Wholesale = rdr.GetBoolean(20),
                                CustomerPaysRoyalties = rdr.GetBoolean(21),
                                AutoPrice = rdr.GetBoolean(22),
                                Contact = rdr.GetString(23),
                                Notes = rdr.GetString(24)
                            };

                            if (!rdr.IsDBNull(14))
                            {
                                var raw = rdr.GetDecimal(14);

                                if (raw != 0)
                                {
                                    var s = raw.ToString("00000000", CultureInfo.InvariantCulture);
                                    var dateOnly = DateOnly.ParseExact(s, "yyyyMMdd", CultureInfo.InvariantCulture);
                                    newRow.TaxExemptDate = dateOnly;
                                }
                            }
                            accountList.Add(newRow);
                        }
                    }
                }






                await UpsertAccountsAsync(accountList, logDTOs);




            }
            catch (OdbcException ex)
            {
                await _log.LogError(logDTOs, ex, "OdbcException in Syncing Accounts From Agvantage:");


            }
            catch (Exception ex)
            {
                await _log.LogError(logDTOs, ex, "Exception in Syncing Accounts From Agvantage:");
                return new ObjectResult($"An error occurred: {ex.Message}") { StatusCode = 500 };
            }
            return new OkObjectResult(logDTOs);
        }



        private async Task UpsertAccountsAsync(IEnumerable<Account> accountList, List<LogDTO> logDTOs)
        {
            try
            {
                // Load all current accounts from DB
                var dbAccounts = await _ctx.Accounts.ToListAsync();
                var dbAccountsById = dbAccounts.ToDictionary(a => a.AccountId);

                // Track incoming AccountIds
                var incomingIds = new HashSet<long>(accountList.Select(a => a.AccountId));
                var updated = 0;
                var added = 0;
                var deactivated = 0;
                // Update or add incoming accounts
                foreach (var incoming in accountList)
                {
                    if (dbAccountsById.TryGetValue(incoming.AccountId, out var dbAcc))
                    {
                        updated++;
                        // Update fields
                        dbAcc.EntityName = incoming.EntityName;
                        dbAcc.LookupName = incoming.LookupName;
                        dbAcc.OwnerFirstName = incoming.OwnerFirstName;
                        dbAcc.OwnerLastName = incoming.OwnerLastName;
                        dbAcc.Address1 = incoming.Address1;
                        dbAcc.Address2 = incoming.Address2;
                        dbAcc.City = incoming.City;
                        dbAcc.State = incoming.State;
                        dbAcc.Zip = incoming.Zip;
                        dbAcc.Phone1 = incoming.Phone1;
                        dbAcc.Phone2 = incoming.Phone2;
                        dbAcc.Phone3 = incoming.Phone3;
                        dbAcc.Email = incoming.Email;
                        dbAcc.TaxExempt = incoming.TaxExempt;
                        dbAcc.TaxExemptDate = incoming.TaxExemptDate;
                        dbAcc.IsProducer = incoming.IsProducer;
                        dbAcc.Active = incoming.Active;
                        dbAcc.EmailStatement = incoming.EmailStatement;
                        dbAcc.PrintStatement = incoming.PrintStatement;
                        // dbAcc.Wholesale = incoming.Wholesale;
                        // dbAcc.CustomerPaysRoyalties = incoming.CustomerPaysRoyalties;
                        //dbAcc.AutoPrice = incoming.AutoPrice;
                        //dbAcc.Contact = incoming.Contact;
                        dbAcc.Notes = incoming.Notes;
                        //dbAcc.HedgedAccount = incoming.HedgedAccount;
                        //dbAcc.IsHauler = incoming.IsHauler;
                    }
                    else
                    {
                        added++;
                        // Add new account
                        _ctx.Accounts.Add(incoming);
                    }
                }

                // Mark accounts as inactive if not in incoming list
                foreach (var dbAcc in dbAccounts)
                {
                    if (!incomingIds.Contains(dbAcc.AccountId) && dbAcc.Active)
                    {
                        deactivated++;
                        dbAcc.Active = false;
                    }
                }

                await _ctx.SaveChangesAsync();
                await _log.LogInfo(logDTOs, $"Accounts UpsertAsync completed: {added} added, {updated} updated, {deactivated} deactivated.", "Accounts");
            }
            catch (Exception ex)
            {
                await _log.LogError(logDTOs, ex, $"An error occurred in Accounts UpsertAsync");
            }
        }

    }
}
