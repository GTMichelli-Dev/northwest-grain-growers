
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
    public class ItemMasterApiController : ControllerBase
    {
        private readonly dbContext _ctx;

        private readonly ILog _log = new Services.Log();
        private readonly IConfiguration _config;

        public ItemMasterApiController(dbContext ctx ,  ILog log, IConfiguration config)
        {
            _ctx = ctx;
           
            _log = log;
            _config = config;
        }



        [HttpGet("UpsertItemMasterFromAgvantage")]
        public async Task<IActionResult> UpsertItemMasterFromAgvantage()
        {
           List<LogDTO> logDTOs = new List<LogDTO>();   
           await SyncItemMastersFromAgvantage(logDTOs);
           return Ok(logDTOs);
        }


        private async Task<IActionResult> SyncItemMastersFromAgvantage(List<LogDTO> logDTOs)
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
                SELECT  IMPROD,
                        IMDESC,
                        IMFLCD,
                        IMDEPT,
                        CASE WHEN IMDEL='I' THEN 0 ELSE 1 END AS ACTIVE,
                        IMSAGL,
                        IMIVGL,
                        IMPVGL
                FROM    {companyDataFile}.U4ITMMR
                ";

                var itemMasterList = new List<AgvantageItemMaster>();

                using (var conn = new OdbcConnection(connectionString))
                using (var cmd = new OdbcCommand(sql, conn))
                {
                    await conn.OpenAsync();
                    using (var rdr = await cmd.ExecuteReaderAsync())
                    {
                        while (await rdr.ReadAsync())
                        {
                            var newRow = new AgvantageItemMaster
                            {
                               ItemNumber = rdr.GetString(0).ToUpper().Trim(),
                                Description = rdr.GetString(1).ToUpper().Trim(),
                                FineLineCode = rdr.GetInt32(2),
                                DeptNumber = rdr.GetInt32(3),
                                Active = rdr.GetBoolean(4),
                                SalesGlnumber = rdr.GetString(5).ToUpper().Trim(),
                                InventoryGlnumber = rdr.GetString(6).ToUpper().Trim(),
                                PurchaseGlnumber = rdr.GetString(7).ToUpper().Trim()
                            };


                            itemMasterList.Add(newRow);
                        }
                    }
                }





                _ctx.AgvantageItemMasters.RemoveRange(_ctx.AgvantageItemMasters);
                await _ctx.SaveChangesAsync();
                _ctx.AgvantageItemMasters.AddRange(itemMasterList);
                await _ctx.SaveChangesAsync();


                await _log.LogInfo(logDTOs,  $"{companyDataFile} Item Master File Syncing completed successfully with a total of {itemMasterList.Count}.", "Item Master File Syncing");



            }
            catch (OdbcException ex)
            {
                await _log.LogError(logDTOs, ex, "OdbcException in Syncing Item Master File From Agvantage:");


            }
            catch (Exception ex)
            {
                await _log.LogError(logDTOs, ex, "Exception in Syncing Item Master File From Agvantage:");
                return new ObjectResult($"An error occurred: {ex.Message}") { StatusCode = 500 };
            }
            return new OkObjectResult(logDTOs);
        }



      
    }
}
