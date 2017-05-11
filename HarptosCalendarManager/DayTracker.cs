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
    public partial class DayTracker : Form
    {
        Calendar currentCalendar;
        bool altNames;

        public DayTracker(Calendar currCalendar)
        {
            InitializeComponent();
            currentCalendar = currCalendar;
            altNames = false;
            wheelPicture.SendToBack();
            UpdateCalendar();
        }

        #region advance buttons
        private void advanceDayButton_Click(object sender, EventArgs e)
        {
            currentCalendar.calendar.addDay();
            UpdateCalendar();
        }

        private void advanceTendayButton_Click(object sender, EventArgs e)
        {
            currentCalendar.calendar.addTenday();
            UpdateCalendar();
        }

        private void advanceMonthButton_Click(object sender, EventArgs e)
        {
            currentCalendar.calendar.addMonth();
            UpdateCalendar();
        }

        private void advanceYearButton_Click(object sender, EventArgs e)
        {
            currentCalendar.calendar.addYear();
            UpdateCalendar();
        }
#endregion 

        private void customAdvanceButton_Click(object sender, EventArgs e)
        {

        }

        private void UpdateCalendar()
        {
            if (altNames)
                currentDate.Text = currentCalendar.calendar.returnAltDateWithHolidays();
            else
                currentDate.Text = currentCalendar.calendar.returnDateWithHolidays();

            yearNameLabel.Text = currentCalendar.calendar.returnYear();

            if (currentCalendar.activeCampaign == null)
                campaignName.Text = "None";
            else
            {
                campaignName.Text = currentCalendar.activeCampaign.Name;
                currentCalendar.activeCampaign.setCurrentDate(currentCalendar.calendar.ToString());
            }

            StringBuilder noteboxText = new StringBuilder();
            foreach (Campaign c in currentCalendar.CampaignList)
            {
                foreach (Note n in c.notes)
                {
                    if (c.Equals(currentCalendar.activeCampaign))
                    {
                        // If the note should alert the campaign or every campaign, put it in the textbox
                        if ((n.Importance == alertLevel.alertCampaign || n.Importance == alertLevel.alertAll) && currentCalendar.calendar.isAnniversary(n.Date))
                        {
                            if (n.NoteContent.Equals("Current Date"))
                            {
                                // dont print that the current date is the current date of the campaign (duh)
                            }

                            else if (currentCalendar.calendar.yearsAgo(n.Date) == 1)
                                noteboxText.AppendLine("\u2022 " + "(" + c.Tag + ") " + n.NoteContent + " (" + currentCalendar.calendar.yearsAgo(n.Date) + " year ago)\n");
                            else if (currentCalendar.calendar.yearsAgo(n.Date) > 1)
                                noteboxText.AppendLine("\u2022 " + "(" + c.Tag + ") " + n.NoteContent + " (" + currentCalendar.calendar.yearsAgo(n.Date) + " years ago)\n");
                            else if ((currentCalendar.calendar.yearsAgo(n.Date) == 0))
                                noteboxText.AppendLine("\u2022 " + "(" + c.Tag + ") " + n.NoteContent + "\n");
                            else
                                noteboxText.AppendLine("Error.");
                        }
                    }
                    else // If the note (which belongs to another campaign) every campaign, put it in the textbox
                    {
                        if ((n.Importance == alertLevel.alertAll) && currentCalendar.calendar.isAnniversary(n.Date))
                        {
                            if (currentCalendar.calendar.yearsAgo(n.Date) == 1)
                                noteboxText.AppendLine("\u2022 " + "(" + c.Tag + ") " + n.NoteContent + " (" + currentCalendar.calendar.yearsAgo(n.Date) + " year ago)\n");
                            else if (currentCalendar.calendar.yearsAgo(n.Date) > 1)
                                noteboxText.AppendLine("\u2022 " + "(" + c.Tag + ") " + n.NoteContent + " (" + currentCalendar.calendar.yearsAgo(n.Date) + " years ago)\n");
                            else if ((currentCalendar.calendar.yearsAgo(n.Date) == 0))
                                noteboxText.AppendLine("\u2022 " + "(" + c.Tag + ") " + n.NoteContent + "\n");
                            else
                                noteboxText.AppendLine("Error.");
                        }
                    }
                }
            }
            noteBox.Text = noteboxText.ToString();

            wheelPicture.Image = (Image)Properties.Resources.ResourceManager.GetObject(currentCalendar.calendar.getMonthName());
            wheelPicture.Refresh();
        }


        private void currentDateLabel_Click(object sender, EventArgs e)
        {
            altNames = !altNames;
            UpdateCalendar();
        }

        private void addNoteButton_Click(object sender, EventArgs e)
        {
            NewNoteDialog newNoteDialog = new NewNoteDialog(currentCalendar);
            newNoteDialog.ShowDialog();
            UpdateCalendar();
        }

        private void DayTracker_Load(object sender, EventArgs e)
        {
            //currentCalendar.calendar.setDate(currentCalendar.activeCampaign.CurrentDate);
        }

        private void DayTracker_Leave(object sender, EventArgs e)
        {
            UpdateCalendar();
        }

        private void DayTracker_FormClosing(object sender, FormClosingEventArgs e)
        {
           UpdateCalendar();
        }

        private void addDayButton_Click(object sender, EventArgs e)
        {
            currentCalendar.calendar.addDay();
            UpdateCalendar();
        }

        private void subDayButton_Click(object sender, EventArgs e)
        {
            currentCalendar.calendar.subDay();
            UpdateCalendar();
        }

        private void addTenday_Click(object sender, EventArgs e)
        {
            currentCalendar.calendar.addTenday();
            UpdateCalendar();
        }

        private void subTenday_Click(object sender, EventArgs e)
        {
            currentCalendar.calendar.subTenday();
            UpdateCalendar();
        }

        private void addMonth_Click(object sender, EventArgs e)
        {
            currentCalendar.calendar.addMonth();
            UpdateCalendar();
        }

        private void subMonth_Click(object sender, EventArgs e)
        {
            currentCalendar.calendar.subMonth();
            UpdateCalendar();
        }

        private void addYear_Click(object sender, EventArgs e)
        {
            currentCalendar.calendar.addYear();
            UpdateCalendar();
        }

        private void subYear_Click(object sender, EventArgs e)
        {
            currentCalendar.calendar.subYear();
            UpdateCalendar();
        }
    }
}
