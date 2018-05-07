namespace CalendarManager
{
    partial class ImportCalendarDialog
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
            this.textBox = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createCalendarButton = new System.Windows.Forms.Button();
            this.loadJSONbutton = new System.Windows.Forms.Button();
            this.calendarNameBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.helpLabel = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.ContextMenuStrip = this.contextMenuStrip1;
            this.textBox.Location = new System.Drawing.Point(12, 37);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(265, 258);
            this.textBox.TabIndex = 2;
            this.textBox.Text = "Paste calendar JSON data here.";
            this.textBox.Click += new System.EventHandler(this.textBox_Click);
            this.textBox.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.textBox.Enter += new System.EventHandler(this.textBox_Click);
            this.textBox.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pasteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(103, 26);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // createCalendarButton
            // 
            this.createCalendarButton.Location = new System.Drawing.Point(12, 301);
            this.createCalendarButton.Name = "createCalendarButton";
            this.createCalendarButton.Size = new System.Drawing.Size(100, 23);
            this.createCalendarButton.TabIndex = 1;
            this.createCalendarButton.Text = "Create Calendar";
            this.createCalendarButton.UseVisualStyleBackColor = true;
            this.createCalendarButton.Click += new System.EventHandler(this.createCalendarButton_Click);
            // 
            // loadJSONbutton
            // 
            this.loadJSONbutton.Location = new System.Drawing.Point(183, 301);
            this.loadJSONbutton.Name = "loadJSONbutton";
            this.loadJSONbutton.Size = new System.Drawing.Size(87, 23);
            this.loadJSONbutton.TabIndex = 3;
            this.loadJSONbutton.Text = "Load JSON file";
            this.loadJSONbutton.UseVisualStyleBackColor = true;
            this.loadJSONbutton.Click += new System.EventHandler(this.loadJSONbutton_Click);
            // 
            // calendarNameBox
            // 
            this.calendarNameBox.Location = new System.Drawing.Point(101, 11);
            this.calendarNameBox.Name = "calendarNameBox";
            this.calendarNameBox.Size = new System.Drawing.Size(151, 20);
            this.calendarNameBox.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "The Calendar of";
            // 
            // helpLabel
            // 
            this.helpLabel.AutoSize = true;
            this.helpLabel.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helpLabel.Location = new System.Drawing.Point(258, 5);
            this.helpLabel.Name = "helpLabel";
            this.helpLabel.Size = new System.Drawing.Size(22, 26);
            this.helpLabel.TabIndex = 10;
            this.helpLabel.Text = "?";
            this.helpLabel.Click += new System.EventHandler(this.helpLabel_Click);
            // 
            // ImportCalendarDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 336);
            this.Controls.Add(this.helpLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.calendarNameBox);
            this.Controls.Add(this.loadJSONbutton);
            this.Controls.Add(this.createCalendarButton);
            this.Controls.Add(this.textBox);
            this.Name = "ImportCalendarDialog";
            this.ShowIcon = false;
            this.Text = "Import Calendar";
            this.Load += new System.EventHandler(this.ImportCalendarDialog_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox textBox;
        private System.Windows.Forms.Button createCalendarButton;
        private System.Windows.Forms.Button loadJSONbutton;
        private System.Windows.Forms.TextBox calendarNameBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.Label helpLabel;
    }
}