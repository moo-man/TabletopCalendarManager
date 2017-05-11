namespace HarptosCalendarManager
{
    partial class CalendarMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalendarMenu));
            this.titleText = new System.Windows.Forms.Label();
            this.dayTrackerButton = new System.Windows.Forms.Button();
            this.exploreButton = new System.Windows.Forms.Button();
            this.campaignButton = new System.Windows.Forms.Button();
            this.mainMenuButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // titleText
            // 
            this.titleText.AutoSize = true;
            this.titleText.BackColor = System.Drawing.Color.Transparent;
            this.titleText.Font = new System.Drawing.Font("Ozymandias Solid WBW", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleText.Location = new System.Drawing.Point(2, 9);
            this.titleText.Name = "titleText";
            this.titleText.Size = new System.Drawing.Size(838, 80);
            this.titleText.TabIndex = 3;
            this.titleText.Text = "The Calendar of Harptos";
            // 
            // dayTrackerButton
            // 
            this.dayTrackerButton.Location = new System.Drawing.Point(148, 110);
            this.dayTrackerButton.Name = "dayTrackerButton";
            this.dayTrackerButton.Size = new System.Drawing.Size(101, 43);
            this.dayTrackerButton.TabIndex = 4;
            this.dayTrackerButton.Text = "Day Tracker";
            this.dayTrackerButton.UseVisualStyleBackColor = true;
            this.dayTrackerButton.Click += new System.EventHandler(this.dayTrackerButton_Click);
            // 
            // exploreButton
            // 
            this.exploreButton.Location = new System.Drawing.Point(354, 110);
            this.exploreButton.Name = "exploreButton";
            this.exploreButton.Size = new System.Drawing.Size(101, 43);
            this.exploreButton.TabIndex = 5;
            this.exploreButton.Text = "Explore Calendar";
            this.exploreButton.UseVisualStyleBackColor = true;
            // 
            // campaignButton
            // 
            this.campaignButton.Location = new System.Drawing.Point(560, 110);
            this.campaignButton.Name = "campaignButton";
            this.campaignButton.Size = new System.Drawing.Size(101, 43);
            this.campaignButton.TabIndex = 6;
            this.campaignButton.Text = "Campaigns";
            this.campaignButton.UseVisualStyleBackColor = true;
            this.campaignButton.Click += new System.EventHandler(this.campaignButton_Click);
            // 
            // mainMenuButton
            // 
            this.mainMenuButton.Location = new System.Drawing.Point(354, 207);
            this.mainMenuButton.Name = "mainMenuButton";
            this.mainMenuButton.Size = new System.Drawing.Size(101, 43);
            this.mainMenuButton.TabIndex = 7;
            this.mainMenuButton.Text = "Main Menu";
            this.mainMenuButton.UseVisualStyleBackColor = true;
            this.mainMenuButton.Click += new System.EventHandler(this.mainMenuButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.BackgroundImage = global::HarptosCalendarManager.Properties.Resources.save_icon_5404;
            this.saveButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.saveButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.saveButton.Location = new System.Drawing.Point(788, 266);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(32, 32);
            this.saveButton.TabIndex = 8;
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // CalendarMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 310);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.mainMenuButton);
            this.Controls.Add(this.campaignButton);
            this.Controls.Add(this.exploreButton);
            this.Controls.Add(this.dayTrackerButton);
            this.Controls.Add(this.titleText);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CalendarMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Calendar Manager";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CalendarMenu_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleText;
        private System.Windows.Forms.Button dayTrackerButton;
        private System.Windows.Forms.Button exploreButton;
        private System.Windows.Forms.Button campaignButton;
        private System.Windows.Forms.Button mainMenuButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}