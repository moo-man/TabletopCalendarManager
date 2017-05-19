namespace HarptosCalendarManager
{
    partial class DayTracker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DayTracker));
            this.titleText = new System.Windows.Forms.Label();
            this.currentCampaignLabel = new System.Windows.Forms.Label();
            this.currentDate = new System.Windows.Forms.Label();
            this.yearNameLabel = new System.Windows.Forms.Label();
            this.campaignName = new System.Windows.Forms.Label();
            this.notesLabel = new System.Windows.Forms.Label();
            this.addNoteButton = new System.Windows.Forms.Button();
            this.wheelPicture = new System.Windows.Forms.PictureBox();
            this.dayLabel = new System.Windows.Forms.Label();
            this.addDayButton = new System.Windows.Forms.Button();
            this.subDayButton = new System.Windows.Forms.Button();
            this.subTenday = new System.Windows.Forms.Button();
            this.addTenday = new System.Windows.Forms.Button();
            this.tendayLabel = new System.Windows.Forms.Label();
            this.subYear = new System.Windows.Forms.Button();
            this.addYear = new System.Windows.Forms.Button();
            this.yearlabel = new System.Windows.Forms.Label();
            this.subMonth = new System.Windows.Forms.Button();
            this.addMonth = new System.Windows.Forms.Button();
            this.monthLabel = new System.Windows.Forms.Label();
            this.editNotesButton = new System.Windows.Forms.Button();
            this.noteBox = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.wheelPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // titleText
            // 
            this.titleText.AutoSize = true;
            this.titleText.BackColor = System.Drawing.Color.Transparent;
            this.titleText.Font = new System.Drawing.Font("Garamond", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleText.Location = new System.Drawing.Point(0, 44);
            this.titleText.Name = "titleText";
            this.titleText.Size = new System.Drawing.Size(656, 72);
            this.titleText.TabIndex = 1;
            this.titleText.Text = "The Calendar of Harptos";
            // 
            // currentCampaignLabel
            // 
            this.currentCampaignLabel.AutoSize = true;
            this.currentCampaignLabel.BackColor = System.Drawing.Color.Transparent;
            this.currentCampaignLabel.Font = new System.Drawing.Font("Garamond", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentCampaignLabel.Location = new System.Drawing.Point(4, 156);
            this.currentCampaignLabel.Name = "currentCampaignLabel";
            this.currentCampaignLabel.Size = new System.Drawing.Size(162, 39);
            this.currentCampaignLabel.TabIndex = 6;
            this.currentCampaignLabel.Text = "Campaign:";
            this.currentCampaignLabel.Click += new System.EventHandler(this.currentDateLabel_Click);
            // 
            // currentDate
            // 
            this.currentDate.AutoSize = true;
            this.currentDate.BackColor = System.Drawing.Color.Transparent;
            this.currentDate.Font = new System.Drawing.Font("Garamond", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentDate.Location = new System.Drawing.Point(3, 203);
            this.currentDate.Name = "currentDate";
            this.currentDate.Size = new System.Drawing.Size(73, 45);
            this.currentDate.TabIndex = 7;
            this.currentDate.Text = "aeu";
            this.currentDate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // yearNameLabel
            // 
            this.yearNameLabel.AutoSize = true;
            this.yearNameLabel.BackColor = System.Drawing.Color.Transparent;
            this.yearNameLabel.Font = new System.Drawing.Font("Garamond", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.yearNameLabel.Location = new System.Drawing.Point(3, 257);
            this.yearNameLabel.Name = "yearNameLabel";
            this.yearNameLabel.Size = new System.Drawing.Size(73, 45);
            this.yearNameLabel.TabIndex = 10;
            this.yearNameLabel.Text = "aeu";
            this.yearNameLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // campaignName
            // 
            this.campaignName.AutoSize = true;
            this.campaignName.BackColor = System.Drawing.Color.Transparent;
            this.campaignName.Font = new System.Drawing.Font("Garamond", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.campaignName.Location = new System.Drawing.Point(167, 156);
            this.campaignName.Name = "campaignName";
            this.campaignName.Size = new System.Drawing.Size(65, 39);
            this.campaignName.TabIndex = 12;
            this.campaignName.Text = "test";
            // 
            // notesLabel
            // 
            this.notesLabel.AutoSize = true;
            this.notesLabel.Font = new System.Drawing.Font("Minion Pro", 16F);
            this.notesLabel.Location = new System.Drawing.Point(213, 324);
            this.notesLabel.Name = "notesLabel";
            this.notesLabel.Size = new System.Drawing.Size(68, 30);
            this.notesLabel.TabIndex = 14;
            this.notesLabel.Text = "Notes:";
            // 
            // addNoteButton
            // 
            this.addNoteButton.Location = new System.Drawing.Point(218, 570);
            this.addNoteButton.Name = "addNoteButton";
            this.addNoteButton.Size = new System.Drawing.Size(75, 23);
            this.addNoteButton.TabIndex = 16;
            this.addNoteButton.Text = "Add Note";
            this.addNoteButton.UseVisualStyleBackColor = true;
            this.addNoteButton.Click += new System.EventHandler(this.addNoteButton_Click);
            // 
            // wheelPicture
            // 
            this.wheelPicture.BackColor = System.Drawing.Color.Transparent;
            this.wheelPicture.Image = global::HarptosCalendarManager.Properties.Resources.Default;
            this.wheelPicture.Location = new System.Drawing.Point(749, 101);
            this.wheelPicture.Name = "wheelPicture";
            this.wheelPicture.Size = new System.Drawing.Size(595, 549);
            this.wheelPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.wheelPicture.TabIndex = 8;
            this.wheelPicture.TabStop = false;
            // 
            // dayLabel
            // 
            this.dayLabel.AutoSize = true;
            this.dayLabel.Font = new System.Drawing.Font("Minion Pro", 16F, System.Drawing.FontStyle.Bold);
            this.dayLabel.Location = new System.Drawing.Point(73, 357);
            this.dayLabel.Name = "dayLabel";
            this.dayLabel.Size = new System.Drawing.Size(50, 30);
            this.dayLabel.TabIndex = 17;
            this.dayLabel.Text = "Day";
            // 
            // addDayButton
            // 
            this.addDayButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addDayButton.Location = new System.Drawing.Point(142, 357);
            this.addDayButton.Name = "addDayButton";
            this.addDayButton.Size = new System.Drawing.Size(42, 30);
            this.addDayButton.TabIndex = 18;
            this.addDayButton.Text = "+";
            this.addDayButton.UseVisualStyleBackColor = true;
            this.addDayButton.Click += new System.EventHandler(this.addDayButton_Click);
            // 
            // subDayButton
            // 
            this.subDayButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.subDayButton.Location = new System.Drawing.Point(12, 357);
            this.subDayButton.Name = "subDayButton";
            this.subDayButton.Size = new System.Drawing.Size(42, 30);
            this.subDayButton.TabIndex = 20;
            this.subDayButton.Text = "-";
            this.subDayButton.UseVisualStyleBackColor = true;
            this.subDayButton.Click += new System.EventHandler(this.subDayButton_Click);
            // 
            // subTenday
            // 
            this.subTenday.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.subTenday.Location = new System.Drawing.Point(12, 408);
            this.subTenday.Name = "subTenday";
            this.subTenday.Size = new System.Drawing.Size(42, 30);
            this.subTenday.TabIndex = 23;
            this.subTenday.Text = "-";
            this.subTenday.UseVisualStyleBackColor = true;
            this.subTenday.Click += new System.EventHandler(this.subTenday_Click);
            // 
            // addTenday
            // 
            this.addTenday.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addTenday.Location = new System.Drawing.Point(142, 408);
            this.addTenday.Name = "addTenday";
            this.addTenday.Size = new System.Drawing.Size(42, 30);
            this.addTenday.TabIndex = 22;
            this.addTenday.Text = "+";
            this.addTenday.UseVisualStyleBackColor = true;
            this.addTenday.Click += new System.EventHandler(this.addTenday_Click);
            // 
            // tendayLabel
            // 
            this.tendayLabel.AutoSize = true;
            this.tendayLabel.Font = new System.Drawing.Font("Minion Pro", 16F, System.Drawing.FontStyle.Bold);
            this.tendayLabel.Location = new System.Drawing.Point(59, 408);
            this.tendayLabel.Name = "tendayLabel";
            this.tendayLabel.Size = new System.Drawing.Size(79, 30);
            this.tendayLabel.TabIndex = 21;
            this.tendayLabel.Text = "Tenday";
            // 
            // subYear
            // 
            this.subYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.subYear.Location = new System.Drawing.Point(12, 510);
            this.subYear.Name = "subYear";
            this.subYear.Size = new System.Drawing.Size(42, 30);
            this.subYear.TabIndex = 29;
            this.subYear.Text = "-";
            this.subYear.UseVisualStyleBackColor = true;
            this.subYear.Click += new System.EventHandler(this.subYear_Click);
            // 
            // addYear
            // 
            this.addYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addYear.Location = new System.Drawing.Point(142, 510);
            this.addYear.Name = "addYear";
            this.addYear.Size = new System.Drawing.Size(42, 30);
            this.addYear.TabIndex = 28;
            this.addYear.Text = "+";
            this.addYear.UseVisualStyleBackColor = true;
            this.addYear.Click += new System.EventHandler(this.addYear_Click);
            // 
            // yearlabel
            // 
            this.yearlabel.AutoSize = true;
            this.yearlabel.Font = new System.Drawing.Font("Minion Pro", 16F, System.Drawing.FontStyle.Bold);
            this.yearlabel.Location = new System.Drawing.Point(71, 510);
            this.yearlabel.Name = "yearlabel";
            this.yearlabel.Size = new System.Drawing.Size(54, 30);
            this.yearlabel.TabIndex = 27;
            this.yearlabel.Text = "Year";
            // 
            // subMonth
            // 
            this.subMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.subMonth.Location = new System.Drawing.Point(12, 459);
            this.subMonth.Name = "subMonth";
            this.subMonth.Size = new System.Drawing.Size(42, 30);
            this.subMonth.TabIndex = 26;
            this.subMonth.Text = "-";
            this.subMonth.UseVisualStyleBackColor = true;
            this.subMonth.Click += new System.EventHandler(this.subMonth_Click);
            // 
            // addMonth
            // 
            this.addMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addMonth.Location = new System.Drawing.Point(142, 459);
            this.addMonth.Name = "addMonth";
            this.addMonth.Size = new System.Drawing.Size(42, 30);
            this.addMonth.TabIndex = 25;
            this.addMonth.Text = "+";
            this.addMonth.UseVisualStyleBackColor = true;
            this.addMonth.Click += new System.EventHandler(this.addMonth_Click);
            // 
            // monthLabel
            // 
            this.monthLabel.AutoSize = true;
            this.monthLabel.Font = new System.Drawing.Font("Minion Pro", 16F, System.Drawing.FontStyle.Bold);
            this.monthLabel.Location = new System.Drawing.Point(60, 459);
            this.monthLabel.Name = "monthLabel";
            this.monthLabel.Size = new System.Drawing.Size(76, 30);
            this.monthLabel.TabIndex = 24;
            this.monthLabel.Text = "Month";
            // 
            // editNotesButton
            // 
            this.editNotesButton.Location = new System.Drawing.Point(313, 570);
            this.editNotesButton.Name = "editNotesButton";
            this.editNotesButton.Size = new System.Drawing.Size(75, 23);
            this.editNotesButton.TabIndex = 30;
            this.editNotesButton.Text = "Edit Note";
            this.editNotesButton.UseVisualStyleBackColor = true;
            this.editNotesButton.Click += new System.EventHandler(this.editNotesButton_Click);
            // 
            // noteBox
            // 
            this.noteBox.FormattingEnabled = true;
            this.noteBox.HorizontalScrollbar = true;
            this.noteBox.Location = new System.Drawing.Point(218, 357);
            this.noteBox.Name = "noteBox";
            this.noteBox.Size = new System.Drawing.Size(353, 199);
            this.noteBox.TabIndex = 31;
            // 
            // DayTracker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(234)))), ((int)(((byte)(222)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1343, 652);
            this.Controls.Add(this.noteBox);
            this.Controls.Add(this.editNotesButton);
            this.Controls.Add(this.subYear);
            this.Controls.Add(this.addYear);
            this.Controls.Add(this.yearlabel);
            this.Controls.Add(this.subMonth);
            this.Controls.Add(this.addMonth);
            this.Controls.Add(this.monthLabel);
            this.Controls.Add(this.subTenday);
            this.Controls.Add(this.addTenday);
            this.Controls.Add(this.tendayLabel);
            this.Controls.Add(this.subDayButton);
            this.Controls.Add(this.addDayButton);
            this.Controls.Add(this.dayLabel);
            this.Controls.Add(this.addNoteButton);
            this.Controls.Add(this.notesLabel);
            this.Controls.Add(this.campaignName);
            this.Controls.Add(this.yearNameLabel);
            this.Controls.Add(this.wheelPicture);
            this.Controls.Add(this.currentDate);
            this.Controls.Add(this.currentCampaignLabel);
            this.Controls.Add(this.titleText);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DayTracker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Harptos Calendar";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DayTracker_FormClosing);
            this.Load += new System.EventHandler(this.DayTracker_Load);
            this.Leave += new System.EventHandler(this.DayTracker_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.wheelPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label titleText;
        private System.Windows.Forms.Label currentCampaignLabel;
        private System.Windows.Forms.Label currentDate;
        private System.Windows.Forms.PictureBox wheelPicture;
        private System.Windows.Forms.Label yearNameLabel;
        private System.Windows.Forms.Label campaignName;
        private System.Windows.Forms.Label notesLabel;
        private System.Windows.Forms.Button addNoteButton;
        private System.Windows.Forms.Label dayLabel;
        private System.Windows.Forms.Button addDayButton;
        private System.Windows.Forms.Button subDayButton;
        private System.Windows.Forms.Button subTenday;
        private System.Windows.Forms.Button addTenday;
        private System.Windows.Forms.Label tendayLabel;
        private System.Windows.Forms.Button subYear;
        private System.Windows.Forms.Button addYear;
        private System.Windows.Forms.Label yearlabel;
        private System.Windows.Forms.Button subMonth;
        private System.Windows.Forms.Button addMonth;
        private System.Windows.Forms.Label monthLabel;
        private System.Windows.Forms.Button editNotesButton;
        private System.Windows.Forms.ListBox noteBox;
    }
}

