namespace NWGrain
{
    partial class frmHarvest_WS
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label wS_IdLabel;
            System.Windows.Forms.Label producer_ID_DescriptionLabel;
            System.Windows.Forms.Label lot_NumberLabel;
            System.Windows.Forms.Label landlordLabel;
            System.Windows.Forms.Label producer_IdLabel;
            System.Windows.Forms.Label fSA_NumberLabel;
            System.Windows.Forms.Label split_NumberLabel;
            System.Windows.Forms.Label crop_VarietyLabel1;
            System.Windows.Forms.Label varietyLabel;
            System.Windows.Forms.Label commentLabel;
            System.Windows.Forms.Label carrier_IdLabel;
            System.Windows.Forms.Label carrier_DescriptionLabel;
            System.Windows.Forms.Label milesLabel;
            System.Windows.Forms.Label bOL_TypeLabel;
            System.Windows.Forms.Label rateLabel;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHarvest_WS));
            this.btnFixHauler = new System.Windows.Forms.Button();
            this.lot_NumberTextBox = new System.Windows.Forms.TextBox();
            this.vwWeight_Sheet_InformationBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.nWDataset = new NWGrain.NWDataset();
            this.panel1 = new System.Windows.Forms.Panel();
            this.LotCommentRTB = new System.Windows.Forms.RichTextBox();
            this.ck_LoadOut = new System.Windows.Forms.CheckBox();
            this.lblLotStatus = new System.Windows.Forms.Label();
            this.date_CreatedLabel1 = new System.Windows.Forms.Label();
            this.split_NumberTextBox = new System.Windows.Forms.TextBox();
            this.varietyTextBox = new System.Windows.Forms.TextBox();
            this.fSA_NumberTextBox = new System.Windows.Forms.TextBox();
            this.crop_VarietyTextBox1 = new System.Windows.Forms.TextBox();
            this.producer_DescriptionTextBox = new System.Windows.Forms.TextBox();
            this.producer_IdTextBox = new System.Windows.Forms.TextBox();
            this.landlordTextBox = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.commentRichTextBox = new System.Windows.Forms.RichTextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnEdit = new System.Windows.Forms.DataGridViewLinkColumn();
            this.truckIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeInDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeOutDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bolTextBox = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.binTextBox = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProteinTextBox = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grossDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tareDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.netDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.singleWeightSheetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlHauler = new System.Windows.Forms.Panel();
            this.txtTotalBilled = new System.Windows.Forms.TextBox();
            this.rateTextBox = new System.Windows.Forms.TextBox();
            this.bOL_TypeTextBox = new System.Windows.Forms.TextBox();
            this.milesTextBox = new System.Windows.Forms.TextBox();
            this.carrier_IdTextBox = new System.Windows.Forms.TextBox();
            this.carrier_DescriptionTextBox = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblTotalNet = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblWeightSheet = new System.Windows.Forms.Label();
            this.btnClosePrint = new System.Windows.Forms.Button();
            this.tableAdapterManager = new NWGrain.NWDatasetTableAdapters.TableAdapterManager();
            this.btnMoveWS = new System.Windows.Forms.Button();
            this.vwWeight_Sheet_InformationTableAdapter = new NWGrain.NWDatasetTableAdapters.vwWeight_Sheet_InformationTableAdapter();
            this.single_Weight_SheetTableAdapter = new NWGrain.NWDatasetTableAdapters.Single_Weight_SheetTableAdapter();
            this.lblBushels = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuMove = new System.Windows.Forms.ToolStripMenuItem();
            this.label5 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ckIndirt = new System.Windows.Forms.CheckBox();
            this.lblCustom = new System.Windows.Forms.Label();
            wS_IdLabel = new System.Windows.Forms.Label();
            producer_ID_DescriptionLabel = new System.Windows.Forms.Label();
            lot_NumberLabel = new System.Windows.Forms.Label();
            landlordLabel = new System.Windows.Forms.Label();
            producer_IdLabel = new System.Windows.Forms.Label();
            fSA_NumberLabel = new System.Windows.Forms.Label();
            split_NumberLabel = new System.Windows.Forms.Label();
            crop_VarietyLabel1 = new System.Windows.Forms.Label();
            varietyLabel = new System.Windows.Forms.Label();
            commentLabel = new System.Windows.Forms.Label();
            carrier_IdLabel = new System.Windows.Forms.Label();
            carrier_DescriptionLabel = new System.Windows.Forms.Label();
            milesLabel = new System.Windows.Forms.Label();
            bOL_TypeLabel = new System.Windows.Forms.Label();
            rateLabel = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.vwWeight_Sheet_InformationBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.singleWeightSheetBindingSource)).BeginInit();
            this.pnlGrid.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlHauler.SuspendLayout();
            this.panel2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // wS_IdLabel
            // 
            wS_IdLabel.AutoSize = true;
            wS_IdLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            wS_IdLabel.Location = new System.Drawing.Point(535, 89);
            wS_IdLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            wS_IdLabel.Name = "wS_IdLabel";
            wS_IdLabel.Size = new System.Drawing.Size(102, 16);
            wS_IdLabel.TabIndex = 1;
            wS_IdLabel.Text = "Weight Sheet :";
            // 
            // producer_ID_DescriptionLabel
            // 
            producer_ID_DescriptionLabel.AutoSize = true;
            producer_ID_DescriptionLabel.Location = new System.Drawing.Point(11, 125);
            producer_ID_DescriptionLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            producer_ID_DescriptionLabel.Name = "producer_ID_DescriptionLabel";
            producer_ID_DescriptionLabel.Size = new System.Drawing.Size(86, 19);
            producer_ID_DescriptionLabel.TabIndex = 15;
            producer_ID_DescriptionLabel.Text = "Producer:";
            // 
            // lot_NumberLabel
            // 
            lot_NumberLabel.AutoSize = true;
            lot_NumberLabel.Location = new System.Drawing.Point(11, 30);
            lot_NumberLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lot_NumberLabel.Name = "lot_NumberLabel";
            lot_NumberLabel.Size = new System.Drawing.Size(44, 19);
            lot_NumberLabel.TabIndex = 16;
            lot_NumberLabel.Text = "Lot :";
            lot_NumberLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // landlordLabel
            // 
            landlordLabel.AutoSize = true;
            landlordLabel.Location = new System.Drawing.Point(11, 173);
            landlordLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            landlordLabel.Name = "landlordLabel";
            landlordLabel.Size = new System.Drawing.Size(84, 19);
            landlordLabel.TabIndex = 19;
            landlordLabel.Text = "Landlord:";
            // 
            // producer_IdLabel
            // 
            producer_IdLabel.AutoSize = true;
            producer_IdLabel.Location = new System.Drawing.Point(11, 78);
            producer_IdLabel.Name = "producer_IdLabel";
            producer_IdLabel.Size = new System.Drawing.Size(149, 19);
            producer_IdLabel.TabIndex = 20;
            producer_IdLabel.Text = "Customer Number";
            // 
            // fSA_NumberLabel
            // 
            fSA_NumberLabel.AutoSize = true;
            fSA_NumberLabel.Location = new System.Drawing.Point(11, 318);
            fSA_NumberLabel.Name = "fSA_NumberLabel";
            fSA_NumberLabel.Size = new System.Drawing.Size(119, 19);
            fSA_NumberLabel.TabIndex = 22;
            fSA_NumberLabel.Text = "Farm Number:";
            // 
            // split_NumberLabel
            // 
            split_NumberLabel.AutoSize = true;
            split_NumberLabel.Location = new System.Drawing.Point(172, 318);
            split_NumberLabel.Name = "split_NumberLabel";
            split_NumberLabel.Size = new System.Drawing.Size(114, 19);
            split_NumberLabel.TabIndex = 23;
            split_NumberLabel.Text = "Split Number:";
            split_NumberLabel.Visible = false;
            // 
            // crop_VarietyLabel1
            // 
            crop_VarietyLabel1.AutoSize = true;
            crop_VarietyLabel1.Location = new System.Drawing.Point(11, 221);
            crop_VarietyLabel1.Name = "crop_VarietyLabel1";
            crop_VarietyLabel1.Size = new System.Drawing.Size(151, 19);
            crop_VarietyLabel1.TabIndex = 17;
            crop_VarietyLabel1.Text = "Commodity - Code";
            // 
            // varietyLabel
            // 
            varietyLabel.AutoSize = true;
            varietyLabel.Location = new System.Drawing.Point(11, 269);
            varietyLabel.Name = "varietyLabel";
            varietyLabel.Size = new System.Drawing.Size(67, 19);
            varietyLabel.TabIndex = 18;
            varietyLabel.Text = "Variety:";
            // 
            // commentLabel
            // 
            commentLabel.AutoSize = true;
            commentLabel.Location = new System.Drawing.Point(6, 6);
            commentLabel.Name = "commentLabel";
            commentLabel.Size = new System.Drawing.Size(119, 19);
            commentLabel.TabIndex = 0;
            commentLabel.Text = "WS Comment:";
            // 
            // carrier_IdLabel
            // 
            carrier_IdLabel.AutoSize = true;
            carrier_IdLabel.Location = new System.Drawing.Point(11, 3);
            carrier_IdLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            carrier_IdLabel.Name = "carrier_IdLabel";
            carrier_IdLabel.Size = new System.Drawing.Size(130, 19);
            carrier_IdLabel.TabIndex = 5;
            carrier_IdLabel.Text = "Hauler Number:";
            // 
            // carrier_DescriptionLabel
            // 
            carrier_DescriptionLabel.AutoSize = true;
            carrier_DescriptionLabel.Location = new System.Drawing.Point(11, 49);
            carrier_DescriptionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            carrier_DescriptionLabel.Name = "carrier_DescriptionLabel";
            carrier_DescriptionLabel.Size = new System.Drawing.Size(65, 19);
            carrier_DescriptionLabel.TabIndex = 3;
            carrier_DescriptionLabel.Text = "Hauler:";
            // 
            // milesLabel
            // 
            milesLabel.AutoSize = true;
            milesLabel.Location = new System.Drawing.Point(106, 96);
            milesLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            milesLabel.Name = "milesLabel";
            milesLabel.Size = new System.Drawing.Size(54, 19);
            milesLabel.TabIndex = 15;
            milesLabel.Text = "Miles:";
            // 
            // bOL_TypeLabel
            // 
            bOL_TypeLabel.AutoSize = true;
            bOL_TypeLabel.Location = new System.Drawing.Point(11, 96);
            bOL_TypeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            bOL_TypeLabel.Name = "bOL_TypeLabel";
            bOL_TypeLabel.Size = new System.Drawing.Size(90, 19);
            bOL_TypeLabel.TabIndex = 16;
            bOL_TypeLabel.Text = "BOL Type:";
            // 
            // rateLabel
            // 
            rateLabel.AutoSize = true;
            rateLabel.Location = new System.Drawing.Point(11, 142);
            rateLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            rateLabel.Name = "rateLabel";
            rateLabel.Size = new System.Drawing.Size(50, 19);
            rateLabel.TabIndex = 17;
            rateLabel.Text = "Rate:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(11, 188);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(99, 19);
            label2.TabIndex = 19;
            label2.Text = "Total Billed:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(11, 366);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(118, 19);
            label3.TabIndex = 0;
            label3.Text = "Lot Comment:";
            // 
            // btnFixHauler
            // 
            this.btnFixHauler.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnFixHauler.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFixHauler.ForeColor = System.Drawing.Color.White;
            this.btnFixHauler.Location = new System.Drawing.Point(176, 6);
            this.btnFixHauler.Margin = new System.Windows.Forms.Padding(4);
            this.btnFixHauler.Name = "btnFixHauler";
            this.btnFixHauler.Size = new System.Drawing.Size(151, 51);
            this.btnFixHauler.TabIndex = 15;
            this.btnFixHauler.Text = "Fix";
            this.btnFixHauler.UseVisualStyleBackColor = false;
            this.btnFixHauler.Click += new System.EventHandler(this.button1_Click);
            // 
            // lot_NumberTextBox
            // 
            this.lot_NumberTextBox.BackColor = System.Drawing.Color.White;
            this.lot_NumberTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.vwWeight_Sheet_InformationBindingSource, "Lot_Number", true));
            this.lot_NumberTextBox.Location = new System.Drawing.Point(11, 49);
            this.lot_NumberTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.lot_NumberTextBox.Name = "lot_NumberTextBox";
            this.lot_NumberTextBox.ReadOnly = true;
            this.lot_NumberTextBox.Size = new System.Drawing.Size(114, 26);
            this.lot_NumberTextBox.TabIndex = 17;
            // 
            // vwWeight_Sheet_InformationBindingSource
            // 
            this.vwWeight_Sheet_InformationBindingSource.DataMember = "vwWeight_Sheet_Information";
            this.vwWeight_Sheet_InformationBindingSource.DataSource = this.nWDataset;
            // 
            // nWDataset
            // 
            this.nWDataset.DataSetName = "NWDataset";
            this.nWDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(label3);
            this.panel1.Controls.Add(this.LotCommentRTB);
            this.panel1.Controls.Add(this.ck_LoadOut);
            this.panel1.Controls.Add(this.lblLotStatus);
            this.panel1.Controls.Add(split_NumberLabel);
            this.panel1.Controls.Add(varietyLabel);
            this.panel1.Controls.Add(this.date_CreatedLabel1);
            this.panel1.Controls.Add(this.split_NumberTextBox);
            this.panel1.Controls.Add(this.varietyTextBox);
            this.panel1.Controls.Add(fSA_NumberLabel);
            this.panel1.Controls.Add(crop_VarietyLabel1);
            this.panel1.Controls.Add(this.fSA_NumberTextBox);
            this.panel1.Controls.Add(this.crop_VarietyTextBox1);
            this.panel1.Controls.Add(this.producer_DescriptionTextBox);
            this.panel1.Controls.Add(this.lot_NumberTextBox);
            this.panel1.Controls.Add(lot_NumberLabel);
            this.panel1.Controls.Add(producer_IdLabel);
            this.panel1.Controls.Add(this.producer_IdTextBox);
            this.panel1.Controls.Add(landlordLabel);
            this.panel1.Controls.Add(this.landlordTextBox);
            this.panel1.Controls.Add(producer_ID_DescriptionLabel);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(342, 462);
            this.panel1.TabIndex = 19;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // LotCommentRTB
            // 
            this.LotCommentRTB.Location = new System.Drawing.Point(9, 386);
            this.LotCommentRTB.Name = "LotCommentRTB";
            this.LotCommentRTB.Size = new System.Drawing.Size(316, 71);
            this.LotCommentRTB.TabIndex = 1;
            this.LotCommentRTB.Text = "";
            // 
            // ck_LoadOut
            // 
            this.ck_LoadOut.AutoSize = true;
            this.ck_LoadOut.Location = new System.Drawing.Point(188, 13);
            this.ck_LoadOut.Name = "ck_LoadOut";
            this.ck_LoadOut.Size = new System.Drawing.Size(57, 23);
            this.ck_LoadOut.TabIndex = 30;
            this.ck_LoadOut.Text = "Rail";
            this.ck_LoadOut.UseVisualStyleBackColor = true;
            this.ck_LoadOut.CheckedChanged += new System.EventHandler(this.ck_LoadOut_CheckedChanged);
            // 
            // lblLotStatus
            // 
            this.lblLotStatus.AutoSize = true;
            this.lblLotStatus.BackColor = System.Drawing.SystemColors.Control;
            this.lblLotStatus.ForeColor = System.Drawing.Color.Red;
            this.lblLotStatus.Location = new System.Drawing.Point(59, 30);
            this.lblLotStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLotStatus.Name = "lblLotStatus";
            this.lblLotStatus.Size = new System.Drawing.Size(72, 19);
            this.lblLotStatus.TabIndex = 21;
            this.lblLotStatus.Text = "New Lot";
            this.lblLotStatus.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // date_CreatedLabel1
            // 
            this.date_CreatedLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.date_CreatedLabel1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.vwWeight_Sheet_InformationBindingSource, "Date_Created", true));
            this.date_CreatedLabel1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.date_CreatedLabel1.Location = new System.Drawing.Point(9, 2);
            this.date_CreatedLabel1.Name = "date_CreatedLabel1";
            this.date_CreatedLabel1.Size = new System.Drawing.Size(110, 30);
            this.date_CreatedLabel1.TabIndex = 29;
            this.date_CreatedLabel1.Text = "12/12/2020";
            this.date_CreatedLabel1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // split_NumberTextBox
            // 
            this.split_NumberTextBox.BackColor = System.Drawing.Color.White;
            this.split_NumberTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.vwWeight_Sheet_InformationBindingSource, "Split_Number", true));
            this.split_NumberTextBox.Location = new System.Drawing.Point(172, 337);
            this.split_NumberTextBox.Name = "split_NumberTextBox";
            this.split_NumberTextBox.ReadOnly = true;
            this.split_NumberTextBox.Size = new System.Drawing.Size(155, 26);
            this.split_NumberTextBox.TabIndex = 24;
            this.split_NumberTextBox.Visible = false;
            this.split_NumberTextBox.TextChanged += new System.EventHandler(this.split_NumberTextBox_TextChanged);
            // 
            // varietyTextBox
            // 
            this.varietyTextBox.BackColor = System.Drawing.Color.White;
            this.varietyTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.vwWeight_Sheet_InformationBindingSource, "Variety", true));
            this.varietyTextBox.Location = new System.Drawing.Point(11, 288);
            this.varietyTextBox.Name = "varietyTextBox";
            this.varietyTextBox.ReadOnly = true;
            this.varietyTextBox.Size = new System.Drawing.Size(316, 26);
            this.varietyTextBox.TabIndex = 19;
            // 
            // fSA_NumberTextBox
            // 
            this.fSA_NumberTextBox.BackColor = System.Drawing.Color.White;
            this.fSA_NumberTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.vwWeight_Sheet_InformationBindingSource, "FSA_Number", true));
            this.fSA_NumberTextBox.Location = new System.Drawing.Point(11, 337);
            this.fSA_NumberTextBox.Name = "fSA_NumberTextBox";
            this.fSA_NumberTextBox.ReadOnly = true;
            this.fSA_NumberTextBox.Size = new System.Drawing.Size(155, 26);
            this.fSA_NumberTextBox.TabIndex = 23;
            this.fSA_NumberTextBox.TextChanged += new System.EventHandler(this.fSA_NumberTextBox_TextChanged);
            // 
            // crop_VarietyTextBox1
            // 
            this.crop_VarietyTextBox1.BackColor = System.Drawing.Color.White;
            this.crop_VarietyTextBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.vwWeight_Sheet_InformationBindingSource, "Commodity_Code", true));
            this.crop_VarietyTextBox1.Location = new System.Drawing.Point(11, 240);
            this.crop_VarietyTextBox1.Name = "crop_VarietyTextBox1";
            this.crop_VarietyTextBox1.ReadOnly = true;
            this.crop_VarietyTextBox1.Size = new System.Drawing.Size(316, 26);
            this.crop_VarietyTextBox1.TabIndex = 18;
            // 
            // producer_DescriptionTextBox
            // 
            this.producer_DescriptionTextBox.BackColor = System.Drawing.Color.White;
            this.producer_DescriptionTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.vwWeight_Sheet_InformationBindingSource, "Producer_Description", true));
            this.producer_DescriptionTextBox.Location = new System.Drawing.Point(11, 144);
            this.producer_DescriptionTextBox.Name = "producer_DescriptionTextBox";
            this.producer_DescriptionTextBox.ReadOnly = true;
            this.producer_DescriptionTextBox.Size = new System.Drawing.Size(316, 26);
            this.producer_DescriptionTextBox.TabIndex = 22;
            // 
            // producer_IdTextBox
            // 
            this.producer_IdTextBox.BackColor = System.Drawing.Color.White;
            this.producer_IdTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.vwWeight_Sheet_InformationBindingSource, "Producer_Id", true));
            this.producer_IdTextBox.Location = new System.Drawing.Point(11, 97);
            this.producer_IdTextBox.Name = "producer_IdTextBox";
            this.producer_IdTextBox.ReadOnly = true;
            this.producer_IdTextBox.Size = new System.Drawing.Size(155, 26);
            this.producer_IdTextBox.TabIndex = 21;
            this.producer_IdTextBox.TextChanged += new System.EventHandler(this.producer_IdTextBox_TextChanged);
            // 
            // landlordTextBox
            // 
            this.landlordTextBox.BackColor = System.Drawing.Color.White;
            this.landlordTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.vwWeight_Sheet_InformationBindingSource, "Landlord", true));
            this.landlordTextBox.Location = new System.Drawing.Point(11, 192);
            this.landlordTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.landlordTextBox.Name = "landlordTextBox";
            this.landlordTextBox.ReadOnly = true;
            this.landlordTextBox.Size = new System.Drawing.Size(316, 26);
            this.landlordTextBox.TabIndex = 20;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(commentLabel);
            this.panel4.Controls.Add(this.commentRichTextBox);
            this.panel4.Location = new System.Drawing.Point(3, 713);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(342, 113);
            this.panel4.TabIndex = 22;
            // 
            // commentRichTextBox
            // 
            this.commentRichTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.vwWeight_Sheet_InformationBindingSource, "Comment", true));
            this.commentRichTextBox.Location = new System.Drawing.Point(8, 28);
            this.commentRichTextBox.Name = "commentRichTextBox";
            this.commentRichTextBox.Size = new System.Drawing.Size(316, 71);
            this.commentRichTextBox.TabIndex = 1;
            this.commentRichTextBox.Text = "";
            this.commentRichTextBox.Validated += new System.EventHandler(this.commentRichTextBox_Validated);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightYellow;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.btnEdit,
            this.truckIdDataGridViewTextBoxColumn,
            this.timeInDataGridViewTextBoxColumn,
            this.timeOutDataGridViewTextBoxColumn,
            this.bolTextBox,
            this.binTextBox,
            this.ProteinTextBox,
            this.grossDataGridViewTextBoxColumn,
            this.tareDataGridViewTextBoxColumn,
            this.netDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.singleWeightSheetBindingSource;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(245)))), ((int)(((byte)(39)))));
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle10;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 45;
            this.dataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1511, 761);
            this.dataGridView1.TabIndex = 23;
            this.dataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_CellBeginEdit);
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView1_CellValidating);
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            this.dataGridView1.Validating += new System.ComponentModel.CancelEventHandler(this.dataGridView1_Validating);
            // 
            // btnEdit
            // 
            this.btnEdit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.btnEdit.DataPropertyName = "Load_Id";
            this.btnEdit.HeaderText = "Ticket";
            this.btnEdit.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnEdit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.btnEdit.Width = 107;
            // 
            // truckIdDataGridViewTextBoxColumn
            // 
            this.truckIdDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.truckIdDataGridViewTextBoxColumn.DataPropertyName = "Truck_Id";
            this.truckIdDataGridViewTextBoxColumn.HeaderText = "Truck Id";
            this.truckIdDataGridViewTextBoxColumn.Name = "truckIdDataGridViewTextBoxColumn";
            this.truckIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.truckIdDataGridViewTextBoxColumn.Width = 131;
            // 
            // timeInDataGridViewTextBoxColumn
            // 
            this.timeInDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.timeInDataGridViewTextBoxColumn.DataPropertyName = "Time_In";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "t";
            dataGridViewCellStyle2.NullValue = null;
            this.timeInDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.timeInDataGridViewTextBoxColumn.HeaderText = "In";
            this.timeInDataGridViewTextBoxColumn.Name = "timeInDataGridViewTextBoxColumn";
            this.timeInDataGridViewTextBoxColumn.ReadOnly = true;
            this.timeInDataGridViewTextBoxColumn.Width = 60;
            // 
            // timeOutDataGridViewTextBoxColumn
            // 
            this.timeOutDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.timeOutDataGridViewTextBoxColumn.DataPropertyName = "Time_Out";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "t";
            dataGridViewCellStyle3.NullValue = null;
            this.timeOutDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.timeOutDataGridViewTextBoxColumn.HeaderText = "Out";
            this.timeOutDataGridViewTextBoxColumn.Name = "timeOutDataGridViewTextBoxColumn";
            this.timeOutDataGridViewTextBoxColumn.ReadOnly = true;
            this.timeOutDataGridViewTextBoxColumn.Width = 79;
            // 
            // bolTextBox
            // 
            this.bolTextBox.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.bolTextBox.DataPropertyName = "Bol";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.bolTextBox.DefaultCellStyle = dataGridViewCellStyle4;
            this.bolTextBox.HeaderText = "Bol";
            this.bolTextBox.Name = "bolTextBox";
            this.bolTextBox.Width = 77;
            // 
            // binTextBox
            // 
            this.binTextBox.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.binTextBox.DataPropertyName = "Bin";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.binTextBox.DefaultCellStyle = dataGridViewCellStyle5;
            this.binTextBox.HeaderText = "Bin";
            this.binTextBox.Name = "binTextBox";
            this.binTextBox.ReadOnly = true;
            this.binTextBox.Width = 77;
            // 
            // ProteinTextBox
            // 
            this.ProteinTextBox.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ProteinTextBox.DataPropertyName = "Protien";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N2";
            dataGridViewCellStyle6.NullValue = null;
            this.ProteinTextBox.DefaultCellStyle = dataGridViewCellStyle6;
            this.ProteinTextBox.HeaderText = "Protein";
            this.ProteinTextBox.Name = "ProteinTextBox";
            this.ProteinTextBox.Width = 121;
            // 
            // grossDataGridViewTextBoxColumn
            // 
            this.grossDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.grossDataGridViewTextBoxColumn.DataPropertyName = "Gross";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "N0";
            dataGridViewCellStyle7.NullValue = null;
            this.grossDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle7;
            this.grossDataGridViewTextBoxColumn.HeaderText = "Gross";
            this.grossDataGridViewTextBoxColumn.Name = "grossDataGridViewTextBoxColumn";
            this.grossDataGridViewTextBoxColumn.ReadOnly = true;
            this.grossDataGridViewTextBoxColumn.Width = 107;
            // 
            // tareDataGridViewTextBoxColumn
            // 
            this.tareDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.tareDataGridViewTextBoxColumn.DataPropertyName = "Tare";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Format = "N0";
            dataGridViewCellStyle8.NullValue = null;
            this.tareDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle8;
            this.tareDataGridViewTextBoxColumn.HeaderText = "Tare";
            this.tareDataGridViewTextBoxColumn.Name = "tareDataGridViewTextBoxColumn";
            this.tareDataGridViewTextBoxColumn.ReadOnly = true;
            this.tareDataGridViewTextBoxColumn.Width = 86;
            // 
            // netDataGridViewTextBoxColumn
            // 
            this.netDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.netDataGridViewTextBoxColumn.DataPropertyName = "Net";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Format = "N0";
            dataGridViewCellStyle9.NullValue = null;
            this.netDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle9;
            this.netDataGridViewTextBoxColumn.HeaderText = "Net";
            this.netDataGridViewTextBoxColumn.Name = "netDataGridViewTextBoxColumn";
            this.netDataGridViewTextBoxColumn.ReadOnly = true;
            this.netDataGridViewTextBoxColumn.Width = 76;
            // 
            // singleWeightSheetBindingSource
            // 
            this.singleWeightSheetBindingSource.DataMember = "Single_Weight_Sheet";
            this.singleWeightSheetBindingSource.DataSource = this.nWDataset;
            // 
            // pnlGrid
            // 
            this.pnlGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlGrid.Controls.Add(this.dataGridView1);
            this.pnlGrid.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlGrid.Location = new System.Drawing.Point(351, 156);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new System.Drawing.Size(1511, 761);
            this.pnlGrid.TabIndex = 24;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnlHauler, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 36);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(351, 893);
            this.tableLayoutPanel1.TabIndex = 25;
            // 
            // pnlHauler
            // 
            this.pnlHauler.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHauler.Controls.Add(this.lblCustom);
            this.pnlHauler.Controls.Add(this.txtTotalBilled);
            this.pnlHauler.Controls.Add(label2);
            this.pnlHauler.Controls.Add(rateLabel);
            this.pnlHauler.Controls.Add(this.rateTextBox);
            this.pnlHauler.Controls.Add(bOL_TypeLabel);
            this.pnlHauler.Controls.Add(this.bOL_TypeTextBox);
            this.pnlHauler.Controls.Add(milesLabel);
            this.pnlHauler.Controls.Add(this.milesTextBox);
            this.pnlHauler.Controls.Add(this.carrier_IdTextBox);
            this.pnlHauler.Controls.Add(this.carrier_DescriptionTextBox);
            this.pnlHauler.Controls.Add(this.btnFixHauler);
            this.pnlHauler.Controls.Add(carrier_DescriptionLabel);
            this.pnlHauler.Controls.Add(carrier_IdLabel);
            this.pnlHauler.Location = new System.Drawing.Point(2, 468);
            this.pnlHauler.Margin = new System.Windows.Forms.Padding(2);
            this.pnlHauler.Name = "pnlHauler";
            this.pnlHauler.Size = new System.Drawing.Size(342, 240);
            this.pnlHauler.TabIndex = 20;
            this.pnlHauler.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // txtTotalBilled
            // 
            this.txtTotalBilled.BackColor = System.Drawing.Color.White;
            this.txtTotalBilled.Location = new System.Drawing.Point(11, 207);
            this.txtTotalBilled.Margin = new System.Windows.Forms.Padding(4);
            this.txtTotalBilled.Name = "txtTotalBilled";
            this.txtTotalBilled.ReadOnly = true;
            this.txtTotalBilled.Size = new System.Drawing.Size(192, 26);
            this.txtTotalBilled.TabIndex = 20;
            this.txtTotalBilled.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // rateTextBox
            // 
            this.rateTextBox.BackColor = System.Drawing.Color.White;
            this.rateTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.vwWeight_Sheet_InformationBindingSource, "Rate", true));
            this.rateTextBox.Location = new System.Drawing.Point(11, 161);
            this.rateTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.rateTextBox.Name = "rateTextBox";
            this.rateTextBox.ReadOnly = true;
            this.rateTextBox.Size = new System.Drawing.Size(68, 26);
            this.rateTextBox.TabIndex = 18;
            this.rateTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // bOL_TypeTextBox
            // 
            this.bOL_TypeTextBox.BackColor = System.Drawing.Color.White;
            this.bOL_TypeTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.vwWeight_Sheet_InformationBindingSource, "BOL_Type", true));
            this.bOL_TypeTextBox.Location = new System.Drawing.Point(11, 115);
            this.bOL_TypeTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.bOL_TypeTextBox.Name = "bOL_TypeTextBox";
            this.bOL_TypeTextBox.ReadOnly = true;
            this.bOL_TypeTextBox.Size = new System.Drawing.Size(84, 26);
            this.bOL_TypeTextBox.TabIndex = 17;
            this.bOL_TypeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // milesTextBox
            // 
            this.milesTextBox.BackColor = System.Drawing.Color.White;
            this.milesTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.vwWeight_Sheet_InformationBindingSource, "Miles", true));
            this.milesTextBox.Location = new System.Drawing.Point(105, 115);
            this.milesTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.milesTextBox.Name = "milesTextBox";
            this.milesTextBox.ReadOnly = true;
            this.milesTextBox.Size = new System.Drawing.Size(68, 26);
            this.milesTextBox.TabIndex = 16;
            this.milesTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // carrier_IdTextBox
            // 
            this.carrier_IdTextBox.BackColor = System.Drawing.Color.White;
            this.carrier_IdTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.vwWeight_Sheet_InformationBindingSource, "Carrier_Id", true));
            this.carrier_IdTextBox.Location = new System.Drawing.Point(11, 22);
            this.carrier_IdTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.carrier_IdTextBox.Name = "carrier_IdTextBox";
            this.carrier_IdTextBox.ReadOnly = true;
            this.carrier_IdTextBox.Size = new System.Drawing.Size(141, 26);
            this.carrier_IdTextBox.TabIndex = 6;
            // 
            // carrier_DescriptionTextBox
            // 
            this.carrier_DescriptionTextBox.BackColor = System.Drawing.Color.White;
            this.carrier_DescriptionTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.vwWeight_Sheet_InformationBindingSource, "Carrier_Description", true));
            this.carrier_DescriptionTextBox.Location = new System.Drawing.Point(11, 68);
            this.carrier_DescriptionTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.carrier_DescriptionTextBox.Name = "carrier_DescriptionTextBox";
            this.carrier_DescriptionTextBox.ReadOnly = true;
            this.carrier_DescriptionTextBox.Size = new System.Drawing.Size(297, 26);
            this.carrier_DescriptionTextBox.TabIndex = 4;
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.SeaGreen;
            this.btnOk.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Location = new System.Drawing.Point(360, 97);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(151, 51);
            this.btnOk.TabIndex = 26;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lblTotalNet
            // 
            this.lblTotalNet.BackColor = System.Drawing.Color.White;
            this.lblTotalNet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTotalNet.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalNet.ForeColor = System.Drawing.Color.Black;
            this.lblTotalNet.Location = new System.Drawing.Point(779, 92);
            this.lblTotalNet.Name = "lblTotalNet";
            this.lblTotalNet.Size = new System.Drawing.Size(219, 26);
            this.lblTotalNet.TabIndex = 1;
            this.lblTotalNet.Text = "9,999,999 lbs";
            this.lblTotalNet.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(705, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Total Net:";
            // 
            // lblWeightSheet
            // 
            this.lblWeightSheet.BackColor = System.Drawing.Color.White;
            this.lblWeightSheet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWeightSheet.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.vwWeight_Sheet_InformationBindingSource, "WS_Id", true));
            this.lblWeightSheet.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWeightSheet.Location = new System.Drawing.Point(517, 108);
            this.lblWeightSheet.Name = "lblWeightSheet";
            this.lblWeightSheet.Size = new System.Drawing.Size(155, 34);
            this.lblWeightSheet.TabIndex = 28;
            this.lblWeightSheet.Text = "123456789";
            this.lblWeightSheet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClosePrint
            // 
            this.btnClosePrint.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnClosePrint.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClosePrint.ForeColor = System.Drawing.Color.White;
            this.btnClosePrint.Location = new System.Drawing.Point(1409, 93);
            this.btnClosePrint.Name = "btnClosePrint";
            this.btnClosePrint.Size = new System.Drawing.Size(206, 59);
            this.btnClosePrint.TabIndex = 30;
            this.btnClosePrint.Text = "Close \r\nWeight Sheet";
            this.btnClosePrint.UseVisualStyleBackColor = false;
            this.btnClosePrint.Click += new System.EventHandler(this.button4_Click);
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.Audit_TrailTableAdapter = null;
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.BinsTableAdapter = null;
            this.tableAdapterManager.CarriersTableAdapter = null;
            this.tableAdapterManager.Connection = null;
            this.tableAdapterManager.CropsTableAdapter = null;
            this.tableAdapterManager.Harvest_RatesTableAdapter = null;
            this.tableAdapterManager.LoadsTableAdapter = null;
            this.tableAdapterManager.LocationsTableAdapter = null;
            this.tableAdapterManager.LotsTableAdapter = null;
            this.tableAdapterManager.Outbound_CarriersTableAdapter = null;
            this.tableAdapterManager.Outbound_DestinationsTableAdapter = null;
            this.tableAdapterManager.ProducersTableAdapter = null;
            this.tableAdapterManager.Rate_SurchargeTableAdapter = null;
            this.tableAdapterManager.Site_SetupTableAdapter = null;
            this.tableAdapterManager.System_LogTableAdapter = null;
            this.tableAdapterManager.Transfer_RatesTableAdapter = null;
            this.tableAdapterManager.Unit_Of_MeasureTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = NWGrain.NWDatasetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            this.tableAdapterManager.VarietyTableAdapter = null;
            this.tableAdapterManager.Weigh_ScalesTableAdapter = null;
            this.tableAdapterManager.WeighMastersTableAdapter = null;
            this.tableAdapterManager.Weight_SheetsTableAdapter = null;
            this.tableAdapterManager.WorkStation_SetupTableAdapter = null;
            // 
            // btnMoveWS
            // 
            this.btnMoveWS.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnMoveWS.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoveWS.ForeColor = System.Drawing.Color.White;
            this.btnMoveWS.Location = new System.Drawing.Point(1005, 93);
            this.btnMoveWS.Margin = new System.Windows.Forms.Padding(4);
            this.btnMoveWS.Name = "btnMoveWS";
            this.btnMoveWS.Size = new System.Drawing.Size(209, 59);
            this.btnMoveWS.TabIndex = 31;
            this.btnMoveWS.Text = "Change Lot";
            this.btnMoveWS.UseVisualStyleBackColor = false;
            this.btnMoveWS.Click += new System.EventHandler(this.btnMoveWS_Click);
            // 
            // vwWeight_Sheet_InformationTableAdapter
            // 
            this.vwWeight_Sheet_InformationTableAdapter.ClearBeforeFill = true;
            // 
            // single_Weight_SheetTableAdapter
            // 
            this.single_Weight_SheetTableAdapter.ClearBeforeFill = true;
            // 
            // lblBushels
            // 
            this.lblBushels.BackColor = System.Drawing.Color.White;
            this.lblBushels.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBushels.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBushels.ForeColor = System.Drawing.Color.Black;
            this.lblBushels.Location = new System.Drawing.Point(779, 118);
            this.lblBushels.Name = "lblBushels";
            this.lblBushels.Size = new System.Drawing.Size(219, 26);
            this.lblBushels.TabIndex = 33;
            this.lblBushels.Text = "9,999,999";
            this.lblBushels.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(677, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 16);
            this.label4.TabIndex = 32;
            this.label4.Text = "Total Bushels:";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Yellow;
            this.btnClose.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Location = new System.Drawing.Point(1208, 93);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(206, 59);
            this.btnClose.TabIndex = 34;
            this.btnClose.Text = "Close\r\nWeight Sheet / Lot";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.menuStrip1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1874, 36);
            this.panel2.TabIndex = 35;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuMove});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1874, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuMove
            // 
            this.mnuMove.Name = "mnuMove";
            this.mnuMove.Size = new System.Drawing.Size(183, 20);
            this.mnuMove.Text = "Move To Transfer Weight Sheet";
            this.mnuMove.Click += new System.EventHandler(this.mnuMove_Click);
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(351, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(1523, 40);
            this.label5.TabIndex = 47;
            this.label5.Text = "Intake";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ckIndirt
            // 
            this.ckIndirt.AutoSize = true;
            this.ckIndirt.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.vwWeight_Sheet_InformationBindingSource, "InDirt", true));
            this.ckIndirt.Location = new System.Drawing.Point(384, 48);
            this.ckIndirt.Name = "ckIndirt";
            this.ckIndirt.Size = new System.Drawing.Size(73, 23);
            this.ckIndirt.TabIndex = 48;
            this.ckIndirt.Text = "In Dirt";
            this.ckIndirt.UseVisualStyleBackColor = true;
            this.ckIndirt.CheckedChanged += new System.EventHandler(this.ckIndirt_CheckedChanged);
            this.ckIndirt.Click += new System.EventHandler(this.ckIndirt_Click);
            // 
            // lblCustom
            // 
            this.lblCustom.ForeColor = System.Drawing.Color.Red;
            this.lblCustom.Location = new System.Drawing.Point(181, 98);
            this.lblCustom.Name = "lblCustom";
            this.lblCustom.Size = new System.Drawing.Size(200, 44);
            this.lblCustom.TabIndex = 22;
            this.lblCustom.Text = "Custom";
            this.lblCustom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmHarvest_WS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1874, 929);
            this.Controls.Add(this.ckIndirt);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblBushels);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnMoveWS);
            this.Controls.Add(this.btnClosePrint);
            this.Controls.Add(this.lblTotalNet);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblWeightSheet);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(wS_IdLabel);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.MinimizeBox = false;
            this.Name = "frmHarvest_WS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Weight Sheet";
            this.Activated += new System.EventHandler(this.frmHarvest_WS_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmHarvest_WS_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmHarvest_WS_FormClosed);
            this.Load += new System.EventHandler(this.frmHarvest_WS_Load);
            ((System.ComponentModel.ISupportInitialize)(this.vwWeight_Sheet_InformationBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.singleWeightSheetBindingSource)).EndInit();
            this.pnlGrid.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlHauler.ResumeLayout(false);
            this.pnlHauler.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NWDataset nWDataset;
        private System.Windows.Forms.BindingSource vwWeight_Sheet_InformationBindingSource;
        private NWDatasetTableAdapters.vwWeight_Sheet_InformationTableAdapter vwWeight_Sheet_InformationTableAdapter;
        private NWDatasetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.Button btnFixHauler;
        private System.Windows.Forms.TextBox lot_NumberTextBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox landlordTextBox;
        private System.Windows.Forms.TextBox producer_DescriptionTextBox;
        private System.Windows.Forms.TextBox producer_IdTextBox;
        private System.Windows.Forms.TextBox fSA_NumberTextBox;
        private System.Windows.Forms.TextBox split_NumberTextBox;
        private System.Windows.Forms.TextBox crop_VarietyTextBox1;
        private System.Windows.Forms.TextBox varietyTextBox;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.RichTextBox commentRichTextBox;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource singleWeightSheetBindingSource;
        private NWDatasetTableAdapters.Single_Weight_SheetTableAdapter single_Weight_SheetTableAdapter;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblWeightSheet;
        private System.Windows.Forms.Label date_CreatedLabel1;
        private System.Windows.Forms.Label lblTotalNet;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClosePrint;
        private System.Windows.Forms.Panel pnlHauler;
        private System.Windows.Forms.TextBox txtTotalBilled;
        private System.Windows.Forms.TextBox rateTextBox;
        private System.Windows.Forms.TextBox bOL_TypeTextBox;
        private System.Windows.Forms.TextBox milesTextBox;
        private System.Windows.Forms.TextBox carrier_IdTextBox;
        private System.Windows.Forms.TextBox carrier_DescriptionTextBox;
        private System.Windows.Forms.Label lblLotStatus;
        private System.Windows.Forms.CheckBox ck_LoadOut;
        private System.Windows.Forms.Button btnMoveWS;
        private System.Windows.Forms.Label lblBushels;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewLinkColumn btnEdit;
        private System.Windows.Forms.DataGridViewTextBoxColumn truckIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn timeInDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn timeOutDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bolTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn binTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProteinTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn grossDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tareDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn netDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuMove;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.RichTextBox LotCommentRTB;
        private System.Windows.Forms.CheckBox ckIndirt;
        private System.Windows.Forms.Label lblCustom;
    }
}