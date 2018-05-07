namespace CalendarManager
{
    partial class NewCampaignDialog
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
            this.nameLabel = new System.Windows.Forms.Label();
            this.tagLabel = new System.Windows.Forms.Label();
            this.startDateLabel = new System.Windows.Forms.Label();
            this.currentDateLabel = new System.Windows.Forms.Label();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.tagBox = new System.Windows.Forms.TextBox();
            this.startD = new System.Windows.Forms.TextBox();
            this.startM = new System.Windows.Forms.TextBox();
            this.startY = new System.Windows.Forms.TextBox();
            this.currY = new System.Windows.Forms.TextBox();
            this.currM = new System.Windows.Forms.TextBox();
            this.currD = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(10, 15);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(85, 13);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Campaign Name";
            // 
            // tagLabel
            // 
            this.tagLabel.AutoSize = true;
            this.tagLabel.Location = new System.Drawing.Point(10, 54);
            this.tagLabel.Name = "tagLabel";
            this.tagLabel.Size = new System.Drawing.Size(76, 13);
            this.tagLabel.TabIndex = 1;
            this.tagLabel.Text = "Campaign Tag";
            // 
            // startDateLabel
            // 
            this.startDateLabel.AutoSize = true;
            this.startDateLabel.Location = new System.Drawing.Point(10, 90);
            this.startDateLabel.Name = "startDateLabel";
            this.startDateLabel.Size = new System.Drawing.Size(85, 26);
            this.startDateLabel.TabIndex = 2;
            this.startDateLabel.Text = "Start Date\r\n(MM/DD/YYYY)\r\n";
            // 
            // currentDateLabel
            // 
            this.currentDateLabel.AutoSize = true;
            this.currentDateLabel.Location = new System.Drawing.Point(10, 136);
            this.currentDateLabel.Name = "currentDateLabel";
            this.currentDateLabel.Size = new System.Drawing.Size(85, 26);
            this.currentDateLabel.TabIndex = 3;
            this.currentDateLabel.Text = "Current Date\r\n(MM/DD/YYYY)";
            // 
            // nameBox
            // 
            this.nameBox.Location = new System.Drawing.Point(101, 12);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(194, 20);
            this.nameBox.TabIndex = 1;
            // 
            // tagBox
            // 
            this.tagBox.Location = new System.Drawing.Point(101, 51);
            this.tagBox.Name = "tagBox";
            this.tagBox.Size = new System.Drawing.Size(92, 20);
            this.tagBox.TabIndex = 2;
            this.tagBox.TextChanged += new System.EventHandler(this.tagBox_TextChanged);
            this.tagBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tagBox_KeyPress);
            // 
            // startD
            // 
            this.startD.Location = new System.Drawing.Point(164, 87);
            this.startD.Name = "startD";
            this.startD.Size = new System.Drawing.Size(29, 20);
            this.startD.TabIndex = 4;
            this.startD.TextChanged += new System.EventHandler(this.startD_TextChanged);
            this.startD.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.date_keypress);
            this.startD.Leave += new System.EventHandler(this.startD_Leave);
            // 
            // startM
            // 
            this.startM.Location = new System.Drawing.Point(118, 87);
            this.startM.Name = "startM";
            this.startM.Size = new System.Drawing.Size(29, 20);
            this.startM.TabIndex = 3;
            this.startM.TextChanged += new System.EventHandler(this.startM_TextChanged);
            this.startM.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.date_keypress);
            this.startM.Leave += new System.EventHandler(this.startM_Leave);
            // 
            // startY
            // 
            this.startY.Location = new System.Drawing.Point(212, 87);
            this.startY.Name = "startY";
            this.startY.Size = new System.Drawing.Size(64, 20);
            this.startY.TabIndex = 5;
            this.startY.TextChanged += new System.EventHandler(this.startY_TextChanged);
            this.startY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.date_keypress);
            this.startY.Leave += new System.EventHandler(this.startY_Leave);
            // 
            // currY
            // 
            this.currY.Location = new System.Drawing.Point(212, 129);
            this.currY.Name = "currY";
            this.currY.Size = new System.Drawing.Size(64, 20);
            this.currY.TabIndex = 8;
            this.currY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.date_keypress);
            this.currY.Leave += new System.EventHandler(this.currY_Leave);
            // 
            // currM
            // 
            this.currM.Location = new System.Drawing.Point(118, 129);
            this.currM.Name = "currM";
            this.currM.Size = new System.Drawing.Size(29, 20);
            this.currM.TabIndex = 6;
            this.currM.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.date_keypress);
            this.currM.Leave += new System.EventHandler(this.currM_Leave);
            // 
            // currD
            // 
            this.currD.Location = new System.Drawing.Point(164, 129);
            this.currD.Name = "currD";
            this.currD.Size = new System.Drawing.Size(29, 20);
            this.currD.TabIndex = 7;
            this.currD.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.date_keypress);
            this.currD.Leave += new System.EventHandler(this.currD_Leave);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(166, 161);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 13;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(261, 161);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 14;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // NewCampaignDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 196);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.currY);
            this.Controls.Add(this.currM);
            this.Controls.Add(this.currD);
            this.Controls.Add(this.startY);
            this.Controls.Add(this.startM);
            this.Controls.Add(this.startD);
            this.Controls.Add(this.tagBox);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.currentDateLabel);
            this.Controls.Add(this.startDateLabel);
            this.Controls.Add(this.tagLabel);
            this.Controls.Add(this.nameLabel);
            this.Name = "NewCampaignDialog";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Campaign";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label tagLabel;
        private System.Windows.Forms.Label startDateLabel;
        private System.Windows.Forms.Label currentDateLabel;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.TextBox tagBox;
        private System.Windows.Forms.TextBox startD;
        private System.Windows.Forms.TextBox startM;
        private System.Windows.Forms.TextBox startY;
        private System.Windows.Forms.TextBox currY;
        private System.Windows.Forms.TextBox currM;
        private System.Windows.Forms.TextBox currD;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
    }
}