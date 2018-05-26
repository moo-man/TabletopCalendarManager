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

public enum noteType { note, generalNote, timer, universal };

namespace CalendarManager
{
    public partial class DayTracker : Form
    {
        static Calendar currentCalendar; // Changed to static, beware of issues
        List<Note> listOfNotes;
        List<PictureBox> moonPictures;

        public DayTracker(Calendar currCalendar)
        {
            InitializeComponent();
            currentCalendar = currCalendar;
            listOfNotes = new List<Note>();

            if (currentCalendar.activeCampaign == null)
            {
                addTimerButton.Enabled = editTimerButton.Enabled = deleteButtonTimer.Enabled = false;
                timerToolStripMenuItem.Enabled = false;
            }

            addNoteButton.Visible = editNotesButton.Visible = deleteNoteButton.Visible = addTimerButton.Visible = editTimerButton.Visible = deleteButtonTimer.Visible = false;
            noteTT.SetToolTip(noteButton, "Notes");
            timerTT.SetToolTip(alarmButton, "Timers");
            showTT.SetToolTip(showHiddenTimersButton, "Show hidden timers");

            noteBox.ContextMenuStrip = noteboxRightClickMenu;
            noneSelectedContextMenu();


            Point moonPictureLocation = new Point(moonsLabel.Location.X + 60, moonsLabel.Location.Y - 10);
            moonPictures = new List<PictureBox>();
            moonsLabel.Text = currentCalendar.calendar.returnMoonNames();
            moonsLabel.SendToBack();
            for (int i = 0; i < currentCalendar.calendar.currentMoonPhase().Length; i++)
            {
                moonPictures.Add(new PictureBox());
                moonPictures[i].Location = moonPictureLocation;
                moonPictures[i].Size = new Size(30, 30);
                moonPictures[i].SizeMode = PictureBoxSizeMode.StretchImage;
                moonPictures[i].BringToFront();
                this.Controls.Add(moonPictures[i]);

                moonPictureLocation.Y += 53;
            }

            /*if (currentCalendar.calendar.calendarName != "")
                titleText.Text = "The Calendar of " + currentCalendar.calendar.calendarName;
            else
                titleText.Text = "";*/
            goButton.Hide();
            UpdateCalendar();
            setFonts();
        }

        public void setFonts()
        {
            MainMenu.applyFont(titleText, 1);
            MainMenu.applyFont(currentCampaignLabel, 0);
            MainMenu.applyFont(campaignName, 0);
            MainMenu.applyFont(currentDate, 0);
            MainMenu.applyFont(dayLabel, 0, FontStyle.Bold);
            MainMenu.applyFont(weekLabel, 0, FontStyle.Bold);
            MainMenu.applyFont(monthLabel, 0, FontStyle.Bold);
            MainMenu.applyFont(yearlabel, 0, FontStyle.Bold);
            MainMenu.applyFont(notesLabel, 0);
            MainMenu.applyFont(gotoLabel, 0, FontStyle.Bold);
        }

        private void UpdateCalendar()
        {
            if (currentCalendar.activeCampaign == null)
                campaignName.Text = "None";
            else
            {
                campaignName.Text = currentCalendar.activeCampaign.Name;
                currentCalendar.activeCampaign.setCurrentDate(currentCalendar.calendar.ToString());
            }

            currentDate.Text = currentCalendar.calendar.returnCurrentDateWithWeekday();

            displayMoons();
            DetermineTimerButtonVisibility();

            listOfNotes.Clear();

            checkIfTimerPassed();

            listOfNotes = currentCalendar.findNotesToList();


            //if (currentCalendar.activeCampaign != null)
            //{
            //    foreach (Timer t in currentCalendar.activeCampaign.timers)
            //    {
            //        if (CalendarType.FarthestInTime(t.returnDateString(), currentCalendar.calendar.ToString()) == 0)
            //        {
            //            if (MessageBox.Show(this, t.message + "\n\nCreate a note?", "Timer Reached", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            //                new EditNotesDialog(new Note(t.returnDateString(), AlertScope.campaign, t.message, currentCalendar.activeCampaign), currentCalendar).ShowDialog(this);

            //            // Remove, then restart updating (can't remove and iterate)
            //            currentCalendar.activeCampaign.timers.Remove(t);
            //            UpdateCalendar();
            //            return;
            //        }
            //        else if (CalendarType.FarthestInTime(t.returnDateString(), currentCalendar.calendar.ToString()) < 0)
            //        {
            //            if (MessageBox.Show(this, t.message + " (" + CalendarType.returnGivenDate(t.returnDateString()) + ")" + "\n\nCreate a note?", "Timer Reached", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            //                new EditNotesDialog(new Note(t.returnDateString(), AlertScope.campaign, t.message, currentCalendar.activeCampaign), currentCalendar).ShowDialog(this);
            //            if (MessageBox.Show(this, "Go to date? (" + CalendarType.returnGivenDate(t.returnDateString()) + ")", "Go to date", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //                currentCalendar.calendar.setDate(t.returnDateString());
            //            // Remove, then restart updating (can't remove and iterate)
            //            currentCalendar.activeCampaign.timers.Remove(t);
            //            UpdateCalendar();
            //            return;
            //        }

            //    }
            //}


            //foreach (Note n in currentCalendar.GeneralNoteList)
            //{
            //    if (n.Importance == AlertScope.global && currentCalendar.calendar.isAnniversary(n.Date) || (n.Importance == AlertScope.dontAlert && currentCalendar.calendar.sameDate(n.Date)))
            //        listOfNotes.Add(n);
            //}

            //foreach (Campaign c in currentCalendar.CampaignList)
            //{
            //    foreach (Note n in c.notes)
            //    {
            //        if (c.Equals(currentCalendar.activeCampaign)) // If the note belongs to current campaign, and has appropriate visibilty, and is anniversary of this date
            //        {
            //            if ((n.Importance == AlertScope.campaign || n.Importance == AlertScope.global) && currentCalendar.calendar.isAnniversary(n.Date))
            //            {
            //                if (n.NoteContent.Equals("Current Date") == false) // don't print the current date of current campaign, as that is always the current date
            //                    listOfNotes.Add(n);
            //            }
            //            else if (n.Importance == AlertScope.dontAlert && currentCalendar.calendar.sameDate(n.Date))
            //                    listOfNotes.Add(n);
            //        }

            //        else // If the note does not belong in the current campaign
            //            if ((n.Importance == AlertScope.global) && currentCalendar.calendar.isAnniversary(n.Date)) // if the note happened on this day and is of                                                                                        // sufficient importance level
            //                    listOfNotes.Add(n);


            //    } // end foreach note
            //} // end foreach campaign

            if (currentCalendar.activeCampaign != null)
                writeNotes(currentCalendar.activeCampaign.timers, listOfNotes);
            else
                writeNotes(null, listOfNotes);
        }

        public void writeNotes(List<Timer> timerList, List<Note> list)
        {
            noteBox.Items.Clear();


            string UniversalNote = currentCalendar.calendar.ReturnUniversalNoteContent();
            if (UniversalNote != null)
            {
                noteBox.Items.Add("\u2022 (Holiday) " + UniversalNote);
            }


            if (timerList != null)
                foreach (Timer t in timerList)
                {
                    if (t.keepTrack && currentCalendar.calendar.sameDate(t.returnDateString()) == false)
                    {
                        int numDays = (currentCalendar.calendar.daysTo(t.returnDateString()));
                        if (t.pausedTime == 0)
                        {
                            if (numDays > 1)
                                noteBox.Items.Add("(TIMER) " + t.message + " (in " + currentCalendar.calendar.daysTo(t.returnDateString()) + " days)");
                            else
                                noteBox.Items.Add("(TIMER) " + t.message + " (in " + currentCalendar.calendar.daysTo(t.returnDateString()) + " days)");
                        }
                        else
                        {
                            if (numDays > 1)
                                noteBox.Items.Add("(TIMER)(PAUSED) " + t.message + " (in " + currentCalendar.calendar.daysTo(t.returnDateString()) + " days)");
                            else
                                noteBox.Items.Add("(TIMER)(PAUSED) " + t.message + " (in " + currentCalendar.calendar.daysTo(t.returnDateString()) + " days)");

                        }
                    }
                }

            foreach (Note n in list)
            {
                noteBox.Items.Add(ReturnNoteDisplay(n));
            }
        }

        public string ReturnNoteDisplay(Note n)
        {
            if (n.isGeneral()) // if note is general
            {
                if (currentCalendar.calendar.yearsAgo(n.Date) == 1)
                    return ("\u2022 " + " " + ReturnContentAndDateDifference(n));
                else if (currentCalendar.calendar.yearsAgo(n.Date) > 1)                                                                                                        // Note happened in past
                    return ("\u2022 " + " " + ReturnContentAndDateDifference(n));
                else if ((currentCalendar.calendar.yearsAgo(n.Date) == 0))
                    return ("\u2022 " + " " + ReturnContentAndDateDifference(n));                                                                                                 // Note happened this very day
                else if (currentCalendar.calendar.yearsAgo(n.Date) == -1)
                    return ("\u2022 " + " " + ReturnContentAndDateDifference(n));
                else if (currentCalendar.calendar.yearsAgo(n.Date) < -1)                                                                                                       // Note happens in future
                    return ("\u2022 " + " " + ReturnContentAndDateDifference(n));
                else
                    return ("Error.");
            }

            else // if note belongs to a campaign
            {
                if (currentCalendar.calendar.yearsAgo(n.Date) == 1)
                    return ("\u2022 " + "(" + n.Campaign.Tag + ") " + ReturnContentAndDateDifference(n));
                else if (currentCalendar.calendar.yearsAgo(n.Date) > 1)                                                                                                        // Note happened in past
                    return ("\u2022 " + "(" + n.Campaign.Tag + ") " + ReturnContentAndDateDifference(n));
                else if ((currentCalendar.calendar.yearsAgo(n.Date) == 0))
                    return ("\u2022 " + "(" + n.Campaign.Tag + ") " + ReturnContentAndDateDifference(n));                                                                         // Note happened this very day
                else if (currentCalendar.calendar.yearsAgo(n.Date) == -1)
                    return ("\u2022 " + "(" + n.Campaign.Tag + ") " + ReturnContentAndDateDifference(n));
                else if (currentCalendar.calendar.yearsAgo(n.Date) < -1)                                                                                                       // Note happens in future
                    return ("\u2022 " + "(" + n.Campaign.Tag + ") " + ReturnContentAndDateDifference(n));
                else
                    return ("Error.");
            }
        }


        /// <summary>
        /// Returns the contents of the note, with the difference of time of the current date
        /// Eg. "Note Content" (5 years ago)
        /// Necessary for PassedNoteGrid to call
        /// </summary>
        /// <param name="n">Note whose contents are being returned</param>
        /// <returns></returns>
        public static string ReturnContentAndDateDifference(Note n)
        {
            if (currentCalendar.calendar.yearsAgo(n.Date) == 1)
                return (n.NoteContent + " (" + currentCalendar.calendar.yearsAgo(n.Date) + " year ago)\n");
            else if (currentCalendar.calendar.yearsAgo(n.Date) > 1)                                                    // Note happened in past
                return (n.NoteContent + " (" + currentCalendar.calendar.yearsAgo(n.Date) + " years ago)\n");
            else if ((currentCalendar.calendar.yearsAgo(n.Date) == 0))
                return (n.NoteContent + "\n");                                                                         // Note happened this very day
            else if (currentCalendar.calendar.yearsAgo(n.Date) == -1)
                return (n.NoteContent + " (in " + Math.Abs(currentCalendar.calendar.yearsAgo(n.Date)) + " year)\n");
            else if (currentCalendar.calendar.yearsAgo(n.Date) < -1)                                                   // Note happens in future
                return (n.NoteContent + " (in " + Math.Abs(currentCalendar.calendar.yearsAgo(n.Date)) + " years)\n");
            else
                return ("Error.");
        }

        public void checkIfTimerPassed()
        {
            if (currentCalendar.activeCampaign != null)
            {
                foreach (Timer t in currentCalendar.activeCampaign.timers)
                {
                    t.AdjustForPause(currentCalendar.calendar); // If paused, adjust timer

                    // If the current date is same date a timer (0 means same date)
                    if (CalendarType.FarthestInTime(t.returnDateString(), currentCalendar.calendar.ToString()) == 0)
                    {
                        if (MessageBox.Show(this, t.message + "\n\nCreate a note?", "Timer Reached", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            new EditNotesDialog(new Note(t.returnDateString(), AlertScope.campaign, t.message, currentCalendar.activeCampaign), currentCalendar).ShowDialog(this);

                        // Remove, then restart updating (can't remove and iterate)
                        currentCalendar.activeCampaign.timers.Remove(t);
                        checkIfTimerPassed();
                        return;
                    }
                    else if (CalendarType.FarthestInTime(t.returnDateString(), currentCalendar.calendar.ToString()) < 0)
                    {
                        if (MessageBox.Show(this, t.message + " (" + CalendarType.returnGivenDateWithWeekday(t.returnDateString()) + ")" + "\n\nCreate a note?", "Timer Passed", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            new EditNotesDialog(new Note(t.returnDateString(), AlertScope.campaign, t.message, currentCalendar.activeCampaign), currentCalendar).ShowDialog(this);
                        if (MessageBox.Show(this, "Go to date? (" + CalendarType.returnGivenDateWithWeekday(t.returnDateString()) + ")", "Go to date", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            currentCalendar.calendar.setDate(t.returnDateString());
                        // Remove, then restart updating (can't remove and iterate)
                        currentCalendar.activeCampaign.timers.Remove(t);
                        checkIfTimerPassed();
                        return;
                    }
                }
            }
        }

        public void displayMoons()
        {
            string[] moonPhases = currentCalendar.calendar.currentMoonPhase();
            for (int i = 0; i < moonPhases.Length; i++)
            {
                moonPictures[i].Image = (Image)Properties.Resources.ResourceManager.GetObject(moonPhases[i].Replace(' ', '_'));
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
            Utility.AutoSave(currentCalendar);
        }

        private void subDayButton_Click(object sender, EventArgs e)
        {
            currentCalendar.calendar.subDay();
            UpdateCalendar();
        }

        private void addWeek_Click(object sender, EventArgs e)
        {
            List<Tuple<Note, string>> passedNotes = currentCalendar.addWeek();
            UpdateCalendar();
            ShowPassedNotes(passedNotes);
            Utility.AutoSave(currentCalendar);
        }

        private void subWeek_Click(object sender, EventArgs e)
        {
            currentCalendar.calendar.subWeek();
            UpdateCalendar();
        }

        private void addMonth_Click(object sender, EventArgs e)
        {
            List<Tuple<Note, string>> passedNotes = currentCalendar.addMonth();
            UpdateCalendar();
            Utility.AutoSave(currentCalendar);
            ShowPassedNotes(passedNotes);
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
            Utility.AutoSave(currentCalendar);
        }

        private void subYear_Click(object sender, EventArgs e)
        {
            currentCalendar.calendar.subYear();
            UpdateCalendar();
        }

        #endregion

        /// <summary>
        /// If many days were passed, such as a month, and there were notes on one of the days passed, this function notifies the user
        /// </summary>
        /// <param name="passedNotes"></param>
        public void ShowPassedNotes(List<Tuple<Note, string>> passedNotes)
        {
            if (passedNotes.Count == 0)
                return;

            // If you land on a note, don't include it, I don't like this solution but it may be the simplest.
            passedNotes.RemoveAll(x => x.Item1.Date == currentCalendar.calendar.ToString());

            if (MessageBox.Show(this, passedNotes.Count + " notes were passed. View?", "Passed Notes", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                PassedNoteGrid noteGrid = new PassedNoteGrid(passedNotes, currentCalendar.calendar.ToString());
                noteGrid.Show(this);
                noteGrid.GoToDate += goButton_Click;
                // I don't know how events really work but this seems to fine, make it static and increment in the constructor?
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

            if (stringToParse.Substring(0, 11) == "\u2022 (Holiday)")
            {
                type = noteType.universal;
                return null;
            }

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

                if (stringToParse.Contains("(PAUSED)") && type == noteType.timer)
                {
                    for (int i = ClosedParenIndex + 2; i < stringToParse.Length && stringToParse[i] != ')'; i++)
                        ClosedParenIndex = i;
                }

                int startIndex = 0;

                if (OpenParenIndex < ClosedParenIndex)
                    startIndex = ClosedParenIndex + 3;
                else
                    stringToParse = null;

                stringToParse = stringToParse.Substring(startIndex).TrimEnd('\n');
            }

            // This part applies if the current date is not the note's date
            // Trims off the (x years ago) or (in x years)
            if (stringToParse.ElementAt(stringToParse.Length - 1) == ')')
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

        private void deleteButtonTimer_Click(object sender, EventArgs e)
        {
            if (noteBox.SelectedItem == null || noteBox.SelectedItem.ToString().Contains("(TIMER)") == false)
                return;

            Timer timerToDelete = currentCalendar.activeCampaign.timers.Find(t => t.message == parseNoteContent(noteBox.SelectedItem.ToString(), out noteType type));

            if (MessageBox.Show(this, "Are you sure you wish to delete this timer?", "Delete timer", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                currentCalendar.activeCampaign.timers.Remove(timerToDelete);

            UpdateCalendar();
        }

        private void showHiddenTimersButton_Click(object sender, EventArgs e)
        {
            foreach (Timer t in currentCalendar.activeCampaign.timers)
                t.keepTrack = true;

            UpdateCalendar();
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

        private void date_Leave(object sender, EventArgs e)
        {

            year.Text = CalendarType.enforceYearFormat(year.Text);
            month.Text = CalendarType.enforceMonthFormat(month.Text);
            day.Text = CalendarType.enforceDayFormat(month.Text, day.Text, year.Text);

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

        /// <summary>
        /// Go to event coming from PassedNoteGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goButton_Click(object sender, GoToEventArgs e)
        {
            if (currentCalendar.activeCampaign != null)
            {
                currentCalendar.activeCampaign.setCurrentDate(e.date);
                currentCalendar.goToCurrentDate();
            }
            else
                currentCalendar.calendar.setDate(e.date);

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
            pauseToolStripMenuItem.Enabled = false;
        }

        private void timerSelectedContextMenu()
        {
            editToolStripMenuItem1.Enabled = false;  // Note edit
            deleteToolStripMenuItem1.Enabled = false;// Note delete

            editToolStripMenuItem.Enabled = true;  // Timer edit
            deleteToolStripMenuItem.Enabled = true;// Timer delete
            hideToolStripMenuItem.Enabled = true;  // Timer hide
            pauseToolStripMenuItem.Enabled = true;

        }

        private void noneSelectedContextMenu()
        {
            editToolStripMenuItem1.Enabled = false;  // Note edit
            deleteToolStripMenuItem1.Enabled = false;// Note delete

            editToolStripMenuItem.Enabled = false;  // Timer edit
            deleteToolStripMenuItem.Enabled = false;// Timer delete
            hideToolStripMenuItem.Enabled = false;  // Timer hide
            pauseToolStripMenuItem.Enabled = false;
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
    "If you have an active campaign, the Day Tracker should be set to its current date. As you advance (or reverse) by day, week, etc. it will correspondingly change the campaign's current date",
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
            if (e.Control && e.Shift && e.KeyCode == Keys.S)
                saveAsToolStripMenuItem_Click(sender, e);

            else if (e.Control && e.KeyCode == Keys.S)
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

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utility.SaveAs(currentCalendar);
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Timer t = currentCalendar.activeCampaign.timers.Find(x => x.message == parseNoteContent(noteBox.SelectedItem.ToString(), out noteType type));

            if (t != null)
            {
                t.TogglePause(currentCalendar.calendar);
            }
            UpdateCalendar();
        }
    }
}
