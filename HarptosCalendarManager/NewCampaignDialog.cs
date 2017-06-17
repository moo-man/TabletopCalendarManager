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
            if (tagBox.Text == "")
            {
                MessageBox.Show("The campaign must have a tag.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (nameBox.Text == "")
            {
                MessageBox.Show("The campaign must have a name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (tagBox.Text == "TIMER")
            {
                MessageBox.Show("This tag is not allowed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (currentCalendar.CampaignList.Find(x => x.Tag == tagBox.Text) != null)
            {
                MessageBox.Show("A campaign with this tag already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (validDateBoxes())
            {

                startDate = startM.Text + startD.Text + startY.Text;
                currentDate = currM.Text + currD.Text + currY.Text;

                Campaign newCampaign = new Campaign(nameBox.Text, tagBox.Text, startDate, currentDate);

                currentCalendar.AddCampaign(newCampaign);

                Cviewer.UpdateTree();

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

        private void currD_Leave(object sender, EventArgs e)
        {
            currD.Text = HarptosCalendar.enforceDayFormat(currM.Text, currD.Text, currY.Text);
        }

        private void currM_Leave(object sender, EventArgs e)
        {
            currM.Text = HarptosCalendar.enforceMonthFormat(currM.Text);
        }

        private void startY_Leave(object sender, EventArgs e)
        {
            startY.Text = HarptosCalendar.enforceYearFormat(startY.Text);
        }

        private void currY_Leave(object sender, EventArgs e)
        {
            currY.Text = HarptosCalendar.enforceYearFormat(currY.Text);
        }

        public string startBoxesToDate()
        {
            return startM.Text + startD.Text + startY.Text;
        }
        public string currentBoxesToDate()
        {
            return currM.Text + currD.Text + currY.Text;
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

        private void tagBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char keypress = e.KeyChar;

            if (keypress == '(' || keypress == ')')
                e.Handled = true;
        } 
    }
}
