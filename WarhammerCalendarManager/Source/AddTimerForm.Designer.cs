
namespace WarhammerCalendarManager
{
    partial class AddTimerForm
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
            this.timerMessageBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dateRadioButton = new System.Windows.Forms.RadioButton();
            this.daysRadioButton = new System.Windows.Forms.RadioButton();
            this.trackCheck = new System.Windows.Forms.CheckBox();
            this.year = new System.Windows.Forms.TextBox();
            this.month = new System.Windows.Forms.TextBox();
            this.day = new System.Windows.Forms.TextBox();
            this.numDays = new System.Windows.Forms.TextBox();
            this.addButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.keepTrackToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // timerMessageBox
            // 
            this.timerMessageBox.Location = new System.Drawing.Point(12, 25);
            this.timerMessageBox.Multiline = true;
            this.timerMessageBox.Name = "timerMessageBox";
            this.timerMessageBox.Size = new System.Drawing.Size(275, 57);
            this.timerMessageBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Timer Message";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Notifies:";
            // 
            // dateRadioButton
            // 
            this.dateRadioButton.AutoSize = true;
            this.dateRadioButton.Location = new System.Drawing.Point(12, 126);
            this.dateRadioButton.Name = "dateRadioButton";
            this.dateRadioButton.Size = new System.Drawing.Size(107, 17);
            this.dateRadioButton.TabIndex = 1;
            this.dateRadioButton.TabStop = true;
            this.dateRadioButton.Text = "On a certain date";
            this.dateRadioButton.UseVisualStyleBackColor = true;
            this.dateRadioButton.CheckedChanged += new System.EventHandler(this.dateRadioButton_CheckedChanged);
            // 
            // daysRadioButton
            // 
            this.daysRadioButton.AutoSize = true;
            this.daysRadioButton.Location = new System.Drawing.Point(12, 149);
            this.daysRadioButton.Name = "daysRadioButton";
            this.daysRadioButton.Size = new System.Drawing.Size(118, 17);
            this.daysRadioButton.TabIndex = 5;
            this.daysRadioButton.TabStop = true;
            this.daysRadioButton.Text = "In a number of days";
            this.daysRadioButton.UseVisualStyleBackColor = true;
            this.daysRadioButton.CheckedChanged += new System.EventHandler(this.daysRadioButton_CheckedChanged);
            // 
            // trackCheck
            // 
            this.trackCheck.AutoSize = true;
            this.trackCheck.Location = new System.Drawing.Point(12, 175);
            this.trackCheck.Name = "trackCheck";
            this.trackCheck.Size = new System.Drawing.Size(82, 17);
            this.trackCheck.TabIndex = 7;
            this.trackCheck.Text = "Keep Track";
            this.trackCheck.UseVisualStyleBackColor = true;
            this.trackCheck.CheckedChanged += new System.EventHandler(this.trackCheck_CheckedChanged);
            // 
            // year
            // 
            this.year.Location = new System.Drawing.Point(225, 123);
            this.year.Name = "year";
            this.year.Size = new System.Drawing.Size(64, 20);
            this.year.TabIndex = 4;
            this.year.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.date_KeyPress);
            this.year.Leave += new System.EventHandler(this.date_Leave);
            // 
            // month
            // 
            this.month.Location = new System.Drawing.Point(131, 123);
            this.month.Name = "month";
            this.month.Size = new System.Drawing.Size(29, 20);
            this.month.TabIndex = 2;
            this.month.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.date_KeyPress);
            this.month.Leave += new System.EventHandler(this.date_Leave);
            // 
            // day
            // 
            this.day.Location = new System.Drawing.Point(177, 123);
            this.day.Name = "day";
            this.day.Size = new System.Drawing.Size(29, 20);
            this.day.TabIndex = 3;
            this.day.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.date_KeyPress);
            this.day.Leave += new System.EventHandler(this.date_Leave);
            // 
            // numDays
            // 
            this.numDays.Location = new System.Drawing.Point(131, 149);
            this.numDays.Name = "numDays";
            this.numDays.Size = new System.Drawing.Size(75, 20);
            this.numDays.TabIndex = 6;
            this.numDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.date_KeyPress);
            this.numDays.Leave += new System.EventHandler(this.numDays_Leave);
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(131, 175);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 8;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(225, 175);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(128, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "MM          DD           YYYY";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(295, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 15);
            this.label4.TabIndex = 13;
            this.label4.Text = "?";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // AddTimerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 205);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.numDays);
            this.Controls.Add(this.year);
            this.Controls.Add(this.month);
            this.Controls.Add(this.day);
            this.Controls.Add(this.trackCheck);
            this.Controls.Add(this.daysRadioButton);
            this.Controls.Add(this.dateRadioButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.timerMessageBox);
            this.Name = "AddTimerForm";
            this.ShowIcon = false;
            this.Text = "Add Timer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox timerMessageBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton dateRadioButton;
        private System.Windows.Forms.RadioButton daysRadioButton;
        private System.Windows.Forms.CheckBox trackCheck;
        private System.Windows.Forms.TextBox year;
        private System.Windows.Forms.TextBox month;
        private System.Windows.Forms.TextBox day;
        private System.Windows.Forms.TextBox numDays;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolTip keepTrackToolTip;
    }
}