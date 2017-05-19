namespace HarptosCalendarManager
{
    partial class CampaignViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CampaignViewer));
            this.campaignTree = new System.Windows.Forms.TreeView();
            this.addCampaignButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.makeActiveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // campaignTree
            // 
            this.campaignTree.BackColor = System.Drawing.SystemColors.Window;
            this.campaignTree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.campaignTree.Location = new System.Drawing.Point(12, 12);
            this.campaignTree.Name = "campaignTree";
            this.campaignTree.Size = new System.Drawing.Size(444, 370);
            this.campaignTree.TabIndex = 0;
            this.campaignTree.Enter += new System.EventHandler(this.campaignTree_Enter);
            // 
            // addCampaignButton
            // 
            this.addCampaignButton.Location = new System.Drawing.Point(24, 388);
            this.addCampaignButton.Name = "addCampaignButton";
            this.addCampaignButton.Size = new System.Drawing.Size(89, 23);
            this.addCampaignButton.TabIndex = 1;
            this.addCampaignButton.Text = "Add Campaign";
            this.addCampaignButton.UseVisualStyleBackColor = true;
            this.addCampaignButton.Click += new System.EventHandler(this.addCampaignButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(323, 388);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(109, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Delete Campaign";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // makeActiveButton
            // 
            this.makeActiveButton.Location = new System.Drawing.Point(173, 388);
            this.makeActiveButton.Name = "makeActiveButton";
            this.makeActiveButton.Size = new System.Drawing.Size(97, 23);
            this.makeActiveButton.TabIndex = 3;
            this.makeActiveButton.Text = "Make Active";
            this.makeActiveButton.UseVisualStyleBackColor = true;
            this.makeActiveButton.Click += new System.EventHandler(this.makeActiveButton_Click);
            // 
            // CampaignViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 420);
            this.Controls.Add(this.makeActiveButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.addCampaignButton);
            this.Controls.Add(this.campaignTree);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CampaignViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Campaigns";
            this.Load += new System.EventHandler(this.CampaignViewer_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView campaignTree;
        private System.Windows.Forms.Button addCampaignButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button makeActiveButton;
    }
}