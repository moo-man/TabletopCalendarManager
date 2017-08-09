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
                addTimerButton.Enabled = editTimerButton.Enabled = deleteButtonTimer.Enabled = false;
                timerToolStripMenuItem.Enabled = false; // disable toolstrip add timer button
            }

            addNoteButton.Visible = editNotesButton.Visible = deleteNoteButton.Visible = addTimerButton.Visible = editTimerButton.Visible = deleteButtonTimer.Visible = false;
            noteTT.SetToolTip(noteButton, "Notes");
            timerTT.SetToolTip(alarmButton, "Timers");
            showTT.SetToolTip(showHiddenTimersButton, "Show hidden timers");

            noteBox.ContextMenuStrip = noteboxRightClickMenu;
            noneSelectedContextMenu();

            // daytrackerToolStrip.Enabled = false; // placeholder

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

            DetermineTimerButtonVisibility();

            listOfNotes.Clear();

            checkIfTimerPassed();

            currentCalendar.findNotesToList(listOfNotes);

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

        public void checkIfTimerPassed()
        {
            if (currentCalendar.activeCampaign != null)
            {
                foreach (Timer t in currentCalendar.activeCampaign.timers)
                {
                    // If the current date is same date a timer (0 means same date)
                    if (HarptosCalendar.FarthestInTime(t.returnDateString(), currentCalendar.calendar.ToString()) == 0)
                    {
                        if (MessageBox.Show(this, t.message + "\n\nCreate a note?", "Timer Reached", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            new EditNotesDialog(new Note(t.returnDateString(), AlertScope.campaign, t.message, currentCalendar.activeCampaign), currentCalendar).ShowDialog(this);

                        // Remove, then restart updating (can't remove and iterate)
                        currentCalendar.activeCampaign.timers.Remove(t);
                        checkIfTimerPassed();
                        return;
                    }
                    else if (HarptosCalendar.FarthestInTime(t.returnDateString(), currentCalendar.calendar.ToString()) < 0)
                    {
                        if (MessageBox.Show(this, t.message + " (" + HarptosCalendar.returnGivenDate(t.returnDateString()) + ")" + "\n\nCreate a note?", "Timer Passed", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            new EditNotesDialog(new Note(t.returnDateString(), AlertScope.campaign, t.message, currentCalendar.activeCampaign), currentCalendar).ShowDialog(this);
                        if (MessageBox.Show(this, "Go to date? (" + HarptosCalendar.returnGivenDate(t.returnDateString()) + ")", "Go to date", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            currentCalendar.calendar.setDate(t.returnDateString());
                        // Remove, then restart updating (can't remove and iterate)
                        currentCalendar.activeCampaign.timers.Remove(t);
                        checkIfTimerPassed();
                        return;
                    }

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
            currentCalendar.addDay();
            UpdateCalendar();
        }

        private void subDayButton_Click(object sender, EventArgs e)
        {
            currentCalendar.subDay();
            UpdateCalendar();
        }

        private void addTenday_Click(object sender, EventArgs e)
        {
            List<Note> passedNotes = currentCalendar.addTenday();
            UpdateCalendar();
            ShowPassedNotes(passedNotes);
        }

        private void subTenday_Click(object sender, EventArgs e)
        {
            currentCalendar.subTenday();
            UpdateCalendar();
        }

        private void addMonth_Click(object sender, EventArgs e)
        {
            List<Note> passedNotes = currentCalendar.addMonth();
            UpdateCalendar();
            ShowPassedNotes(passedNotes);
        }

        private void subMonth_Click(object sender, EventArgs e)
        {
            currentCalendar.subMonth();
            UpdateCalendar();
        }

        private void addYear_Click(object sender, EventArgs e)
        {
            currentCalendar.addYear();
            UpdateCalendar();
        }

        private void subYear_Click(object sender, EventArgs e)
        {
            currentCalendar.subYear();
            UpdateCalendar();
        }

        #endregion

        /// <summary>
        /// If many days were passed, such as a month, and there were notes on one of the days passed, this function notifies the user
        /// </summary>
        /// <param name="passedNotes"></param>
        public void ShowPassedNotes(List<Note> passedNotes)
        {
            if (passedNotes.Count == 0)
                return;

            // If you land on a note, don't include it, I don't like this solution but it may be the simplest.
            passedNotes.RemoveAll(x => x.Date == currentCalendar.calendar.ToString());

            if (MessageBox.Show(this, passedNotes.Count + " notes were passed. View?", "Passed Notes", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                new PassedNoteGrid(passedNotes).ShowDialog(this);
            }

        }

        private void editNotesButton_Click(object sender, EventArgs e)
        {
            if (noteBox.SelectedItem != null && noteBox.SelectedItem.ToString().Contains("(TIMER)") == false)
            {
                noteType type; // if the selected note is general
                Note selectedNote = currentCalendar.findNote(parseNoteContent(noteBox.SelectedItem.ToString(), out type), type);
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
        private string parseNoteContent(string stringToParse, out noteType type)
        {
            int OpenParenIndex = 0;
            int ClosedParenIndex = 0;

            if (stringToParse.ElementAt(2) != '(' && stringToParse.ElementAt(0) == '\u2022') // Applies to general notes, no tag
            {
                stringToParse = stringToParse.Substring(3).TrimEnd("\n ".ToCharArray()); // remove bullet point and space in the beginning
                type = noteType.generalNote;
            }

            else // if the note belongs to a campaign, trim off tag
            {
                if (stringToParse.Contains("(TIMER)"))
                    type = noteType.timer;
                else
                    type = noteType.note;
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

        private void deleteNoteButton_Click(object sender, EventArgs e)
        {
            if (noteBox.SelectedItem == null || noteBox.SelectedItem.ToString().Contains("(TIMER)"))
                return;
            noteType type;
            Note noteToDelete = currentCalendar.findNote(parseNoteContent(noteBox.SelectedItem.ToString(), out type), type);

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
            UpdateCalendar();
        }

        private void deleteButtonTimer_Click(object sender, EventArgs e)
        {
            if (noteBox.SelectedItem == null || noteBox.SelectedItem.ToString().Contains("(TIMER)") == false)
                return;

            Timer timerToDelete = currentCalendar.activeCampaign.timers.Find(t => t.message == parseNoteContent(noteBox.SelectedItem.ToString(), out noteType type));

            if (MessageBox.Show(this, "Are you sure you wish to delete this timer?", "Delete timer", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                currentCalendar.activeCampaign.timers.Remove(timerToDelete);

            UpdateCalendar();
        }

        private void editTimerButton_Click(object sender, EventArgs e)
        {
            if (noteBox.SelectedItem == null || noteBox.SelectedItem.ToString().Contains("(TIMER)") == false)
                return;

            Timer timerToEdit = currentCalendar.activeCampaign.timers.Find(t => t.message == parseNoteContent(noteBox.SelectedItem.ToString(), out noteType type));

            if (timerToEdit == null)
            {
                MessageBox.Show(this, "Error: Could not find timer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                new AddTimerForm(currentCalendar, timerToEdit).ShowDialog();
            }
            UpdateCalendar();
        }

        private void showHiddenTimersButton_Click(object sender, EventArgs e)
        {
            foreach (Timer t in currentCalendar.activeCampaign.timers)
                t.keepTrack = true;

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

        private void showTimerButtons()
        {
            addNoteButton.Visible = editNotesButton.Visible = deleteNoteButton.Visible = false;
            addTimerButton.Visible = editTimerButton.Visible = deleteButtonTimer.Visible = true;
        }


        private void showNoteButtons()
        {
            addNoteButton.Visible = editNotesButton.Visible = deleteNoteButton.Visible = true;
            addTimerButton.Visible = editTimerButton.Visible = deleteButtonTimer.Visible = false;
        }

        private void noteButton_Click(object sender, EventArgs e)
        {
            showNoteButtons();
        }

        private void alarmButton_Click(object sender, EventArgs e)
        {
            showTimerButtons();
        }

        private void noteSelectedContextMenu()
        {
            editToolStripMenuItem1.Enabled = true;  // Note edit
            deleteToolStripMenuItem1.Enabled = true;// Note delete

            editToolStripMenuItem.Enabled = false;  // Timer edit
            deleteToolStripMenuItem.Enabled = false;// Timer delete
            hideToolStripMenuItem.Enabled = false;  // Timer hide
        }

        private void timerSelectedContextMenu()
        {
            editToolStripMenuItem1.Enabled = false;  // Note edit
            deleteToolStripMenuItem1.Enabled = false;// Note delete

            editToolStripMenuItem.Enabled = true;  // Timer edit
            deleteToolStripMenuItem.Enabled = true;// Timer delete
            hideToolStripMenuItem.Enabled = true;  // Timer hide

        }

        private void noneSelectedContextMenu()
        {
            editToolStripMenuItem1.Enabled = false;  // Note edit
            deleteToolStripMenuItem1.Enabled = false;// Note delete

            editToolStripMenuItem.Enabled = false;  // Timer edit
            deleteToolStripMenuItem.Enabled = false;// Timer delete
            hideToolStripMenuItem.Enabled = false;  // Timer hide
        }

        private void noteBox_MouseDown(object sender, MouseEventArgs e)
        {
            noteBox.SelectedIndex = noteBox.IndexFromPoint(e.X, e.Y);



            if (e.Button == MouseButtons.Right)
            {
                noteType selectedType;
                if (noteBox.SelectedItem != null)
                {
                    parseNoteContent(noteBox.SelectedItem.ToString(), out selectedType);

                    switch (selectedType)
                    {
                        case noteType.generalNote:
                            noteSelectedContextMenu();
                            break;
                        case noteType.note:
                            noteSelectedContextMenu();
                            break;
                        case noteType.timer:
                            timerSelectedContextMenu();
                            break;
                    }
                }
                else
                    noneSelectedContextMenu();

                // if no campaign, disable timers
                if (currentCalendar.activeCampaign == null)
                    timersToolStripMenuItem.Enabled = false;
            }
        }

        private void DetermineTimerButtonVisibility()
        {
            if (currentCalendar.activeCampaign != null && currentCalendar.activeCampaign.hiddenTimerCount() != 0)
            {
                showHiddenTimersButton.Show();
                showHiddenTimersToolStripMenuItem.Enabled = true;
            }
            else
            {
                showHiddenTimersButton.Hide();
                showHiddenTimersToolStripMenuItem.Enabled = false;
            }
        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Timer t = currentCalendar.activeCampaign.timers.Find(x => x.message == parseNoteContent(noteBox.SelectedItem.ToString(), out noteType type));
            if (t != null)
            {
                t.keepTrack = false;
            }
            UpdateCalendar();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DayTracker_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
                save_Click(sender, e);

            if (e.Control && e.KeyCode == Keys.N)
                addNoteButton_Click(sender, e);

            if (e.Control && e.KeyCode == Keys.T)
                addTimerButton_Click(sender, e);

            if (e.Control && e.KeyCode == Keys.Right)
                addDayButton_Click(sender, e);

            if (e.Control && e.KeyCode == Keys.Left)
                subDayButton_Click(sender, e);
        }

        private void save_Click(object sender, EventArgs e)
        {
            Utility.Save(currentCalendar);
        }

        private void moonNameLabel_Click(object sender, EventArgs e)
        {

            String messageString = "Open wiki page on Selune?";

            DialogResult result = MessageBox.Show(this, messageString, "Opening wiki page", MessageBoxButtons.YesNo, MessageBoxIcon.None);

            if (result == DialogResult.Yes)
            {
                string webpage = ("http://forgottenrealms.wikia.com/wiki/Sel%C3%BBne_(moon)");

                ProcessStartInfo sInfo = new ProcessStartInfo(webpage);
                Process.Start(sInfo);
            }

        }

        private void calendarOfHarptosToolStripMenuItem_Click(object sender, EventArgs e)
        {

            String messageString = "Open wiki page on the Calendar of Harptos?";

            DialogResult result = MessageBox.Show(this, messageString, "Opening wiki page", MessageBoxButtons.YesNo, MessageBoxIcon.None);

            if (result == DialogResult.Yes)
            {
                string webpage = ("http://forgottenrealms.wikia.com/wiki/Calendar_of_Harptos");

                ProcessStartInfo sInfo = new ProcessStartInfo(webpage);
                Process.Start(sInfo);
            }
        }

        private void HolidayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var holidayButton = (ToolStripMenuItem)sender;

            String messageString = "Open wiki page on " + holidayButton.Text + "?";

            DialogResult result = MessageBox.Show(this, messageString, "Opening wiki page", MessageBoxButtons.YesNo, MessageBoxIcon.None);

            if (result == DialogResult.Yes)
            {
                string webpage = ("http://forgottenrealms.wikia.com/wiki/" + holidayButton.Text.Replace(' ', '_'));

                ProcessStartInfo sInfo = new ProcessStartInfo(webpage);
                Process.Start(sInfo);
            }
        }
    }
}