namespace NWGrain
{
    partial class frmLoads
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLoads));
            this.cboCrop = new System.Windows.Forms.ComboBox();
            this.cropsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.nWDataset = new NWGrain.NWDataset();
            this.cboProducer = new System.Windows.Forms.ComboBox();
            this.producerSourceSelectionBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cboType = new System.Windows.Forms.ComboBox();
            this.pnlDate = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtDateEnd = new System.Windows.Forms.DateTimePicker();
            this.dtDateStart = new System.Windows.Forms.DateTimePicker();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Weight_Sheet = new System.Windows.Forms.DataGridViewLinkColumn();
            this.btnReprint = new System.Windows.Forms.DataGridViewLinkColumn();
            this.closedXDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weightSheetTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cropDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.producerSourceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lotNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creationDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnReopen = new System.Windows.Forms.DataGridViewLinkColumn();
            this.vwWeightSheetByTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.vw_Weight_Sheet_By_TypeTableAdapter = new NWGrain.NWDatasetTableAdapters.vw_Weight_Sheet_By_TypeTableAdapter();
            this.producer_SourceSelectionTableAdapter = new NWGrain.NWDatasetTableAdapters.Producer_SourceSelectionTableAdapter();
            this.cropsTableAdapter = new NWGrain.NWDatasetTableAdapters.CropsTableAdapter();
            this.btnReset = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.cropsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.producerSourceSelectionBindingSource)).BeginInit();
            this.pnlDate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vwWeightSheetByTypeBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // cboCrop
            // 
            this.cboCrop.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCrop.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCrop.DataSource = this.cropsBindingSource;
            this.cboCrop.DisplayMember = "Description";
            this.cboCrop.FormattingEnabled = true;
            this.cboCrop.Location = new System.Drawing.Point(899, 110);
            this.cboCrop.Name = "cboCrop";
            this.cboCrop.Size = new System.Drawing.Size(548, 37);
            this.cboCrop.TabIndex = 21;
            this.cboCrop.ValueMember = "Description";
            this.cboCrop.SelectedIndexChanged += new System.EventHandler(this.cbo_SelectedIndexChanged);
            // 
            // cropsBindingSource
            // 
            this.cropsBindingSource.DataMember = "Crops";
            this.cropsBindingSource.DataSource = this.nWDataset;
            // 
            // nWDataset
            // 
            this.nWDataset.DataSetName = "NWDataset";
            this.nWDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // cboProducer
            // 
            this.cboProducer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboProducer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboProducer.DataSource = this.producerSourceSelectionBindingSource;
            this.cboProducer.DisplayMember = "text";
            this.cboProducer.FormattingEnabled = true;
            this.cboProducer.Location = new System.Drawing.Point(329, 110);
            this.cboProducer.Name = "cboProducer";
            this.cboProducer.Size = new System.Drawing.Size(548, 37);
            this.cboProducer.TabIndex = 20;
            this.cboProducer.ValueMember = "value";
            this.cboProducer.SelectedIndexChanged += new System.EventHandler(this.cbo_SelectedIndexChanged);
            // 
            // producerSourceSelectionBindingSource
            // 
            this.producerSourceSelectionBindingSource.DataMember = "Producer_SourceSelection";
            this.producerSourceSelectionBindingSource.DataSource = this.nWDataset;
            // 
            // cboType
            // 
            this.cboType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboType.FormattingEnabled = true;
            this.cboType.Items.AddRange(new object[] {
            "All",
            "Inbound",
            "Transfer"});
            this.cboType.Location = new System.Drawing.Point(329, 65);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(413, 37);
            this.cboType.TabIndex = 19;
            this.cboType.SelectedIndexChanged += new System.EventHandler(this.cbo_SelectedIndexChanged);
            // 
            // pnlDate
            // 
            this.pnlDate.Controls.Add(this.label1);
            this.pnlDate.Controls.Add(this.label2);
            this.pnlDate.Controls.Add(this.dtDateEnd);
            this.pnlDate.Controls.Add(this.dtDateStart);
            this.pnlDate.Location = new System.Drawing.Point(743, 66);
            this.pnlDate.Name = "pnlDate";
            this.pnlDate.Size = new System.Drawing.Size(546, 38);
            this.pnlDate.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 29);
            this.label1.TabIndex = 10;
            this.label1.Text = "From";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(264, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 29);
            this.label2.TabIndex = 9;
            this.label2.Text = "To";
            // 
            // dtDateEnd
            // 
            this.dtDateEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtDateEnd.Location = new System.Drawing.Point(311, 0);
            this.dtDateEnd.Name = "dtDateEnd";
            this.dtDateEnd.Size = new System.Drawing.Size(179, 35);
            this.dtDateEnd.TabIndex = 8;
            this.dtDateEnd.CloseUp += new System.EventHandler(this.dtDateStart_CloseUp);
            // 
            // dtDateStart
            // 
            this.dtDateStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtDateStart.Location = new System.Drawing.Point(79, 0);
            this.dtDateStart.Name = "dtDateStart";
            this.dtDateStart.Size = new System.Drawing.Size(179, 35);
            this.dtDateStart.TabIndex = 6;
            this.dtDateStart.CloseUp += new System.EventHandler(this.dtDateStart_CloseUp);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.SeaGreen;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(12, 56);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(146, 69);
            this.button1.TabIndex = 17;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
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
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Weight_Sheet,
            this.btnReprint,
            this.closedXDataGridViewTextBoxColumn,
            this.weightSheetTypeDataGridViewTextBoxColumn,
            this.cropDataGridViewTextBoxColumn,
            this.producerSourceDataGridViewTextBoxColumn,
            this.lotNumberDataGridViewTextBoxColumn,
            this.creationDateDataGridViewTextBoxColumn,
            this.btnReopen});
            this.dataGridView1.DataSource = this.vwWeightSheetByTypeBindingSource;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(245)))), ((int)(((byte)(39)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridView1.Location = new System.Drawing.Point(14, 153);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 41;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1561, 679);
            this.dataGridView1.TabIndex = 16;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // Weight_Sheet
            // 
            this.Weight_Sheet.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Weight_Sheet.DataPropertyName = "WS_Id";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Weight_Sheet.DefaultCellStyle = dataGridViewCellStyle2;
            this.Weight_Sheet.HeaderText = "Weight Sheet";
            this.Weight_Sheet.Name = "Weight_Sheet";
            this.Weight_Sheet.ReadOnly = true;
            this.Weight_Sheet.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Weight_Sheet.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Weight_Sheet.Width = 191;
            // 
            // btnReprint
            // 
            this.btnReprint.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.btnReprint.DataPropertyName = "btnReprint";
            this.btnReprint.HeaderText = "";
            this.btnReprint.Name = "btnReprint";
            this.btnReprint.ReadOnly = true;
            this.btnReprint.Width = 5;
            // 
            // closedXDataGridViewTextBoxColumn
            // 
            this.closedXDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.closedXDataGridViewTextBoxColumn.DataPropertyName = "Closed_X";
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Red;
            this.closedXDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.closedXDataGridViewTextBoxColumn.HeaderText = "Closed";
            this.closedXDataGridViewTextBoxColumn.Name = "closedXDataGridViewTextBoxColumn";
            this.closedXDataGridViewTextBoxColumn.ReadOnly = true;
            this.closedXDataGridViewTextBoxColumn.Width = 118;
            // 
            // weightSheetTypeDataGridViewTextBoxColumn
            // 
            this.weightSheetTypeDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.weightSheetTypeDataGridViewTextBoxColumn.DataPropertyName = "Weight_Sheet_Type";
            this.weightSheetTypeDataGridViewTextBoxColumn.HeaderText = "Type";
            this.weightSheetTypeDataGridViewTextBoxColumn.Name = "weightSheetTypeDataGridViewTextBoxColumn";
            this.weightSheetTypeDataGridViewTextBoxColumn.ReadOnly = true;
            this.weightSheetTypeDataGridViewTextBoxColumn.Width = 92;
            // 
            // cropDataGridViewTextBoxColumn
            // 
            this.cropDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cropDataGridViewTextBoxColumn.DataPropertyName = "Crop";
            this.cropDataGridViewTextBoxColumn.HeaderText = "Crop";
            this.cropDataGridViewTextBoxColumn.Name = "cropDataGridViewTextBoxColumn";
            this.cropDataGridViewTextBoxColumn.ReadOnly = true;
            this.cropDataGridViewTextBoxColumn.Width = 94;
            // 
            // producerSourceDataGridViewTextBoxColumn
            // 
            this.producerSourceDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.producerSourceDataGridViewTextBoxColumn.DataPropertyName = "Producer_Source";
            this.producerSourceDataGridViewTextBoxColumn.HeaderText = "Producer/Source";
            this.producerSourceDataGridViewTextBoxColumn.MinimumWidth = 50;
            this.producerSourceDataGridViewTextBoxColumn.Name = "producerSourceDataGridViewTextBoxColumn";
            this.producerSourceDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lotNumberDataGridViewTextBoxColumn
            // 
            this.lotNumberDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.lotNumberDataGridViewTextBoxColumn.DataPropertyName = "Lot_Number";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.lotNumberDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.lotNumberDataGridViewTextBoxColumn.HeaderText = "Lot";
            this.lotNumberDataGridViewTextBoxColumn.Name = "lotNumberDataGridViewTextBoxColumn";
            this.lotNumberDataGridViewTextBoxColumn.ReadOnly = true;
            this.lotNumberDataGridViewTextBoxColumn.Width = 76;
            // 
            // creationDateDataGridViewTextBoxColumn
            // 
            this.creationDateDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.creationDateDataGridViewTextBoxColumn.DataPropertyName = "Creation_Date";
            dataGridViewCellStyle5.Format = "d";
            dataGridViewCellStyle5.NullValue = null;
            this.creationDateDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.creationDateDataGridViewTextBoxColumn.HeaderText = "Created";
            this.creationDateDataGridViewTextBoxColumn.Name = "creationDateDataGridViewTextBoxColumn";
            this.creationDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.creationDateDataGridViewTextBoxColumn.Width = 126;
            // 
            // btnReopen
            // 
            this.btnReopen.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.btnReopen.DataPropertyName = "btnReopen";
            this.btnReopen.HeaderText = "";
            this.btnReopen.Name = "btnReopen";
            this.btnReopen.ReadOnly = true;
            this.btnReopen.Width = 5;
            // 
            // vwWeightSheetByTypeBindingSource
            // 
            this.vwWeightSheetByTypeBindingSource.DataMember = "vw_Weight_Sheet_By_Type";
            this.vwWeightSheetByTypeBindingSource.DataSource = this.nWDataset;
            // 
            // vw_Weight_Sheet_By_TypeTableAdapter
            // 
            this.vw_Weight_Sheet_By_TypeTableAdapter.ClearBeforeFill = true;
            // 
            // producer_SourceSelectionTableAdapter
            // 
            this.producer_SourceSelectionTableAdapter.ClearBeforeFill = true;
            // 
            // cropsTableAdapter
            // 
            this.cropsTableAdapter.ClearBeforeFill = true;
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.Location = new System.Drawing.Point(1305, 67);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(142, 38);
            this.btnReset.TabIndex = 22;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(1588, 29);
            this.label3.TabIndex = 23;
            this.label3.Text = "Weight Sheet Loads";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // timer1
            // 
            this.timer1.Interval = 20;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frmLoads
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1588, 865);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.cboCrop);
            this.Controls.Add(this.cboProducer);
            this.Controls.Add(this.cboType);
            this.Controls.Add(this.pnlDate);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.MinimizeBox = false;
            this.Name = "frmLoads";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.frmLoads_Activated);
            this.Load += new System.EventHandler(this.frmLoads_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cropsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.producerSourceSelectionBindingSource)).EndInit();
            this.pnlDate.ResumeLayout(false);
            this.pnlDate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vwWeightSheetByTypeBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cboCrop;
        private System.Windows.Forms.ComboBox cboProducer;
        private System.Windows.Forms.ComboBox cboType;
        private System.Windows.Forms.Panel pnlDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtDateEnd;
        private System.Windows.Forms.DateTimePicker dtDateStart;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource vwWeightSheetByTypeBindingSource;
        private NWDataset nWDataset;
        private NWDatasetTableAdapters.vw_Weight_Sheet_By_TypeTableAdapter vw_Weight_Sheet_By_TypeTableAdapter;
        private System.Windows.Forms.BindingSource producerSourceSelectionBindingSource;
        private NWDatasetTableAdapters.Producer_SourceSelectionTableAdapter producer_SourceSelectionTableAdapter;
        private System.Windows.Forms.BindingSource cropsBindingSource;
        private NWDatasetTableAdapters.CropsTableAdapter cropsTableAdapter;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewLinkColumn Weight_Sheet;
        private System.Windows.Forms.DataGridViewLinkColumn btnReprint;
        private System.Windows.Forms.DataGridViewTextBoxColumn closedXDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn weightSheetTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cropDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn producerSourceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lotNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn creationDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewLinkColumn btnReopen;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
        private System.Windows.Forms.Timer timer1;
    }
}