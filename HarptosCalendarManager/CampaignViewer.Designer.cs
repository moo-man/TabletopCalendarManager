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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CampaignViewer));
            this.campaignTree = new System.Windows.Forms.TreeView();
            this.addCampaignButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.makeActiveButton = new System.Windows.Forms.Button();
            this.editCampaignButton = new System.Windows.Forms.Button();
            this.endButton = new System.Windows.Forms.Button();
            this.deactivateButton = new System.Windows.Forms.Button();
            this.campaignLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.editNoteButton = new System.Windows.Forms.Button();
            this.deleteNoteButton = new System.Windows.Forms.Button();
            this.addNoteButton = new System.Windows.Forms.Button();
            this.activateToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // campaignTree
            // 
            this.campaignTree.BackColor = System.Drawing.SystemColors.Window;
            this.campaignTree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.campaignTree.Location = new System.Drawing.Point(12, 12);
            this.campaignTree.Name = "campaignTree";
            this.campaignTree.Size = new System.Drawing.Size(338, 370);
            this.campaignTree.TabIndex = 0;
            // 
            // addCampaignButton
            // 
            this.addCampaignButton.Location = new System.Drawing.Point(372, 39);
            this.addCampaignButton.Name = "addCampaignButton";
            this.addCampaignButton.Size = new System.Drawing.Size(64, 23);
            this.addCampaignButton.TabIndex = 1;
            this.addCampaignButton.Text = "Add";
            this.addCampaignButton.UseVisualStyleBackColor = true;
            this.addCampaignButton.Click += new System.EventHandler(this.AddCampaignButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(372, 126);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(64, 23);
            this.deleteButton.TabIndex = 2;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteCampaignButton_Click);
            // 
            // makeActiveButton
            // 
            this.makeActiveButton.Location = new System.Drawing.Point(69, 388);
            this.makeActiveButton.Name = "makeActiveButton";
            this.makeActiveButton.Size = new System.Drawing.Size(97, 23);
            this.makeActiveButton.TabIndex = 3;
            this.makeActiveButton.Text = "Activate";
            this.makeActiveButton.UseVisualStyleBackColor = true;
            this.makeActiveButton.Click += new System.EventHandler(this.makeActiveButton_Click);
            // 
            // editCampaignButton
            // 
            this.editCampaignButton.Location = new System.Drawing.Point(372, 68);
            this.editCampaignButton.Name = "editCampaignButton";
            this.editCampaignButton.Size = new System.Drawing.Size(64, 23);
            this.editCampaignButton.TabIndex = 4;
            this.editCampaignButton.Text = "Edit";
            this.editCampaignButton.UseVisualStyleBackColor = true;
            this.editCampaignButton.Click += new System.EventHandler(this.editCampaignButton_Click);
            // 
            // endButton
            // 
            this.endButton.Location = new System.Drawing.Point(372, 97);
            this.endButton.Name = "endButton";
            this.endButton.Size = new System.Drawing.Size(64, 23);
            this.endButton.TabIndex = 5;
            this.endButton.Text = "End";
            this.endButton.UseVisualStyleBackColor = true;
            this.endButton.Click += new System.EventHandler(this.endButton_Click);
            // 
            // deactivateButton
            // 
            this.deactivateButton.Location = new System.Drawing.Point(205, 388);
            this.deactivateButton.Name = "deactivateButton";
            this.deactivateButton.Size = new System.Drawing.Size(97, 23);
            this.deactivateButton.TabIndex = 6;
            this.deactivateButton.Text = "Deactivate";
            this.deactivateButton.UseVisualStyleBackColor = true;
            this.deactivateButton.Click += new System.EventHandler(this.deactivateButton_Click);
            // 
            // campaignLabel
            // 
            this.campaignLabel.AutoSize = true;
            this.campaignLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.campaignLabel.Location = new System.Drawing.Point(360, 13);
            this.campaignLabel.Name = "campaignLabel";
            this.campaignLabel.Size = new System.Drawing.Size(89, 20);
            this.campaignLabel.TabIndex = 7;
            this.campaignLabel.Text = "Campaigns";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(379, 167);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 20);
            this.label1.TabIndex = 12;
            this.label1.Text = "Notes";
            // 
            // editNoteButton
            // 
            this.editNoteButton.Location = new System.Drawing.Point(372, 222);
            this.editNoteButton.Name = "editNoteButton";
            this.editNoteButton.Size = new System.Drawing.Size(64, 23);
            this.editNoteButton.TabIndex = 10;
            this.editNoteButton.Text = "Edit";
            this.editNoteButton.UseVisualStyleBackColor = true;
            this.editNoteButton.Click += new System.EventHandler(this.editNoteButton_Click);
            // 
            // deleteNoteButton
            // 
            this.deleteNoteButton.Location = new System.Drawing.Point(372, 251);
            this.deleteNoteButton.Name = "deleteNoteButton";
            this.deleteNoteButton.Size = new System.Drawing.Size(64, 23);
            this.deleteNoteButton.TabIndex = 9;
            this.deleteNoteButton.Text = "Delete";
            this.deleteNoteButton.UseVisualStyleBackColor = true;
            this.deleteNoteButton.Click += new System.EventHandler(this.deleteNoteButton_Click);
            // 
            // addNoteButton
            // 
            this.addNoteButton.Location = new System.Drawing.Point(372, 193);
            this.addNoteButton.Name = "addNoteButton";
            this.addNoteButton.Size = new System.Drawing.Size(64, 23);
            this.addNoteButton.TabIndex = 8;
            this.addNoteButton.Text = "Add";
            this.addNoteButton.UseVisualStyleBackColor = true;
            this.addNoteButton.Click += new System.EventHandler(this.addNoteButton_Click);
            // 
            // CampaignViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 420);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.editNoteButton);
            this.Controls.Add(this.deleteNoteButton);
            this.Controls.Add(this.addNoteButton);
            this.Controls.Add(this.campaignLabel);
            this.Controls.Add(this.deactivateButton);
            this.Controls.Add(this.endButton);
            this.Controls.Add(this.editCampaignButton);
            this.Controls.Add(this.makeActiveButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.addCampaignButton);
            this.Controls.Add(this.campaignTree);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CampaignViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Campaigns";
            this.Activated += new System.EventHandler(this.CampaignViewer_Activated);
            this.Load += new System.EventHandler(this.CampaignViewer_Load);
            this.Enter += new System.EventHandler(this.CampaignViewer_Enter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView campaignTree;
        private System.Windows.Forms.Button addCampaignButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button makeActiveButton;
        private System.Windows.Forms.Button editCampaignButton;
        private System.Windows.Forms.Button endButton;
        private System.Windows.Forms.Button deactivateButton;
        private System.Windows.Forms.Label campaignLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button editNoteButton;
        private System.Windows.Forms.Button deleteNoteButton;
        private System.Windows.Forms.Button addNoteButton;
        private System.Windows.Forms.ToolTip activateToolTip;
    }
}