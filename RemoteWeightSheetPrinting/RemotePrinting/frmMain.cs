using RemotePrinting.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemotePrinting
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            ServerProcessor.LogMessage += ServerProcessor_LogMessage;

            ServerProcessor.ProcessServerCompleted += ServerProcessor_ProcessServerCompleted;
            ServerProcessor.ProcessAllServersCompleted += ServerProcessor_ProcessAllServersCompleted;
        }

        private void ServerProcessor_ProcessAllServersCompleted(List<WeightSheetResult> obj)
        {
            if (dgvResults.InvokeRequired)
            {
                dgvResults.Invoke(new Action(() =>
                {
                    grpDistrict.Enabled = true;
                    this.grpOptions.Enabled = true;
                    tsProgress.Text = $"Processing Complete Total # Weight Sheets:{obj.Count}";
                    dgvResults.Rows.Clear();
                    btnMarkAsOriginal.Enabled = false; 
                    foreach (var item in obj)
                    {
                        int rowIndex = dgvResults.Rows.Add(false, item.WeightSheetId, item.WSType, item.LocationId, item.LocationDescription, item.CreationDate, item.ServerName);
                        dgvResults.Rows[rowIndex].Tag = item.UID; // Set UID as the key
                    }
                    if (dgvResults.Rows.Count > 0)
                    {
                        int lastRow = dgvResults.Rows.Count - 1;
                        dgvResults.FirstDisplayedScrollingRowIndex = lastRow;
                        dgvResults.ClearSelection();
                        dgvResults.Rows[lastRow].Selected = true;
                    }
                }));
            }
            else
            {
                grpDistrict.Enabled = true;
                this.grpOptions.Enabled = true;
                tsProgress.Text = $"Processing Complete Total # Weight Sheets:{obj.Count}";
                dgvResults.Rows.Clear();
                btnMarkAsOriginal.Enabled = false;
                foreach (var item in obj)
                {
                    int rowIndex = dgvResults.Rows.Add(false, item.WeightSheetId,item.WSType, item.LocationId, item.LocationDescription, item.CreationDate, item.ServerName);
                    dgvResults.Rows[rowIndex].Tag = item.UID; // Set UID as the key
                }
                if (dgvResults.Rows.Count > 0)
                {
                    int lastRow = dgvResults.Rows.Count - 1;
                    dgvResults.FirstDisplayedScrollingRowIndex = lastRow;
                    dgvResults.ClearSelection();
                    dgvResults.Rows[lastRow].Selected = true;
                }
            }
        }

        private void ServerProcessor_ProcessServerCompleted(List<WeightSheetResult> obj)
        {

        }

        private void ServerProcessor_LogMessage(string message, Color color)
        {
            // Ensure thread-safe update of the RichTextBox
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.Invoke(new Action(() =>
                {
                    AppendColoredText(message + Environment.NewLine, color);
                }));
            }
            else
            {
                AppendColoredText(message + Environment.NewLine, color);
            }
        }


        private void AppendColoredText(string text, Color color)
        {
            int start = richTextBox1.TextLength;
            richTextBox1.AppendText(text);
            int end = richTextBox1.TextLength;

            richTextBox1.Select(start, end - start);
            richTextBox1.SelectionColor = color;
            richTextBox1.SelectionLength = 0;
            richTextBox1.SelectionColor = richTextBox1.ForeColor; // Reset color

            // Scroll to the last entry
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.ScrollToCaret();
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // Unsubscribe to prevent memory leaks
            ServerProcessor.LogMessage -= ServerProcessor_LogMessage;
            ServerProcessor.ProcessServerCompleted -= ServerProcessor_ProcessServerCompleted;
            ServerProcessor.ProcessAllServersCompleted -= ServerProcessor_ProcessAllServersCompleted;
            base.OnFormClosed(e);
        }

        public void ProcessDistrict(string district)
        {
            // Call the method to process all servers for the specified district
            ServerProcessor.ProcessAllServers(district);
        }

        private void serverButton_Click(object sender, EventArgs e)
        {
            
            dgvResults.Rows.Clear();
            this.grpOptions.Enabled = false; // Disable the group box to prevent further clicks
            var district = ((Button)sender).Text.Trim();
            grpDistrict.Enabled = false; // Disable the group box to prevent further clicks
            this.richTextBox1.Clear();
            tsProgress.Text = $"Processing {district} District ..."; // Update the status text

            ServerProcessor.ProcessAllServers(district);
        }

        private void btnSelectNone_Click(object sender, EventArgs e)
        {

            foreach (DataGridViewRow row in dgvResults.Rows)
            {
                row.Cells[0].Value = false; // Assuming the first column is a checkbox
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvResults.Rows)
            {
                row.Cells[0].Value = true; // Assuming the first column is a checkbox
            }
        }

        private void btnRemoveToday_Click(object sender, EventArgs e)
        {
            // Remove rows where the CreationDate is today
            DateTime today = DateTime.Today;
            for (int i = dgvResults.Rows.Count - 1; i >= 0; i--)
            {
                DataGridViewRow row = dgvResults.Rows[i];
                if (row.Cells[5].Value is DateTime creationDate && creationDate.Date == today)
                {
                    dgvResults.Rows.RemoveAt(i);
                }
            }
            var count = dgvResults.Rows.Count;
            tsProgress.Text = $"Removed today's entries from the list. Total Weight Sheet left:{count} ";
        }

        private bool HasSelectedRows()
        {
            foreach (DataGridViewRow row in dgvResults.Rows)
            {
                if (Convert.ToBoolean(row.Cells[0].Value)) // Assuming the first column is a checkbox
                {
                    return true;
                }
            }
            return false;
        }   

        private void btnPrintSelected_Click(object sender, EventArgs e)
        {
           
          
            if (!HasSelectedRows())
            {
                MessageBox.Show("Please select at least one row to print.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //open the print diologe and select a printer
            using (var printDialog = new PrintDialog())
            {
                printDialog.AllowSomePages = false;
                printDialog.ShowHelp = false;
                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPrinter = printDialog.PrinterSettings.PrinterName;
                    tsProgress.Text = $"Printing Weight Sheets to printer: {selectedPrinter}";
                    Application.DoEvents();
                    foreach (DataGridViewRow row in dgvResults.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells[0].Value)) // Assuming the first column is a checkbox
                        {
                            var weightSheetId = Convert.ToInt64(row.Cells[1].Value);
                            var serverName = row.Cells[6].Value.ToString();
                            var transfer = (row.Cells[2].Value.ToString()== "Transfer");
                            var uid = row.Tag is Guid ? (Guid)row.Tag : Guid.Empty; // Get UID from Tag
                            
                            if (transfer)
                            {
                                //Update the tsProgress text with the selected weight sheet id
                            
                                // Call the method to print transfer weight sheet
                                Printing.PrintTransferWeightSheet(uid , selectedPrinter,serverName);
                            }
                            else
                            {
                                // Call the method to print intake weight sheet
                            
                                Printing.PrintWeightSheet(uid, selectedPrinter, serverName);
                            }
                            // Use selectedPrinter as needed for printing
                        }
                    }
                    tsProgress.Text = $"All Selected Weight Sheets Printed To: {selectedPrinter}";
                    btnMarkAsOriginal.Enabled = true;

                }
            }
        }

        private void btnMarkAsOriginal_Click(object sender, EventArgs e)
        {
            if (!HasSelectedRows())
            {
                MessageBox.Show("Please select at least one row to mark as original.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.grpOptions.Enabled = false; // Disable the group box to prevent further clicks
            this.richTextBox1.Clear();
            this.grpDistrict.Enabled = false; // Disable the group box to prevent further clicks
            foreach (DataGridViewRow row in dgvResults.Rows)
            {
                if (Convert.ToBoolean(row.Cells[0].Value)) {
                    var weightSheetId = Convert.ToInt64(row.Cells[1].Value);
                    var serverName = row.Cells[6].Value.ToString();
                    var transfer = (row.Cells[2].Value.ToString() == "Transfer");
                    var uid = row.Tag is Guid ? (Guid)row.Tag : Guid.Empty; // Get UID from Tag

                    ServerProcessor.MarkOriginalPrintedForWeightSheet(uid,serverName,weightSheetId); 

                }
            }
            tsProgress.Text = $"All Selected Weight Sheets marked as original";
            this.grpOptions.Enabled = true; 
           
            this.grpDistrict.Enabled = true;
        }
    }
 
 }
