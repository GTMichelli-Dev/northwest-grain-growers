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
    public partial class frmProducers : Form
    {
        public frmProducers()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Owner = Program.frmMdiMain;
        }

        private void producersBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            SaveData();
        }


        private bool SaveData()
        {
                try
                {
                    this.Validate();
                    this.producersBindingSource.EndEdit();
                    this.tableAdapterManager.UpdateAll(this.nWDataset);
                    return true;
                }
                catch (Exception ex)
                {
                    Alert.Show("Something Went Wrong Saving Data. See Log For Details");
                    System_Log.Log_Message("frmProducers.SaveData", ex.Message);
                    return false;
                }
        }

        private void Producers_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'nWDataset.Producers' table. You can move, or remove it, as needed.
            this.producersTableAdapter.Fill(this.nWDataset.Producers);

        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            
            string Description = string.Empty;
            int ID = -1;
            GETNAME:
            using (frmEdit_Screen frm = new frmEdit_Screen("Producer Name"))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    Description = frm.UserInput.ToUpper();
                    if (this.producersBindingSource.Find("Description", Description) > -1)
                    {
                        Alert.Show("Name Already Used", "Error");
                        Description = string.Empty;
                        goto GETNAME;
                    }
                    else
                    {
                    GETID:
                        using (frmEdit_Screen frmID = new frmEdit_Screen("Producer ID"))
                        {
                            if (frmID.ShowDialog()== System.Windows.Forms.DialogResult.OK)
                            {
                                if (int.TryParse(frmID.UserInput, out ID))
                                {
                                    if (this.producersBindingSource.Find("ID", ID) > -1)
                                    {
                                        Alert.Show("ID Is Already Used", "Error");
                                        ID = -1;
                                        goto GETID;
                                    }
                                    else
                                    {
                                        this.nWDataset.Producers.AddProducersRow(Guid.NewGuid()
                                                                                    , ID
                                                                                    , Description
                                                                                    , ""
                                                                                    , false
                                                                                    , false
                                                                                    , true);
                                        this.producersBindingSource.Position = this.producersBindingSource.Find("Description", Description); 
                                    }
                                }
                                else
                                {
                                    Alert.Show("ID Must Be A Number", "Error");
                                    goto GETID;
                                }

                            }
                        }

                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (SaveData())
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void producersDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            this.producersBindingSource.CancelEdit();
        }
    }
}
