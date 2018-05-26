using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalendarManager
{
    public partial class EditNotesDialog : Form
    {
        Note editNote;
        CalendarContents currentCalendar;
        public EditNotesDialog(Note n, CalendarContents currCal)
        {
            InitializeComponent();
            editNote = n;
            currentCalendar = currCal;
            editNoteBox.Text = editNote.NoteContent;
            fillDateBoxes();
            populateCampaignDropDown();
            setVisibility();
            SetToolTips();
        }

        // This constructor is used for adding a brand new note
        // used by CampaignViewer, because NewNoteDialog (used by daytracker)
        // does not ask for enough info, such as date and campaign
        public EditNotesDialog(CalendarContents currCal)
        {
            InitializeComponent();
            editNote = new Note(null, 0, null, null); // dummy values
            currentCalendar = currCal;
            populateCampaignDropDown();
            this.Text = "New Note";
            SetToolTips();
        }

        public void SetToolTips()
        {
            generalTip.SetToolTip(generalBox, "General notes are not tied to a campaign.");
            globalTip.SetToolTip(alertAll, "Global notes will be shown to all campaigns on their date or anniversary.");
            campaignTip.SetToolTip(AlertCampaign, "Campaign notes will only be shown to their campaign.");
            noAlertTip.SetToolTip(noAlert, "These notes will only be shown on the actual date of occurrence (not anniversary), and only to their campaign");
        }

        public void setVisibility()
        {
            switch(editNote.Importance)
            {
                case AlertScope.global:
                    alertAll.Checked = true;
                    break;
                case AlertScope.campaign:
                    AlertCampaign.Checked = true;
                    break;
                case AlertScope.dontAlert:
                    noAlert.Checked = true;
                    break;
                default:
                    break;
            }
            if (editNote.Campaign == null)
            {
                generalBox.Checked = true;
            }
        }

        public void fillDateBoxes()
        {
            month.Text = editNote.Date.Substring(0, 2);
            day.Text = editNote.Date.Substring(2, 2);
            year.Text = editNote.Date.Substring(4, 4);
        }

        public void populateCampaignDropDown()
        {

            if (editNote.Campaign != null)
            {
                campaignSelector.Items.Insert(0, editNote.Campaign.Tag);
                foreach (Campaign c in currentCalendar.CampaignList)
                    if (c.Tag != editNote.Campaign.Tag)
                        campaignSelector.Items.Add(c.Tag);
                campaignSelector.SelectedIndex = 0;
            }
            else
            {
                foreach (Campaign c in currentCalendar.CampaignList)
                    campaignSelector.Items.Add(c.Tag);
                campaignSelector.SelectedItem = null;
            }
        }

        private void generalBox_CheckedChanged(object sender, EventArgs e)
        {
            // These 2 lines ensure that when general is checked, it both
            // disables campaign visibility button, and unchecks it if it was checked
            AlertCampaign.Enabled = !generalBox.Checked;
            if (AlertCampaign.Checked)
                AlertCampaign.Checked = false;

            if (generalBox.Checked)
            {
                campaignSelector.Enabled = false;
                campaignSelector.SelectedItem = null;
            }

            else
                campaignSelector.Enabled = true;
        }


        private void okButton_Click(object sender, EventArgs e)
        {
            Campaign notesCampaign;

            if (alertAll.Checked == false && AlertCampaign.Checked == false && noAlert.Checked == false)
            {
                MessageBox.Show("Choose a visibilty level.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (month.Text == "" || day.Text == "" || year.Text == "")
            {
                MessageBox.Show("Please enter a valid date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (editNoteBox.Text == "")
            {
                MessageBox.Show("The note must have some content.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (campaignSelector.SelectedItem == null && generalBox.Checked)
                notesCampaign = null;

            else if (campaignSelector.SelectedItem == null && generalBox.Checked == false)
            {
                MessageBox.Show("Please select an associated campaign.\nIf it is a general note, check the general box.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
                notesCampaign = currentCalendar.CampaignList.Find(x => x.Tag == campaignSelector.SelectedItem.ToString());


            if (editNote.Campaign == null)
                currentCalendar.deleteNote(editNote);
            else
                editNote.Campaign.deleteNote(editNote);

            AlertScope importance = AlertScope.dontAlert;
            if (alertAll.Checked)
                importance = AlertScope.global;
            else if (AlertCampaign.Checked)
                importance = AlertScope.campaign;
            else if (noAlert.Checked)
                importance = AlertScope.dontAlert;

                currentCalendar.AddNote( new Note(textBoxToDate(), importance, editNoteBox.Text, notesCampaign));

            this.Close();
        }

        public string textBoxToDate()
        {
            return month.Text + day.Text + year.Text;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void date_Leave(object sender, EventArgs e)
        {
            year.Text = CalendarType.enforceYearFormat(year.Text);
            month.Text = CalendarType.enforceMonthFormat(month.Text);
            day.Text = CalendarType.enforceDayFormat(month.Text, day.Text, year.Text);
        }

        private void date_keypress(object sender, KeyPressEventArgs e)
        {
            char keypress = e.KeyChar;

            if (Char.IsControl(keypress))
                return;

            if (Char.IsNumber(keypress) == false)
            {
                e.Handled = true;
            }
        }

    }
}
