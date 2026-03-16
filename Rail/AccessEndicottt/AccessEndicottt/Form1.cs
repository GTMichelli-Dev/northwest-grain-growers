using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;

namespace AccessEndicottt
{
    public partial class Form1 : Form
    {
        private const string LastPathFileName = "lastpath.txt";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(LastPathFileName))
            {
                textBoxPath.Text = File.ReadAllText(LastPathFileName);
            }

            LoadTables();
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Access Database Files (*.mdb)|*.mdb|All Files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxPath.Text = openFileDialog.FileName;
                File.WriteAllText(LastPathFileName, openFileDialog.FileName);
                LoadTables();
            }
        }

        private void LoadTables()
        {
            string databasePath = textBoxPath.Text;
            if (string.IsNullOrEmpty(databasePath) || !File.Exists(databasePath))
            {
                MessageBox.Show("Please select a valid database file.");
                return;
            }

            string connectionString = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={databasePath};";

            using var connection = new OleDbConnection(connectionString);
            try
            {
                connection.Open();

                DataTable schema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                comboBoxTables.Items.Clear();
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

                string databasePath = textBoxPath.Text;
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

        private void textBoxPath_TextChanged(object sender, EventArgs e)
        {


        }

        private void textBoxPath_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            File.WriteAllText(LastPathFileName, textBoxPath.Text);
            LoadTables();
        }
    }
}
