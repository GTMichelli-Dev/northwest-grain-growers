using System;
using System.Data;
using System.Data.OleDb; // Ensure this namespace is included
using System.Windows.Forms;

namespace AccessEndicottt
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string databasePath = @"C:\Junk\IS Bulkweigh.mdb";
            string connectionString = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={databasePath};";


            //string connectionString = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={databasePath};";

            using var connection = new OleDbConnection(connectionString);
            try
            {
                connection.Open();

                DataTable schema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                foreach (DataRow row in schema.Rows)
                {
                    string tableName = row["TABLE_NAME"]?.ToString();
                    if (!string.IsNullOrEmpty(tableName))
                    {
                        comboBoxTables.Items.Add(tableName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void comboBoxTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string selectedTable = comboBoxTables.SelectedItem.ToString();
                string query = $"SELECT * FROM [{selectedTable}]";

                string databasePath = @"C:\Junk\IS Bulkweigh.mdb";
                string connectionString = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={databasePath};";



                using var connection = new OleDbConnection(connectionString);
                using var adapter = new OleDbDataAdapter(query, connection);

                DataTable table = new DataTable();
                adapter.Fill(table);

                dataGridView.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;

        }
    }
}
