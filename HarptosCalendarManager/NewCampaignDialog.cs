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
    public partial class NewCampaignDialog : Form
    {
        Calendar currentCalendar;
        TreeView campaignTree;
        CampaignViewer Cviewer;
        public NewCampaignDialog(Calendar cal, TreeView cTree, CampaignViewer cviewer)
        {
            InitializeComponent();
            currentCalendar = cal;
            campaignTree = cTree;
            Cviewer = cviewer;
        }

        private void startD_TextChanged(object sender, EventArgs e)
        {
            currD.Text = startD.Text;
        }

        private void startM_TextChanged(object sender, EventArgs e)
        {
            currM.Text = startM.Text;
        }

        private void startY_TextChanged(object sender, EventArgs e)
        {
            currY.Text = startY.Text;
        }

        private void tagBox_TextChanged(object sender, EventArgs e)
        {
            tagBox.Text = Campaign.fixTag(tagBox.Text);
            tagBox.SelectionStart = tagBox.Text.Length;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            string startDate;
            string currentDate;

            startDate = startM.Text + startD.Text + startY.Text;
            currentDate = currM.Text + currD.Text + currY.Text;

            Campaign newCampaign = new Campaign(nameBox.Text, tagBox.Text, startDate, currentDate);

            currentCalendar.addCampaign(newCampaign);

            Cviewer.UpdateCampaigns();

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
