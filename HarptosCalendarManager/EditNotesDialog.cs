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
    public partial class EditNotesDialog : Form
    {
        Note editNote;
        List<Campaign> campaignList;
        public EditNotesDialog(Note n, List<Campaign> clist)
        {
            InitializeComponent();
            editNote = n;
            campaignList = clist;
            editNoteBox.Text = editNote.NoteContent;
            fillDateBoxes();
            populateCampaignDropDown();
            setVisibility();
        }

        public void setVisibility()
        {
            switch(editNote.Importance)
            {
                case alertLevel.alertAll:
                    alertAll.Checked = true;
                    break;
                case alertLevel.alertCampaign:
                    AlertCampaign.Checked = true;
                    break;
                case alertLevel.dontAlert:
                    noAlert.Checked = true;
                    break;
                default:

                    break;
                    // TODO: general visibility
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
            campaignSelector.Items.Insert(0, editNote.Campaign.Tag);
            foreach (Campaign c in campaignList)
                if (c.Tag != editNote.Campaign.Tag)
                    campaignSelector.Items.Add(c.Tag);
            campaignSelector.SelectedIndex = 0;
        }

        private void generalBox_CheckedChanged(object sender, EventArgs e)
        {
            AlertCampaign.Enabled = !generalBox.Checked;
        }


        private void okButton_Click(object sender, EventArgs e)
        {
            // todo: add verification
            Campaign notesCampaign;

            notesCampaign = campaignList.Find(x => x.Tag == campaignSelector.SelectedItem.ToString());
            editNote.Campaign.deleteNote(editNote);

            alertLevel importance = alertLevel.dontAlert;
            if (alertAll.Checked)
                importance = alertLevel.alertAll;
            else if (AlertCampaign.Checked)
                importance = alertLevel.alertCampaign;
            else if (noAlert.Checked)
                importance = alertLevel.dontAlert;

            notesCampaign.addNote(textBoxToDate(), importance, editNoteBox.Text);

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

        private void month_Leave(object sender, EventArgs e)
        {
            if (month.Text.Length < 2)
                month.Text = "0" + month.Text;
        }

        private void day_Leave(object sender, EventArgs e)
        {
            if (day.Text.Length < 2)
                day.Text = "0" + day.Text;
        }

        private void year_Leave(object sender, EventArgs e)
        {
            if (year.Text.Length == 3)
                year.Text = "0" + year.Text;

            if (year.Text.Length == 2)
                year.Text = "00" + year.Text;

            if (year.Text.Length == 1)
                year.Text = "000" + year.Text;
        }
    }
}
