using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace RemotePrinting
{
    public class ServerProcessor
    {
        private static string baseConnectionString = "Data Source={0};Initial Catalog=NW_Data;Persist Security Info=True;User ID=sa;Password=Scale_Us3r;TrustServerCertificate=True;Connection Timeout=15";

        public static event Action<string, System.Drawing.Color> LogMessage;

        public static event Action<List<WeightSheetResult>> ProcessServerCompleted;
        public static event Action<List<WeightSheetResult>> ProcessAllServersCompleted;


        

        public static List<string> GetServerNames()
        {
            OnLogMessage("Retrieving server names from database...", System.Drawing.Color.Black);
            string connectionString = @"Data Source=WALDB001\;Initial Catalog=NW_Data;Persist Security Info=True;User ID=sa;Password=Scale_Us3r;TrustServerCertificate=True";
            try
            {
                var serverNames = new List<string>();
                string query = "SELECT DISTINCT Server_Name FROM dbo.Site_Setup";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                                serverNames.Add(reader.GetString(0));
                        }
                    }
                }

                return serverNames;
            }
            catch (Exception ex)
            {
                OnLogMessage($"Error retrieving server names: {ex.Message}", System.Drawing.Color.Red);
                return new List<string>();
            }
        }

        private static void OnLogMessage(string message, System.Drawing.Color color)
        {
            LogMessage?.Invoke(message, color);
        }

        public static void ProcessAllServers(string district)
        {
            
            var serverNames = GetServerNames();
            if (serverNames == null || serverNames.Count == 0)
            {
                OnLogMessage("No server names provided to process.", System.Drawing.Color.Red);
                return;
            }

            int total = serverNames.Count;
            int completed = 0;
            object lockObj = new object();
            var allResults = new List<WeightSheetResult>();

            foreach (var serverName in serverNames)
            {
                var bw = new BackgroundWorker();
                OnLogMessage($"Getting Weight Sheets From: {serverName}", System.Drawing.Color.Black);
                bw.DoWork += (s, e) =>
                {
                    e.Result = ProcessServerInternal(serverName,district);
                };
                bw.RunWorkerCompleted += (s, e) =>
                {
                    List<WeightSheetResult> results = e.Result as List<WeightSheetResult>;
                    var count = results?.Count ?? 0;
                    if (e.Error != null)
                    {
                        OnLogMessage($"Server: {serverName},Error {e.Error.Message}", System.Drawing.Color.Red);
                        Console.WriteLine($"Server: {serverName},Error {e.Error.Message}");
                    }
                    else
                    {
                        lock (allResults)
                        {
                            if (results != null)
                                allResults.AddRange(results);
                        }
                        if (count > 0)
                        {
                            OnLogMessage($"Server: {serverName}, Weight Sheets: {count}", System.Drawing.Color.Green);
                        }
                        else
                        {
                            OnLogMessage($"Server: {serverName}, No Weight Sheets", System.Drawing.Color.Black);
                        }
                        Console.WriteLine($"Server: {serverName}, Weight Sheets: {results?.Count ?? 0}");
                    }

                    lock (lockObj)
                    {
                        completed++;
                        if (completed == total)
                        {
                            OnLogMessage("All servers have been processed.", System.Drawing.Color.Green);
                            ProcessAllServersCompleted?.Invoke(new List<WeightSheetResult>(allResults));
                        }
                    }
                };
                bw.RunWorkerAsync();
            }
        }

        public static void MarkOriginalPrintedForWeightSheet(Guid uid, string serverName, long wsId)
        {
            var bw = new System.ComponentModel.BackgroundWorker();
            bw.DoWork += (s, e) =>
            {
                OnLogMessage($"Marking Weight Sheet {wsId} as Original Printed on server {serverName}", System.Drawing.Color.Black);
                string connStr = string.Format(baseConnectionString, serverName);
                string query = "UPDATE Weight_Sheets SET Original_Printed = 1 WHERE UID = @UID";
                try
                {
                    using (var conn = new System.Data.SqlClient.SqlConnection(connStr))
                    using (var cmd = new System.Data.SqlClient.SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UID", uid);
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            OnLogMessage($"Weight Sheet {wsId} marked as Original Printed on server {serverName}", System.Drawing.Color.Green);
                        }
                        else
                        {
                            OnLogMessage($"No Weight Sheet found with UID {wsId} on server {serverName}", System.Drawing.Color.Yellow);
                        }
                    }
                }
                catch (Exception ex)
                {
                    OnLogMessage($"Error marking Weight Sheet {wsId} as Original Printed: {ex.Message}", System.Drawing.Color.Red);
                }
            };
            bw.RunWorkerAsync();
        }


        // Helper method to allow returning results from DoWork
        private static List<WeightSheetResult> ProcessServerInternal(string serverName,string district)
        {
            string connStr = string.Format(baseConnectionString, serverName);
            var results = new List<WeightSheetResult>();

            string query = @"
                SELECT Weight_Sheets.UID, Weight_Sheets.WS_Id AS WeightSheetId, Weight_Sheets.Location_Id AS LocationId, Locations.Description AS LocationDescription,
                Weight_Sheets.Creation_Date AS CreationDate, Weight_Sheets.Server_Name, 
                Locations.District,case when Weight_Inbound_Loads.UID is null then convert(bit,1) else convert(bit,0) end as Transfer  
                FROM   Weight_Sheets INNER JOIN
                         Locations ON Weight_Sheets.Location_Id = Locations.Id LEFT OUTER JOIN
                         Weight_Inbound_Loads ON Weight_Sheets.UID = Weight_Inbound_Loads.Weight_Sheet_UID
                WHERE (Weight_Sheets.Original_Printed = 0) AND (Weight_Sheets.Closed = 1) AND (Weight_Sheets.Server_Name = @Server_Name) AND (Locations.District = @District)        ";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@District", district);
                    cmd.Parameters.AddWithValue("@Server_Name", serverName);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new WeightSheetResult
                            {
                                UID = reader.GetGuid(0),
                                WeightSheetId = reader.GetInt64(1),
                                ServerName = serverName,
                                LocationId = reader.GetInt32(2),
                                LocationDescription = reader.GetString(3),
                                CreationDate = reader.GetDateTime(4),
                                Transfer = reader.GetBoolean(7),
                                WSType= reader.GetBoolean(7) ? "Transfer" : "Intake"

                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OnLogMessage($"Server: {serverName},Error {ex.Message}", System.Drawing.Color.Red);
            }

            return results;
        }

        private static void OnProcessServerCompleted(List<WeightSheetResult> results)
        {
            // This method can be used to handle the results or trigger further actions
            Console.WriteLine($"Processing completed. Rows: {results.Count}");
            ProcessServerCompleted?.Invoke(results);
        }
    }

}
