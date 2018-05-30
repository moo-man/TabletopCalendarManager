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
    public partial class NewNoteDialog : Form
    {
        CalendarContents currCalendar;
        public NewNoteDialog(CalendarContents calendar)
        {
            InitializeComponent();
            currCalendar = calendar;
            generalTip.SetToolTip(generalBox, "General notes are not tied to a campaign.");
            globalTip.SetToolTip(globalButton, "Global notes will be shown to all campaigns on their date or anniversary.");
            campaignTip.SetToolTip(AlertCampaignButton, "Campaign notes will only be shown to their campaign.");
            noAlertTip.SetToolTip(noAlert, "These notes will only be shown on the actual date of occurrence (not anniversary), and only to their campaign");
            if (currCalendar.activeCampaign == null)
                generalBox.Checked = true;
        }

        private void generalBox_CheckedChanged(object sender, EventArgs e)
        {
            // Force general if no active campaign
            if (currCalendar.activeCampaign == null)
                generalBox.Checked = true;

            // If generalbox is checked, force campaignbutton to uncheck
            if (AlertCampaignButton.Checked) 
                AlertCampaignButton.Checked = false;

            // Disable campaign button if general box is checked
            AlertCampaignButton.Enabled = !generalBox.Checked;
        }
        
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (newNoteBox.Text == "")
            {
                MessageBox.Show("The note must have some content.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (globalButton.Checked == false && AlertCampaignButton.Checked == false && noAlert.Checked == false)
            {
                MessageBox.Show("Choose a visibilty level.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);   
                return;
            }

            else
            {
                AlertScope importance = AlertScope.dontAlert;
                if (generalBox.Checked)
                {
                    if (globalButton.Checked)
                        importance = AlertScope.global;
                    else if (noAlert.Checked)
                        importance = AlertScope.dontAlert;

                    currCalendar.AddGeneralNote(new Note(currCalendar.calendar.ToString(), importance, newNoteBox.Text, null));
                    MessageBox.Show("Note successfully added", "Note Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }

                else // if not general
                {
                    if (globalButton.Checked)
                        importance = AlertScope.global;
                    else if (AlertCampaignButton.Checked)
                        importance  = AlertScope.campaign;
                    else if (noAlert.Checked)
                        importance = AlertScope.dontAlert;

                    currCalendar.activeCampaign.addNote(currCalendar.calendar.ToString(), importance, newNoteBox.Text);
                    MessageBox.Show("Note successfully added", "Note Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
        }
    }
}
