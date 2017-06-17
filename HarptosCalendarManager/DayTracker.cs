using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

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
            if (currentCalendar.activeCampaign == null)
            {
                showHiddenTimersButton.Hide();
                addTimerButton.Enabled = editTimerButton.Enabled = deleteButtonTimer.Enabled = false;
            }

            else if (currentCalendar.activeCampaign.hiddenTimerCount() == 0)
                showHiddenTimersButton.Hide();
            else
                showHiddenTimersButton.Show();

            altNames = false;
            goButton.Hide();
            wheelPicture.SendToBack();
            listOfNotes = new List<Note>();
            UpdateCalendar();
            setFonts();
        }

        public void setFonts()
        {
            MainMenu.applyFont(titleText, 1);
            MainMenu.applyFont(currentCampaignLabel, 0);
            MainMenu.applyFont(campaignName, 0);
            MainMenu.applyFont(currentDate, 0);
            MainMenu.applyFont(yearNameLabel, 0);
            MainMenu.applyFont(dayLabel, 0, FontStyle.Bold);
            MainMenu.applyFont(tendayLabel, 0, FontStyle.Bold);
            MainMenu.applyFont(monthLabel, 0, FontStyle.Bold);
            MainMenu.applyFont(yearlabel, 0, FontStyle.Bold);
            MainMenu.applyFont(notesLabel, 0);
            MainMenu.applyFont(gotoLabel, 0, FontStyle.Bold);
            MainMenu.applyFont(moonNameLabel, 0, FontStyle.Bold);
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


            if (currentCalendar.activeCampaign != null)
            {
                foreach (Timer t in currentCalendar.activeCampaign.timers)
                {
                    if (HarptosCalendar.FarthestInTime(t.returnDateString(), currentCalendar.calendar.ToString()) == 0)
                    {
                        if (MessageBox.Show(this, t.message + "\n\nCreate a note?", "Timer Reached", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            new EditNotesDialog(new Note(t.returnDateString(), AlertScope.campaign, t.message, currentCalendar.activeCampaign), currentCalendar).ShowDialog(this);

                        // Remove, then restart updating (can't remove and iterate)
                        currentCalendar.activeCampaign.timers.Remove(t);
                        UpdateCalendar();
                        return;
                    }
                    else if (HarptosCalendar.FarthestInTime(t.returnDateString(), currentCalendar.calendar.ToString()) < 0)
                    {
                        if (MessageBox.Show(this, t.message + " (" + HarptosCalendar.returnGivenDate(t.returnDateString()) + ")" + "\n\nCreate a note?", "Timer Reached", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            new EditNotesDialog(new Note(t.returnDateString(), AlertScope.campaign, t.message, currentCalendar.activeCampaign), currentCalendar).ShowDialog(this);

                        // Remove, then restart updating (can't remove and iterate)
                        currentCalendar.activeCampaign.timers.Remove(t);
                        UpdateCalendar();
                        return;
                    }

                }
            }

            foreach (Note n in currentCalendar.GeneralNoteList)
            {
                if (n.Importance == AlertScope.global && currentCalendar.calendar.isAnniversary(n.Date) || (n.Importance == AlertScope.dontAlert && currentCalendar.calendar.sameDate(n.Date)))
                    listOfNotes.Add(n);
            }

            foreach (Campaign c in currentCalendar.CampaignList)
            {
                foreach (Note n in c.notes)
                {
                    if (c.Equals(currentCalendar.activeCampaign)) // If the note belongs to current campaign, and has appropriate visibilty, and is anniversary of this date
                    {
                        if ((n.Importance == AlertScope.campaign || n.Importance == AlertScope.global) && currentCalendar.calendar.isAnniversary(n.Date))
                        {
                            if (n.NoteContent.Equals("Current Date") == false) // don't print the current date of current campaign, as that is always the current date
                                listOfNotes.Add(n);
                        }
                        else if (n.Importance == AlertScope.dontAlert && currentCalendar.calendar.sameDate(n.Date))
                            listOfNotes.Add(n);
                    }

                    else // If the note does not belong in the current campaign
                        if ((n.Importance == AlertScope.global) && currentCalendar.calendar.isAnniversary(n.Date)) // if the note happened on this day and is of                                                                                        // sufficient importance level
                        listOfNotes.Add(n);


                } // end foreach note
            } // end foreach campaign

            if (currentCalendar.activeCampaign != null)
                writeNotes(currentCalendar.activeCampaign.timers, listOfNotes);
            else
                writeNotes(null, listOfNotes);

            moonPicture.Image = (Image)Properties.Resources.ResourceManager.GetObject(currentCalendar.calendar.currentMoonPhase().Replace(' ', '_'));

            if (currentCalendar.calendar.isHoliday == false)
                wheelPicture.Image = (Image)Properties.Resources.ResourceManager.GetObject(currentCalendar.calendar.getMonthName());
            else
                wheelPicture.Image = (Image)Properties.Resources.ResourceManager.GetObject(currentCalendar.calendar.returnJustHoliday().Replace(' ', '_'));
            wheelPicture.Refresh();
        }

        public void writeNotes(List<Timer> timerList, List<Note> noteList)
        {
            noteBox.Items.Clear();
            if (timerList != null)
                foreach (Timer t in timerList)
                {
                    if (t.keepTrack && currentCalendar.calendar.sameDate(t.returnDateString()) == false)
                    {
                        int numDays = (currentCalendar.calendar.daysTo(t.returnDateString()));
                        if (numDays > 1)
                            noteBox.Items.Add("(TIMER) " + t.message + " (in " + currentCalendar.calendar.daysTo(t.returnDateString()) + " days)");
                        else
                            noteBox.Items.Add("(TIMER) " + t.message + " (in " + currentCalendar.calendar.daysTo(t.returnDateString()) + " days)");
                    }
                }

            foreach (Note n in noteList)
            {
                if (n.isGeneral()) // if note is general
                {
                    if (currentCalendar.calendar.yearsAgo(n.Date) == 1)
                        noteBox.Items.Add("\u2022 " + " " + n.NoteContent + " (" + currentCalendar.calendar.yearsAgo(n.Date) + " year ago)\n");
                    else if (currentCalendar.calendar.yearsAgo(n.Date) > 1)                                                                                                        // Note happened in past
                        noteBox.Items.Add("\u2022 " + " " + n.NoteContent + " (" + currentCalendar.calendar.yearsAgo(n.Date) + " years ago)\n");
                    else if ((currentCalendar.calendar.yearsAgo(n.Date) == 0))
                        noteBox.Items.Add("\u2022 " + " " + n.NoteContent + "\n");                                                                         // Note happened this very day
                    else if (currentCalendar.calendar.yearsAgo(n.Date) == -1)
                        noteBox.Items.Add("\u2022 " + " " + n.NoteContent + " (in " + Math.Abs(currentCalendar.calendar.yearsAgo(n.Date)) + " year)\n");
                    else if (currentCalendar.calendar.yearsAgo(n.Date) < -1)                                                                                                       // Note happens in future
                        noteBox.Items.Add("\u2022 " + " " + n.NoteContent + " (in " + Math.Abs(currentCalendar.calendar.yearsAgo(n.Date)) + " years)\n");
                    else
                        noteBox.Items.Add("Error.");
                }

                else // if note belongs to a campaign
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
        }

        private void currentDateLabel_Click(object sender, EventArgs e)
        {

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
            if (noteBox.SelectedItem != null && noteBox.SelectedItem.ToString().Contains("(TIMER)") == false)
            {
                bool generalNote; // if the selected note is general
                Note selectedNote = currentCalendar.findNote(parseNoteContent(noteBox.SelectedItem.ToString(), out generalNote), generalNote);
                if (Calendar.CanEditOrDelete(selectedNote) == false)
                {
                    MessageBox.Show(this, "This note cannot be edited.", "Cannot edit note", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                EditNotesDialog editNoteDialog = new EditNotesDialog(selectedNote, currentCalendar);
                editNoteDialog.ShowDialog(this);
            }
            UpdateCalendar();
        }

        // The list has notes that look like "(tag) note contents", so when selecting a note we have a parse 
        // the note contents to give to findNote
        private string parseNoteContent(string stringToParse, out bool general)
        {
            int OpenParenIndex = 0;
            int ClosedParenIndex = 0;

            if (stringToParse.ElementAt(2) != '(' && stringToParse.ElementAt(0) == '\u2022') // Applies to general notes, no tag
            {
                stringToParse = stringToParse.Substring(3).TrimEnd("\n ".ToCharArray()); // remove bullet point and space in the beginning
                general = true;
            }

            else // if the note belongs to a campaign, trim off tag
            {

                // First half trims the tag off the noteContent
                for (int i = 0; i < stringToParse.Length && stringToParse[i] != '('; i++)
                    OpenParenIndex = i;
                for (int i = OpenParenIndex; i < stringToParse.Length && stringToParse[i] != ')'; i++)
                    ClosedParenIndex = i;

                int startIndex = 0;

                if (OpenParenIndex < ClosedParenIndex)
                    startIndex = ClosedParenIndex + 3;
                else
                    stringToParse = null;

                stringToParse = stringToParse.Substring(startIndex).TrimEnd('\n');
                general = false;
            }

            // This part applies if the current date is not the note's date
            // Trims off the (x years ago) or (in x years)
            if (stringToParse.ElementAt(stringToParse.Length - 1) == ')') // TODO: add verification for strings that aren't "" (edit: no need, can't input notes that are empty)
            {
                if (stringToParse.Contains(" years ago)") || stringToParse.Contains(" year ago)") ||
                stringToParse.Contains(" years)") || stringToParse.Contains(" year)") ||
                stringToParse.Contains(" days)") || stringToParse.Contains(" day)")) // If the end parenthesis is not part of the note
                {                                                                                  // such as (string) (1 year ago)
                    for (int i = stringToParse.Length - 1; i > 0 && stringToParse[i] != '('; i--)
                        OpenParenIndex = i;
                    stringToParse = stringToParse.Substring(0, OpenParenIndex - 2);
                }

                else // if the end parentheses IS part of the string, such as the note being "this happened one year ago (about)"
                {
                    // do nothing
                }

            }

            return stringToParse;
        }

        private void currentDate_Click(object sender, EventArgs e)
        {
            if (currentCalendar.calendar.isHoliday == false)
            {
                altNames = !altNames;
                UpdateCalendar();
            }
            else
            {
                string holiday = currentCalendar.calendar.returnJustHoliday();
                String messageString = "Open wiki page on " + holiday + "?";

                DialogResult result = MessageBox.Show(this, messageString, "Opening wiki page", MessageBoxButtons.YesNo, MessageBoxIcon.None);

                if (result == DialogResult.Yes)
                {
                    string webpage = ("http://forgottenrealms.wikia.com/wiki/" + holiday);

                    ProcessStartInfo sInfo = new ProcessStartInfo(webpage);
                    Process.Start(sInfo);
                }
            }
        }

        private void yearNameLabel_Click(object sender, EventArgs e)
        {
            String messageString = "Open wiki page on " + currentCalendar.calendar.returnYear() + " (" + currentCalendar.calendar.getYear() + " DR)?";

            DialogResult result = MessageBox.Show(this, messageString, "Opening wiki page", MessageBoxButtons.YesNo, MessageBoxIcon.None);

            if (result == DialogResult.Yes)
            {
                string webpage = ("http://forgottenrealms.wikia.com/wiki/" + currentCalendar.calendar.getYear() + "_DR");

                ProcessStartInfo sInfo = new ProcessStartInfo(webpage);
                Process.Start(sInfo);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "This is the Day Tracker, here is where you keep track of the passage of time in your active campaign, as well as take notes.\n\n" +
                "Some Tips:\n" +
                "\u2022 The 'current date' of the active campaign changes as you advance (or reverse) in days, tendays, etc.\n" +
                "\u2022 Click on the date to switch between alternate month names.\n" +
                "\u2022 If it is a holiday, click on the holiday name to open the wiki page for detailed info.\n" +
                "\u2022 Click on the name of the year to open the wiki page for more info on what happened in that year.\n",
                "Day Tracker",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void deleteNoteButton_Click(object sender, EventArgs e)
        {
            if (noteBox.SelectedItem == null || noteBox.SelectedItem.ToString().Contains("(TIMER)"))
                return;
            bool generalNote;
            Note noteToDelete = currentCalendar.findNote(parseNoteContent(noteBox.SelectedItem.ToString(), out generalNote), generalNote);

            if (Calendar.CanEditOrDelete(noteToDelete) == false)
                MessageBox.Show(this, "This note cannot be deleted", "Cannot delete note", MessageBoxButtons.OK, MessageBoxIcon.Error);

            else if (MessageBox.Show(this, "Are you sure you wish to delete this note?", "Delete note", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                if (noteToDelete.Campaign == null)
                    currentCalendar.deleteNote(noteToDelete);
                else
                    noteToDelete.Campaign.deleteNote(noteToDelete);

            }
            UpdateCalendar();
        }

        private void addTimerButton_Click(object sender, EventArgs e)
        {
            new AddTimerForm(currentCalendar).ShowDialog(this);

            if (currentCalendar.activeCampaign.hiddenTimerCount() != 0)
                showHiddenTimersButton.Show();

            UpdateCalendar();
        }

        private void deleteButtonTimer_Click(object sender, EventArgs e)
        {
            if (noteBox.SelectedItem == null || noteBox.SelectedItem.ToString().Contains("(TIMER)") == false)
                return;

            Timer timerToDelete = currentCalendar.activeCampaign.timers.Find(t => t.message == parseNoteContent(noteBox.SelectedItem.ToString(), out bool general));

            if (MessageBox.Show(this, "Are you sure you wish to delete this timer?", "Delete timer", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                currentCalendar.activeCampaign.timers.Remove(timerToDelete);

            if (currentCalendar.activeCampaign.hiddenTimerCount() != 0)
                showHiddenTimersButton.Show();
            else
                showHiddenTimersButton.Hide();
            UpdateCalendar();
        }

        private void editTimerButton_Click(object sender, EventArgs e)
        {
            if (noteBox.SelectedItem == null || noteBox.SelectedItem.ToString().Contains("(TIMER)") == false)
                return;

            Timer timerToEdit = currentCalendar.activeCampaign.timers.Find(t => t.message == parseNoteContent(noteBox.SelectedItem.ToString(), out bool general));

            if (timerToEdit == null)
            {
                MessageBox.Show(this, "Error: Could not find timer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                new AddTimerForm(currentCalendar, timerToEdit).ShowDialog() ;
            }

            if (currentCalendar.activeCampaign.hiddenTimerCount() != 0)
                showHiddenTimersButton.Show();
            else
                showHiddenTimersButton.Hide();
            UpdateCalendar();
        }

        private void showHiddenTimersButton_Click(object sender, EventArgs e)
        {
            foreach (Timer t in currentCalendar.activeCampaign.timers)
                t.keepTrack = true;

            showHiddenTimersButton.Hide();

            UpdateCalendar();
        }

        private void month_Leave(object sender, EventArgs e)
        {
            month.Text = HarptosCalendar.enforceMonthFormat(month.Text);
        }

        private void day_Leave(object sender, EventArgs e)
        {
            day.Text = HarptosCalendar.enforceDayFormat(month.Text, day.Text, year.Text);
        }

        private void year_Leave(object sender, EventArgs e)
        {
            year.Text = HarptosCalendar.enforceYearFormat(year.Text);
        }

        private void date_KeyPress(object sender, KeyPressEventArgs e)
        {
            char keypress = e.KeyChar;

            if (Char.IsControl(keypress))
                return;

            if (Char.IsNumber(keypress) == false)
            {
                e.Handled = true;
            }
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            if (currentCalendar.activeCampaign != null)
            {
                currentCalendar.activeCampaign.setCurrentDate(month.Text + day.Text + year.Text);
                currentCalendar.goToCurrentDate();
            }
            else
                currentCalendar.calendar.setDate(month.Text + day.Text + year.Text);

            UpdateCalendar();
        }

        private void date_TextChanged(object sender, EventArgs e)
        {
            if (month.Text != "" && day.Text != "" && year.Text != "")
                goButton.Show();
            else
                goButton.Hide();
        }
    }
}