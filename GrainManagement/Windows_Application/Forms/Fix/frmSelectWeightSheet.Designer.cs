namespace NWGrain.Forms.Fix
{
    partial class frmSelectWeightSheet
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lnkWSID = new System.Windows.Forms.DataGridViewLinkColumn();
            this.Total_Loads = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Completed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Not_Completed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Lot_Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Crop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Landlord = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weightSheetsForSelectionBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.nWDataset = new NWGrain.NWDataset();
            this.weightSheetsForSelectionTableAdapter = new NWGrain.NWDatasetTableAdapters.WeightSheetsForSelectionTableAdapter();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.weightSheetsForSelectionBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.LightYellow;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoGenerateColumns = false;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Arial Narrow", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.lnkWSID,
            this.Total_Loads,
            this.Completed,
            this.Not_Completed,
            this.Lot_Number,
            this.Crop,
            this.Landlord});
            this.dataGridView1.DataSource = this.weightSheetsForSelectionBindingSource;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Arial Narrow", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(245)))), ((int)(((byte)(39)))));
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle12;
            this.dataGridView1.Location = new System.Drawing.Point(12, 70);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridView1.RowTemplate.Height = 50;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1207, 562);
            this.dataGridView1.TabIndex = 17;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // lnkWSID
            // 
            this.lnkWSID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.lnkWSID.DataPropertyName = "WS_Id";
            this.lnkWSID.HeaderText = "Weight Sheet";
            this.lnkWSID.Name = "lnkWSID";
            this.lnkWSID.ReadOnly = true;
            this.lnkWSID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.lnkWSID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.lnkWSID.Width = 163;
            // 
            // Total_Loads
            // 
            this.Total_Loads.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Total_Loads.DataPropertyName = "Total_Loads";
            this.Total_Loads.HeaderText = "Total Loads";
            this.Total_Loads.Name = "Total_Loads";
            this.Total_Loads.ReadOnly = true;
            this.Total_Loads.Width = 148;
            // 
            // Completed
            // 
            this.Completed.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Completed.DataPropertyName = "Completed";
            this.Completed.HeaderText = "Completed";
            this.Completed.Name = "Completed";
            this.Completed.ReadOnly = true;
            this.Completed.Width = 140;
            // 
            // Not_Completed
            // 
            this.Not_Completed.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Not_Completed.DataPropertyName = "Not_Completed";
            this.Not_Completed.HeaderText = "Not Completed";
            this.Not_Completed.Name = "Not_Completed";
            this.Not_Completed.ReadOnly = true;
            this.Not_Completed.Width = 178;
            // 
            // Lot_Number
            // 
            this.Lot_Number.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Lot_Number.DataPropertyName = "Lot_Number";
            this.Lot_Number.HeaderText = "Lot";
            this.Lot_Number.Name = "Lot_Number";
            this.Lot_Number.ReadOnly = true;
            this.Lot_Number.Width = 69;
            // 
            // Crop
            // 
            this.Crop.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Crop.DataPropertyName = "Crop";
            this.Crop.HeaderText = "Crop";
            this.Crop.Name = "Crop";
            this.Crop.ReadOnly = true;
            this.Crop.Width = 84;
            // 
            // Landlord
            // 
            this.Landlord.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Landlord.DataPropertyName = "Landlord";
            this.Landlord.HeaderText = "Landlord";
            this.Landlord.Name = "Landlord";
            this.Landlord.ReadOnly = true;
            this.Landlord.Width = 122;
            // 
            // weightSheetsForSelectionBindingSource
            // 
            this.weightSheetsForSelectionBindingSource.DataMember = "WeightSheetsForSelection";
            this.weightSheetsForSelectionBindingSource.DataSource = this.nWDataset;
            this.weightSheetsForSelectionBindingSource.Filter = "";
            // 
            // nWDataset
            // 
            this.nWDataset.DataSetName = "NWDataset";
            this.nWDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // weightSheetsForSelectionTableAdapter
            // 
            this.weightSheetsForSelectionTableAdapter.ClearBeforeFill = true;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Red;
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(12, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(141, 61);
            this.btnCancel.TabIndex = 35;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1236, 64);
            this.label1.TabIndex = 36;
            this.label1.Text = "label1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmSelectWeightSheet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1236, 644);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial Narrow", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.Name = "frmSelectWeightSheet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Move Weight Sheet";
            this.Resize += new System.EventHandler(this.frmSelectWeightSheet_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.weightSheetsForSelectionBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource weightSheetsForSelectionBindingSource;
        private NWDataset nWDataset;
        private NWDatasetTableAdapters.WeightSheetsForSelectionTableAdapter weightSheetsForSelectionTableAdapter;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewLinkColumn lnkWSID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Total_Loads;
        private System.Windows.Forms.DataGridViewTextBoxColumn Completed;
        private System.Windows.Forms.DataGridViewTextBoxColumn Not_Completed;
        private System.Windows.Forms.DataGridViewTextBoxColumn Lot_Number;
        private System.Windows.Forms.DataGridViewTextBoxColumn Crop;
        private System.Windows.Forms.DataGridViewTextBoxColumn Landlord;
    }
}