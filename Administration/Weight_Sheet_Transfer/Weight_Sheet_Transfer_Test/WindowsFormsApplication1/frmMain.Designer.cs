namespace Weight_Sheet_Export
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.locationIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wSIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Net = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lotNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cropIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.producerIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.carrierIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Outbound_Location = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Transfer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vwWeightSheetInformationBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.nW_Data_MasterDataSet = new Weight_Sheet_Export.NW_Data_MasterDataSet();
            this.dtDate = new System.Windows.Forms.DateTimePicker();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFolderPath = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.ckListLocations = new System.Windows.Forms.CheckedListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.ckListDistricts = new System.Windows.Forms.CheckedListBox();
            this.locationsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.locationsTableAdapter = new Weight_Sheet_Export.NW_Data_MasterDataSetTableAdapters.LocationsTableAdapter();
            this.districtListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.district_ListTableAdapter = new Weight_Sheet_Export.NW_Data_MasterDataSetTableAdapters.District_ListTableAdapter();
            this.location_DistrictsTableAdapter1 = new Weight_Sheet_Export.NW_Data_MasterDataSetTableAdapters.Location_DistrictsTableAdapter();
            this.button4 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vwWeightSheetInformationBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nW_Data_MasterDataSet)).BeginInit();
            this.pnlStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.locationsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.districtListBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.locationIdDataGridViewTextBoxColumn,
            this.wSIdDataGridViewTextBoxColumn,
            this.Net,
            this.lotNumberDataGridViewTextBoxColumn,
            this.cropIdDataGridViewTextBoxColumn,
            this.producerIdDataGridViewTextBoxColumn,
            this.carrierIdDataGridViewTextBoxColumn,
            this.rateDataGridViewTextBoxColumn,
            this.Outbound_Location,
            this.Transfer});
            this.dataGridView1.DataSource = this.vwWeightSheetInformationBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(230, 60);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(855, 404);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick_1);
            // 
            // locationIdDataGridViewTextBoxColumn
            // 
            this.locationIdDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.locationIdDataGridViewTextBoxColumn.DataPropertyName = "Location_Id";
            this.locationIdDataGridViewTextBoxColumn.HeaderText = "Location";
            this.locationIdDataGridViewTextBoxColumn.Name = "locationIdDataGridViewTextBoxColumn";
            this.locationIdDataGridViewTextBoxColumn.Width = 73;
            // 
            // wSIdDataGridViewTextBoxColumn
            // 
            this.wSIdDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.wSIdDataGridViewTextBoxColumn.DataPropertyName = "WS_Id";
            this.wSIdDataGridViewTextBoxColumn.HeaderText = "Weight Sheet";
            this.wSIdDataGridViewTextBoxColumn.Name = "wSIdDataGridViewTextBoxColumn";
            this.wSIdDataGridViewTextBoxColumn.Width = 97;
            // 
            // Net
            // 
            this.Net.DataPropertyName = "Net";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N0";
            dataGridViewCellStyle1.NullValue = null;
            this.Net.DefaultCellStyle = dataGridViewCellStyle1;
            this.Net.HeaderText = "Net";
            this.Net.Name = "Net";
            this.Net.ReadOnly = true;
            // 
            // lotNumberDataGridViewTextBoxColumn
            // 
            this.lotNumberDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.lotNumberDataGridViewTextBoxColumn.DataPropertyName = "Lot_Number";
            this.lotNumberDataGridViewTextBoxColumn.HeaderText = "Lot";
            this.lotNumberDataGridViewTextBoxColumn.Name = "lotNumberDataGridViewTextBoxColumn";
            this.lotNumberDataGridViewTextBoxColumn.Width = 47;
            // 
            // cropIdDataGridViewTextBoxColumn
            // 
            this.cropIdDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cropIdDataGridViewTextBoxColumn.DataPropertyName = "Crop_Id";
            this.cropIdDataGridViewTextBoxColumn.HeaderText = "Crop";
            this.cropIdDataGridViewTextBoxColumn.Name = "cropIdDataGridViewTextBoxColumn";
            this.cropIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.cropIdDataGridViewTextBoxColumn.Width = 54;
            // 
            // producerIdDataGridViewTextBoxColumn
            // 
            this.producerIdDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.producerIdDataGridViewTextBoxColumn.DataPropertyName = "Producer_Id";
            this.producerIdDataGridViewTextBoxColumn.HeaderText = "Producer";
            this.producerIdDataGridViewTextBoxColumn.Name = "producerIdDataGridViewTextBoxColumn";
            this.producerIdDataGridViewTextBoxColumn.Width = 75;
            // 
            // carrierIdDataGridViewTextBoxColumn
            // 
            this.carrierIdDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.carrierIdDataGridViewTextBoxColumn.DataPropertyName = "Carrier_Id";
            this.carrierIdDataGridViewTextBoxColumn.HeaderText = "Hauler";
            this.carrierIdDataGridViewTextBoxColumn.Name = "carrierIdDataGridViewTextBoxColumn";
            this.carrierIdDataGridViewTextBoxColumn.Width = 63;
            // 
            // rateDataGridViewTextBoxColumn
            // 
            this.rateDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.rateDataGridViewTextBoxColumn.DataPropertyName = "Rate";
            this.rateDataGridViewTextBoxColumn.HeaderText = "Rate";
            this.rateDataGridViewTextBoxColumn.Name = "rateDataGridViewTextBoxColumn";
            this.rateDataGridViewTextBoxColumn.Width = 55;
            // 
            // Outbound_Location
            // 
            this.Outbound_Location.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Outbound_Location.DataPropertyName = "Outbound_Location";
            this.Outbound_Location.HeaderText = "Out Location";
            this.Outbound_Location.Name = "Outbound_Location";
            this.Outbound_Location.Width = 93;
            // 
            // Transfer
            // 
            this.Transfer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Transfer.DataPropertyName = "Transfer";
            this.Transfer.HeaderText = "Transfer";
            this.Transfer.Name = "Transfer";
            this.Transfer.ReadOnly = true;
            this.Transfer.Width = 71;
            // 
            // vwWeightSheetInformationBindingSource
            // 
            this.vwWeightSheetInformationBindingSource.DataMember = "vwWeight_Sheet_Information";
            this.vwWeightSheetInformationBindingSource.DataSource = this.nW_Data_MasterDataSet;
            // 
            // nW_Data_MasterDataSet
            // 
            this.nW_Data_MasterDataSet.DataSetName = "NW_Data_MasterDataSet";
            this.nW_Data_MasterDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dtDate
            // 
            this.dtDate.Location = new System.Drawing.Point(18, 63);
            this.dtDate.Name = "dtDate";
            this.dtDate.Size = new System.Drawing.Size(200, 20);
            this.dtDate.TabIndex = 1;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(143, 5);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(116, 49);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "Get Data";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(529, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Path of export file";
            // 
            // txtFolderPath
            // 
            this.txtFolderPath.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Weight_Sheet_Export.Properties.Settings.Default, "FolderPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtFolderPath.Location = new System.Drawing.Point(532, 34);
            this.txtFolderPath.Name = "txtFolderPath";
            this.txtFolderPath.Size = new System.Drawing.Size(350, 20);
            this.txtFolderPath.TabIndex = 7;
            this.txtFolderPath.Text = global::Weight_Sheet_Export.Properties.Settings.Default.FolderPath;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(889, 33);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(33, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtFileName
            // 
            this.txtFileName.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Weight_Sheet_Export.Properties.Settings.Default, "FileName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtFileName.Location = new System.Drawing.Point(936, 34);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(144, 20);
            this.txtFileName.TabIndex = 10;
            this.txtFileName.Text = global::Weight_Sheet_Export.Properties.Settings.Default.FileName;
            this.txtFileName.DoubleClick += new System.EventHandler(this.txtFileName_DoubleClick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(933, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Export File Name";
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(265, 5);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(116, 49);
            this.btnExport.TabIndex = 11;
            this.btnExport.Text = "Export Data";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // ckListLocations
            // 
            this.ckListLocations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ckListLocations.CheckOnClick = true;
            this.ckListLocations.FormattingEnabled = true;
            this.ckListLocations.Location = new System.Drawing.Point(18, 205);
            this.ckListLocations.Name = "ckListLocations";
            this.ckListLocations.Size = new System.Drawing.Size(200, 259);
            this.ckListLocations.TabIndex = 12;
            this.ckListLocations.Click += new System.EventHandler(this.ckListLocations_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(18, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(116, 49);
            this.button2.TabIndex = 13;
            this.button2.Text = "Select All";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(18, 174);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(116, 23);
            this.button3.TabIndex = 14;
            this.button3.Text = "Clear Selected";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // pnlStatus
            // 
            this.pnlStatus.Controls.Add(this.lblStatus);
            this.pnlStatus.Location = new System.Drawing.Point(300, 180);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Size = new System.Drawing.Size(497, 117);
            this.pnlStatus.TabIndex = 15;
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(0, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(497, 117);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "lblStatus";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ckListDistricts
            // 
            this.ckListDistricts.CheckOnClick = true;
            this.ckListDistricts.FormattingEnabled = true;
            this.ckListDistricts.Location = new System.Drawing.Point(18, 89);
            this.ckListDistricts.Name = "ckListDistricts";
            this.ckListDistricts.Size = new System.Drawing.Size(200, 79);
            this.ckListDistricts.TabIndex = 18;
            this.ckListDistricts.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ckListDistricts_ItemCheck);
            this.ckListDistricts.Click += new System.EventHandler(this.ckListDistricts_Click);
            this.ckListDistricts.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ckListDistricts_MouseClick);
            this.ckListDistricts.SelectedIndexChanged += new System.EventHandler(this.ckListDistricts_SelectedIndexChanged);
            this.ckListDistricts.SelectedValueChanged += new System.EventHandler(this.ckListDistricts_SelectedValueChanged);
            this.ckListDistricts.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ckListDistricts_MouseDown);
            this.ckListDistricts.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ckListDistricts_MouseUp);
            // 
            // locationsBindingSource
            // 
            this.locationsBindingSource.DataMember = "Locations";
            this.locationsBindingSource.DataSource = this.nW_Data_MasterDataSet;
            // 
            // locationsTableAdapter
            // 
            this.locationsTableAdapter.ClearBeforeFill = true;
            // 
            // districtListBindingSource
            // 
            this.districtListBindingSource.DataMember = "District_List";
            this.districtListBindingSource.DataSource = this.nW_Data_MasterDataSet;
            // 
            // district_ListTableAdapter
            // 
            this.district_ListTableAdapter.ClearBeforeFill = true;
            // 
            // location_DistrictsTableAdapter1
            // 
            this.location_DistrictsTableAdapter1.ClearBeforeFill = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(387, 5);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(116, 49);
            this.button4.TabIndex = 19;
            this.button4.Text = "DPR";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1097, 476);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.ckListDistricts);
            this.Controls.Add(this.pnlStatus);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.ckListLocations);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtFolderPath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.dtDate);
            this.Controls.Add(this.dataGridView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Weight Sheet Transfer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.frmMain_GiveFeedback);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vwWeightSheetInformationBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nW_Data_MasterDataSet)).EndInit();
            this.pnlStatus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.locationsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.districtListBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn bOLTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateCreatedDataGridViewTextBoxColumn;
        private NW_Data_MasterDataSet nW_Data_MasterDataSet;
        private System.Windows.Forms.BindingSource vwWeightSheetInformationBindingSource;
        private NW_Data_MasterDataSetTableAdapters.vwWeight_Sheet_InformationTableAdapter vwWeight_Sheet_InformationTableAdapter;
        private System.Windows.Forms.DateTimePicker dtDate;
        private System.Windows.Forms.BindingSource locationsBindingSource;
        private NW_Data_MasterDataSetTableAdapters.LocationsTableAdapter locationsTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn grossDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFolderPath;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.CheckedListBox ckListLocations;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn locationIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn wSIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Net;
        private System.Windows.Forms.DataGridViewTextBoxColumn lotNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cropIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn producerIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn carrierIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Outbound_Location;
        private System.Windows.Forms.DataGridViewTextBoxColumn Transfer;
        private System.Windows.Forms.BindingSource districtListBindingSource;
        private NW_Data_MasterDataSetTableAdapters.District_ListTableAdapter district_ListTableAdapter;
        private System.Windows.Forms.CheckedListBox ckListDistricts;
        private NW_Data_MasterDataSetTableAdapters.Location_DistrictsTableAdapter location_DistrictsTableAdapter1;
        private System.Windows.Forms.Button button4;
    }
}

