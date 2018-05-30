namespace WarhammerCalendarManager
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
            this.loadCalendarButton = new System.Windows.Forms.Button();
            this.newCalendarButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.changelogPicture = new System.Windows.Forms.PictureBox();
            this.helpLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.changelogPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // titleText
            // 
            this.titleText.AutoSize = true;
            this.titleText.BackColor = System.Drawing.Color.Transparent;
            this.titleText.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleText.Location = new System.Drawing.Point(15, 29);
            this.titleText.Name = "titleText";
            this.titleText.Size = new System.Drawing.Size(240, 68);
            this.titleText.TabIndex = 2;
            this.titleText.Text = "   Warhammer\r\nCalendar Manager";
            this.titleText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.titleText.UseCompatibleTextRendering = true;
            // 
            // loadCalendarButton
            // 
            this.loadCalendarButton.Location = new System.Drawing.Point(169, 111);
            this.loadCalendarButton.Name = "loadCalendarButton";
            this.loadCalendarButton.Size = new System.Drawing.Size(86, 23);
            this.loadCalendarButton.TabIndex = 4;
            this.loadCalendarButton.Text = "Load Calendar";
            this.loadCalendarButton.UseVisualStyleBackColor = true;
            this.loadCalendarButton.Click += new System.EventHandler(this.LoadCalendarButton_Click);
            // 
            // newCalendarButton
            // 
            this.newCalendarButton.Location = new System.Drawing.Point(51, 111);
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
            this.label1.Location = new System.Drawing.Point(12, 163);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Version: 0.9.6";
            // 
            // changelogPicture
            // 
            this.changelogPicture.Image = global::WarhammerCalendarManager.Properties.Resources.unrollingscroll;
            this.changelogPicture.Location = new System.Drawing.Point(266, 150);
            this.changelogPicture.Name = "changelogPicture";
            this.changelogPicture.Size = new System.Drawing.Size(27, 27);
            this.changelogPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.changelogPicture.TabIndex = 7;
            this.changelogPicture.TabStop = false;
            this.changelogPicture.Click += new System.EventHandler(this.changelogPicture_Click);
            // 
            // helpLabel
            // 
            this.helpLabel.AutoSize = true;
            this.helpLabel.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helpLabel.Location = new System.Drawing.Point(271, 0);
            this.helpLabel.Name = "helpLabel";
            this.helpLabel.Size = new System.Drawing.Size(22, 26);
            this.helpLabel.TabIndex = 9;
            this.helpLabel.Text = "?";
            this.helpLabel.Click += new System.EventHandler(this.helpLabel_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 185);
            this.Controls.Add(this.helpLabel);
            this.Controls.Add(this.changelogPicture);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.newCalendarButton);
            this.Controls.Add(this.loadCalendarButton);
            this.Controls.Add(this.titleText);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainMenu";
            this.Text = "Main Menu";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainMenu_FormClosed);
            this.Load += new System.EventHandler(this.MainMenu_Load);
            ((System.ComponentModel.ISupportInitialize)(this.changelogPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleText;
        private System.Windows.Forms.Button loadCalendarButton;
        private System.Windows.Forms.Button newCalendarButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox changelogPicture;
        private System.Windows.Forms.Label helpLabel;
    }
}