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
    public partial class NewCampaignDialog : Form
    {
        CalendarContents currentCalendar;
        TreeView campaignTree;
        CampaignViewer Cviewer;
        Campaign campaignToAdd;
        public NewCampaignDialog(CalendarContents cal, TreeView cTree, CampaignViewer cviewer)
        {
            InitializeComponent();
            currentCalendar = cal;
            campaignTree = cTree;
            Cviewer = cviewer;
            campaignToAdd = null;
        }

        // This constructor is called if a campaign is passed to it, turning the dialog into an 'edit campaign' dialog
        // Throughout the form, it checks to see if editCampaign is null, if it is, that means it's a 'new campaign' form
        public NewCampaignDialog(CalendarContents cal, Campaign editCampaign, TreeView cTree, CampaignViewer cviewer) : this(cal, cTree, cviewer)
        {
            campaignToAdd = editCampaign;
            nameBox.Text = campaignToAdd.Name;
            tagBox.Text = campaignToAdd.Tag;

            startM.Text = campaignToAdd.findNote(campaignToAdd.Name + " began!").Date.Substring(0, 2);
            startD.Text = campaignToAdd.findNote(campaignToAdd.Name + " began!").Date.Substring(2, 2);
            startY.Text = campaignToAdd.findNote(campaignToAdd.Name + " began!").Date.Substring(4, 4);

            currM.Text = campaignToAdd.CurrentDate.Substring(0, 2);
            currD.Text = campaignToAdd.CurrentDate.Substring(2, 2);
            currY.Text = campaignToAdd.CurrentDate.Substring(4, 4);

            this.Text = "Edit Campaign";
        }

        private void startD_TextChanged(object sender, EventArgs e)
        {
            // Don't mirror start and current dates if editing an existing campaign
            if (campaignToAdd == null)
                currD.Text = startD.Text;
        }

        private void startM_TextChanged(object sender, EventArgs e)
        {
            if (campaignToAdd == null)
                currM.Text = startM.Text;
        }

        private void startY_TextChanged(object sender, EventArgs e)
        {
            if (campaignToAdd == null)
                currY.Text = startY.Text;
        }

        private void start_Leave(object sender, EventArgs e)
        {
            startY.Text = CalendarType.enforceYearFormat(startY.Text);
            startM.Text = CalendarType.enforceMonthFormat(startM.Text);
            startD.Text = CalendarType.enforceDayFormat(startM.Text, startD.Text, startY.Text);
        }

        private void curr_Leave(object sender, EventArgs e)
        {
            currY.Text = CalendarType.enforceYearFormat(currY.Text);
            currM.Text = CalendarType.enforceMonthFormat(currM.Text);
            currD.Text = CalendarType.enforceDayFormat(currM.Text, currD.Text, currY.Text);
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


            if (currentCalendar.CampaignList.Find(x => x.Tag == tagBox.Text) != null && campaignToAdd == null) // added check for 'if editing existing campaign, allow tag if it already exists 
            {                                                                                                  // (without this, to edit a campaign you would need to change the tag)
                MessageBox.Show("A campaign with this tag already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (validDateBoxes())
            {
                startDate = startM.Text + startD.Text + startY.Text;
                currentDate = currM.Text + currD.Text + currY.Text;

                if (campaignToAdd == null)
                {
                    campaignToAdd = new Campaign(nameBox.Text, tagBox.Text, startDate, currentDate);
                    currentCalendar.AddCampaign(campaignToAdd);
                }

                else
                {
                    campaignToAdd.Name = nameBox.Text;
                    campaignToAdd.setCurrentDate(currentDate);
                    campaignToAdd.setStartDate(startDate);
                    campaignToAdd.Tag = tagBox.Text;
                }

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
