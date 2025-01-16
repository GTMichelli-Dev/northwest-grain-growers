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
    public partial class Manual_Weight : Form
    {
        public int Weight = 0;
        public int DefaultWeight;
        public string Record_Id;
        public enum enumWeighType { HarvestLoad, OutboundLoad, TransferLoad };
        public enumWeighType TypeOfLoad;
        public bool AskWhy = true ;

        public Manual_Weight(string Prompt,int DefaultWt,long? Load_Id , long Weight_Sheet_Id ,bool Ask_Why= true )
        {
            InitializeComponent();
            AskWhy = Ask_Why;
            TypeOfLoad = enumWeighType.HarvestLoad;
            if (Load_Id == null)
            {
                Record_Id = string.Format("Weight Sheet {0} Load (Not Set)", Weight_Sheet_Id);
            }
            else
            {
                Record_Id = string.Format("Weight Sheet {0} Load {1}", Weight_Sheet_Id,Load_Id );
            }
            if (DefaultWt < 0) DefaultWt = 0;
            this.lblPrompt.Text = Prompt;
            this.DefaultWeight = DefaultWt; 
            this.numWeight.Value = (decimal)DefaultWt;
            this.numWeight.GotFocus += new EventHandler(numWeight_GotFocus);
        }





        public Manual_Weight(string Prompt, int DefaultWt,bool Ask_Why= true )
        {
            InitializeComponent();
            AskWhy = Ask_Why;
            TypeOfLoad = enumWeighType.HarvestLoad;
            this.lblPrompt.Text = Prompt;
            if (DefaultWt < 0) DefaultWt = 0; 
            this.DefaultWeight = DefaultWt;
           
            this.numWeight.Value = (decimal)DefaultWt;
            this.numWeight.GotFocus += new EventHandler(numWeight_GotFocus);
        }

        void numWeight_GotFocus(object sender, EventArgs e)
        {
            this.numWeight.Select(0, 100);
        }



        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Weight = (int)this.numWeight.Value;
            
            if (Weight != this.DefaultWeight && AskWhy )
            {
                //using (frmEdit_Screen frm = new frmEdit_Screen("Why Are You Changing The Weight"))
                //{
                //    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //    {
                Logging.Add_Audit_Trail("Weight Change", Record_Id, this.lblPrompt.Text, this.DefaultWeight.ToString(), this.numWeight.Value.ToString(), "User Changed Weight");
                        //Logging.Add_Audit_Trail("Weight Change", Record_Id, this.lblPrompt.Text, this.DefaultWeight.ToString(), this.numWeight.Value.ToString(), frm.UserInput);
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        this.Close();
                //    }
                    
                //}
                
            }
            else
            {
                Logging.Add_Audit_Trail("Weight Change", Record_Id, this.lblPrompt.Text, this.DefaultWeight.ToString(), this.numWeight.Value.ToString(), "Weight Locked In");
                this.DialogResult = System.Windows.Forms.DialogResult.OK  ;
                this.Close();
            }
            
        }

        private void lblError_Click(object sender, EventArgs e)
        {

        }

        private void numWeight_ValueChanged(object sender, EventArgs e)
        {
           
        }

        private void Manual_Weight_Load(object sender, EventArgs e)
        {
            string Pass = string.Empty;
           
            //try
            //{
            //    using (OptionsDatasetTableAdapters.QueriesTableAdapter Q = new OptionsDatasetTableAdapters.QueriesTableAdapter())
            //    {
            //        Pass= Q.GetManualPassword().ToString();
                    
            //    }
            //}
            //catch
            //{

            //}
            if (!string.IsNullOrEmpty(Pass))
            {
                using (frmPassword frm = new NWGrain.frmPassword(Pass))
                {
                    if (frm.ShowDialog()!= DialogResult.OK )
                    {
                        this.DialogResult = DialogResult.Cancel;
                        this.Close();
                    }
                }
            }
        }

        private void lblPrompt_Click(object sender, EventArgs e)
        {

        }
    }
}
