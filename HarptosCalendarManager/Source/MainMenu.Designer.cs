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
            this.changelogPicture = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.changelogPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // titleText
            // 
            this.titleText.AutoSize = true;
            this.titleText.BackColor = System.Drawing.Color.Transparent;
            this.titleText.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleText.Location = new System.Drawing.Point(-7, 7);
            this.titleText.Name = "titleText";
            this.titleText.Size = new System.Drawing.Size(247, 106);
            this.titleText.TabIndex = 2;
            this.titleText.Text = "    The Calendar \r\n    of \r\nHarptos";
            this.titleText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.titleText.UseCompatibleTextRendering = true;
            // 
            // howToUseButton
            // 
            this.howToUseButton.Location = new System.Drawing.Point(83, 176);
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
            this.loadCalendarButton.Click += new System.EventHandler(this.LoadCalendarButton_Click);
            // 
            // newCalendarButton
            // 
            this.newCalendarButton.Location = new System.Drawing.Point(23, 125);
            this.newCalendarButton.Name = "newCalendarButton";
            this.newCalendarButton.Size = new System.Drawing.Size(86, 23);
            this.newCalendarButton.TabIndex = 5;
            this.newCalendarButton.Text = "New Calendar";
            this.newCalendarButton.UseVisualStyleBackColor = true;
            this.newCalendarButton.Click += new System.EventHandler(this.NewCalendarButton_Click);
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
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Version: 0.9.3";
            // 
            // changelogPicture
            // 
            this.changelogPicture.Image = global::HarptosCalendarManager.Properties.Resources.unrollingscroll;
            this.changelogPicture.Location = new System.Drawing.Point(221, 221);
            this.changelogPicture.Name = "changelogPicture";
            this.changelogPicture.Size = new System.Drawing.Size(20, 20);
            this.changelogPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.changelogPicture.TabIndex = 7;
            this.changelogPicture.TabStop = false;
            this.changelogPicture.Click += new System.EventHandler(this.changelogPicture_Click);
            this.changelogPicture.MouseUp += new System.Windows.Forms.MouseEventHandler(this.changelogPicture_MouseUp);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 253);
            this.Controls.Add(this.changelogPicture);
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
            ((System.ComponentModel.ISupportInitialize)(this.changelogPicture)).EndInit();
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
        private System.Windows.Forms.PictureBox changelogPicture;
    }
}