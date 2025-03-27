namespace NWGrain
{
    partial class frmMain
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.button1 = new System.Windows.Forms.Button();
            this.btnTodaysLoads = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tmrSetup = new System.Windows.Forms.Timer(this.components);
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnEDIT = new System.Windows.Forms.DataGridViewLinkColumn();
            this.btnNew = new System.Windows.Forms.DataGridViewLinkColumn();
            this.totalLoadsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.notCompletedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Source = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Crop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lotNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Carrier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Comment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Landlord = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Farm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Train = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Variety = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weightSheetsForSelectionBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.nWDataset = new NWGrain.NWDataset();
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.setupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fixToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.intakeTransferToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transferIntakeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printINtakeReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printTransferReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printWeightSheetsAscendingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printDailyLoadsByCommodityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printDailyBinReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printClosedLotsReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.certWeightOnlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblLocation = new System.Windows.Forms.Label();
            this.cboLocations = new System.Windows.Forms.ComboBox();
            this.vwSiteLocationsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.settingsDataSet = new NWGrain.SettingsDataSet();
            this.current_WorkStation_SetupTableAdapter = new NWGrain.NWDatasetTableAdapters.WorkStation_SetupTableAdapter();
            this.vwSiteLocationsTableAdapter = new NWGrain.SettingsDataSetTableAdapters.vwSiteLocationsTableAdapter();
            this.LblSiteSettings = new System.Windows.Forms.Label();
            this.tmrRefresh = new System.Windows.Forms.Timer(this.components);
            this.bwCheckForUpdate = new System.ComponentModel.BackgroundWorker();
            this.weightSheetsForSelectionTableAdapter = new NWGrain.NWDatasetTableAdapters.WeightSheetsForSelectionTableAdapter();
            this.vw_Open_Weight_SheetsTableAdapter = new NWGrain.NWDatasetTableAdapters.vw_Open_Weight_SheetsTableAdapter();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.weightSheetsForSelectionBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vwSiteLocationsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingsDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.SeaGreen;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(12, 65);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(252, 80);
            this.button1.TabIndex = 0;
            this.button1.Text = "New Weight Sheet";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnTodaysLoads
            // 
            this.btnTodaysLoads.BackColor = System.Drawing.Color.SeaGreen;
            this.btnTodaysLoads.ForeColor = System.Drawing.Color.White;
            this.btnTodaysLoads.Location = new System.Drawing.Point(12, 377);
            this.btnTodaysLoads.Name = "btnTodaysLoads";
            this.btnTodaysLoads.Size = new System.Drawing.Size(252, 80);
            this.btnTodaysLoads.TabIndex = 1;
            this.btnTodaysLoads.Text = "# Loads Today\r\n123\r\n";
            this.btnTodaysLoads.UseVisualStyleBackColor = false;
            this.btnTodaysLoads.Click += new System.EventHandler(this.btnTodaysLoads_Click_1);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.SeaGreen;
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.Location = new System.Drawing.Point(12, 169);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(252, 80);
            this.button4.TabIndex = 3;
            this.button4.Text = "Lots";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.SeaGreen;
            this.button5.ForeColor = System.Drawing.Color.White;
            this.button5.Location = new System.Drawing.Point(12, 273);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(252, 80);
            this.button5.TabIndex = 4;
            this.button5.Text = "Weight Sheets";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button7
            // 
            this.button7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button7.BackColor = System.Drawing.Color.SeaGreen;
            this.button7.ForeColor = System.Drawing.Color.White;
            this.button7.Location = new System.Drawing.Point(12, 837);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(252, 80);
            this.button7.TabIndex = 6;
            this.button7.Text = "End Day";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(331, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(258, 28);
            this.label1.TabIndex = 8;
            this.label1.Text = "Open Weight Sheets";
            // 
            // tmrSetup
            // 
            this.tmrSetup.Interval = 3000;
            this.tmrSetup.Tick += new System.EventHandler(this.tmrSetup_Tick);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightYellow;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoGenerateColumns = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.btnEDIT,
            this.btnNew,
            this.totalLoadsDataGridViewTextBoxColumn,
            this.notCompletedDataGridViewTextBoxColumn,
            this.Source,
            this.Crop,
            this.lotNumberDataGridViewTextBoxColumn,
            this.Carrier,
            this.Comment,
            this.Landlord,
            this.Farm,
            this.Train,
            this.Variety});
            this.dataGridView1.DataSource = this.weightSheetsForSelectionBindingSource;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(245)))), ((int)(((byte)(39)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridView1.Location = new System.Drawing.Point(286, 65);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridView1.RowTemplate.Height = 50;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1588, 852);
            this.dataGridView1.TabIndex = 16;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            this.dataGridView1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGridView1_Scroll);
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            this.dataGridView1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseMove);
            // 
            // btnEDIT
            // 
            this.btnEDIT.ActiveLinkColor = System.Drawing.Color.Blue;
            this.btnEDIT.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.btnEDIT.DataPropertyName = "WS_Id";
            this.btnEDIT.Frozen = true;
            this.btnEDIT.HeaderText = "Weight Sheet";
            this.btnEDIT.MinimumWidth = 100;
            this.btnEDIT.Name = "btnEDIT";
            this.btnEDIT.ReadOnly = true;
            this.btnEDIT.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnEDIT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.btnEDIT.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // btnNew
            // 
            this.btnNew.ActiveLinkColor = System.Drawing.Color.Blue;
            this.btnNew.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.btnNew.DataPropertyName = "Weight_Sheet_Type";
            this.btnNew.HeaderText = "Type";
            this.btnNew.Name = "btnNew";
            this.btnNew.ReadOnly = true;
            this.btnNew.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnNew.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.btnNew.VisitedLinkColor = System.Drawing.Color.Blue;
            this.btnNew.Width = 92;
            // 
            // totalLoadsDataGridViewTextBoxColumn
            // 
            this.totalLoadsDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.totalLoadsDataGridViewTextBoxColumn.DataPropertyName = "Total_Loads";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.totalLoadsDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.totalLoadsDataGridViewTextBoxColumn.HeaderText = "Total";
            this.totalLoadsDataGridViewTextBoxColumn.Name = "totalLoadsDataGridViewTextBoxColumn";
            this.totalLoadsDataGridViewTextBoxColumn.ReadOnly = true;
            this.totalLoadsDataGridViewTextBoxColumn.Width = 94;
            // 
            // notCompletedDataGridViewTextBoxColumn
            // 
            this.notCompletedDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.notCompletedDataGridViewTextBoxColumn.DataPropertyName = "Not_Completed";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.notCompletedDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.notCompletedDataGridViewTextBoxColumn.HeaderText = "In Yard";
            this.notCompletedDataGridViewTextBoxColumn.Name = "notCompletedDataGridViewTextBoxColumn";
            this.notCompletedDataGridViewTextBoxColumn.ReadOnly = true;
            this.notCompletedDataGridViewTextBoxColumn.Width = 118;
            // 
            // Source
            // 
            this.Source.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Source.DataPropertyName = "Source";
            this.Source.HeaderText = "Source";
            this.Source.Name = "Source";
            this.Source.ReadOnly = true;
            this.Source.Width = 119;
            // 
            // Crop
            // 
            this.Crop.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Crop.DataPropertyName = "Crop";
            this.Crop.HeaderText = "Crop";
            this.Crop.Name = "Crop";
            this.Crop.ReadOnly = true;
            this.Crop.Width = 94;
            // 
            // lotNumberDataGridViewTextBoxColumn
            // 
            this.lotNumberDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.lotNumberDataGridViewTextBoxColumn.DataPropertyName = "Lot_Number";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.NullValue = null;
            this.lotNumberDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.lotNumberDataGridViewTextBoxColumn.HeaderText = "Lot";
            this.lotNumberDataGridViewTextBoxColumn.Name = "lotNumberDataGridViewTextBoxColumn";
            this.lotNumberDataGridViewTextBoxColumn.ReadOnly = true;
            this.lotNumberDataGridViewTextBoxColumn.Width = 76;
            // 
            // Carrier
            // 
            this.Carrier.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Carrier.DataPropertyName = "Carrier";
            this.Carrier.HeaderText = "Hauler";
            this.Carrier.Name = "Carrier";
            this.Carrier.ReadOnly = true;
            this.Carrier.Width = 112;
            // 
            // Comment
            // 
            this.Comment.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Comment.DataPropertyName = "Comment";
            this.Comment.HeaderText = "Comment";
            this.Comment.Name = "Comment";
            this.Comment.ReadOnly = true;
            this.Comment.Width = 148;
            // 
            // Landlord
            // 
            this.Landlord.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Landlord.DataPropertyName = "Landlord";
            this.Landlord.HeaderText = "Landlord";
            this.Landlord.Name = "Landlord";
            this.Landlord.ReadOnly = true;
            this.Landlord.Width = 142;
            // 
            // Farm
            // 
            this.Farm.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Farm.DataPropertyName = "FSA_Number";
            this.Farm.HeaderText = "Farm";
            this.Farm.Name = "Farm";
            this.Farm.ReadOnly = true;
            this.Farm.Width = 96;
            // 
            // Train
            // 
            this.Train.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Train.DataPropertyName = "Train";
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Red;
            this.Train.DefaultCellStyle = dataGridViewCellStyle6;
            this.Train.HeaderText = "";
            this.Train.Name = "Train";
            this.Train.ReadOnly = true;
            this.Train.Width = 19;
            // 
            // Variety
            // 
            this.Variety.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Variety.DataPropertyName = "Variety";
            this.Variety.HeaderText = "Variety";
            this.Variety.Name = "Variety";
            this.Variety.ReadOnly = true;
            this.Variety.Width = 116;
            // 
            // weightSheetsForSelectionBindingSource
            // 
            this.weightSheetsForSelectionBindingSource.DataMember = "WeightSheetsForSelection";
            this.weightSheetsForSelectionBindingSource.DataSource = this.nWDataset;
            // 
            // nWDataset
            // 
            this.nWDataset.DataSetName = "NWDataset";
            this.nWDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Interval = 10000;
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setupToolStripMenuItem,
            this.refreshToolStripMenuItem,
            this.fixToolStripMenuItem,
            this.reportsToolStripMenuItem,
            this.certWeightOnlyToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1874, 24);
            this.menuStrip1.TabIndex = 18;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.VisibleChanged += new System.EventHandler(this.menuStrip1_VisibleChanged);
            // 
            // setupToolStripMenuItem
            // 
            this.setupToolStripMenuItem.Name = "setupToolStripMenuItem";
            this.setupToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.setupToolStripMenuItem.Text = "Setup";
            this.setupToolStripMenuItem.Click += new System.EventHandler(this.setupToolStripMenuItem_Click);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // fixToolStripMenuItem
            // 
            this.fixToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.intakeTransferToolStripMenuItem,
            this.transferIntakeToolStripMenuItem});
            this.fixToolStripMenuItem.Name = "fixToolStripMenuItem";
            this.fixToolStripMenuItem.Size = new System.Drawing.Size(33, 20);
            this.fixToolStripMenuItem.Text = "Fix";
            this.fixToolStripMenuItem.Visible = false;
            // 
            // intakeTransferToolStripMenuItem
            // 
            this.intakeTransferToolStripMenuItem.Name = "intakeTransferToolStripMenuItem";
            this.intakeTransferToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.intakeTransferToolStripMenuItem.Text = "Intake->Transfer";
            this.intakeTransferToolStripMenuItem.Click += new System.EventHandler(this.intakeTransferToolStripMenuItem_Click);
            // 
            // transferIntakeToolStripMenuItem
            // 
            this.transferIntakeToolStripMenuItem.Name = "transferIntakeToolStripMenuItem";
            this.transferIntakeToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.transferIntakeToolStripMenuItem.Text = "Transfer->Intake";
            this.transferIntakeToolStripMenuItem.Click += new System.EventHandler(this.transferIntakeToolStripMenuItem_Click);
            // 
            // reportsToolStripMenuItem
            // 
            this.reportsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.printINtakeReportToolStripMenuItem,
            this.printTransferReportToolStripMenuItem,
            this.printWeightSheetsAscendingToolStripMenuItem,
            this.printDailyLoadsByCommodityToolStripMenuItem,
            this.printDailyBinReportToolStripMenuItem,
            this.printClosedLotsReportToolStripMenuItem});
            this.reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
            this.reportsToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.reportsToolStripMenuItem.Text = "Reports";
            // 
            // printINtakeReportToolStripMenuItem
            // 
            this.printINtakeReportToolStripMenuItem.Name = "printINtakeReportToolStripMenuItem";
            this.printINtakeReportToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
            this.printINtakeReportToolStripMenuItem.Text = "Prnit Intake Report";
            this.printINtakeReportToolStripMenuItem.Click += new System.EventHandler(this.printINtakeReportToolStripMenuItem_Click);
            // 
            // printTransferReportToolStripMenuItem
            // 
            this.printTransferReportToolStripMenuItem.Name = "printTransferReportToolStripMenuItem";
            this.printTransferReportToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
            this.printTransferReportToolStripMenuItem.Text = "Print Transfer Report";
            this.printTransferReportToolStripMenuItem.Click += new System.EventHandler(this.printTransferReportToolStripMenuItem_Click);
            // 
            // printWeightSheetsAscendingToolStripMenuItem
            // 
            this.printWeightSheetsAscendingToolStripMenuItem.Name = "printWeightSheetsAscendingToolStripMenuItem";
            this.printWeightSheetsAscendingToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
            this.printWeightSheetsAscendingToolStripMenuItem.Text = "Print Weight Sheets Ascending";
            this.printWeightSheetsAscendingToolStripMenuItem.Click += new System.EventHandler(this.printWeightSheetsAscendingToolStripMenuItem_Click);
            // 
            // printDailyLoadsByCommodityToolStripMenuItem
            // 
            this.printDailyLoadsByCommodityToolStripMenuItem.Name = "printDailyLoadsByCommodityToolStripMenuItem";
            this.printDailyLoadsByCommodityToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
            this.printDailyLoadsByCommodityToolStripMenuItem.Text = "Print Daily Loads By Commodity";
            this.printDailyLoadsByCommodityToolStripMenuItem.Click += new System.EventHandler(this.printDailyLoadsByCommodityToolStripMenuItem_Click);
            // 
            // printDailyBinReportToolStripMenuItem
            // 
            this.printDailyBinReportToolStripMenuItem.Name = "printDailyBinReportToolStripMenuItem";
            this.printDailyBinReportToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
            this.printDailyBinReportToolStripMenuItem.Text = "Print Daily Bin Report";
            this.printDailyBinReportToolStripMenuItem.Click += new System.EventHandler(this.printDailyBinReportToolStripMenuItem_Click);
            // 
            // printClosedLotsReportToolStripMenuItem
            // 
            this.printClosedLotsReportToolStripMenuItem.Name = "printClosedLotsReportToolStripMenuItem";
            this.printClosedLotsReportToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
            this.printClosedLotsReportToolStripMenuItem.Text = "Print Closed Lots Report";
            this.printClosedLotsReportToolStripMenuItem.Click += new System.EventHandler(this.printClosedLotsReportToolStripMenuItem_Click);
            // 
            // certWeightOnlyToolStripMenuItem
            // 
            this.certWeightOnlyToolStripMenuItem.Name = "certWeightOnlyToolStripMenuItem";
            this.certWeightOnlyToolStripMenuItem.Size = new System.Drawing.Size(110, 20);
            this.certWeightOnlyToolStripMenuItem.Text = "Cert Weight Only";
            this.certWeightOnlyToolStripMenuItem.Click += new System.EventHandler(this.certWeightOnlyToolStripMenuItem_Click);
            // 
            // lblLocation
            // 
            this.lblLocation.BackColor = System.Drawing.Color.Transparent;
            this.lblLocation.Location = new System.Drawing.Point(602, 33);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(425, 29);
            this.lblLocation.TabIndex = 19;
            this.lblLocation.Text = "lblLocation";
            this.lblLocation.Visible = false;
            // 
            // cboLocations
            // 
            this.cboLocations.DataSource = this.vwSiteLocationsBindingSource;
            this.cboLocations.DisplayMember = "ID_Description";
            this.cboLocations.FormattingEnabled = true;
            this.cboLocations.Location = new System.Drawing.Point(595, 27);
            this.cboLocations.Name = "cboLocations";
            this.cboLocations.Size = new System.Drawing.Size(356, 37);
            this.cboLocations.TabIndex = 20;
            this.cboLocations.ValueMember = "Id";
            this.cboLocations.Visible = false;
            this.cboLocations.SelectedIndexChanged += new System.EventHandler(this.cboLocations_SelectedIndexChanged);
            // 
            // vwSiteLocationsBindingSource
            // 
            this.vwSiteLocationsBindingSource.DataMember = "vwSiteLocations";
            this.vwSiteLocationsBindingSource.DataSource = this.settingsDataSet;
            // 
            // settingsDataSet
            // 
            this.settingsDataSet.DataSetName = "SettingsDataSet";
            this.settingsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // current_WorkStation_SetupTableAdapter
            // 
            this.current_WorkStation_SetupTableAdapter.ClearBeforeFill = true;
            // 
            // vwSiteLocationsTableAdapter
            // 
            this.vwSiteLocationsTableAdapter.ClearBeforeFill = true;
            // 
            // LblSiteSettings
            // 
            this.LblSiteSettings.BackColor = System.Drawing.Color.Transparent;
            this.LblSiteSettings.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSiteSettings.Location = new System.Drawing.Point(1218, 41);
            this.LblSiteSettings.Name = "LblSiteSettings";
            this.LblSiteSettings.Size = new System.Drawing.Size(434, 17);
            this.LblSiteSettings.TabIndex = 21;
            this.LblSiteSettings.Text = "LblSiteSettings";
            this.LblSiteSettings.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tmrRefresh
            // 
            this.tmrRefresh.Enabled = true;
            this.tmrRefresh.Interval = 200;
            this.tmrRefresh.Tick += new System.EventHandler(this.tmrRefresh_Tick);
            // 
            // bwCheckForUpdate
            // 
            this.bwCheckForUpdate.WorkerReportsProgress = true;
            this.bwCheckForUpdate.WorkerSupportsCancellation = true;
            this.bwCheckForUpdate.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwCheckForUpdate_DoWork);
            this.bwCheckForUpdate.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwCheckForUpdate_RunWorkerCompleted);
            // 
            // weightSheetsForSelectionTableAdapter
            // 
            this.weightSheetsForSelectionTableAdapter.ClearBeforeFill = true;
            // 
            // vw_Open_Weight_SheetsTableAdapter
            // 
            this.vw_Open_Weight_SheetsTableAdapter.ClearBeforeFill = true;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.SeaGreen;
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(12, 522);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(252, 80);
            this.button2.TabIndex = 23;
            this.button2.Text = "Refresh";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Beige;
            this.ClientSize = new System.Drawing.Size(1874, 929);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.LblSiteSettings);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnTodaysLoads);
            this.Controls.Add(this.cboLocations);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main Screen";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.frmMain_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.VisibleChanged += new System.EventHandler(this.frmMain_VisibleChanged);
            this.Move += new System.EventHandler(this.frmMain_Move);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.weightSheetsForSelectionBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vwSiteLocationsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingsDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnTodaysLoads;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Label label1;
        
        private System.Windows.Forms.Timer tmrSetup;
        private System.Windows.Forms.DataGridView dataGridView1;
        private NWDataset nWDataset;
        
        private System.Windows.Forms.Timer tmrUpdate;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem setupToolStripMenuItem;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.ComboBox cboLocations;
       
        private SettingsDataSet settingsDataSet;
        private NWDatasetTableAdapters.WorkStation_SetupTableAdapter  current_WorkStation_SetupTableAdapter;
        private System.Windows.Forms.BindingSource vwSiteLocationsBindingSource;
        private SettingsDataSetTableAdapters.vwSiteLocationsTableAdapter vwSiteLocationsTableAdapter;
        private System.Windows.Forms.Label LblSiteSettings;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.Timer tmrRefresh;
        private System.ComponentModel.BackgroundWorker bwCheckForUpdate;
        private System.Windows.Forms.BindingSource weightSheetsForSelectionBindingSource;
        private NWDatasetTableAdapters.WeightSheetsForSelectionTableAdapter weightSheetsForSelectionTableAdapter;
        private NWDatasetTableAdapters.vw_Open_Weight_SheetsTableAdapter vw_Open_Weight_SheetsTableAdapter;
        private System.Windows.Forms.ToolStripMenuItem fixToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem intakeTransferToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transferIntakeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printINtakeReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printTransferReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printWeightSheetsAscendingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printDailyLoadsByCommodityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printDailyBinReportToolStripMenuItem;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolStripMenuItem certWeightOnlyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printClosedLotsReportToolStripMenuItem;
        private System.Windows.Forms.DataGridViewLinkColumn btnEDIT;
        private System.Windows.Forms.DataGridViewLinkColumn btnNew;
        private System.Windows.Forms.DataGridViewTextBoxColumn totalLoadsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn notCompletedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Source;
        private System.Windows.Forms.DataGridViewTextBoxColumn Crop;
        private System.Windows.Forms.DataGridViewTextBoxColumn lotNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Carrier;
        private System.Windows.Forms.DataGridViewTextBoxColumn Comment;
        private System.Windows.Forms.DataGridViewTextBoxColumn Landlord;
        private System.Windows.Forms.DataGridViewTextBoxColumn Farm;
        private System.Windows.Forms.DataGridViewTextBoxColumn Train;
        private System.Windows.Forms.DataGridViewTextBoxColumn Variety;
    }
}

