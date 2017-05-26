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
    public partial class EditCampaignDialog : Form
    {
        Campaign campaignToEdit;
        public EditCampaignDialog(Campaign editCampaign)
        {
            InitializeComponent();
            try
            {

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

            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            // todo: verification

            string newStartDate = startM.Text + startD.Text+ startY.Text;
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

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void startD_Leave(object sender, EventArgs e)
        {
            if (startD.Text.Length < 2)
                startD.Text = "0" + startD.Text;
        }

        private void startM_Leave(object sender, EventArgs e)
        {
            if (startM.Text.Length < 2)
                startM.Text = "0" + startM.Text;
        }

        private void currD_Leave(object sender, EventArgs e)
        {
            if (currD.Text.Length < 2)
                currD.Text = "0" + currD.Text;
        }

        private void currM_Leave(object sender, EventArgs e)
        {
            if (currM.Text.Length < 2)
                currM.Text = "0" + currM.Text;
        }

        private void startY_Leave(object sender, EventArgs e)
        {
            if (startY.Text.Length == 3)
                startY.Text = "0" + startY.Text;

            if (startY.Text.Length == 2)
                startY.Text = "00" + startY.Text;

            if (startY.Text.Length == 1)
                startY.Text = "000" + startY.Text;
        }

        private void currY_Leave(object sender, EventArgs e)
        {
            if (currY.Text.Length == 3)
                currY.Text = "0" + currY.Text;

            if (currY.Text.Length == 2)
                currY.Text = "00" + currY.Text;

            if (currY.Text.Length == 1)
                currY.Text = "000" + currY.Text;
        }
    }
}
