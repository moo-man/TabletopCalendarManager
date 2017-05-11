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

        public CalendarMenu(MainMenu m)
        {
            InitializeComponent();
            main = m;
            currentCalendar = new Calendar();
        }

        public CalendarMenu(MainMenu m, Calendar loadedCalendar)
        {
            InitializeComponent();
            main = m;
            currentCalendar = loadedCalendar;
        }

        private void campaignButton_Click(object sender, EventArgs e)
        {
            if (campaignViewer == null)
            {
                campaignViewer = new CampaignViewer(currentCalendar);
                campaignViewer.Show();
            }
            else if (campaignViewer.IsDisposed)
            {
                campaignViewer = new CampaignViewer(currentCalendar);
                campaignViewer.Show();
            }
        }

        private void mainMenuButton_Click(object sender, EventArgs e)
        {
            main.Show();
            this.Close();
        }

        private void dayTrackerButton_Click(object sender, EventArgs e)
        {
            if (currentCalendar.CampaignList.Count == 0)
                currentCalendar = new Calendar();
            if (dayTracker == null)
            {
                dayTracker = new DayTracker(currentCalendar);
                dayTracker.Show();
            }
            else if (dayTracker.IsDisposed)
            {
                dayTracker = new DayTracker(currentCalendar);
                dayTracker.Show();
            }
     
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            System.IO.Stream outStream;
            saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1. RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
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
            int campaignCount = currentCalendar.CampaignList.Count;
            if (campaignCount != 0)
            {
                writer.WriteLine(campaignCount);
                for (int campIndex = 0; campIndex < campaignCount; campIndex++)
                {
                    writer.WriteLine(currentCalendar.CampaignList[campIndex].Name);
                    writer.WriteLine(currentCalendar.CampaignList[campIndex].Tag);
                    writer.WriteLine(currentCalendar.CampaignList[campIndex].CurrentDate);
                    int noteCount = currentCalendar.CampaignList[campIndex].notes.Count();
                    writer.WriteLine(noteCount);
                    for (int noteIndex = 0; noteIndex < noteCount; noteIndex++)
                    {
                        writer.WriteLine(currentCalendar.CampaignList[campIndex].notes[noteIndex].Date);
                        writer.WriteLine((int)currentCalendar.CampaignList[campIndex].notes[noteIndex].Importance);
                        writer.WriteLine(0);
                        writer.WriteLine(currentCalendar.CampaignList[campIndex].notes[noteIndex].NoteContent);
                    }
                }
            }
            writer.Close();
        }

        private void CalendarMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (main.Visible == false)
                base.Dispose();

            if (main.Visible == false)
                Application.Exit();
        }
    }
}
