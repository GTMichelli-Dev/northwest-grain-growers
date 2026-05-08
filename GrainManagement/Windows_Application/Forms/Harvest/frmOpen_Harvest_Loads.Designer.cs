namespace NWGrain
{
    partial class frmOpen_Harvest_Loads
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnEdit = new System.Windows.Forms.DataGridViewLinkColumn();
            this.wSIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lotNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeInDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weightInDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cropDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.producerDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.landlordDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.carrierDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fSANumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vwOpenHarvestLoadsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.nWDataset = new NWGrain.NWDataset();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.vw_Open_Harvest_LoadsTableAdapter = new NWGrain.NWDatasetTableAdapters.vw_Open_Harvest_LoadsTableAdapter();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vwOpenHarvestLoadsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(8, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1300, 627);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
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
            this.btnEdit,
            this.wSIdDataGridViewTextBoxColumn,
            this.lotNumberDataGridViewTextBoxColumn,
            this.timeInDataGridViewTextBoxColumn,
            this.weightInDataGridViewTextBoxColumn,
            this.cropDataGridViewTextBoxColumn,
            this.producerDataGridViewTextBoxColumn,
            this.landlordDataGridViewTextBoxColumn,
            this.carrierDataGridViewTextBoxColumn,
            this.fSANumberDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.vwOpenHarvestLoadsBindingSource;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(245)))), ((int)(((byte)(39)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridView1.Location = new System.Drawing.Point(9, 89);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridView1.RowTemplate.Height = 50;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1282, 519);
            this.dataGridView1.TabIndex = 17;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick_1);
            // 
            // btnEdit
            // 
            this.btnEdit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.btnEdit.DataPropertyName = "Load_Id";
            this.btnEdit.HeaderText = "Ticket";
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.ReadOnly = true;
            this.btnEdit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnEdit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.btnEdit.Width = 107;
            // 
            // wSIdDataGridViewTextBoxColumn
            // 
            this.wSIdDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            this.wSIdDataGridViewTextBoxColumn.DataPropertyName = "WS_Id";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.wSIdDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.wSIdDataGridViewTextBoxColumn.HeaderText = "Weight Sheet";
            this.wSIdDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.wSIdDataGridViewTextBoxColumn.Name = "wSIdDataGridViewTextBoxColumn";
            this.wSIdDataGridViewTextBoxColumn.ReadOnly = true;
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
            // timeInDataGridViewTextBoxColumn
            // 
            this.timeInDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.timeInDataGridViewTextBoxColumn.DataPropertyName = "Time_In";
            dataGridViewCellStyle5.Format = "d";
            dataGridViewCellStyle5.NullValue = null;
            this.timeInDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.timeInDataGridViewTextBoxColumn.HeaderText = "In";
            this.timeInDataGridViewTextBoxColumn.Name = "timeInDataGridViewTextBoxColumn";
            this.timeInDataGridViewTextBoxColumn.ReadOnly = true;
            this.timeInDataGridViewTextBoxColumn.Width = 60;
            // 
            // weightInDataGridViewTextBoxColumn
            // 
            this.weightInDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.weightInDataGridViewTextBoxColumn.DataPropertyName = "Weight_In";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N0";
            dataGridViewCellStyle6.NullValue = null;
            this.weightInDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.weightInDataGridViewTextBoxColumn.HeaderText = "Weight";
            this.weightInDataGridViewTextBoxColumn.Name = "weightInDataGridViewTextBoxColumn";
            this.weightInDataGridViewTextBoxColumn.ReadOnly = true;
            this.weightInDataGridViewTextBoxColumn.Width = 119;
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
            // producerDataGridViewTextBoxColumn
            // 
            this.producerDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.producerDataGridViewTextBoxColumn.DataPropertyName = "Producer";
            this.producerDataGridViewTextBoxColumn.HeaderText = "Producer";
            this.producerDataGridViewTextBoxColumn.Name = "producerDataGridViewTextBoxColumn";
            this.producerDataGridViewTextBoxColumn.ReadOnly = true;
            this.producerDataGridViewTextBoxColumn.Width = 143;
            // 
            // landlordDataGridViewTextBoxColumn
            // 
            this.landlordDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.landlordDataGridViewTextBoxColumn.DataPropertyName = "Landlord";
            this.landlordDataGridViewTextBoxColumn.HeaderText = "Landlord";
            this.landlordDataGridViewTextBoxColumn.Name = "landlordDataGridViewTextBoxColumn";
            this.landlordDataGridViewTextBoxColumn.ReadOnly = true;
            this.landlordDataGridViewTextBoxColumn.Width = 142;
            // 
            // carrierDataGridViewTextBoxColumn
            // 
            this.carrierDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.carrierDataGridViewTextBoxColumn.DataPropertyName = "Carrier";
            this.carrierDataGridViewTextBoxColumn.HeaderText = "Carrier";
            this.carrierDataGridViewTextBoxColumn.Name = "carrierDataGridViewTextBoxColumn";
            this.carrierDataGridViewTextBoxColumn.ReadOnly = true;
            this.carrierDataGridViewTextBoxColumn.Width = 115;
            // 
            // fSANumberDataGridViewTextBoxColumn
            // 
            this.fSANumberDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.fSANumberDataGridViewTextBoxColumn.DataPropertyName = "FSA_Number";
            this.fSANumberDataGridViewTextBoxColumn.HeaderText = "Farm";
            this.fSANumberDataGridViewTextBoxColumn.Name = "fSANumberDataGridViewTextBoxColumn";
            this.fSANumberDataGridViewTextBoxColumn.ReadOnly = true;
            this.fSANumberDataGridViewTextBoxColumn.Width = 96;
            // 
            // vwOpenHarvestLoadsBindingSource
            // 
            this.vwOpenHarvestLoadsBindingSource.DataMember = "vw_Open_Harvest_Loads";
            this.vwOpenHarvestLoadsBindingSource.DataSource = this.nWDataset;
            // 
            // nWDataset
            // 
            this.nWDataset.DataSetName = "NWDataset";
            this.nWDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.SeaGreen;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(146, 69);
            this.button1.TabIndex = 4;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1300, 29);
            this.label1.TabIndex = 5;
            this.label1.Text = "Intake Loads Not Weighed Out";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // vw_Open_Harvest_LoadsTableAdapter
            // 
            this.vw_Open_Harvest_LoadsTableAdapter.ClearBeforeFill = true;
            // 
            // frmOpen_Harvest_Loads
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(1317, 643);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "frmOpen_Harvest_Loads";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.frmHarvest_Loads_Open_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vwOpenHarvestLoadsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private NWDataset nWDataset;
        private System.Windows.Forms.BindingSource vwOpenHarvestLoadsBindingSource;
        private NWDatasetTableAdapters.vw_Open_Harvest_LoadsTableAdapter vw_Open_Harvest_LoadsTableAdapter;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewLinkColumn btnEdit;
        private System.Windows.Forms.DataGridViewTextBoxColumn wSIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lotNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn timeInDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn weightInDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cropDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn producerDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn landlordDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn carrierDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fSANumberDataGridViewTextBoxColumn;
    }
}