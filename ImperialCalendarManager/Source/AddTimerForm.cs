using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WarhammerCalendarManager
{
    public partial class AddTimerForm : Form
    {
        CalendarContents currentCalendar;
        Timer timerToEdit;
        public AddTimerForm(CalendarContents currCalendar) : this (currCalendar, null)
        {

        }

        public AddTimerForm(CalendarContents currCalendar, Timer timer)
        {
            currentCalendar = currCalendar;
            InitializeComponent();
            month.Enabled = day.Enabled = year.Enabled = numDays.Enabled = false;
            keepTrackToolTip.SetToolTip(trackCheck, "Whether this timer should be displayed in the Day Tracker");
            trackCheck.Checked = true;
            if (timer != null)
            {
                this.Text = "Edit Timer";
                timerToEdit = timer;
                timerMessageBox.Text = timerToEdit.message;
                trackCheck.Checked = timerToEdit.keepTrack;
                dateRadioButton.Checked = true;
                month.Text = timerToEdit.returnDateString().Substring(0, 2);
                day.Text = timerToEdit.returnDateString().Substring(2, 2);
                year.Text = timerToEdit.returnDateString().Substring(4, 4);
            }
        }

        private void trackCheck_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void dateRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (dateRadioButton.Checked)
            {
                daysRadioButton.Checked = false;
                month.Enabled = day.Enabled = year.Enabled = true;
                numDays.Enabled = false;
                numDays.Clear();
            }
        }

        private void daysRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (daysRadioButton.Checked)
            {
                dateRadioButton.Checked = false;
                numDays.Enabled = true;
                month.Enabled = day.Enabled = year.Enabled = false;
                month.Clear();
                day.Clear();
                year.Clear();
                if (timerToEdit != null)
                    numDays.Text = ImperialCalendar.daysBetween(currentCalendar.calendar.ToString(), timerToEdit.returnDateString()).ToString();
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (timerMessageBox.Text == "")
            {
                MessageBox.Show("Timer message must have content.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (daysRadioButton.Checked == false && dateRadioButton.Checked == false)
            {
                MessageBox.Show("Specify when the timer should go off.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (daysRadioButton.Checked && numDays.Text == "")
            {
                MessageBox.Show("Specify when the timer should go off.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dateRadioButton.Checked && (day.Text == "" || month.Text == "" || year.Text == ""))
            {
                MessageBox.Show("Specify when the timer should go off.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (currentCalendar.activeCampaign == null)
            {
                MessageBox.Show("No active campaign. Closing Dialog.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }


            if (timerToEdit != null)
                currentCalendar.activeCampaign.timers.Remove(timerToEdit);

            if (dateRadioButton.Checked)
                    currentCalendar.activeCampaign.addTimer(new Timer(month.Text + day.Text + year.Text, trackCheck.Checked, timerMessageBox.Text));

            else if (daysRadioButton.Checked)
                currentCalendar.activeCampaign.addTimer(new Timer(currentCalendar.calendar.dateIn(Int32.Parse(numDays.Text)), trackCheck.Checked, timerMessageBox.Text));

            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void numDays_Leave(object sender, EventArgs e)
        {

        }

        private void date_Leave(object sender, EventArgs e)
        {
            year.Text = ImperialCalendar.enforceYearFormat(year.Text);
            month.Text = ImperialCalendar.enforceMonthFormat(month.Text);
            day.Text = ImperialCalendar.enforceDayFormat(month.Text, day.Text, year.Text);
        }

        private void date_KeyPress(object sender, KeyPressEventArgs e)
        {
            char keypress = e.KeyChar;

            if (Char.IsControl(keypress))
                return;

            if (Char.IsNumber(keypress) == false)
            {
                e.Handled = true;
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "Timers are a good way to keep track of an upcoming date.\n\n" +
                "Input the date for when the timer should go off, this can be done as a specific date, or as a certain number of days from the current date.\n\n"+
                "When that date arrives, your Timer Message will be displayed, and you can then make a Note from it.", 
                "Timer Help",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

    }
}
