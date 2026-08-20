namespace NWGrain
{
    partial class TodaysLoads
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.todaysLoadsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.nWDataset = new NWGrain.NWDataset();
            this.todaysLoadsTableAdapter = new NWGrain.NWDatasetTableAdapters.TodaysLoadsTableAdapter();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDone = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.WS_Id = new System.Windows.Forms.DataGridViewLinkColumn();
            this.LoadType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.loadIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Inyard = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Crop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time_In = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.truckIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bolDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Net = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.binDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Protien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.commentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.todaysLoadsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
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
            this.WS_Id,
            this.LoadType,
            this.loadIdDataGridViewTextBoxColumn,
            this.Inyard,
            this.Crop,
            this.Time_In,
            this.truckIdDataGridViewTextBoxColumn,
            this.bolDataGridViewTextBoxColumn,
            this.Net,
            this.binDataGridViewTextBoxColumn,
            this.Protien,
            this.commentDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.todaysLoadsBindingSource;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(245)))), ((int)(((byte)(39)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridView1.Location = new System.Drawing.Point(21, 129);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 60;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1403, 786);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // todaysLoadsBindingSource
            // 
            this.todaysLoadsBindingSource.DataMember = "TodaysLoads";
            this.todaysLoadsBindingSource.DataSource = this.nWDataset;
            // 
            // nWDataset
            // 
            this.nWDataset.DataSetName = "NWDataset";
            this.nWDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // todaysLoadsTableAdapter
            // 
            this.todaysLoadsTableAdapter.ClearBeforeFill = true;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Arial", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1445, 109);
            this.label1.TabIndex = 1;
            this.label1.Text = "Todays Loads";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnDone
            // 
            this.btnDone.BackColor = System.Drawing.Color.SeaGreen;
            this.btnDone.ForeColor = System.Drawing.Color.White;
            this.btnDone.Location = new System.Drawing.Point(21, 40);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(146, 69);
            this.btnDone.TabIndex = 4;
            this.btnDone.Text = "Ok";
            this.btnDone.UseVisualStyleBackColor = false;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnDone);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1445, 121);
            this.panel1.TabIndex = 5;
            // 
            // timer1
            // 
            this.timer1.Interval = 20;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // WS_Id
            // 
            this.WS_Id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.WS_Id.DataPropertyName = "WS_Id";
            this.WS_Id.HeaderText = "Weight Sheet";
            this.WS_Id.Name = "WS_Id";
            this.WS_Id.ReadOnly = true;
            this.WS_Id.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.WS_Id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.WS_Id.Width = 191;
            // 
            // LoadType
            // 
            this.LoadType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.LoadType.DataPropertyName = "LoadType";
            this.LoadType.HeaderText = "";
            this.LoadType.Name = "LoadType";
            this.LoadType.ReadOnly = true;
            this.LoadType.Width = 19;
            // 
            // loadIdDataGridViewTextBoxColumn
            // 
            this.loadIdDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.loadIdDataGridViewTextBoxColumn.DataPropertyName = "Load_Id";
            this.loadIdDataGridViewTextBoxColumn.HeaderText = "Load";
            this.loadIdDataGridViewTextBoxColumn.Name = "loadIdDataGridViewTextBoxColumn";
            this.loadIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.loadIdDataGridViewTextBoxColumn.Width = 96;
            // 
            // Inyard
            // 
            this.Inyard.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Inyard.DataPropertyName = "Inyard";
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Red;
            this.Inyard.DefaultCellStyle = dataGridViewCellStyle2;
            this.Inyard.HeaderText = "";
            this.Inyard.Name = "Inyard";
            this.Inyard.ReadOnly = true;
            this.Inyard.Width = 19;
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
            // Time_In
            // 
            this.Time_In.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Time_In.DataPropertyName = "Time_In";
            dataGridViewCellStyle3.Format = "t";
            this.Time_In.DefaultCellStyle = dataGridViewCellStyle3;
            this.Time_In.HeaderText = "Time In";
            this.Time_In.Name = "Time_In";
            this.Time_In.ReadOnly = true;
            this.Time_In.Width = 123;
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
            // bolDataGridViewTextBoxColumn
            // 
            this.bolDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.bolDataGridViewTextBoxColumn.DataPropertyName = "Bol";
            this.bolDataGridViewTextBoxColumn.HeaderText = "Bol";
            this.bolDataGridViewTextBoxColumn.Name = "bolDataGridViewTextBoxColumn";
            this.bolDataGridViewTextBoxColumn.ReadOnly = true;
            this.bolDataGridViewTextBoxColumn.Width = 77;
            // 
            // Net
            // 
            this.Net.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Net.DataPropertyName = "Net";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N0";
            dataGridViewCellStyle4.NullValue = null;
            this.Net.DefaultCellStyle = dataGridViewCellStyle4;
            this.Net.HeaderText = "Net";
            this.Net.Name = "Net";
            this.Net.ReadOnly = true;
            this.Net.Width = 76;
            // 
            // binDataGridViewTextBoxColumn
            // 
            this.binDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.binDataGridViewTextBoxColumn.DataPropertyName = "Bin";
            this.binDataGridViewTextBoxColumn.HeaderText = "Bin";
            this.binDataGridViewTextBoxColumn.Name = "binDataGridViewTextBoxColumn";
            this.binDataGridViewTextBoxColumn.ReadOnly = true;
            this.binDataGridViewTextBoxColumn.Width = 77;
            // 
            // Protien
            // 
            this.Protien.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Protien.DataPropertyName = "Protien";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N2";
            dataGridViewCellStyle5.NullValue = null;
            this.Protien.DefaultCellStyle = dataGridViewCellStyle5;
            this.Protien.HeaderText = "Protein";
            this.Protien.Name = "Protien";
            this.Protien.ReadOnly = true;
            this.Protien.Width = 121;
            // 
            // commentDataGridViewTextBoxColumn
            // 
            this.commentDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.commentDataGridViewTextBoxColumn.DataPropertyName = "Comment";
            this.commentDataGridViewTextBoxColumn.HeaderText = "Comment";
            this.commentDataGridViewTextBoxColumn.Name = "commentDataGridViewTextBoxColumn";
            this.commentDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // TodaysLoads
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1445, 929);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "TodaysLoads";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Todays Loads (This was Mark Schreindl Idea )";
            this.Activated += new System.EventHandler(this.TodaysLoads_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.todaysLoadsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource todaysLoadsBindingSource;
        private NWDataset nWDataset;
        private NWDatasetTableAdapters.TodaysLoadsTableAdapter todaysLoadsTableAdapter;
        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataGridViewLinkColumn WS_Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn LoadType;
        private System.Windows.Forms.DataGridViewTextBoxColumn loadIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Inyard;
        private System.Windows.Forms.DataGridViewTextBoxColumn Crop;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time_In;
        private System.Windows.Forms.DataGridViewTextBoxColumn truckIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bolDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Net;
        private System.Windows.Forms.DataGridViewTextBoxColumn binDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Protien;
        private System.Windows.Forms.DataGridViewTextBoxColumn commentDataGridViewTextBoxColumn;
    }
}