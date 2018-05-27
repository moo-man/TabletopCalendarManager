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

public enum noteType { note, generalNote, timer };

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

            noteBox.DisplayMember = "DisplayString";
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

            WriteNotes();

        }

        public void WriteNotes()
        {
            noteBox.Items.Clear();
            List<Timer> timers = currentCalendar.returnTimersToDisplay();
            List<Note> notes = currentCalendar.returnNotesToDisplay();

            if (timers != null)
                foreach (Timer t in timers)
                {
                    noteBox.Items.Add(t);
                }

            foreach (Note n in notes)
            {
                noteBox.Items.Add(n);
            }
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

        // The list has notes that look like "(tag) note contents", so when selecting a note we have a parse 
        // the note contents to give to findNote
        private void CastListObject(object input, out Note noteSelected, out Timer timerSelected)
        {
            timerSelected = null;
            noteSelected = null;
            if (input.GetType() == typeof(Timer))
            {
                timerSelected = (Timer)input;
            }
            else if (input.GetType() == typeof(Note))
            {
                noteSelected = (Note)input;
            }
        }

        private void currentDate_Click(object sender, EventArgs e)
        {

        }

        private void addTimerButton_Click(object sender, EventArgs e)
        {
            new AddTimerForm(currentCalendar).ShowDialog(this);


            UpdateCalendar();
        }

        private void editNotesButton_Click(object sender, EventArgs e)
        {
            if (noteBox.SelectedItem != null)
            {
                CastListObject(noteBox.SelectedItem, out Note selectedNote, out Timer selectedTimer);
                if (selectedNote != null)
                {
                    if (Calendar.CanEditOrDelete(selectedNote) == false)
                    {
                        MessageBox.Show(this, "This note cannot be edited.", "Cannot edit note", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    EditNotesDialog editNoteDialog = new EditNotesDialog(selectedNote, currentCalendar);
                    editNoteDialog.ShowDialog(this);
                    UpdateCalendar();
                }
            }

        }

        private void editTimerButton_Click(object sender, EventArgs e)
        {
            if (noteBox.SelectedItem == null)
                return;

            CastListObject(noteBox.SelectedItem, out Note selectedNote, out Timer timerToEdit);
            if (timerToEdit != null)
            {
                new AddTimerForm(currentCalendar, timerToEdit).ShowDialog();
            }
            UpdateCalendar();
        }

        private void deleteNoteButton_Click(object sender, EventArgs e)
        {
            if (noteBox.SelectedItem != null)
            {
                CastListObject(noteBox.SelectedItem, out Note noteToDelete, out Timer selectedTimer);
                if (noteToDelete != null)
                {

                    if (Calendar.CanEditOrDelete(noteToDelete) == false)
                        MessageBox.Show(this, "This note cannot be deleted", "Cannot delete note", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    else if (MessageBox.Show(this, "Are you sure you wish to delete this note?", "Delete note", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        if (noteToDelete.Campaign == null)
                            currentCalendar.deleteNote(noteToDelete);
                        else
                            noteToDelete.Campaign.deleteNote(noteToDelete);
                        UpdateCalendar();
                    }
                }
            }
        }

        private void deleteButtonTimer_Click(object sender, EventArgs e)
        {
            if (noteBox.SelectedItem == null)
                return;

            CastListObject(noteBox.SelectedItem, out Note selectedNote, out Timer timerToDelete);
            if (timerToDelete != null)
            {
                if (MessageBox.Show(this, "Are you sure you wish to delete this timer?", "Delete timer", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    currentCalendar.activeCampaign.timers.Remove(timerToDelete);

                UpdateCalendar();
            }
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
                if (noteBox.SelectedItem != null)
                {
                    CastListObject(noteBox.SelectedItem, out Note selectedNote, out Timer selectedTimer);

                    if (selectedNote != null)
                    {
                        noteSelectedContextMenu();
                    }
                    else if (selectedTimer != null)
                    {
                        timerSelectedContextMenu();
                    }
                    else
                        noneSelectedContextMenu();
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
            CastListObject(noteBox.SelectedItem, out Note selectedNote, out Timer selectedTimer);
            if (selectedTimer != null)
            {
                selectedTimer.keepTrack = false;
            }
            UpdateCalendar();
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CastListObject(noteBox.SelectedItem, out Note selectedNote, out Timer selectedTimer);
            if (selectedTimer != null)
            {
                selectedTimer.TogglePause(currentCalendar.calendar);
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
    }
}
