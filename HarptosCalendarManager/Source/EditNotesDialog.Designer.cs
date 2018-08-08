namespace HarptosCalendarManager
{
    partial class EditNotesDialog
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
            this.alertAll = new System.Windows.Forms.RadioButton();
            this.AlertCampaign = new System.Windows.Forms.RadioButton();
            this.noAlert = new System.Windows.Forms.RadioButton();
            this.generalBox = new System.Windows.Forms.CheckBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.editNoteBox = new System.Windows.Forms.TextBox();
            this.dateLabel = new System.Windows.Forms.Label();
            this.year = new System.Windows.Forms.TextBox();
            this.month = new System.Windows.Forms.TextBox();
            this.day = new System.Windows.Forms.TextBox();
            this.campaignSelector = new System.Windows.Forms.ComboBox();
            this.noAlertTip = new System.Windows.Forms.ToolTip(this.components);
            this.campaignTip = new System.Windows.Forms.ToolTip(this.components);
            this.globalTip = new System.Windows.Forms.ToolTip(this.components);
            this.generalTip = new System.Windows.Forms.ToolTip(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // alertAll
            // 
            this.alertAll.AutoSize = true;
            this.alertAll.Location = new System.Drawing.Point(212, 34);
            this.alertAll.Name = "alertAll";
            this.alertAll.Size = new System.Drawing.Size(55, 17);
            this.alertAll.TabIndex = 7;
            this.alertAll.TabStop = true;
            this.alertAll.Text = "Global";
            this.alertAll.UseVisualStyleBackColor = true;
            // 
            // AlertCampaign
            // 
            this.AlertCampaign.AutoSize = true;
            this.AlertCampaign.Location = new System.Drawing.Point(212, 57);
            this.AlertCampaign.Name = "AlertCampaign";
            this.AlertCampaign.Size = new System.Drawing.Size(72, 17);
            this.AlertCampaign.TabIndex = 8;
            this.AlertCampaign.TabStop = true;
            this.AlertCampaign.Text = "Campaign";
            this.AlertCampaign.UseVisualStyleBackColor = true;
            // 
            // noAlert
            // 
            this.noAlert.AutoSize = true;
            this.noAlert.Location = new System.Drawing.Point(212, 80);
            this.noAlert.Name = "noAlert";
            this.noAlert.Size = new System.Drawing.Size(68, 17);
            this.noAlert.TabIndex = 9;
            this.noAlert.TabStop = true;
            this.noAlert.Text = "No Alerts";
            this.noAlert.UseVisualStyleBackColor = true;
            // 
            // generalBox
            // 
            this.generalBox.AutoSize = true;
            this.generalBox.Location = new System.Drawing.Point(212, 11);
            this.generalBox.Name = "generalBox";
            this.generalBox.Size = new System.Drawing.Size(63, 17);
            this.generalBox.TabIndex = 6;
            this.generalBox.Text = "General";
            this.generalBox.UseVisualStyleBackColor = true;
            this.generalBox.CheckedChanged += new System.EventHandler(this.generalBox_CheckedChanged);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(200, 146);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(120, 146);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 4;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Note Contents:";
            // 
            // editNoteBox
            // 
            this.editNoteBox.Location = new System.Drawing.Point(12, 31);
            this.editNoteBox.Multiline = true;
            this.editNoteBox.Name = "editNoteBox";
            this.editNoteBox.Size = new System.Drawing.Size(183, 66);
            this.editNoteBox.TabIndex = 0;
            // 
            // dateLabel
            // 
            this.dateLabel.AutoSize = true;
            this.dateLabel.Location = new System.Drawing.Point(2, 113);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(85, 26);
            this.dateLabel.TabIndex = 15;
            this.dateLabel.Text = "Date:\r\n(MM/DD/YYYY)";
            // 
            // year
            // 
            this.year.Location = new System.Drawing.Point(190, 110);
            this.year.Name = "year";
            this.year.Size = new System.Drawing.Size(64, 20);
            this.year.TabIndex = 3;
            this.year.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.date_keypress);
            this.year.Leave += new System.EventHandler(this.date_Leave);
            // 
            // month
            // 
            this.month.Location = new System.Drawing.Point(96, 110);
            this.month.Name = "month";
            this.month.Size = new System.Drawing.Size(29, 20);
            this.month.TabIndex = 1;
            this.month.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.date_keypress);
            this.month.Leave += new System.EventHandler(this.date_Leave);
            // 
            // day
            // 
            this.day.Location = new System.Drawing.Point(142, 110);
            this.day.Name = "day";
            this.day.Size = new System.Drawing.Size(29, 20);
            this.day.TabIndex = 2;
            this.day.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.date_keypress);
            this.day.Leave += new System.EventHandler(this.date_Leave);
            // 
            // campaignSelector
            // 
            this.campaignSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.campaignSelector.FormattingEnabled = true;
            this.campaignSelector.Location = new System.Drawing.Point(12, 148);
            this.campaignSelector.Name = "campaignSelector";
            this.campaignSelector.Size = new System.Drawing.Size(82, 21);
            this.campaignSelector.TabIndex = 10;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(198, 240);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 16;
            // 
            // EditNotesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 178);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.campaignSelector);
            this.Controls.Add(this.year);
            this.Controls.Add(this.month);
            this.Controls.Add(this.day);
            this.Controls.Add(this.dateLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.editNoteBox);
            this.Controls.Add(this.alertAll);
            this.Controls.Add(this.AlertCampaign);
            this.Controls.Add(this.noAlert);
            this.Controls.Add(this.generalBox);
            this.Name = "EditNotesDialog";
            this.ShowIcon = false;
            this.Text = "Edit Notes";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton alertAll;
        private System.Windows.Forms.RadioButton AlertCampaign;
        private System.Windows.Forms.RadioButton noAlert;
        private System.Windows.Forms.CheckBox generalBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox editNoteBox;
        private System.Windows.Forms.Label dateLabel;
        private System.Windows.Forms.TextBox year;
        private System.Windows.Forms.TextBox month;
        private System.Windows.Forms.TextBox day;
        private System.Windows.Forms.ComboBox campaignSelector;
        private System.Windows.Forms.ToolTip noAlertTip;
        private System.Windows.Forms.ToolTip campaignTip;
        private System.Windows.Forms.ToolTip globalTip;
        private System.Windows.Forms.ToolTip generalTip;
        private System.Windows.Forms.TextBox textBox1;
    }
}