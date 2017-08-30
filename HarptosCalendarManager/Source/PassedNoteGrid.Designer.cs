namespace HarptosCalendarManager
{
    partial class PassedNoteGrid
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
            this.noteGrid = new System.Windows.Forms.DataGridView();
            this.tagColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contentColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gotoColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.currentDateLabel = new System.Windows.Forms.Label();
            this.currentDate = new System.Windows.Forms.Label();
            this.goButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.noteGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // noteGrid
            // 
            this.noteGrid.AllowUserToAddRows = false;
            this.noteGrid.AllowUserToDeleteRows = false;
            this.noteGrid.AllowUserToResizeRows = false;
            this.noteGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.noteGrid.BackgroundColor = System.Drawing.SystemColors.Menu;
            this.noteGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.noteGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tagColumn,
            this.dateColumn,
            this.contentColumn,
            this.gotoColumn});
            this.noteGrid.Location = new System.Drawing.Point(0, 33);
            this.noteGrid.Name = "noteGrid";
            this.noteGrid.RowHeadersVisible = false;
            this.noteGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.noteGrid.Size = new System.Drawing.Size(484, 247);
            this.noteGrid.TabIndex = 0;
            this.noteGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.noteGrid_CellClick);
            // 
            // tagColumn
            // 
            this.tagColumn.FillWeight = 36.25907F;
            this.tagColumn.HeaderText = "Tag";
            this.tagColumn.Name = "tagColumn";
            this.tagColumn.ReadOnly = true;
            this.tagColumn.Width = 50;
            // 
            // dateColumn
            // 
            this.dateColumn.FillWeight = 61.29682F;
            this.dateColumn.HeaderText = "Date";
            this.dateColumn.Name = "dateColumn";
            this.dateColumn.ReadOnly = true;
            this.dateColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dateColumn.Width = 80;
            // 
            // contentColumn
            // 
            this.contentColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.contentColumn.FillWeight = 215.1345F;
            this.contentColumn.HeaderText = "Note Content";
            this.contentColumn.Name = "contentColumn";
            this.contentColumn.ReadOnly = true;
            // 
            // gotoColumn
            // 
            this.gotoColumn.FillWeight = 87.30965F;
            this.gotoColumn.HeaderText = "Go To";
            this.gotoColumn.Name = "gotoColumn";
            this.gotoColumn.ReadOnly = true;
            this.gotoColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.gotoColumn.Width = 43;
            // 
            // currentDateLabel
            // 
            this.currentDateLabel.AutoSize = true;
            this.currentDateLabel.Location = new System.Drawing.Point(12, 9);
            this.currentDateLabel.Name = "currentDateLabel";
            this.currentDateLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.currentDateLabel.Size = new System.Drawing.Size(70, 13);
            this.currentDateLabel.TabIndex = 1;
            this.currentDateLabel.Text = "Current Date:";
            // 
            // currentDate
            // 
            this.currentDate.AutoSize = true;
            this.currentDate.Location = new System.Drawing.Point(88, 9);
            this.currentDate.Name = "currentDate";
            this.currentDate.Size = new System.Drawing.Size(64, 13);
            this.currentDate.TabIndex = 2;
            this.currentDate.Text = "current date";
            // 
            // goButton
            // 
            this.goButton.Location = new System.Drawing.Point(158, 4);
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(60, 23);
            this.goButton.TabIndex = 3;
            this.goButton.Text = "Go Back";
            this.goButton.UseVisualStyleBackColor = true;
            this.goButton.Click += new System.EventHandler(this.goButton_Click);
            // 
            // PassedNoteGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(484, 280);
            this.Controls.Add(this.goButton);
            this.Controls.Add(this.currentDate);
            this.Controls.Add(this.currentDateLabel);
            this.Controls.Add(this.noteGrid);
            this.Name = "PassedNoteGrid";
            this.ShowIcon = false;
            this.Text = "Passed Notes";
            ((System.ComponentModel.ISupportInitialize)(this.noteGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView noteGrid;
        private System.Windows.Forms.Label currentDateLabel;
        private System.Windows.Forms.Label currentDate;
        private System.Windows.Forms.Button goButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn tagColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn contentColumn;
        private System.Windows.Forms.DataGridViewButtonColumn gotoColumn;
    }
}