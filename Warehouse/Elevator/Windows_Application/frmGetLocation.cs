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
    public partial class frmGetLocation : Form
    {
        public frmGetLocation()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmGetLocation_Load(object sender, EventArgs e)
        {
         this.locations_ListTableAdapter.Fill(this.listsDataSet.Locations_List);
            var Description = listsDataSet.Locations_List.Where(L => L.Id == Settings.Location_Id).FirstOrDefault();
            if (Description == null)
            {
                this.cboLocations.SelectedIndex =0;
            }
            else
            {
                try
                {
                    var Index = this.cboLocations.FindString(Description.text);

                    this.cboLocations.SelectedIndex = Index;
                }
                catch
                {
                    this.cboLocations.SelectedIndex = 0;
                }
            }



        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Settings.Location_Id = (int)this.cboLocations.SelectedValue;
  
             
                this.DialogResult = DialogResult.OK;
                this.Close();
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cboLocations_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.btnOk.Visible = (this.cboLocations.SelectedIndex != -1 && (int)this.cboLocations.SelectedValue > -1);
        }
    }
}
