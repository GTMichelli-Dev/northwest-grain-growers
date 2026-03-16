using System.Diagnostics;
using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string databasePath = $"c:\\Junk\\Bulkweigh.mdb";
            string connectionString = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={databasePath};";

            using var connection = new OleDbConnection(connectionString);
            try
            {
                connection.Open();

                DataTable schema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                List<string > columns = new List<string>();               
                foreach (DataRow row in schema.Rows)
                {
                    string tableName = row["TABLE_NAME"]?.ToString();
                    if (!string.IsNullOrEmpty(tableName))
                    {
                        columns.Add(tableName);
                    }
                }
            }
            catch (Exception ex)
            {
              return BadRequest ("Error: " + ex.Message);
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
