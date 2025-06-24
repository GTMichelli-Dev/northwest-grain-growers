namespace RemotePrinting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.grpDistrict = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.dgvResults = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsProgress = new System.Windows.Forms.ToolStripStatusLabel();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.btnMarkAsOriginal = new System.Windows.Forms.Button();
            this.btnPrintSelected = new System.Windows.Forms.Button();
            this.btnSelectNone = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnRemoveToday = new System.Windows.Forms.Button();
            this.CkSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.WeightSheetId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WSType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocationId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocationDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreationDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpDistrict.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.richTextBox1.Location = new System.Drawing.Point(0, 325);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(561, 110);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // grpDistrict
            // 
            this.grpDistrict.Controls.Add(this.button5);
            this.grpDistrict.Controls.Add(this.button4);
            this.grpDistrict.Controls.Add(this.button3);
            this.grpDistrict.Controls.Add(this.button2);
            this.grpDistrict.Controls.Add(this.button1);
            this.grpDistrict.Location = new System.Drawing.Point(9, 13);
            this.grpDistrict.Name = "grpDistrict";
            this.grpDistrict.Size = new System.Drawing.Size(535, 70);
            this.grpDistrict.TabIndex = 12;
            this.grpDistrict.TabStop = false;
            this.grpDistrict.Text = "District";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(424, 24);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 16;
            this.button5.Text = "West";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.serverButton_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(327, 24);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 15;
            this.button4.Text = "South";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.serverButton_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(230, 24);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 14;
            this.button3.Text = "North";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.serverButton_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(133, 24);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "East";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.serverButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(36, 24);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "Central";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.serverButton_Click);
            // 
            // dgvResults
            // 
            this.dgvResults.AllowUserToAddRows = false;
            this.dgvResults.AllowUserToDeleteRows = false;
            this.dgvResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CkSelect,
            this.WeightSheetId,
            this.WSType,
            this.LocationId,
            this.LocationDescription,
            this.CreationDate,
            this.ServerName});
            this.dgvResults.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvResults.Location = new System.Drawing.Point(0, 169);
            this.dgvResults.MultiSelect = false;
            this.dgvResults.Name = "dgvResults";
            this.dgvResults.RowHeadersVisible = false;
            this.dgvResults.RowHeadersWidth = 82;
            this.dgvResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvResults.Size = new System.Drawing.Size(561, 156);
            this.dgvResults.TabIndex = 13;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsProgress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 435);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(561, 22);
            this.statusStrip1.TabIndex = 14;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsProgress
            // 
            this.tsProgress.Name = "tsProgress";
            this.tsProgress.Size = new System.Drawing.Size(121, 17);
            this.tsProgress.Text = "Select District To Start";
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.btnMarkAsOriginal);
            this.grpOptions.Controls.Add(this.btnPrintSelected);
            this.grpOptions.Controls.Add(this.btnSelectNone);
            this.grpOptions.Controls.Add(this.btnSelectAll);
            this.grpOptions.Controls.Add(this.btnRemoveToday);
            this.grpOptions.Enabled = false;
            this.grpOptions.Location = new System.Drawing.Point(9, 90);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(535, 70);
            this.grpOptions.TabIndex = 15;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // btnMarkAsOriginal
            // 
            this.btnMarkAsOriginal.Enabled = false;
            this.btnMarkAsOriginal.Location = new System.Drawing.Point(36, 44);
            this.btnMarkAsOriginal.Name = "btnMarkAsOriginal";
            this.btnMarkAsOriginal.Size = new System.Drawing.Size(161, 23);
            this.btnMarkAsOriginal.TabIndex = 4;
            this.btnMarkAsOriginal.Text = "Mark Selected as Original";
            this.btnMarkAsOriginal.UseVisualStyleBackColor = true;
            this.btnMarkAsOriginal.Click += new System.EventHandler(this.btnMarkAsOriginal_Click);
            // 
            // btnPrintSelected
            // 
            this.btnPrintSelected.Location = new System.Drawing.Point(36, 16);
            this.btnPrintSelected.Name = "btnPrintSelected";
            this.btnPrintSelected.Size = new System.Drawing.Size(161, 23);
            this.btnPrintSelected.TabIndex = 3;
            this.btnPrintSelected.Text = "Print Selected";
            this.btnPrintSelected.UseVisualStyleBackColor = true;
            this.btnPrintSelected.Click += new System.EventHandler(this.btnPrintSelected_Click);
            // 
            // btnSelectNone
            // 
            this.btnSelectNone.Location = new System.Drawing.Point(244, 16);
            this.btnSelectNone.Name = "btnSelectNone";
            this.btnSelectNone.Size = new System.Drawing.Size(91, 23);
            this.btnSelectNone.TabIndex = 2;
            this.btnSelectNone.Text = "Select None";
            this.btnSelectNone.UseVisualStyleBackColor = true;
            this.btnSelectNone.Click += new System.EventHandler(this.btnSelectNone_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(244, 44);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(91, 23);
            this.btnSelectAll.TabIndex = 1;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnRemoveToday
            // 
            this.btnRemoveToday.Location = new System.Drawing.Point(347, 24);
            this.btnRemoveToday.Name = "btnRemoveToday";
            this.btnRemoveToday.Size = new System.Drawing.Size(126, 23);
            this.btnRemoveToday.TabIndex = 0;
            this.btnRemoveToday.Text = "Remove Todays Date";
            this.btnRemoveToday.UseVisualStyleBackColor = true;
            this.btnRemoveToday.Click += new System.EventHandler(this.btnRemoveToday_Click);
            // 
            // CkSelect
            // 
            this.CkSelect.HeaderText = "";
            this.CkSelect.MinimumWidth = 10;
            this.CkSelect.Name = "CkSelect";
            this.CkSelect.Width = 10;
            // 
            // WeightSheetId
            // 
            this.WeightSheetId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.WeightSheetId.HeaderText = "Weight Sheet";
            this.WeightSheetId.MinimumWidth = 10;
            this.WeightSheetId.Name = "WeightSheetId";
            this.WeightSheetId.ReadOnly = true;
            this.WeightSheetId.Width = 97;
            // 
            // WSType
            // 
            this.WSType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.WSType.HeaderText = "Type";
            this.WSType.Name = "WSType";
            this.WSType.ReadOnly = true;
            this.WSType.Width = 56;
            // 
            // LocationId
            // 
            this.LocationId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.LocationId.HeaderText = "Location Id";
            this.LocationId.MinimumWidth = 100;
            this.LocationId.Name = "LocationId";
            this.LocationId.ReadOnly = true;
            // 
            // LocationDescription
            // 
            this.LocationDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.LocationDescription.HeaderText = "Location";
            this.LocationDescription.MinimumWidth = 10;
            this.LocationDescription.Name = "LocationDescription";
            this.LocationDescription.ReadOnly = true;
            this.LocationDescription.Width = 73;
            // 
            // CreationDate
            // 
            this.CreationDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.CreationDate.HeaderText = "Created";
            this.CreationDate.MinimumWidth = 10;
            this.CreationDate.Name = "CreationDate";
            this.CreationDate.ReadOnly = true;
            this.CreationDate.Width = 69;
            // 
            // ServerName
            // 
            this.ServerName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ServerName.HeaderText = "Server";
            this.ServerName.MinimumWidth = 10;
            this.ServerName.Name = "ServerName";
            this.ServerName.ReadOnly = true;
            this.ServerName.Width = 63;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 457);
            this.Controls.Add(this.grpOptions);
            this.Controls.Add(this.dgvResults);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.grpDistrict);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(577, 496);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Remote Printing";
            this.grpDistrict.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.grpOptions.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.GroupBox grpDistrict;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dgvResults;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsProgress;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SelectRow;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.Button btnPrintSelected;
        private System.Windows.Forms.Button btnSelectNone;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnRemoveToday;
        private System.Windows.Forms.Button btnMarkAsOriginal;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CkSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn WeightSheetId;
        private System.Windows.Forms.DataGridViewTextBoxColumn WSType;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocationId;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocationDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreationDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServerName;
    }
}

