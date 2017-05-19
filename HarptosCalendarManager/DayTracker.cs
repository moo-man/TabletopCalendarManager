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
        List<Note> listOfNotes;

        public DayTracker(Calendar currCalendar)
        {
            InitializeComponent();
            currentCalendar = currCalendar;
            altNames = false;
            wheelPicture.SendToBack();
            listOfNotes = new List<Note>();
            UpdateCalendar();
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

            // The following section of code finds all notes that should be listed and puts them in a list to give to writeNotes() function
            StringBuilder noteboxText = new StringBuilder();
            listOfNotes.Clear();
            foreach (Campaign c in currentCalendar.CampaignList)
            {
                foreach (Note n in c.notes)
                {
                    if (c.Equals(currentCalendar.activeCampaign)) // If the note belongs to current campaign, and has appropriate visibilty, and is anniversary of this date
                    {
                        if ((n.Importance == alertLevel.alertCampaign || n.Importance == alertLevel.alertAll) && currentCalendar.calendar.isAnniversary(n.Date))
                        {
                            if (n.NoteContent.Equals("Current Date") == false) // don't print the current date of current campaign, as that is always the current date
                                listOfNotes.Add(n);
                        }
                        else if (n.Importance == alertLevel.dontAlert && currentCalendar.calendar.sameDate(n.Date))
                                listOfNotes.Add(n);
                    }
             
                    else // If the note does not belong in the current campaign
                        if ((n.Importance == alertLevel.alertAll) && currentCalendar.calendar.isAnniversary(n.Date)) // if the note happened on this day and is of                                                                                        // sufficient importance level
                                listOfNotes.Add(n);


                } // end foreach note
            } // end foreach campaign

            writeNotes(listOfNotes);

            // decide whether to show edit button or hide it

            wheelPicture.Image = (Image)Properties.Resources.ResourceManager.GetObject(currentCalendar.calendar.getMonthName());
            wheelPicture.Refresh();
        }

        public void writeNotes(List<Note> list)
        {
            noteBox.Items.Clear();
            foreach (Note n in list)
            {
                if (currentCalendar.calendar.yearsAgo(n.Date) == 1)
                    noteBox.Items.Add("\u2022 " + "(" + n.Campaign.Tag + ") " + n.NoteContent + " (" + currentCalendar.calendar.yearsAgo(n.Date) + " year ago)\n");
                else if (currentCalendar.calendar.yearsAgo(n.Date) > 1)                                                                                                        // Note happened in past
                    noteBox.Items.Add("\u2022 " + "(" + n.Campaign.Tag + ") " + n.NoteContent + " (" + currentCalendar.calendar.yearsAgo(n.Date) + " years ago)\n");
                else if ((currentCalendar.calendar.yearsAgo(n.Date) == 0))
                    noteBox.Items.Add("\u2022 " + "(" + n.Campaign.Tag + ") " + n.NoteContent + "\n");                                                                         // Note happened this very day
                else if (currentCalendar.calendar.yearsAgo(n.Date) == -1)
                    noteBox.Items.Add("\u2022 " + "(" + n.Campaign.Tag + ") " + n.NoteContent + " (in " + Math.Abs(currentCalendar.calendar.yearsAgo(n.Date)) + " year)\n");   
                else if (currentCalendar.calendar.yearsAgo(n.Date) < -1)                                                                                                       // Note happens in future
                    noteBox.Items.Add("\u2022 " + "(" + n.Campaign.Tag + ") " + n.NoteContent + " (in " + Math.Abs(currentCalendar.calendar.yearsAgo(n.Date)) + " years)\n");
                else
                    noteBox.Items.Add("Error.");
            }
        }

        private void currentDateLabel_Click(object sender, EventArgs e)
        {
            altNames = !altNames;
            UpdateCalendar();
        }

        private void addNoteButton_Click(object sender, EventArgs e)
        {
            NewNoteDialog newNoteDialog = new NewNoteDialog(currentCalendar);
            newNoteDialog.ShowDialog(this);
            UpdateCalendar();
        }

        private void DayTracker_Load(object sender, EventArgs e)
        {
        }

        private void DayTracker_Leave(object sender, EventArgs e)
        {
            UpdateCalendar();
        }

        private void DayTracker_FormClosing(object sender, FormClosingEventArgs e)
        {
           UpdateCalendar();
        }

        #region add/sub days

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

        #endregion

        private void editNotesButton_Click(object sender, EventArgs e)
        {
            if (noteBox.SelectedItem == null)
            {
            }

            else // TODO: find note to put into dialog
            {
                Note selectedNote = currentCalendar.findNote(parseNoteContent(noteBox.SelectedItem.ToString()));
                EditNotesDialog editNoteDialog = new EditNotesDialog(selectedNote, currentCalendar.CampaignList);
                editNoteDialog.ShowDialog(this);
                UpdateCalendar();
            }

        }

        // The list has notes that look like "(tag) note contents", so when selecting a note we have a parse 
        // the note contents to give to findNote
        private string parseNoteContent(string stringToParse)
        {
            int OpenParenIndex = 0;
            int ClosedParenIndex = 0;


            // First half trims the tag off the noteContent
            for (int i = 0; i < stringToParse.Length && stringToParse[i] != '('; i++)
                OpenParenIndex = i;
            for (int i = OpenParenIndex; i < stringToParse.Length && stringToParse[i] != ')'; i++)
                ClosedParenIndex = i;

            int startIndex = 0;

            if (OpenParenIndex < ClosedParenIndex)
                startIndex = ClosedParenIndex + 3;
            else
                return null;

            stringToParse = stringToParse.Substring(startIndex).TrimEnd('\n');

            // Second part applies if the current date is not the note's date
            // Trims off the (x years ago) or (in x years)
            if (stringToParse[stringToParse.Length - 1] == ')')
            {
                for (int i = stringToParse.Length - 1; i > 0 && stringToParse[i] != '('; i--)
                    OpenParenIndex = i;
                stringToParse = stringToParse.Substring(0, OpenParenIndex-2);
            }

            return stringToParse;
        }
    }
}
