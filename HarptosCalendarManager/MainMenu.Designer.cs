namespace HarptosCalendarManager
{
    partial class MainMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenu));
            this.titleText = new System.Windows.Forms.Label();
            this.howToUseButton = new System.Windows.Forms.Button();
            this.loadCalendarButton = new System.Windows.Forms.Button();
            this.newCalendarButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // titleText
            // 
            this.titleText.AutoSize = true;
            this.titleText.BackColor = System.Drawing.Color.Transparent;
            this.titleText.Font = new System.Drawing.Font("Ozymandias Solid WBW", 22F);
            this.titleText.Location = new System.Drawing.Point(26, 8);
            this.titleText.Name = "titleText";
            this.titleText.Size = new System.Drawing.Size(223, 114);
            this.titleText.TabIndex = 2;
            this.titleText.Text = "The Calendar \r\nof \r\nHarptos";
            this.titleText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // howToUseButton
            // 
            this.howToUseButton.Location = new System.Drawing.Point(93, 181);
            this.howToUseButton.Name = "howToUseButton";
            this.howToUseButton.Size = new System.Drawing.Size(86, 23);
            this.howToUseButton.TabIndex = 3;
            this.howToUseButton.Text = "How To Use";
            this.howToUseButton.UseVisualStyleBackColor = true;
            this.howToUseButton.Click += new System.EventHandler(this.howToUseButton_Click);
            // 
            // loadCalendarButton
            // 
            this.loadCalendarButton.Location = new System.Drawing.Point(147, 125);
            this.loadCalendarButton.Name = "loadCalendarButton";
            this.loadCalendarButton.Size = new System.Drawing.Size(86, 23);
            this.loadCalendarButton.TabIndex = 4;
            this.loadCalendarButton.Text = "Load Calendar";
            this.loadCalendarButton.UseVisualStyleBackColor = true;
            this.loadCalendarButton.Click += new System.EventHandler(this.loadCalendarButton_Click);
            // 
            // newCalendarButton
            // 
            this.newCalendarButton.Location = new System.Drawing.Point(33, 125);
            this.newCalendarButton.Name = "newCalendarButton";
            this.newCalendarButton.Size = new System.Drawing.Size(86, 23);
            this.newCalendarButton.TabIndex = 5;
            this.newCalendarButton.Text = "New Calendar";
            this.newCalendarButton.UseVisualStyleBackColor = true;
            this.newCalendarButton.Click += new System.EventHandler(this.newCalendarButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 231);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(205, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Version: 0.22 (Apprentice\'s Advancement)";
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(273, 253);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.newCalendarButton);
            this.Controls.Add(this.loadCalendarButton);
            this.Controls.Add(this.howToUseButton);
            this.Controls.Add(this.titleText);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainMenu";
            this.Text = "Harptos Calendar";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainMenu_FormClosed);
            this.Load += new System.EventHandler(this.MainMenu_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleText;
        private System.Windows.Forms.Button howToUseButton;
        private System.Windows.Forms.Button loadCalendarButton;
        private System.Windows.Forms.Button newCalendarButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label1;
    }
}