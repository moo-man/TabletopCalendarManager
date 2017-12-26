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
    public partial class CalendarMenu : Form
    {
        MainMenu main;
        Calendar currentCalendar;
        CampaignViewer campaignViewer;
        DayTracker dayTracker;

        public CalendarMenu(MainMenu m, Calendar loadedCalendar)
        {
            InitializeComponent();
            main = m;
            currentCalendar = loadedCalendar;
            MainMenu.applyFont(titleText, 1);
            campaignSelector.Items.Insert(0, "No Campaign");
            campaignSelector.SelectedIndex = 0;
            updateCampaignList();

            dayTrackerTip.SetToolTip(dayTrackerButton, "Track the passage of time in the active campaign and add notes");
            campaignsTip.SetToolTip(campaignButton, "View and manage campaigns, as well as the notes within them.");
            campaignSelectorTip.SetToolTip(campaignSelector, "Select the active campaign");
        }

        private void campaignButton_Click(object sender, EventArgs e)
        {
            if (campaignViewer == null)
            {
                campaignViewer = new CampaignViewer(currentCalendar);
                campaignViewer.Show(this);
            }
            else if (campaignViewer.IsDisposed)
            {
                campaignViewer = new CampaignViewer(currentCalendar);
                campaignViewer.Show(this);
            }
        }

        private void mainMenuButton_Click(object sender, EventArgs e)
        {
            this.Close();
            if (this.Visible == false)
                main.Show();
        }

        private void dayTrackerButton_Click(object sender, EventArgs e)
        {
            if (currentCalendar.CampaignList.Count == 0 && currentCalendar.GeneralNoteList.Count == 0)
                currentCalendar = new Calendar();

            if (dayTracker == null)
            {
                dayTracker = new DayTracker(currentCalendar);
                dayTracker.Show(this);
            }
            else if (dayTracker.IsDisposed)
            {
                dayTracker = new DayTracker(currentCalendar);
                dayTracker.Show(this);
            }
     
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Utility.Save(currentCalendar);
        }

        private void CalendarMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (main.Visible == false)
                main.Visible = true;
        }

        private void updateCampaignList()
        {
            foreach (Campaign c in currentCalendar.CampaignList)
            {
                if (campaignSelector.Items.Contains(c.Tag) == false && c.isEnded() == false)
                    campaignSelector.Items.Add(c.Tag);
            }

            if (currentCalendar.activeCampaign != null)
            {
                campaignSelector.SelectedItem = currentCalendar.activeCampaign.Tag;
            }
            else
                campaignSelector.SelectedItem = "No Campaign";

            foreach (string s in campaignSelector.Items)
            {
                // Remove item if it doesnt exist in the campaign list, if it has been ended, but don't remove "No Campaign" option
                if ((currentCalendar.CampaignList.Find(x => x.Tag == s) == null || currentCalendar.CampaignList.Find(x=>x.Tag == s).isEnded()) && s != "No Campaign")
                {
                    campaignSelector.Items.Remove(s);
                    updateCampaignList();
                    break;                // Can't remove items from the from the list while iterating
                }                         // so remove it and then call the function again (start over);
            }
        }

        private void CalendarMenu_Activated(object sender, EventArgs e)
        {
            updateCampaignList();
        }

        private void campaignSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Only perform this function IF:
            // * the newly selected index is not the default "Active campaign" text
            // * the newly selected index is not the current campaign or there is no active campaign
            if (campaignSelector.SelectedItem.ToString() == "No Campaign")
                currentCalendar.activeCampaign = null;
            else if (currentCalendar.activeCampaign != null && campaignSelector.SelectedItem.ToString() == currentCalendar.activeCampaign.Tag)
                return;
            else
            {
                foreach (Campaign c in currentCalendar.CampaignList)
                {
                    if (c.Tag == campaignSelector.SelectedItem.ToString())
                    {
                        MessageBox.Show("Successufully activated " + c.Name, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        currentCalendar.setActiveCampaign(c);
                        updateCampaignList();
                        return;
                    }
                }
                MessageBox.Show("Error: Could not activate campaign.", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                campaignSelector.SelectedIndex = 0;
                return;
            }
        }

        private void CalendarMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (currentCalendar.CampaignList.Count != 0 || currentCalendar.GeneralNoteList.Count != 0)
            {
                DialogResult result = MessageBox.Show(this, "Save?", "Save Calendar", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                    saveButton_Click(this, new EventArgs());

                else if (result == DialogResult.Cancel)
                    e.Cancel = true;
            }

            Utility.Clear(); // Ensure old file path is cleared

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainMenuButton_Click(sender, e);
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            Calendar tempCal = currentCalendar;
            currentCalendar = Utility.Load();
            if (currentCalendar == null)
            {
                currentCalendar = tempCal;
            }
            updateCampaignList();
        }

        private void CalendarMenu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Shift && e.KeyCode == Keys.S)
                saveAsToolstripMenuItem_Click(sender, e);
            if (e.Control && e.KeyCode == Keys.S)
                saveButton_Click(sender, e);
        }

        private void saveAsToolstripMenuItem_Click(object sender, EventArgs e)
        {
            Utility.SaveAs(currentCalendar);
        }
    }
}
