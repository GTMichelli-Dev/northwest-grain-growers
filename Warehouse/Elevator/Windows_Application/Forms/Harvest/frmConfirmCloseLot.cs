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
    public partial class frmConfirmCloseLot : Form
    {
        private long _Lot_ID;
        private Guid _Lot_UID;

        public frmConfirmCloseLot(long Lot_Id, Guid Lot_UID)
        {
            InitializeComponent();
            _Lot_ID = Lot_Id;
            _Lot_UID = Lot_UID;
            this.label1.Text = string.Format("There Are Open Weight Sheets For Lot {0}" + System.Environment.NewLine + "Do You Want To Close Them?",Lot_Id);
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var RemotePrint = SiteOptions.GetRemotePrintOriginal();
            Weight_Sheet.Close_Weight_Sheets(RemotePrint,Weight_Sheet.enumFilterType.Lot, _Lot_ID,true );
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            using (frmLot_Weight_Sheets frm = new frmLot_Weight_Sheets(_Lot_UID))
            {
                frm.ShowDialog();
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ConfirmCloseLot_Load(object sender, EventArgs e)
        {

        }
    }
}
