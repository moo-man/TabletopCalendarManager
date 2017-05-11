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
    public partial class NewNoteDialog : Form
    {
        Calendar currCalendar;
        public NewNoteDialog(Calendar calendar)
        {
            InitializeComponent();
            currCalendar = calendar;
            generalTip.SetToolTip(generalBox, "General notes are not tied to a campaign.");
            globalTip.SetToolTip(alertAll, "Global notes will be shown to all campaigns on their date.");
            campaignTip.SetToolTip(AlertCampaign, "Campaign notes will only be shown to their respective campaign.");
            noAlertTip.SetToolTip(noAlert, "These notes won't be shown on their date.");
        }

        private void generalBox_CheckedChanged(object sender, EventArgs e)
        {
            AlertCampaign.Enabled = !generalBox.Checked;
        }



        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (newNoteBox.Text == null)
                this.Close();
            else if (alertAll.Checked == false && AlertCampaign.Checked == false && noAlert.Checked == false)
            {
                MessageBox.Show("Choose a visibilty level.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);   
                return;
            }

            else
            {
                if (generalBox.Checked)
                {

                }

                else
                {
                    alertLevel importance = alertLevel.dontAlert;
                    if (alertAll.Checked)
                        importance = alertLevel.alertAll;
                    else if (AlertCampaign.Checked)
                        importance  = alertLevel.alertCampaign;
                    else if (noAlert.Checked)
                        importance = alertLevel.dontAlert;

                    currCalendar.activeCampaign.addNote(currCalendar.calendar.ToString(), importance, newNoteBox.Text);
                    MessageBox.Show("Note successfully added", "Note Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
        }
    }
}
