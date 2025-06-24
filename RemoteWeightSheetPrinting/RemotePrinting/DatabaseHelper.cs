using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Data.SqlClient;

namespace RemotePrinting
{
    internal class DatabaseHelper
    {

    
        private static string connectionString = @"Data Source=.\;Initial Catalog=NW_Data;Persist Security Info=True;User ID=sa;Password=Scale_Us3r;TrustServerCertificate=True";

        public static List<string> GetServerNames()
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




    }
}
