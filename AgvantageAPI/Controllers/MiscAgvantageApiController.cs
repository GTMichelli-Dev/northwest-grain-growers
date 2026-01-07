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
    public class MiscAgvantageApiController : ControllerBase
    {
        private readonly dbContext _ctx;

        private readonly ILog _log = new Services.Log();
        private readonly IConfiguration _config;
        
        public MiscAgvantageApiController(dbContext ctx, ILog log, IConfiguration config)
        {
            _ctx = ctx;
            _log = log;
            _config = config;
        }

        [HttpGet("GetGLAccountsFromAgvantage")]
        public async Task<IActionResult> GetGLAccountsFromAgvantage()
        {
            List<LogDTO> logDTOs = new List<LogDTO>();
            try
            {
                string? companyDataFile = _config["CompanyDataFile"];
                string? connectionString = _config.GetConnectionString("AgvantageConnectionString");
                if (string.IsNullOrEmpty(connectionString))
                {
                    await _log.LogError(logDTOs, new Exception("Connection string 'AgvantageConnectionString' is null or empty."), "Connection string 'AgvantageConnectionString' is null or empty.");
                    return new ObjectResult("Connection string is missing.") { StatusCode = 500 };
                }
                if (string.IsNullOrEmpty(companyDataFile))
                {
                    await _log.LogError(logDTOs, new Exception("Company Data File Name is null or empty."), "Company Data File Name is null or empty.");
                    return new ObjectResult("Company Data File is missing.") { StatusCode = 500 };
                }
                string sql = $@"
                SELECT
                    case when GAACTV='A' then 1 else 0 end as Active,
                    GADVSN as LocationCode,
                    GAACCT as AccountNumber,
                    GATYPE as AccountType,
                    GATITL as Name
                FROM {companyDataFile}.GLSACCT
                where GAACTV='A'";

                var glAccountList = new List<Glaccount>();

                using (var conn = new OdbcConnection(connectionString))
                using (var cmd = new OdbcCommand(sql, conn))
                {
                    await conn.OpenAsync();
                    using (var rdr = await cmd.ExecuteReaderAsync())
                    {
                        while (await rdr.ReadAsync())
                        {
                            var newRow = new Glaccount
                            {
                                Active = rdr.GetBoolean(0),
                                LocationCode = rdr.GetInt32(1),
                                AccountNumber = rdr.GetString(2).ToUpper().Trim(),
                                AccountType = rdr.GetString(3).ToUpper().Trim(),
                                Name = rdr.GetString(4).ToUpper().Trim()
                            };
                            glAccountList.Add(newRow);
                        }
                    }
                }

                // Upsert logic
                var dbGlAccounts = await _ctx.Glaccounts.ToListAsync();
                var dbGlAccountsByKey = dbGlAccounts.ToDictionary(
                    a => $"{a.LocationCode}-{a.AccountNumber}");

                var incomingKeys = new HashSet<string>(
                    glAccountList.Select(a => $"{a.LocationCode}-{a.AccountNumber}"));

                int updated = 0, added = 0, deactivated = 0;

                foreach (var incoming in glAccountList)
                {
                    var key = $"{incoming.LocationCode}-{incoming.AccountNumber}";
                    if (dbGlAccountsByKey.TryGetValue(key, out var dbAcc))
                    {
                        updated++;
                        dbAcc.Active = incoming.Active;
                        dbAcc.AccountType = incoming.AccountType;
                        dbAcc.Name = incoming.Name;
                        // Add other fields as needed
                    }
                    else
                    {
                        added++;
                        _ctx.Glaccounts.Add(incoming);
                    }
                }

                foreach (var dbAcc in dbGlAccounts)
                {
                    var key = $"{dbAcc.LocationCode}-{dbAcc.AccountNumber}";
                    if (!incomingKeys.Contains(key) && dbAcc.Active)
                    {
                        deactivated++;
                        dbAcc.Active = false;
                    }
                }

                await _ctx.SaveChangesAsync();
                await _log.LogInfo(logDTOs, $"GLAccounts Upsert completed: {added} added, {updated} updated, {deactivated} deactivated.", "GLAccounts");
            }
            catch (OdbcException ex)
            {
                await _log.LogError(logDTOs, ex, "OdbcException in Syncing GLAccounts From Agvantage:");
            }
            catch (Exception ex)
            {
                await _log.LogError(logDTOs, ex, "Exception in Syncing GLAccounts From Agvantage:");
                //return new ObjectResult($"An error occurred: {ex.Message}") { StatusCode = 500 };
            }

            return Ok(logDTOs);
        }
    }
}
