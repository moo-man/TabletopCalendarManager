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
            System.IO.Stream outStream;
            saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Harptos Calendar files (*.hcal)|*.hcal|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 0;
            saveFileDialog1. RestoreDirectory = true;
            saveFileDialog1.DefaultExt = ".hcal";

            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                if ((outStream = saveFileDialog1.OpenFile()) != null)
                {
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(outStream);
                    saveFile(sw);
                }
            }
        }
        public void saveFile(System.IO.StreamWriter writer)
        {
            int generalNoteCount = currentCalendar.GeneralNoteList.Count;
            writer.WriteLine(generalNoteCount);
            foreach (Note n in currentCalendar.GeneralNoteList)
            {
                writer.WriteLine(n.Date);
                writer.WriteLine((int)n.Importance);
                writer.WriteLine(n.NoteContent);
            }
            int campaignCount = currentCalendar.CampaignList.Count;
            if (campaignCount != 0)
            {
                writer.WriteLine(campaignCount);
                for (int campIndex = 0; campIndex < campaignCount; campIndex++)
                {
                    writer.WriteLine(currentCalendar.CampaignList[campIndex].Name);
                    writer.WriteLine(currentCalendar.CampaignList[campIndex].Tag);
                    writer.WriteLine(currentCalendar.CampaignList[campIndex].CurrentDate);

                    int timerCount = currentCalendar.CampaignList[campIndex].timers.Count();
                    writer.WriteLine(timerCount);
                    for (int timerIndex = 0; timerIndex < timerCount; timerIndex++)
                    {
                        writer.WriteLine(currentCalendar.CampaignList[campIndex].timers[timerIndex].message);
                        writer.WriteLine(currentCalendar.CampaignList[campIndex].timers[timerIndex].keepTrack);
                        writer.WriteLine(currentCalendar.CampaignList[campIndex].timers[timerIndex].returnDateString());
                    }

                    int noteCount = currentCalendar.CampaignList[campIndex].notes.Count();
                    writer.WriteLine(noteCount);
                    for (int noteIndex = 0; noteIndex < noteCount; noteIndex++)
                    {
                        writer.WriteLine(currentCalendar.CampaignList[campIndex].notes[noteIndex].Date);
                        writer.WriteLine((int)currentCalendar.CampaignList[campIndex].notes[noteIndex].Importance);
                        writer.WriteLine(currentCalendar.CampaignList[campIndex].notes[noteIndex].NoteContent);
                    }
                }
            }
            writer.Close();
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

        }
    }
}
