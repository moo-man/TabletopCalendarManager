using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HarptosCalendarManager
{
    /// <summary>
    /// OBSOLETE, use NewCampaignDialog and pass a campaign into the constructor to edit
    /// </summary>
    public partial class EditCampaignDialog : Form
    {
        Campaign campaignToEdit;
        CalendarContents currentCalendar;
        public EditCampaignDialog(Campaign editCampaign, CalendarContents currCalendar)
        {
            InitializeComponent();
            try
            {
                currentCalendar = currCalendar;
                campaignToEdit = editCampaign;
                nameBox.Text = campaignToEdit.Name;
                tagBox.Text = campaignToEdit.Tag;
                Note startDate = campaignToEdit.findNote(campaignToEdit.Name + " began!");
                Note currentDate = campaignToEdit.getCurrentDateOrEndNote();

                startM.Text = startDate.Date.Substring(0, 2);
                startD.Text = startDate.Date.Substring(2, 2);
                startY.Text = startDate.Date.Substring(4, 4);

                currM.Text = currentDate.Date.Substring(0, 2);
                currD.Text = currentDate.Date.Substring(2, 2);
                currY.Text = currentDate.Date.Substring(4, 4);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {

            if (tagBox.Text == "")
            {
                MessageBox.Show("The campaign must have a tag.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (tagBox.Text == "TIMER")
            {
                MessageBox.Show("This tag is not allowed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (nameBox.Text == "")
            {
                MessageBox.Show("The campaign must have a name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (currentCalendar.CampaignList.Find(x => x.Tag == tagBox.Text) != null)
            {
                MessageBox.Show("A campaign with this tag already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (validDateBoxes())
            {

                string newStartDate = startM.Text + startD.Text + startY.Text;
                string newCurrentDate = currM.Text + currD.Text + currY.Text;
                Note startDate = campaignToEdit.findNote(campaignToEdit.Name + " began!");
                Note currentDate = campaignToEdit.getCurrentDateOrEndNote();

                startDate.editDate(newStartDate);
                currentDate.editDate(newCurrentDate);
                campaignToEdit.setCurrentDate(newCurrentDate);
                campaignToEdit.Name = nameBox.Text;
                campaignToEdit.Tag = tagBox.Text;
                startDate.editNoteContent(campaignToEdit.Name + " began!");
                if (campaignToEdit.isEnded())
                    currentDate.editNoteContent(campaignToEdit.Name + " ended.");

                this.Close();
            }
            else
                return;
        }

        public bool validDateBoxes()
        {
            if (startM.Text == "" || startD.Text == "" || startY.Text == "" || currM.Text == "" || currD.Text == "" || currY.Text == "")
            {
                MessageBox.Show("Please fill out start date and current date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void startD_Leave(object sender, EventArgs e)
        {
            startD.Text = HarptosCalendar.enforceDayFormat(startM.Text, startD.Text, startY.Text);
        }

        private void startM_Leave(object sender, EventArgs e)
        {
            startM.Text = HarptosCalendar.enforceMonthFormat(startM.Text);
        }

        private void startY_Leave(object sender, EventArgs e)
        {
            startY.Text = HarptosCalendar.enforceYearFormat(startY.Text);
        }

        private void currD_Leave(object sender, EventArgs e)
        {
            currD.Text = HarptosCalendar.enforceDayFormat(currM.Text, currD.Text, currY.Text);
        }

        private void currM_Leave(object sender, EventArgs e)
        {
            currM.Text = HarptosCalendar.enforceMonthFormat(currM.Text);
        }

        private void currY_Leave(object sender, EventArgs e)
        {
            currY.Text = HarptosCalendar.enforceYearFormat(currY.Text);
        }

        public string currentBoxesToDate()
        {
            return currM.Text + currD.Text + currY.Text;
        }

        private void dateBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char keypress = e.KeyChar;

            if (Char.IsControl(keypress))
                return;

            if (Char.IsNumber(keypress) == false)
            {
                e.Handled = true;
            }
        }

        private void tagBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char keypress = e.KeyChar;

            if (keypress == '(' || keypress == ')')
                e.Handled = true;
        }

        private void tagBox_TextChanged(object sender, EventArgs e)
        {
            tagBox.Text = Campaign.fixTag(tagBox.Text);
            tagBox.SelectionStart = tagBox.Text.Length;
        }
    }
}
