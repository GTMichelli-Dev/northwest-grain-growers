namespace NWGrain
{
    partial class frmPast_Due_Loads
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
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnSelectWeightSheet = new System.Windows.Forms.DataGridViewLinkColumn();
            this.Load_Type_Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSelectTicket = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.truckIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeInDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.loadTypeDescriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Crop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weightInDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.openWeightSheetLoadsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.nWDataset = new NWGrain.NWDataset();
            this.open_Weight_Sheet_LoadsTableAdapter = new NWGrain.NWDatasetTableAdapters.Open_Weight_Sheet_LoadsTableAdapter();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.openWeightSheetLoadsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Arial", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1417, 37);
            this.label1.TabIndex = 18;
            this.label1.Text = "** These Loads Must Be Closed Moved Or Deleted  Before Continuing **";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
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
            this.btnSelectWeightSheet,
            this.Load_Type_Description,
            this.btnSelectTicket,
            this.truckIdDataGridViewTextBoxColumn,
            this.timeInDataGridViewTextBoxColumn,
            this.loadTypeDescriptionDataGridViewTextBoxColumn,
            this.Crop,
            this.weightInDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.openWeightSheetLoadsBindingSource;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(245)))), ((int)(((byte)(39)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridView1.Location = new System.Drawing.Point(26, 40);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridView1.RowTemplate.Height = 50;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1364, 779);
            this.dataGridView1.TabIndex = 20;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // btnSelectWeightSheet
            // 
            this.btnSelectWeightSheet.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.btnSelectWeightSheet.DataPropertyName = "Weight_Sheet_Id";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.btnSelectWeightSheet.DefaultCellStyle = dataGridViewCellStyle3;
            this.btnSelectWeightSheet.HeaderText = "Weight Sheet";
            this.btnSelectWeightSheet.Name = "btnSelectWeightSheet";
            this.btnSelectWeightSheet.ReadOnly = true;
            this.btnSelectWeightSheet.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnSelectWeightSheet.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.btnSelectWeightSheet.Width = 191;
            // 
            // Load_Type_Description
            // 
            this.Load_Type_Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Load_Type_Description.DataPropertyName = "Load_Type_Description";
            this.Load_Type_Description.HeaderText = "Type";
            this.Load_Type_Description.Name = "Load_Type_Description";
            this.Load_Type_Description.ReadOnly = true;
            this.Load_Type_Description.Width = 92;
            // 
            // btnSelectTicket
            // 
            this.btnSelectTicket.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.btnSelectTicket.DataPropertyName = "Load_Id";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.btnSelectTicket.DefaultCellStyle = dataGridViewCellStyle4;
            this.btnSelectTicket.HeaderText = "Ticket";
            this.btnSelectTicket.Name = "btnSelectTicket";
            this.btnSelectTicket.ReadOnly = true;
            this.btnSelectTicket.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnSelectTicket.Width = 107;
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
            dataGridViewCellStyle5.Format = "g";
            dataGridViewCellStyle5.NullValue = null;
            this.timeInDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.timeInDataGridViewTextBoxColumn.HeaderText = "Time In";
            this.timeInDataGridViewTextBoxColumn.Name = "timeInDataGridViewTextBoxColumn";
            this.timeInDataGridViewTextBoxColumn.ReadOnly = true;
            this.timeInDataGridViewTextBoxColumn.Width = 123;
            // 
            // loadTypeDescriptionDataGridViewTextBoxColumn
            // 
            this.loadTypeDescriptionDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.loadTypeDescriptionDataGridViewTextBoxColumn.DataPropertyName = "Load_Type_Description";
            this.loadTypeDescriptionDataGridViewTextBoxColumn.HeaderText = "Type";
            this.loadTypeDescriptionDataGridViewTextBoxColumn.Name = "loadTypeDescriptionDataGridViewTextBoxColumn";
            this.loadTypeDescriptionDataGridViewTextBoxColumn.ReadOnly = true;
            this.loadTypeDescriptionDataGridViewTextBoxColumn.Width = 92;
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
            // weightInDataGridViewTextBoxColumn
            // 
            this.weightInDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.weightInDataGridViewTextBoxColumn.DataPropertyName = "Weight_In";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N0";
            dataGridViewCellStyle6.NullValue = null;
            this.weightInDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.weightInDataGridViewTextBoxColumn.HeaderText = "Weight In";
            this.weightInDataGridViewTextBoxColumn.Name = "weightInDataGridViewTextBoxColumn";
            this.weightInDataGridViewTextBoxColumn.ReadOnly = true;
            this.weightInDataGridViewTextBoxColumn.Width = 148;
            // 
            // openWeightSheetLoadsBindingSource
            // 
            this.openWeightSheetLoadsBindingSource.DataMember = "Open_Weight_Sheet_Loads";
            this.openWeightSheetLoadsBindingSource.DataSource = this.nWDataset;
            // 
            // nWDataset
            // 
            this.nWDataset.DataSetName = "NWDataset";
            this.nWDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // open_Weight_Sheet_LoadsTableAdapter
            // 
            this.open_Weight_Sheet_LoadsTableAdapter.ClearBeforeFill = true;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Red;
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(6, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(175, 37);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmPast_Due_Loads
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1417, 831);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "frmPast_Due_Loads";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Past Due Loads";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPast_Due_Loads_FormClosing);
            this.Load += new System.EventHandler(this.froPast_Due_Loads_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.openWeightSheetLoadsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource openWeightSheetLoadsBindingSource;
        private NWDataset nWDataset;
        private NWDatasetTableAdapters.Open_Weight_Sheet_LoadsTableAdapter open_Weight_Sheet_LoadsTableAdapter;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridViewLinkColumn btnSelectWeightSheet;
        private System.Windows.Forms.DataGridViewTextBoxColumn Load_Type_Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn btnSelectTicket;
        private System.Windows.Forms.DataGridViewTextBoxColumn truckIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn timeInDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn loadTypeDescriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Crop;
        private System.Windows.Forms.DataGridViewTextBoxColumn weightInDataGridViewTextBoxColumn;
    }
}