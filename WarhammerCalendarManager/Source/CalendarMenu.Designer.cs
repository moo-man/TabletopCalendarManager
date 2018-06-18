namespace WarhammerCalendarManager
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalendarMenu));
            this.titleText = new System.Windows.Forms.Label();
            this.dayTrackerButton = new System.Windows.Forms.Button();
            this.campaignButton = new System.Windows.Forms.Button();
            this.mainMenuButton = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.campaignSelector = new System.Windows.Forms.ComboBox();
            this.dayTrackerTip = new System.Windows.Forms.ToolTip(this.components);
            this.campaignsTip = new System.Windows.Forms.ToolTip(this.components);
            this.campaignSelectorTip = new System.Windows.Forms.ToolTip(this.components);
            this.filebutton = new System.Windows.Forms.ToolStripDropDownButton();
            this.calendarMenuToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.calendarMenuToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // titleText
            // 
            this.titleText.AutoSize = true;
            this.titleText.BackColor = System.Drawing.Color.Transparent;
            this.titleText.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleText.Location = new System.Drawing.Point(12, 25);
            this.titleText.Name = "titleText";
            this.titleText.Size = new System.Drawing.Size(567, 84);
            this.titleText.TabIndex = 3;
            this.titleText.Text = "Calendar Manager";
            this.titleText.UseCompatibleTextRendering = true;
            // 
            // dayTrackerButton
            // 
            this.dayTrackerButton.Location = new System.Drawing.Point(141, 110);
            this.dayTrackerButton.Name = "dayTrackerButton";
            this.dayTrackerButton.Size = new System.Drawing.Size(101, 43);
            this.dayTrackerButton.TabIndex = 4;
            this.dayTrackerButton.Text = "Day Tracker";
            this.dayTrackerButton.UseVisualStyleBackColor = true;
            this.dayTrackerButton.Click += new System.EventHandler(this.dayTrackerButton_Click);
            // 
            // campaignButton
            // 
            this.campaignButton.Location = new System.Drawing.Point(372, 110);
            this.campaignButton.Name = "campaignButton";
            this.campaignButton.Size = new System.Drawing.Size(101, 43);
            this.campaignButton.TabIndex = 6;
            this.campaignButton.Text = "Campaigns";
            this.campaignButton.UseVisualStyleBackColor = true;
            this.campaignButton.Click += new System.EventHandler(this.campaignButton_Click);
            // 
            // mainMenuButton
            // 
            this.mainMenuButton.Location = new System.Drawing.Point(252, 205);
            this.mainMenuButton.Name = "mainMenuButton";
            this.mainMenuButton.Size = new System.Drawing.Size(101, 43);
            this.mainMenuButton.TabIndex = 7;
            this.mainMenuButton.Text = "Main Menu";
            this.mainMenuButton.UseVisualStyleBackColor = true;
            this.mainMenuButton.Click += new System.EventHandler(this.mainMenuButton_Click);
            // 
            // campaignSelector
            // 
            this.campaignSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.campaignSelector.FormattingEnabled = true;
            this.campaignSelector.Location = new System.Drawing.Point(372, 159);
            this.campaignSelector.Name = "campaignSelector";
            this.campaignSelector.Size = new System.Drawing.Size(101, 21);
            this.campaignSelector.TabIndex = 9;
            this.campaignSelector.SelectedIndexChanged += new System.EventHandler(this.campaignSelector_SelectedIndexChanged);
            // 
            // filebutton
            // 
            this.filebutton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.filebutton.Image = ((System.Drawing.Image)(resources.GetObject("filebutton.Image")));
            this.filebutton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.filebutton.Name = "filebutton";
            this.filebutton.Size = new System.Drawing.Size(38, 22);
            this.filebutton.Text = "File";
            // 
            // calendarMenuToolStrip
            // 
            this.calendarMenuToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
            this.calendarMenuToolStrip.Location = new System.Drawing.Point(0, 0);
            this.calendarMenuToolStrip.Name = "calendarMenuToolStrip";
            this.calendarMenuToolStrip.Size = new System.Drawing.Size(641, 25);
            this.calendarMenuToolStrip.TabIndex = 12;
            this.calendarMenuToolStrip.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.exitToolStripMenuItem1});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(38, 22);
            this.toolStripDropDownButton1.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::WarhammerCalendarManager.Properties.Resources.save_icon_5404;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeyDisplayString = "Crtl + S";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeyDisplayString = "Crtl + Shift + S";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(207, 22);
            this.exitToolStripMenuItem1.Text = "Exit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitToolStripMenuItem1_Click);
            // 
            // CalendarMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 280);
            this.Controls.Add(this.calendarMenuToolStrip);
            this.Controls.Add(this.campaignSelector);
            this.Controls.Add(this.mainMenuButton);
            this.Controls.Add(this.campaignButton);
            this.Controls.Add(this.dayTrackerButton);
            this.Controls.Add(this.titleText);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "CalendarMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Calendar Manager";
            this.Activated += new System.EventHandler(this.CalendarMenu_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CalendarMenu_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CalendarMenu_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CalendarMenu_KeyDown);
            this.calendarMenuToolStrip.ResumeLayout(false);
            this.calendarMenuToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleText;
        private System.Windows.Forms.Button dayTrackerButton;
        private System.Windows.Forms.Button campaignButton;
        private System.Windows.Forms.Button mainMenuButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ComboBox campaignSelector;
        private System.Windows.Forms.ToolTip dayTrackerTip;
        private System.Windows.Forms.ToolTip campaignsTip;
        private System.Windows.Forms.ToolTip campaignSelectorTip;
        private System.Windows.Forms.ToolStripDropDownButton filebutton;
        private System.Windows.Forms.ToolStrip calendarMenuToolStrip;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
    }
}