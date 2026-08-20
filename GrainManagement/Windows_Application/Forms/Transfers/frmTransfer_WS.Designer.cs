namespace NWGrain
{
    partial class frmTransfer_WS
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
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label rateLabel;
            System.Windows.Forms.Label carrier_DescriptionLabel;
            System.Windows.Forms.Label carrier_IdLabel;
            System.Windows.Forms.Label commentLabel;
            System.Windows.Forms.Label producer_ID_DescriptionLabel;
            System.Windows.Forms.Label crop_VarietyLabel1;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label wS_IdLabel;
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTransfer_WS));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.rowNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.SingleTransferWeightSheetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.nWDataset = new NWGrain.NWDataset();
            this.btnClosePrint = new System.Windows.Forms.Button();
            this.vwTransferWeight_Sheet_InformationBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnOk = new System.Windows.Forms.Button();
            this.txtTotalBilled = new System.Windows.Forms.TextBox();
            this.commentRichTextBox = new System.Windows.Forms.RichTextBox();
            this.rateTextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.ck_LoadOut = new System.Windows.Forms.CheckBox();
            this.date_CreatedLabel1 = new System.Windows.Forms.Label();
            this.crop_VarietyTextBox1 = new System.Windows.Forms.TextBox();
            this.producer_DescriptionTextBox = new System.Windows.Forms.TextBox();
            this.btnFixLot = new System.Windows.Forms.Button();
            this.pnlHauler = new System.Windows.Forms.Panel();
            this.carrier_IdTextBox = new System.Windows.Forms.TextBox();
            this.carrier_DescriptionTextBox = new System.Windows.Forms.TextBox();
            this.btnFixHauler = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.tableAdapterManager = new NWGrain.NWDatasetTableAdapters.TableAdapterManager();
            this.vwTransfer_Weight_Sheet_InformationTableAdapter = new NWGrain.NWDatasetTableAdapters.vwTransfer_Weight_Sheet_InformationTableAdapter();
            this.single_Transfer_Weight_SheetTableAdapter = new NWGrain.NWDatasetTableAdapters.Single_Transfer_Weight_SheetTableAdapter();
            this.lblBushels = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblTotalNet = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblWeightSheet = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuMove = new System.Windows.Forms.ToolStripMenuItem();
            this.label5 = new System.Windows.Forms.Label();
            this.ckIndirt = new System.Windows.Forms.CheckBox();
            this.lblCustom = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            rateLabel = new System.Windows.Forms.Label();
            carrier_DescriptionLabel = new System.Windows.Forms.Label();
            carrier_IdLabel = new System.Windows.Forms.Label();
            commentLabel = new System.Windows.Forms.Label();
            producer_ID_DescriptionLabel = new System.Windows.Forms.Label();
            crop_VarietyLabel1 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            wS_IdLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SingleTransferWeightSheetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vwTransferWeight_Sheet_InformationBindingSource)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlHauler.SuspendLayout();
            this.panel4.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            this.panel2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(11, 237);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(103, 20);
            label2.TabIndex = 19;
            label2.Text = "Total Billed:";
            // 
            // rateLabel
            // 
            rateLabel.AutoSize = true;
            rateLabel.Location = new System.Drawing.Point(11, 159);
            rateLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            rateLabel.Name = "rateLabel";
            rateLabel.Size = new System.Drawing.Size(53, 20);
            rateLabel.TabIndex = 17;
            rateLabel.Text = "Rate:";
            // 
            // carrier_DescriptionLabel
            // 
            carrier_DescriptionLabel.AutoSize = true;
            carrier_DescriptionLabel.Location = new System.Drawing.Point(11, 81);
            carrier_DescriptionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            carrier_DescriptionLabel.Name = "carrier_DescriptionLabel";
            carrier_DescriptionLabel.Size = new System.Drawing.Size(67, 20);
            carrier_DescriptionLabel.TabIndex = 3;
            carrier_DescriptionLabel.Text = "Hauler:";
            // 
            // carrier_IdLabel
            // 
            carrier_IdLabel.AutoSize = true;
            carrier_IdLabel.Location = new System.Drawing.Point(11, 3);
            carrier_IdLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            carrier_IdLabel.Name = "carrier_IdLabel";
            carrier_IdLabel.Size = new System.Drawing.Size(134, 20);
            carrier_IdLabel.TabIndex = 5;
            carrier_IdLabel.Text = "Hauler Number:";
            // 
            // commentLabel
            // 
            commentLabel.AutoSize = true;
            commentLabel.Location = new System.Drawing.Point(6, 6);
            commentLabel.Name = "commentLabel";
            commentLabel.Size = new System.Drawing.Size(90, 20);
            commentLabel.TabIndex = 0;
            commentLabel.Text = "Comment:";
            // 
            // producer_ID_DescriptionLabel
            // 
            producer_ID_DescriptionLabel.AutoSize = true;
            producer_ID_DescriptionLabel.Location = new System.Drawing.Point(11, 92);
            producer_ID_DescriptionLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            producer_ID_DescriptionLabel.Name = "producer_ID_DescriptionLabel";
            producer_ID_DescriptionLabel.Size = new System.Drawing.Size(66, 20);
            producer_ID_DescriptionLabel.TabIndex = 15;
            producer_ID_DescriptionLabel.Text = "Source";
            // 
            // crop_VarietyLabel1
            // 
            crop_VarietyLabel1.AutoSize = true;
            crop_VarietyLabel1.Location = new System.Drawing.Point(9, 159);
            crop_VarietyLabel1.Name = "crop_VarietyLabel1";
            crop_VarietyLabel1.Size = new System.Drawing.Size(102, 20);
            crop_VarietyLabel1.TabIndex = 17;
            crop_VarietyLabel1.Text = "Commodity ";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(9, 233);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(65, 20);
            label3.TabIndex = 32;
            label3.Text = "Variety";
            // 
            // wS_IdLabel
            // 
            wS_IdLabel.AutoSize = true;
            wS_IdLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            wS_IdLabel.Location = new System.Drawing.Point(560, 88);
            wS_IdLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            wS_IdLabel.Name = "wS_IdLabel";
            wS_IdLabel.Size = new System.Drawing.Size(102, 16);
            wS_IdLabel.TabIndex = 41;
            wS_IdLabel.Text = "Weight Sheet :";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightYellow;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.rowNumberDataGridViewTextBoxColumn,
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
            this.dataGridView1.DataSource = this.SingleTransferWeightSheetBindingSource;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(245)))), ((int)(((byte)(39)))));
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle11;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 45;
            this.dataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1505, 756);
            this.dataGridView1.TabIndex = 23;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            this.dataGridView1.Validating += new System.ComponentModel.CancelEventHandler(this.dataGridView1_Validating);
            // 
            // rowNumberDataGridViewTextBoxColumn
            // 
            this.rowNumberDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.rowNumberDataGridViewTextBoxColumn.DataPropertyName = "Row_Number";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N0";
            dataGridViewCellStyle2.NullValue = null;
            this.rowNumberDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.rowNumberDataGridViewTextBoxColumn.HeaderText = "";
            this.rowNumberDataGridViewTextBoxColumn.Name = "rowNumberDataGridViewTextBoxColumn";
            this.rowNumberDataGridViewTextBoxColumn.ReadOnly = true;
            this.rowNumberDataGridViewTextBoxColumn.Width = 19;
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
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "t";
            dataGridViewCellStyle3.NullValue = null;
            this.timeInDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.timeInDataGridViewTextBoxColumn.HeaderText = "In";
            this.timeInDataGridViewTextBoxColumn.Name = "timeInDataGridViewTextBoxColumn";
            this.timeInDataGridViewTextBoxColumn.ReadOnly = true;
            this.timeInDataGridViewTextBoxColumn.Width = 60;
            // 
            // timeOutDataGridViewTextBoxColumn
            // 
            this.timeOutDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.timeOutDataGridViewTextBoxColumn.DataPropertyName = "Time_Out";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "t";
            dataGridViewCellStyle4.NullValue = null;
            this.timeOutDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.timeOutDataGridViewTextBoxColumn.HeaderText = "Out";
            this.timeOutDataGridViewTextBoxColumn.Name = "timeOutDataGridViewTextBoxColumn";
            this.timeOutDataGridViewTextBoxColumn.ReadOnly = true;
            this.timeOutDataGridViewTextBoxColumn.Width = 79;
            // 
            // bolTextBox
            // 
            this.bolTextBox.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.bolTextBox.DataPropertyName = "Bol";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.bolTextBox.DefaultCellStyle = dataGridViewCellStyle5;
            this.bolTextBox.HeaderText = "Bol";
            this.bolTextBox.Name = "bolTextBox";
            this.bolTextBox.Width = 77;
            // 
            // binTextBox
            // 
            this.binTextBox.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.binTextBox.DataPropertyName = "Bin";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.binTextBox.DefaultCellStyle = dataGridViewCellStyle6;
            this.binTextBox.HeaderText = "Bin";
            this.binTextBox.Name = "binTextBox";
            this.binTextBox.Width = 77;
            // 
            // ProteinTextBox
            // 
            this.ProteinTextBox.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ProteinTextBox.DataPropertyName = "Protein";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "N2";
            dataGridViewCellStyle7.NullValue = null;
            this.ProteinTextBox.DefaultCellStyle = dataGridViewCellStyle7;
            this.ProteinTextBox.HeaderText = "Protein";
            this.ProteinTextBox.Name = "ProteinTextBox";
            this.ProteinTextBox.Width = 121;
            // 
            // grossDataGridViewTextBoxColumn
            // 
            this.grossDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.grossDataGridViewTextBoxColumn.DataPropertyName = "Gross";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Format = "N0";
            dataGridViewCellStyle8.NullValue = null;
            this.grossDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle8;
            this.grossDataGridViewTextBoxColumn.HeaderText = "Gross";
            this.grossDataGridViewTextBoxColumn.Name = "grossDataGridViewTextBoxColumn";
            this.grossDataGridViewTextBoxColumn.ReadOnly = true;
            this.grossDataGridViewTextBoxColumn.Width = 107;
            // 
            // tareDataGridViewTextBoxColumn
            // 
            this.tareDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.tareDataGridViewTextBoxColumn.DataPropertyName = "Tare";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Format = "N0";
            dataGridViewCellStyle9.NullValue = null;
            this.tareDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle9;
            this.tareDataGridViewTextBoxColumn.HeaderText = "Tare";
            this.tareDataGridViewTextBoxColumn.Name = "tareDataGridViewTextBoxColumn";
            this.tareDataGridViewTextBoxColumn.ReadOnly = true;
            this.tareDataGridViewTextBoxColumn.Width = 86;
            // 
            // netDataGridViewTextBoxColumn
            // 
            this.netDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.netDataGridViewTextBoxColumn.DataPropertyName = "Net";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle10.Format = "N0";
            dataGridViewCellStyle10.NullValue = null;
            this.netDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle10;
            this.netDataGridViewTextBoxColumn.HeaderText = "Net";
            this.netDataGridViewTextBoxColumn.Name = "netDataGridViewTextBoxColumn";
            this.netDataGridViewTextBoxColumn.ReadOnly = true;
            this.netDataGridViewTextBoxColumn.Width = 76;
            // 
            // SingleTransferWeightSheetBindingSource
            // 
            this.SingleTransferWeightSheetBindingSource.DataMember = "Single_Transfer_Weight_Sheet";
            this.SingleTransferWeightSheetBindingSource.DataSource = this.nWDataset;
            // 
            // nWDataset
            // 
            this.nWDataset.DataSetName = "NWDataset";
            this.nWDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // btnClosePrint
            // 
            this.btnClosePrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClosePrint.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnClosePrint.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClosePrint.ForeColor = System.Drawing.Color.White;
            this.btnClosePrint.Location = new System.Drawing.Point(1608, 93);
            this.btnClosePrint.Name = "btnClosePrint";
            this.btnClosePrint.Size = new System.Drawing.Size(260, 51);
            this.btnClosePrint.TabIndex = 38;
            this.btnClosePrint.Text = "Close Weight Sheet";
            this.btnClosePrint.UseVisualStyleBackColor = false;
            this.btnClosePrint.Click += new System.EventHandler(this.button4_Click);
            // 
            // vwTransferWeight_Sheet_InformationBindingSource
            // 
            this.vwTransferWeight_Sheet_InformationBindingSource.DataMember = "vwTransfer_Weight_Sheet_Information";
            this.vwTransferWeight_Sheet_InformationBindingSource.DataSource = this.nWDataset;
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.SeaGreen;
            this.btnOk.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Location = new System.Drawing.Point(366, 93);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(151, 51);
            this.btnOk.TabIndex = 36;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // txtTotalBilled
            // 
            this.txtTotalBilled.BackColor = System.Drawing.Color.White;
            this.txtTotalBilled.Location = new System.Drawing.Point(11, 273);
            this.txtTotalBilled.Margin = new System.Windows.Forms.Padding(4);
            this.txtTotalBilled.Name = "txtTotalBilled";
            this.txtTotalBilled.ReadOnly = true;
            this.txtTotalBilled.Size = new System.Drawing.Size(192, 26);
            this.txtTotalBilled.TabIndex = 20;
            this.txtTotalBilled.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // commentRichTextBox
            // 
            this.commentRichTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.vwTransferWeight_Sheet_InformationBindingSource, "Comment", true));
            this.commentRichTextBox.Location = new System.Drawing.Point(8, 28);
            this.commentRichTextBox.Name = "commentRichTextBox";
            this.commentRichTextBox.Size = new System.Drawing.Size(316, 128);
            this.commentRichTextBox.TabIndex = 1;
            this.commentRichTextBox.Text = "";
            this.commentRichTextBox.Validated += new System.EventHandler(this.commentRichTextBox_Validated);
            // 
            // rateTextBox
            // 
            this.rateTextBox.BackColor = System.Drawing.Color.White;
            this.rateTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.vwTransferWeight_Sheet_InformationBindingSource, "Rate", true));
            this.rateTextBox.Location = new System.Drawing.Point(11, 195);
            this.rateTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.rateTextBox.Name = "rateTextBox";
            this.rateTextBox.ReadOnly = true;
            this.rateTextBox.Size = new System.Drawing.Size(68, 26);
            this.rateTextBox.TabIndex = 18;
            this.rateTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnlHauler, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 35);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(351, 894);
            this.tableLayoutPanel1.TabIndex = 35;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(label3);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.ck_LoadOut);
            this.panel1.Controls.Add(this.date_CreatedLabel1);
            this.panel1.Controls.Add(crop_VarietyLabel1);
            this.panel1.Controls.Add(this.crop_VarietyTextBox1);
            this.panel1.Controls.Add(this.producer_DescriptionTextBox);
            this.panel1.Controls.Add(this.btnFixLot);
            this.panel1.Controls.Add(producer_ID_DescriptionLabel);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(342, 420);
            this.panel1.TabIndex = 19;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.vwTransferWeight_Sheet_InformationBindingSource, "Variety_Description", true));
            this.textBox1.Location = new System.Drawing.Point(9, 254);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(316, 26);
            this.textBox1.TabIndex = 33;
            // 
            // ck_LoadOut
            // 
            this.ck_LoadOut.AutoSize = true;
            this.ck_LoadOut.Location = new System.Drawing.Point(195, 13);
            this.ck_LoadOut.Name = "ck_LoadOut";
            this.ck_LoadOut.Size = new System.Drawing.Size(113, 24);
            this.ck_LoadOut.TabIndex = 31;
            this.ck_LoadOut.Text = "Train Load";
            this.ck_LoadOut.UseVisualStyleBackColor = true;
            this.ck_LoadOut.CheckedChanged += new System.EventHandler(this.ck_LoadOut_CheckedChanged);
            // 
            // date_CreatedLabel1
            // 
            this.date_CreatedLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.date_CreatedLabel1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.vwTransferWeight_Sheet_InformationBindingSource, "Creation_Date", true));
            this.date_CreatedLabel1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.date_CreatedLabel1.Location = new System.Drawing.Point(9, 2);
            this.date_CreatedLabel1.Name = "date_CreatedLabel1";
            this.date_CreatedLabel1.Size = new System.Drawing.Size(110, 30);
            this.date_CreatedLabel1.TabIndex = 29;
            this.date_CreatedLabel1.Text = "12/12/2020";
            this.date_CreatedLabel1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // crop_VarietyTextBox1
            // 
            this.crop_VarietyTextBox1.BackColor = System.Drawing.Color.White;
            this.crop_VarietyTextBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.vwTransferWeight_Sheet_InformationBindingSource, "Crop_Description", true));
            this.crop_VarietyTextBox1.Location = new System.Drawing.Point(9, 180);
            this.crop_VarietyTextBox1.Name = "crop_VarietyTextBox1";
            this.crop_VarietyTextBox1.ReadOnly = true;
            this.crop_VarietyTextBox1.Size = new System.Drawing.Size(316, 26);
            this.crop_VarietyTextBox1.TabIndex = 18;
            // 
            // producer_DescriptionTextBox
            // 
            this.producer_DescriptionTextBox.BackColor = System.Drawing.Color.White;
            this.producer_DescriptionTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.vwTransferWeight_Sheet_InformationBindingSource, "Source", true));
            this.producer_DescriptionTextBox.Location = new System.Drawing.Point(11, 113);
            this.producer_DescriptionTextBox.Name = "producer_DescriptionTextBox";
            this.producer_DescriptionTextBox.ReadOnly = true;
            this.producer_DescriptionTextBox.Size = new System.Drawing.Size(316, 26);
            this.producer_DescriptionTextBox.TabIndex = 22;
            // 
            // btnFixLot
            // 
            this.btnFixLot.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnFixLot.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFixLot.ForeColor = System.Drawing.Color.White;
            this.btnFixLot.Location = new System.Drawing.Point(176, 55);
            this.btnFixLot.Margin = new System.Windows.Forms.Padding(4);
            this.btnFixLot.Name = "btnFixLot";
            this.btnFixLot.Size = new System.Drawing.Size(151, 51);
            this.btnFixLot.TabIndex = 18;
            this.btnFixLot.Text = "Fix";
            this.btnFixLot.UseVisualStyleBackColor = false;
            this.btnFixLot.Click += new System.EventHandler(this.button2_Click);
            // 
            // pnlHauler
            // 
            this.pnlHauler.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHauler.Controls.Add(this.lblCustom);
            this.pnlHauler.Controls.Add(this.txtTotalBilled);
            this.pnlHauler.Controls.Add(label2);
            this.pnlHauler.Controls.Add(rateLabel);
            this.pnlHauler.Controls.Add(this.rateTextBox);
            this.pnlHauler.Controls.Add(this.carrier_IdTextBox);
            this.pnlHauler.Controls.Add(this.carrier_DescriptionTextBox);
            this.pnlHauler.Controls.Add(this.btnFixHauler);
            this.pnlHauler.Controls.Add(carrier_DescriptionLabel);
            this.pnlHauler.Controls.Add(carrier_IdLabel);
            this.pnlHauler.Location = new System.Drawing.Point(2, 426);
            this.pnlHauler.Margin = new System.Windows.Forms.Padding(2);
            this.pnlHauler.Name = "pnlHauler";
            this.pnlHauler.Size = new System.Drawing.Size(342, 240);
            this.pnlHauler.TabIndex = 20;
            // 
            // carrier_IdTextBox
            // 
            this.carrier_IdTextBox.BackColor = System.Drawing.Color.White;
            this.carrier_IdTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.vwTransferWeight_Sheet_InformationBindingSource, "Carrier_Id", true));
            this.carrier_IdTextBox.Location = new System.Drawing.Point(11, 39);
            this.carrier_IdTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.carrier_IdTextBox.Name = "carrier_IdTextBox";
            this.carrier_IdTextBox.ReadOnly = true;
            this.carrier_IdTextBox.Size = new System.Drawing.Size(141, 26);
            this.carrier_IdTextBox.TabIndex = 6;
            // 
            // carrier_DescriptionTextBox
            // 
            this.carrier_DescriptionTextBox.BackColor = System.Drawing.Color.White;
            this.carrier_DescriptionTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.vwTransferWeight_Sheet_InformationBindingSource, "Carrier_Description", true));
            this.carrier_DescriptionTextBox.Location = new System.Drawing.Point(11, 117);
            this.carrier_DescriptionTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.carrier_DescriptionTextBox.Name = "carrier_DescriptionTextBox";
            this.carrier_DescriptionTextBox.ReadOnly = true;
            this.carrier_DescriptionTextBox.Size = new System.Drawing.Size(297, 26);
            this.carrier_DescriptionTextBox.TabIndex = 4;
            // 
            // btnFixHauler
            // 
            this.btnFixHauler.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnFixHauler.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFixHauler.ForeColor = System.Drawing.Color.White;
            this.btnFixHauler.Location = new System.Drawing.Point(174, 6);
            this.btnFixHauler.Margin = new System.Windows.Forms.Padding(4);
            this.btnFixHauler.Name = "btnFixHauler";
            this.btnFixHauler.Size = new System.Drawing.Size(151, 51);
            this.btnFixHauler.TabIndex = 15;
            this.btnFixHauler.Text = "Fix";
            this.btnFixHauler.UseVisualStyleBackColor = false;
            this.btnFixHauler.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(commentLabel);
            this.panel4.Controls.Add(this.commentRichTextBox);
            this.panel4.Location = new System.Drawing.Point(3, 671);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(342, 164);
            this.panel4.TabIndex = 22;
            // 
            // pnlGrid
            // 
            this.pnlGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlGrid.Controls.Add(this.dataGridView1);
            this.pnlGrid.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlGrid.Location = new System.Drawing.Point(363, 151);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new System.Drawing.Size(1505, 756);
            this.pnlGrid.TabIndex = 34;
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
            // vwTransfer_Weight_Sheet_InformationTableAdapter
            // 
            this.vwTransfer_Weight_Sheet_InformationTableAdapter.ClearBeforeFill = true;
            // 
            // single_Transfer_Weight_SheetTableAdapter
            // 
            this.single_Transfer_Weight_SheetTableAdapter.ClearBeforeFill = true;
            // 
            // lblBushels
            // 
            this.lblBushels.BackColor = System.Drawing.Color.White;
            this.lblBushels.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBushels.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBushels.ForeColor = System.Drawing.Color.Black;
            this.lblBushels.Location = new System.Drawing.Point(910, 118);
            this.lblBushels.Name = "lblBushels";
            this.lblBushels.Size = new System.Drawing.Size(214, 26);
            this.lblBushels.TabIndex = 44;
            this.lblBushels.Text = "9,999,999";
            this.lblBushels.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(806, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 16);
            this.label4.TabIndex = 43;
            this.label4.Text = "Total Bushels:";
            // 
            // lblTotalNet
            // 
            this.lblTotalNet.BackColor = System.Drawing.Color.White;
            this.lblTotalNet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTotalNet.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalNet.ForeColor = System.Drawing.Color.Black;
            this.lblTotalNet.Location = new System.Drawing.Point(910, 92);
            this.lblTotalNet.Name = "lblTotalNet";
            this.lblTotalNet.Size = new System.Drawing.Size(214, 26);
            this.lblTotalNet.TabIndex = 40;
            this.lblTotalNet.Text = "9,999,999 lbs";
            this.lblTotalNet.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(834, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 16);
            this.label1.TabIndex = 39;
            this.label1.Text = "Total Net:";
            // 
            // lblWeightSheet
            // 
            this.lblWeightSheet.BackColor = System.Drawing.Color.White;
            this.lblWeightSheet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWeightSheet.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.vwTransferWeight_Sheet_InformationBindingSource, "WS_Id", true));
            this.lblWeightSheet.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWeightSheet.Location = new System.Drawing.Point(534, 108);
            this.lblWeightSheet.Name = "lblWeightSheet";
            this.lblWeightSheet.Size = new System.Drawing.Size(155, 34);
            this.lblWeightSheet.TabIndex = 42;
            this.lblWeightSheet.Text = "123456789";
            this.lblWeightSheet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.menuStrip1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1874, 35);
            this.panel2.TabIndex = 45;
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
            this.mnuMove.Size = new System.Drawing.Size(173, 20);
            this.mnuMove.Text = "Move To Intake Weight Sheet";
            this.mnuMove.Click += new System.EventHandler(this.mnuMove_Click);
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(351, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(1523, 40);
            this.label5.TabIndex = 46;
            this.label5.Text = "Transfer";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ckIndirt
            // 
            this.ckIndirt.AutoSize = true;
            this.ckIndirt.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.vwTransferWeight_Sheet_InformationBindingSource, "InDirt", true));
            this.ckIndirt.Location = new System.Drawing.Point(366, 46);
            this.ckIndirt.Name = "ckIndirt";
            this.ckIndirt.Size = new System.Drawing.Size(78, 24);
            this.ckIndirt.TabIndex = 49;
            this.ckIndirt.Text = "In Dirt";
            this.ckIndirt.UseVisualStyleBackColor = true;
            this.ckIndirt.Click += new System.EventHandler(this.ckIndirt_Click);
            // 
            // lblCustom
            // 
            this.lblCustom.AutoSize = true;
            this.lblCustom.ForeColor = System.Drawing.Color.Red;
            this.lblCustom.Location = new System.Drawing.Point(95, 198);
            this.lblCustom.Name = "lblCustom";
            this.lblCustom.Size = new System.Drawing.Size(70, 20);
            this.lblCustom.TabIndex = 21;
            this.lblCustom.Text = "Custom";
            // 
            // frmTransfer_WS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1874, 929);
            this.Controls.Add(this.ckIndirt);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblBushels);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblTotalNet);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblWeightSheet);
            this.Controls.Add(wS_IdLabel);
            this.Controls.Add(this.btnClosePrint);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MinimizeBox = false;
            this.Name = "frmTransfer_WS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Transfer Weight Sheet";
            this.Activated += new System.EventHandler(this.frmTransfer_WS_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmTransfer_WS_FormClosed);
            this.Load += new System.EventHandler(this.frmTransfer_WS_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SingleTransferWeightSheetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vwTransferWeight_Sheet_InformationBindingSource)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlHauler.ResumeLayout(false);
            this.pnlHauler.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.pnlGrid.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource SingleTransferWeightSheetBindingSource;
        private NWDataset nWDataset;
        private System.Windows.Forms.BindingSource vwTransferWeight_Sheet_InformationBindingSource;
        private System.Windows.Forms.Button btnClosePrint;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TextBox txtTotalBilled;
        private NWDatasetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.RichTextBox commentRichTextBox;
        private System.Windows.Forms.TextBox rateTextBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pnlHauler;
        private System.Windows.Forms.TextBox carrier_IdTextBox;
        private System.Windows.Forms.TextBox carrier_DescriptionTextBox;
        private System.Windows.Forms.Button btnFixHauler;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel pnlGrid;
        private NWDatasetTableAdapters.vwTransfer_Weight_Sheet_InformationTableAdapter vwTransfer_Weight_Sheet_InformationTableAdapter;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label date_CreatedLabel1;
        private System.Windows.Forms.TextBox crop_VarietyTextBox1;
        private System.Windows.Forms.TextBox producer_DescriptionTextBox;
        private NWDatasetTableAdapters.Single_Transfer_Weight_SheetTableAdapter single_Transfer_Weight_SheetTableAdapter;
        private System.Windows.Forms.CheckBox ck_LoadOut;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lblBushels;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblTotalNet;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblWeightSheet;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuMove;
        private System.Windows.Forms.Button btnFixLot;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox ckIndirt;
        private System.Windows.Forms.DataGridViewTextBoxColumn rowNumberDataGridViewTextBoxColumn;
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
        private System.Windows.Forms.Label lblCustom;
    }
}