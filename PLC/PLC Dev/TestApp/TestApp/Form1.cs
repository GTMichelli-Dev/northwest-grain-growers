using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WW_LGX_PLC;

namespace TestApp
{
    public partial class Form1 : Form
    {
        PLC plc = new PLC();
        public Form1()
        {
            InitializeComponent();
            lblGettingValues.Text = (plc.GettingValues) ? "Getting Values" : "Idle";
            grdtreatments.DataSource = plc.TreatmentList  ;
            grdVarieties.DataSource = plc.BinList;
            plc.StartScanning();
            timer1.Enabled = true;
            button1.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            plc.StartScanning();
            timer1.Enabled = true;
            button1.Visible = false;
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            plc.Dispose();
        }

        


        private void timer1_Tick(object sender, EventArgs e)
        {
            //btnSendBins.Visible=plc.
            lblGettingValues.Text = (plc.GettingValues) ? "Getting Values" : "Idle";
            lblLastBinUpdate.Text =$"Last Bin Update"+ plc.LastBinUpdate.ToString();
            Lastupdate.Text="Last Update "+ plc.LastUpdate.ToString();
           nextUpdate.Text="Update Time "+ plc.UpdateSeconds.ToString();
            lblCleanBatchStatus.Text =$"Clean Batch Status {plc.CleanBatchStatus.Value.ToString()}";
            lblTreatBatchStatus.Text = $"Treat Batch Status {plc.TreatBatchStatus.Value.ToString()}";
            lblCleanUnavailable.Text = (plc.CleanScaleUnavailable.Value ) ? "Clean Not Ready" : "Clean Ready";
            lblTreatUnavailable.Text = (plc.TreatScaleUnavailable.Value) ? "Treat Not Ready" : "Treat Ready";

            if (!plc.GettingValues )
            {
                grdtreatments.DataSource = plc.TreatmentList;
                grdVarieties.DataSource = plc.BinList;
                grdtreatments.Refresh();
                grdVarieties.Refresh();

            }

            try
            {
                using (WebService.WebService proxy = new WebService.WebService())
                {
                    using (WebService.PLCDataSet pLCDataSet = new WebService.PLCDataSet())
                    {
                        foreach(PLC.BinRow bin in plc.BinList )
                        {
                            WebService.PLCDataSet.BinsRow row = pLCDataSet.Bins.NewBinsRow();
                            row.Available_For_Clean_Load = bin.Available_For_Clean_Load;
                            row.Available_For_Treat_Load = bin.Available_For_Treat_Load;
                            row.Bin_Id = bin.BinId;
                            row.Bin_Name = bin.BinName; 
                            row.Variety_Id = bin.Variety_Id;
                            pLCDataSet.Bins.AddBinsRow(row);
                        }
                        foreach (var treatment in plc.TreatmentList )
                        {
                            WebService.PLCDataSet.TreatmentsRow row = pLCDataSet.Treatments.NewTreatmentsRow();
                            row.Description = treatment.Description;
                            row.Pump_Index  = treatment.Id;
                            
                            pLCDataSet.Treatments.AddTreatmentsRow(row);
                        }
                        pLCDataSet.BatchTypeAvailability.AddBatchTypeAvailabilityRow(!plc.TreatScaleUnavailable.Value  , !plc.CleanScaleUnavailable.Value );
                        proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                        WebService.PCResponse pcResponse= proxy.SetPLCValues(pLCDataSet);
                        if ((pcResponse.UID != plc.PcUID) && (pcResponse.UID != Guid.Empty ) && (plc.PcUID!= Guid.Empty )  )
                        {
                            plc.PcUID = pcResponse.UID;
                            SetPLCBatch(pcResponse);
                        }
                        plc.PcUID = pcResponse.UID;
                    }
                }
            }
            catch(Exception ex)
            {
            }
        }



        private void SetPLCBatch(WebService.PCResponse pcResponse)
        {

            if (pcResponse.BatchType == WebService.enumBatchType.Bulk)
            {
                PLC.SetBatchState(PLC.enumBatchType.Treat, PLC.enumBatchStatus.SelectBins);
                foreach (var item in pcResponse.BinStatus)
                {
                    PLC.SetBatchBin(PLC.enumBatchType.Treat, item.ItemId, item.Active);
                }


                for (int i = 1; i < 20; i++)
                {
                    if (i == 9 || i == 10)
                    {
                        PLC.SetTreaterPumpActive(i, true);
                    }
                    else
                    {
                        var item = pcResponse.TreatmentStatus.FirstOrDefault(x => x.ItemId == i);
                        if (item != null)
                        {
                            PLC.SetTreaterPumpActive(item.ItemId, item.Active);
                        }
                        else
                        {
                            PLC.SetTreaterPumpActive(i, false);
                        }
                    }
                }







                List<PLC.booleanTagValues> colorValues = new List<PLC.booleanTagValues>();
                for (int I=0;I<10;I++)
                {
                    colorValues.Add(new PLC.booleanTagValues(I) { Value = false });
                }
                foreach( var item in pcResponse.ColorStatus  )
                {
                    colorValues[item.ItemId].Value = item.Active; 
                }
                PLC.SetColors(colorValues);
            }

            else if (pcResponse.BatchType==WebService.enumBatchType.Clean )
            {
                {
                    PLC.SetBatchState(PLC.enumBatchType.Clean , PLC.enumBatchStatus.SelectBins);
                    foreach (var item in pcResponse.BinStatus)
                    {
                        PLC.SetBatchBin(PLC.enumBatchType.Clean, item.ItemId, item.Active);
                    }

                }



                //PLC.SetBatchState(PLC.enumBatchType.Clean, PLC.enumBatchStatus.SelectBins);
                //for (int i = 0; i < 29; i++)
                //{
                //    string ckBoxName = $"checkBox{i + 11}";
                //    CheckBox ckbx = (CheckBox)this.Controls.Find(ckBoxName, true)[0];
                //    PLC.SetBatchBin(PLC.enumBatchType.Clean, i, ckbx.Checked);
                //}





            }
        }

        private void btnSetColor_Click(object sender, EventArgs e)
        {
            List<PLC.booleanTagValues> colorValues = new List<PLC.booleanTagValues>();
            colorValues.Add(new PLC.booleanTagValues(0) { Value = checkBox1.Checked });
            colorValues.Add(new PLC.booleanTagValues(1) { Value = checkBox2.Checked });
            colorValues.Add(new PLC.booleanTagValues(2) { Value = checkBox3.Checked });
            colorValues.Add(new PLC.booleanTagValues(3) { Value = checkBox4.Checked });
            colorValues.Add(new PLC.booleanTagValues(4) { Value = checkBox5.Checked });
            colorValues.Add(new PLC.booleanTagValues(5) { Value = checkBox6.Checked });
            colorValues.Add(new PLC.booleanTagValues(6) { Value = checkBox7.Checked });
            colorValues.Add(new PLC.booleanTagValues(7) { Value = checkBox8.Checked });
            colorValues.Add(new PLC.booleanTagValues(8) { Value = checkBox9.Checked });
            colorValues.Add(new PLC.booleanTagValues(9) { Value = checkBox10.Checked });
            PLC.SetColors(colorValues);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PLC.SetBatchState(PLC.enumBatchType.Treat, PLC.enumBatchStatus.SelectBins );
            for (int i = 0; i < 29; i++)
            {
                string ckBoxName = $"checkBox{i+11}";
                CheckBox ckbx = (CheckBox)this.Controls.Find(ckBoxName, true)[0];
                PLC.SetBatchBin(PLC.enumBatchType.Treat, i, ckbx.Checked);
            }

            for (int i = 0; i < 19; i++)
            {
                string ckBoxName = $"checkBox{i + 40}";
                CheckBox ckbx = (CheckBox)this.Controls.Find(ckBoxName, true)[0];
                PLC.SetTreaterPumpActive( i+1, ckbx.Checked);
            }




        }

        private void button3_Click(object sender, EventArgs e)
        {
            PLC.SetBatchState(PLC.enumBatchType.Clean , PLC.enumBatchStatus.SelectBins);
            for (int i = 0; i < 29; i++)
            {
                string ckBoxName = $"checkBox{i+11}";
                CheckBox ckbx = (CheckBox)this.Controls.Find(ckBoxName, true)[0];
                PLC.SetBatchBin(PLC.enumBatchType.Clean, i, ckbx.Checked);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            PLC.SetBatchState(PLC.enumBatchType.Treat, PLC.enumBatchStatus.StopCancel );
        }

        private void button5_Click(object sender, EventArgs e)
        {
            PLC.SetBatchState(PLC.enumBatchType.Clean, PLC.enumBatchStatus.StopCancel);
        }

        private void label20_Click(object sender, EventArgs e)
        {
            
        }

        private void btnSendBins_Click(object sender, EventArgs e)
        {
            
        }
    }
}
