
using AgvantageAPI.DTO;
using AgvantageAPI.Models;
using AgvantageAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using System.Numerics;


namespace AgvantageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SplitsApiController : ControllerBase
    {
        private readonly dbContext _ctx;

        private readonly ILog _log = new Services.Log();
        private readonly IConfiguration _config;

        public SplitsApiController(dbContext ctx, ILog log, IConfiguration config)
        {
            _ctx = ctx;

            _log = log;
            _config = config;
        }



        [HttpGet("SyncAccountSplitsFromAgvantage")]
        public async Task<IActionResult> SyncAccountSplitsFromAgvantage()
        {
            List<LogDTO> logDTOs = new List<LogDTO>();
            await SyncAccountSplitsGroupNamesFromAgvantage(logDTOs);
            return Ok(logDTOs);
        }




        private async Task<IActionResult> SyncAccountSplitsGroupNamesFromAgvantage(List<LogDTO> logDTOs)
        {
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
                        SPSPGP AS GroupNumber,
                        MAX(CASE WHEN SPCNO = 0 THEN SPCNM END) AS SplitGroupName,
                        MAX(CASE WHEN SPDEL = 'G' THEN SPCNO END) AS PrimaryAccountID
                     FROM {companyDataFile}.U5SPLTS
                     GROUP BY
                        SPSPGP
                    HAVING MAX(CASE WHEN SPDEL = 'G' THEN 1 ELSE 0 END) = 1
                    ORDER BY
                        SPSPGP";

                var splitGroups = new List<SplitGroupDTO>();

                using (var conn = new OdbcConnection(connectionString))
                using (var cmd = new OdbcCommand(sql, conn))
                {
                    await conn.OpenAsync();
                    using (var rdr = await cmd.ExecuteReaderAsync())
                    {
                        while (await rdr.ReadAsync())
                        {
                            var newRow = new SplitGroupDTO
                            {

                                SplitGroupNumber = rdr.GetInt64(0),
                                Description = rdr.GetString(1),
                                PrimaryAccountId = rdr.GetInt64(2)
                            };


                            splitGroups.Add(newRow);
                        }
                    }
                }






                await UpsertSplitGroupAsync(splitGroups, logDTOs);




            }
            catch (OdbcException ex)
            {
                await _log.LogError(logDTOs, ex, "OdbcException in Syncing Accounts From Agvantage:");


            }
            catch (Exception ex)
            {
                await _log.LogError(logDTOs, ex, "Exception in Syncing Split Groups From Agvantage:");
                return new ObjectResult($"An error occurred: {ex.Message}") { StatusCode = 500 };
            }
            return new OkObjectResult(logDTOs);
        }



        private async Task UpsertSplitGroupAsync(IEnumerable<SplitGroupDTO> splitGroupList, List<LogDTO> logDTOs)
        {
            try
            {
                // Load all current split groups
                var dbSplitGroups = await _ctx.SplitGroups.ToListAsync();
                // Match existing rows by SplitGroupNumber (not Id)
                var dbSplitGroupsByNumber = dbSplitGroups.ToDictionary(sg => sg.SplitGroupNumber);

                // Load all valid account IDs
                var existingAccountIds = await _ctx.Accounts
                    .Select(a => a.Id)
                    .ToListAsync();

                var accountIdSet = new HashSet<long>(existingAccountIds);

                // Track incoming group numbers for deactivation logic
                var incomingGroupNumbers = new HashSet<long>(splitGroupList.Select(sg => sg.SplitGroupNumber));

                int updated = 0;
                int added = 0;
                int deactivated = 0;
                int skipped = 0;

                foreach (var incoming in splitGroupList)
                {
                    // Ensure the PrimaryAccountId exists in Accounts before adding/updating
                    if (!accountIdSet.Contains(incoming.PrimaryAccountId))
                    {
                        skipped++;
                        await _log.LogWarning(
                            logDTOs,
                            $"SplitGroup {incoming.SplitGroupNumber} not added or updated because Primary Account Id {incoming.PrimaryAccountId} does not exist in Accounts.",
                            "SplitGroups"
                        );
                        continue;
                    }

                    if (dbSplitGroupsByNumber.TryGetValue(incoming.SplitGroupNumber, out var dbrow))
                    {
                        // Update existing split group
                        updated++;
                        dbrow.Description = incoming.Description;
                        dbrow.PrimaryAccountId = incoming.PrimaryAccountId;
                        dbrow.Active = true;
                    }
                    else
                    {
                        // Add new split group
                        added++;
                        _ctx.SplitGroups.Add(new SplitGroup
                        {
                            SplitGroupNumber = incoming.SplitGroupNumber,
                            Description = incoming.Description,
                            PrimaryAccountId = incoming.PrimaryAccountId,
                            Active = true,
                            Notes = string.Empty,
                            UseForReceive = true,
                            UseForSales = true
                        });
                    }
                }

                // Deactivate any split groups that are no longer present in the incoming list
                foreach (var dbrow in dbSplitGroups)
                {
                    if (!incomingGroupNumbers.Contains(dbrow.SplitGroupNumber) && dbrow.Active)
                    {
                        deactivated++;
                        dbrow.Active = false;
                    }
                }

                await _ctx.SaveChangesAsync();

                await _log.LogInfo(
                    logDTOs,
                    $"SplitGroups UpsertAsync completed: {added} added, {updated} updated, {deactivated} deactivated, {skipped} skipped (missing account).",
                    "SplitGroups"
                );
                await SyncAccountSplitsGroupPercentsFromAgvantage(logDTOs);
            }
            catch (Exception ex)
            {
                await _log.LogError(logDTOs, ex, "An error occurred in SplitGroups UpsertAsync");
            }
        }

        private async Task<IActionResult> SyncAccountSplitsGroupPercentsFromAgvantage(List<LogDTO> logDTOs)
        {
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
                 SELECT SPCNO, SPSPGP,SPSPPC  FROM {companyDataFile}.U5SPLTS  where SPCNO>0 order by SPSPGP";

                var splitPercents = new List<SplitGroupPercentDTO>();

                using (var conn = new OdbcConnection(connectionString))
                using (var cmd = new OdbcCommand(sql, conn))
                {
                    await conn.OpenAsync();
                    using (var rdr = await cmd.ExecuteReaderAsync())
                    {
                        while (await rdr.ReadAsync())
                        {
                            var newRow = new SplitGroupPercentDTO
                            {

                                AccountId = rdr.GetInt64(0),
                                SplitGroupId = rdr.GetInt32(1),
                                SplitPercent = rdr.GetDecimal(2)
                            };


                            splitPercents.Add(newRow);
                        }
                    }
                }






                await UpsertSplitPercentsAsync(splitPercents, logDTOs);




            }
            catch (OdbcException ex)
            {
                await _log.LogError(logDTOs, ex, "OdbcException in Syncing Accounts From Agvantage:");


            }
            catch (Exception ex)
            {
                await _log.LogError(logDTOs, ex, "Exception in Syncing Split Groups From Agvantage:");
                return new ObjectResult($"An error occurred: {ex.Message}") { StatusCode = 500 };
            }
            return new OkObjectResult(logDTOs);
        }




        private async Task UpsertSplitPercentsAsync(IEnumerable<SplitGroupPercentDTO> splitPercents, List<LogDTO> logDTOs)
        {
            try
            {
                // Load valid account IDs
                var existingAccountIds = await _ctx.Accounts
                    .Select(a => a.Id)
                    .ToListAsync();
                var accountIdSet = new HashSet<long>(existingAccountIds);

                // Load split groups keyed by SplitGroupNumber (Agvantage SPSPGP)
                var splitGroupsByNumber = await _ctx.SplitGroups
                    .ToDictionaryAsync(sg => sg.SplitGroupNumber); // key: GroupNumber (long) :contentReference[oaicite:2]{index=2}

                // Load existing SplitGroupPercents, keyed by (SplitGroupId, AccountId)
                var dbPercents = await _ctx.SplitGroupPercents.ToListAsync();
                var dbPercentByGroupAndAccount = dbPercents.ToDictionary(
                    p => (p.SplitGroupId, p.AccountId)
                );

                int added = 0;
                int updated = 0;
                int skippedMissingAccount = 0;
                int skippedMissingGroup = 0;

                foreach (var incoming in splitPercents)
                {
                    // Make sure the account exists
                    if (!accountIdSet.Contains(incoming.AccountId))
                    {
                        skippedMissingAccount++;
                        await _log.LogWarning(
                            logDTOs,
                            $"Split percent not added or updated for SplitGroup {incoming.SplitGroupId} (group number) and Account {incoming.AccountId} because the Account does not exist in Accounts.",
                            "Split Group Percents"
                        );
                        continue;
                    }

                    // Map incoming SplitGroupId (actually the GroupNumber from Agvantage) to the SplitGroup entity
                    if (!splitGroupsByNumber.TryGetValue(incoming.SplitGroupId, out var splitGroup))
                    {
                        skippedMissingGroup++;
                        await _log.LogWarning(
                            logDTOs,
                            $"Split Group: {incoming.SplitGroupId} Could not be added Because the primary Grower is not Defined In Agvantage U5SPLTS.SPDEL ",
                            "Split Groups"
                        );
                        continue;
                    }

                    var key = (splitGroup.Id, incoming.AccountId);

                    if (dbPercentByGroupAndAccount.TryGetValue(key, out var dbRow))
                    {
                        // Update existing row
                        updated++;
                        dbRow.SplitPercent = incoming.SplitPercent;
                    }
                    else
                    {
                        // Insert new row
                        added++;
                        var entity = new SplitGroupPercent
                        {
                            SplitGroupId = splitGroup.Id,   // FK to SplitGroup.Id
                            AccountId = incoming.AccountId,
                            SplitPercent = incoming.SplitPercent
                        };

                        _ctx.SplitGroupPercents.Add(entity);
                        dbPercentByGroupAndAccount[key] = entity;
                    }
                }

                await _ctx.SaveChangesAsync();

                await _log.LogInfo(
                    logDTOs,
                    $"SplitGroupPercents UpsertAsync completed: {added} added, {updated} updated, " +
                    $"{skippedMissingAccount} skipped (missing account), {skippedMissingGroup} skipped (missing split group).",
                    "SplitGroupPercents"
                );
            }
            catch (Exception ex)
            {
                await _log.LogError(logDTOs, ex, "An error occurred in SplitGroupPercents UpsertAsync");
            }
        }

    }
}
