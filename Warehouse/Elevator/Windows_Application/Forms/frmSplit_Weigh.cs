using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NWGrain

{
    public partial class frmSplit_Weigh : Form
    {
        public VirtualDataset.SplitWeightDataTable splitWeights = new VirtualDataset.SplitWeightDataTable();
        public bool inbound;
        public Guid load_UID;

        private int NumberOfWeighmentsNeeded=0;

        string ScaleDescription;
        
        public frmSplit_Weigh(string scaleDescription, Guid LoadUID )
        {
            InitializeComponent();
            inbound = (LoadUID == Guid.Empty );
            if (!inbound)
            {
                using (VirtualDatasetTableAdapters.Load_SplitsTableAdapter load_SplitsTableAdapter = new VirtualDatasetTableAdapters.Load_SplitsTableAdapter())
                {
                    using (VirtualDataset.Load_SplitsDataTable load_Splits = new VirtualDataset.Load_SplitsDataTable())
                    {
                        NumberOfWeighmentsNeeded = load_SplitsTableAdapter.FillByLoad_UID_Inbound(load_Splits, LoadUID, true);
                    }
                }
            }
            
            ScaleDescription = scaleDescription;
            lblInboundName.Text = ScaleDescription; 
            UpdateWeight();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }



        private void Split_Weigh_Load(object sender, EventArgs e)
        {
            this.weigh_ScalesTableAdapter1.Fill(this.nwDataset.Weigh_Scales, Settings.Location_Id);
           
            
        }


        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            UpdateWeight();
        }

        private void UpdateWeight()
        {
            Scales.ScaleData CurrentScaleData = Scales.GetScaleByDescription(ScaleDescription );
            if (CurrentScaleData != null )
            {
                {
                    
                    if (CurrentScaleData.ConnectionStatus != Scales.ScaleData.enumConnectionStatus.Off && CurrentScaleData.ConnectionStatus != Scales.ScaleData.enumConnectionStatus.Manual)
                    {
                        try
                        {
                            CurrentScaleData = Scales.GetScaleByDescription(ScaleDescription);
                            this.btnAdd.Visible = true;
                        }
                        catch
                        {
                            
                            CurrentScaleData.CurWeight = 0;
                            CurrentScaleData.Motion = false;
                            CurrentScaleData.CurrentStatus = Scales.ScaleData.enumStatus.Fail;
                        }
                    }
                }
            }
            SetScaleDisplay(CurrentScaleData);
        }


        private void SetScaleDisplay(Scales.ScaleData SelectedScaleData)
        {
            string StatusString = "Connecting";
            System.Drawing.Color BackColor = System.Drawing.Color.White;
            if (SelectedScaleData.CurrentStatus  == Scales.ScaleData.enumStatus.OK || SelectedScaleData.ConnectionStatus == Scales.ScaleData.enumConnectionStatus.Off)
            {
                if (SelectedScaleData.Motion)
                {
                    StatusString = "Motion";
                    BackColor = System.Drawing.Color.Yellow;
                }
                else
                {
                    StatusString = "";
                }
            }
            else
            {
                BackColor = System.Drawing.Color.Pink;
                StatusString = SelectedScaleData.CurrentStatus.ToString();
            }
            lblWeight.Text = string.Format("{0:N0} lbs", SelectedScaleData.CurWeight);
            lblStatus.Text = StatusString;
            lblWeight.BackColor = BackColor;
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            Scales.ScaleData CurrentScaleData = Scales.GetScaleByDescription(ScaleDescription);
            if (CurrentScaleData != null)
            {
                if (CurrentScaleData.CurrentStatus == Scales.ScaleData.enumStatus.OK || CurrentScaleData.ConnectionStatus == Scales.ScaleData.enumConnectionStatus.Off)
                {
                    if (CurrentScaleData.Motion)
                    {
                        Alert.Show("Motion. Wait For Scale To Settle Then Retry", "Motion On Scale");
                    }
                    else if (CurrentScaleData.CurWeight < 1000)
                    {
                        Alert.Show("Weight Must Be Above 1000", "Under Load");
                    }
                    else
                    {
                        foreach (VirtualDataset.SplitWeightRow row in virtualDataset.SplitWeight)
                        {
                            
                        }
                        this.virtualDataset.SplitWeight.AddSplitWeightRow("Weight #" + (this.virtualDataset.SplitWeight.Count + 1).ToString(), CurrentScaleData.CurWeight, "Delete",inbound,DateTime.Now );
                    }
                }
                else
                {
                    Alert.Show("Scale Error", "Error");
                }

                GetTotalWeight();
                CurrentScaleData.CurrentStatus = Scales.ScaleData.enumStatus.OK;
              
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Scales.ScaleData CurrentScaleData = Scales.GetScaleByDescription(ScaleDescription);
            if (CurrentScaleData != null)
            {
                if (CurrentScaleData.ConnectionStatus != Scales.ScaleData.enumConnectionStatus.Off)
                {
                    using (Manual_Weight frm = new Manual_Weight("Enter Weight", CurrentScaleData.CurWeight, false))
                    {
                        if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            CurrentScaleData.CurWeight = frm.Weight;
                            CurrentScaleData.Motion = false;
                            CurrentScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Off;
                            
                        }
                    }
                }
                else
                {
                    CurrentScaleData.CurrentStatus = Scales.ScaleData.enumStatus.OK;
                  
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = this.dataGridView1.CurrentRow.Index;
            VirtualDataset.SplitWeightRow SelectedRow;
            SelectedRow = (VirtualDataset.SplitWeightRow)(DataRow)((DataRowView)this.splitWeightBindingSource.Current).Row;
            if (e.ColumnIndex==this.btnDelete.Index)
            {
                SelectedRow.Delete();
            }
            GetTotalWeight();
        }


        public int GetTotalWeight()
        {
            int Total = 0;
           
            this.btnOk.Visible = virtualDataset.SplitWeight.Count>1;
            this.lblCombined.Text = string.Format("{0:N0} lbs", Total);
            this.btnAdd.Visible = this.virtualDataset.SplitWeight.Count < 3;
            return Total;

        }



        public string GetWeightComment()
        {
            string Comment = "";
            string Comma = "";
            int I = 0;
            foreach (VirtualDataset.SplitWeightRow row in virtualDataset.SplitWeight)
            {
                I += 1;
                if (virtualDataset.SplitWeight.Count > I)
                {
                    Comma = ",";
                }
                else
                {
                    Comma = "";
                }
                Comment += string.Format("{0:0}", row.Weight)+Comma;
            }

            return Comment;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            int NumberOfWeighment = this.virtualDataset.SplitWeight.Count;
            if (this.NumberOfWeighmentsNeeded != NumberOfWeighment && this.NumberOfWeighmentsNeeded>0)
            {
                Alert.Show(string.Format("This Truck Was Split Weighted Inbound {0} Times and You Split Weighed {1} Times. Please Fix This", NumberOfWeighmentsNeeded, NumberOfWeighment), "Error", false);
            }
            else
            {
                splitWeights = this.virtualDataset.SplitWeight; 


                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
