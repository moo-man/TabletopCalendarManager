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
            ((System.ComponentModel.ISupportInitialize)(this.noteGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // noteGrid
            // 
            this.noteGrid.AllowUserToAddRows = false;
            this.noteGrid.AllowUserToDeleteRows = false;
            this.noteGrid.BackgroundColor = System.Drawing.SystemColors.Menu;
            this.noteGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.noteGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tagColumn,
            this.dateColumn,
            this.contentColumn,
            this.gotoColumn});
            this.noteGrid.Location = new System.Drawing.Point(12, 12);
            this.noteGrid.Name = "noteGrid";
            this.noteGrid.RowHeadersVisible = false;
            this.noteGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.noteGrid.Size = new System.Drawing.Size(457, 237);
            this.noteGrid.TabIndex = 0;
            // 
            // tagColumn
            // 
            this.tagColumn.HeaderText = "Tag";
            this.tagColumn.Name = "tagColumn";
            this.tagColumn.ReadOnly = true;
            this.tagColumn.Width = 50;
            // 
            // dateColumn
            // 
            this.dateColumn.HeaderText = "Date";
            this.dateColumn.Name = "dateColumn";
            this.dateColumn.ReadOnly = true;
            this.dateColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dateColumn.Width = 80;
            // 
            // contentColumn
            // 
            this.contentColumn.HeaderText = "Note Content";
            this.contentColumn.Name = "contentColumn";
            this.contentColumn.ReadOnly = true;
            this.contentColumn.Width = 281;
            // 
            // gotoColumn
            // 
            this.gotoColumn.HeaderText = "Go To";
            this.gotoColumn.Name = "gotoColumn";
            this.gotoColumn.ReadOnly = true;
            this.gotoColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.gotoColumn.Width = 43;
            // 
            // PassedNoteGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 261);
            this.Controls.Add(this.noteGrid);
            this.Name = "PassedNoteGrid";
            this.ShowIcon = false;
            this.Text = "Passed Notes";
            ((System.ComponentModel.ISupportInitialize)(this.noteGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView noteGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn tagColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn contentColumn;
        private System.Windows.Forms.DataGridViewButtonColumn gotoColumn;
    }
}