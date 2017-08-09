﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarptosCalendarManager
{
    // Notes have 3 levels of importance
    // dontAlert will not alert any campaign when that date is reached
    // alertCampaign will alert the campaign that the note belongs to
    // alertAll will alert all campaigns in this calendar
    public enum AlertScope { dontAlert, campaign, global }

    public class Calendar
    {
        [Newtonsoft.Json.JsonIgnore]
        public HarptosCalendar calendar;

        List<Campaign> campaignList;
        public List<Campaign> CampaignList
        {
            get
            { return campaignList; }

            set
            { campaignList = value; }
        }
        List<Note> generalNoteList;
        public List<Note> GeneralNoteList
        {
            get
            { return generalNoteList; }

            set
            { generalNoteList = value; }
        }

        [Newtonsoft.Json.JsonIgnore]
        public Campaign activeCampaign;

        public Calendar()
        {
            activeCampaign = null;
            calendar = new HarptosCalendar();
            campaignList = new List<Campaign>();
            generalNoteList = new List<Note>();
        }

        public Calendar(dynamic json) : this()
        {

            foreach (var campaign in json["CampaignList"])
                AddCampaign(new Campaign(campaign));

            foreach (var note in json["GeneralNoteList"])
            {
                Note loadedNote = new Note(note);
                loadedNote.Campaign = null;
                AddGeneralNote(loadedNote);
            }
        }

        public void addNewCampaign(string name, string t, string startDate)
        {
            Campaign newCampaign = new Campaign(name, t, startDate);
            campaignList.Add(newCampaign);
        }

        public void addLoadedCampaign(string name, string t, string currentDate)
        {
            // since the constructor of campaign requires start date (then adds a note of start date)
            // we have to start a blank campaign and add to that, kinda gross but whatev
            Campaign loadedCampaign = new Campaign();
            loadedCampaign.Name = name;
            loadedCampaign.Tag = t;
            loadedCampaign.CurrentDate = currentDate;
            campaignList.Add(loadedCampaign);
        }

        public void AddGeneralNote(Note noteToAdd)
        {
            if (noteToAdd.Campaign != null)
                return;
            else
                generalNoteList.Add(noteToAdd);
        }

        public void AddNote(Note noteToAdd)
        {
            if (noteToAdd.Campaign == null)
            {
                generalNoteList.Add(noteToAdd);
                generalNoteList.Sort(delegate (Note x, Note y)
                {
                    return Note.compareNotes(x, y);
                });
            }
            else
                noteToAdd.Campaign.addNote(noteToAdd);
        }

        public void AddCampaign(Campaign c)
        {
            campaignList.Add(c);
        }

        public bool deleteCampaign(string tag)
        {
            return campaignList.Remove(campaignList.Find(x => x.Tag == tag));
        }

        public int numOfCampaigns()
        {
            return campaignList.Count();
        }

        public void setActiveCampaign(int index)
        {
            setActiveCampaign(campaignList[index]);
        }

        public void setActiveCampaign(Campaign c)
        {
            if (campaignList.Contains(c))
            {
                activeCampaign = c;
                calendar = new HarptosCalendar(
                Int32.Parse(activeCampaign.CurrentDate.Substring(0, 2)),
                Int32.Parse(activeCampaign.CurrentDate.Substring(2, 2)),
                Int32.Parse(activeCampaign.CurrentDate.Substring(4, 4)));
            }
            else
                activeCampaign = null;
        }

        /// <summary>
        /// goToCurrentDate() function sets the HarptosCalendar to the current date of the ACTIVE campaign
        /// </summary>
        public void goToCurrentDate()
        {
            if (activeCampaign == null)
                return;
            calendar.setDate(activeCampaign.CurrentDate);
        }

        #region Forward in time
        /// <summary>
        /// Move to the next day in the calendar
        /// </summary>
        public List<Note> addDay()
        {
            calendar.addDay();

            List<Note> notesOnThisDay = new List<Note>();
            findNotesToList(notesOnThisDay);
            return notesOnThisDay;
        }

        /// <summary>
        /// Move to the next n days in the calendar
        /// </summary>
        /// <param name="num">The number of days passing</param>
        public List<Note> addDay(int num)
        {
            List<Note> notesPassed = new List<Note>();
            for (int i = 0; i < num; i++)
            {
                notesPassed.AddRange(addDay());
            }
            return notesPassed;
        }

        public List<Note> addTenday()
        {
            return addDay(10);
        }

        public List<Note> addMonth()
        {
            return addDay(30);
        }

        public List<Note> addMonth(int num)
        {
            List<Note> notesPassed = new List<Note>();
            for (int i = 0; i < num; i++)
                notesPassed.AddRange(addMonth());
            return notesPassed;
        }

        public void addYear()
        {
            calendar.addYear();
        }

        public void addYear(int num)
        {
            for (int i = 0; i < num; i++)
                addYear();
        }
        #endregion

        #region Backward in time
        public void subDay()
        {
            calendar.subDay();

        }

        public void subDay(int num)
        {
            for (int i = 0; i < num; i++)
                subDay();
        }

        public void subTenday()
        {
            subDay(10);
        }

        public void subMonth()
        {
            subDay(30);
        }

        public void subYear()
        {
            calendar.subYear();
        }
        #endregion


        /// <summary>
        /// Finds all notes that should be listed based on the current date of the calendar
        /// </summary>
        /// <param name="listOfNotes">list of notes to put into and return</param>
        /// <returns></returns>
        public void findNotesToList(List<Note> listOfNotes)
        {
            listOfNotes.Clear();

            foreach (Note n in GeneralNoteList)
            {
                if (n.Importance == AlertScope.global && calendar.isAnniversary(n.Date) || (n.Importance == AlertScope.dontAlert && calendar.sameDate(n.Date)))
                    listOfNotes.Add(n);
            }

            foreach (Campaign c in CampaignList)
            {
                foreach (Note n in c.notes)
                {
                    if (c.Equals(activeCampaign)) // If the note belongs to current campaign, and has appropriate visibilty, and is anniversary of this date
                    {
                        if ((n.Importance == AlertScope.campaign || n.Importance == AlertScope.global) && calendar.isAnniversary(n.Date))
                        {
                            if (n.NoteContent.Equals("Current Date") == false) // don't print the current date of current campaign, as that is always the current date
                                listOfNotes.Add(n);
                        }
                        else if (n.Importance == AlertScope.dontAlert && calendar.sameDate(n.Date))
                            listOfNotes.Add(n);
                    }

                    else // If the note does not belong in the current campaign
                        if ((n.Importance == AlertScope.global) && calendar.isAnniversary(n.Date)) // if the note happened on this day and is of                                                                                        // sufficient importance level
                        listOfNotes.Add(n);
                } // end foreach note
            } // end foreach campaign

            //return listOfNotes;
        }

        public Note findNote(string content, noteType type)
        {
            if (type == noteType.generalNote)
            {
                foreach (Note n in generalNoteList)
                    if (n.NoteContent == content)
                        return n;
            }

            else
            {
                foreach (Campaign c in campaignList)
                    foreach (Note n in c.notes)
                        if (n.NoteContent == content)
                            return n;
            }
            return null;
        }

        public void deleteNote(string content, noteType type)
        {
            Note noteToDelete = findNote(content, type);
            deleteNote(noteToDelete);
        }

        public void deleteNote(Note noteToDelete)
        {
            if (noteToDelete == null)
                return;

            if (noteToDelete.Campaign == null)
                generalNoteList.Remove(noteToDelete);
            else
                noteToDelete.Campaign.deleteNote(noteToDelete);
        }
        public static bool CanEditOrDelete(Note noteToTest)
        {
            if (noteToTest.Campaign != null &&                                          // If campaign is not null (note not general)
        (noteToTest.Campaign.getCurrentDateOrEndNote() == noteToTest ||   // AND (the note is not the currentdate note OR the begin note)
        noteToTest.Campaign.returnBeginNote() == noteToTest))
                return false;
            else
                return true;
        }

        public void saveButton()
        {
            Utility.Save(this);
        }
    }

    public class Campaign
    {
        string tag;
        string name;
        public List<Note> notes;
        public List<Timer> timers;
        string currentDate;

        public Campaign(string n, string t, string startDate) : this(n, t, startDate, startDate)
        {

        }

        public Campaign(string n, string t, string startDate, string currDate)
        {
            notes = new List<Note>();
            timers = new List<Timer>();
            name = n;
            tag = t;
            if (Note.VerifyDate(startDate))
            {
                string msg = name + " began!";
                addNote(startDate, AlertScope.global, msg);
                currentDate = currDate;
            }
            if (Note.VerifyDate(currDate))
            {
                string msg = "Current Date";
                addNote(currentDate, AlertScope.global, msg);
                currentDate = currDate;
            }
        }

        public Campaign()
        {
            notes = new List<Note>();
            timers = new List<Timer>();
        }
        public Campaign(dynamic campaignJson)
        {
            notes = new List<Note>();
            foreach (var note in campaignJson["notes"])
            {
                Note loadedNote = new Note(note);
                addNote(loadedNote);
            }

            timers = new List<Timer>();
            foreach (var timer in campaignJson["timers"])
            {
                Timer loadedTimer = new Timer(timer);
                addTimer(loadedTimer);
            }

            Tag = campaignJson["Tag"];
            Name = campaignJson["Name"];
            CurrentDate = campaignJson["CurrentDate"];
        }

        public string Tag
        {
            get { return tag; }
            set { tag = fixTag(value); }
        }

        public string Name
        {
            get { return name; }
            set { ChangeName(value); }
        }

        public string CurrentDate
        {
            get { return currentDate; }
            set { setCurrentDate(value); }
        }

        public void setCurrentDate(string newDate)
        {
            Note currOrEndNote = getCurrentDateOrEndNote();
            currOrEndNote.Date = newDate;
            currentDate = newDate;
            sortNotes();
        }

        public void ChangeName(string newName)
        {
            if (name == null)
                name = newName;
            else
            {
                Note endNote = getCurrentDateOrEndNote();
                if (isEnded())
                    endNote.NoteContent = newName + " ended.";
                else
                    endNote.NoteContent = "Current Date";

                Note startNote = notes.Find(x => x.NoteContent == name + " began!");
                startNote.editNoteContent(newName + " began!");

                name = newName;
            }
        }

        public void setStartDate(string newStartDate)
        {
            Note startNote = notes.Find(x => x.NoteContent == name + " began!");
            startNote.editDate(newStartDate);
        }

        /*public void setStartDate(string newStartDate, string newCampaigName)
        {
            Note startNote = notes.Find(x => x.NoteContent == name + " began!");
            startNote.editDate(newStartDate);
            startNote.editNoteContent(newCampaigName + " began!");
        }*/

        public void setCurrentDate(int m, int d, int y)
        {
            StringBuilder newDate = new StringBuilder();
            if (m < 10)
                newDate.Append("0" + m);
            else
                newDate.Append(m);

            if (d < 10)
                newDate.Append("0" + d);
            else
                newDate.Append(d);

            string yString = y.ToString();
            while (yString.Length < 4)
                yString.Insert(0, "0");

            newDate.Append(yString);

            setCurrentDate(newDate.ToString());
        }

        public Note returnBeginNote()
        {
            return notes.Find(x => x.NoteContent == Name + " began!");
        }

        #region starting/ending campaign
        public void endCampaign()
        {
            Note endNote = findNote("Current Date");
            if (endNote == null)
                return;
            endNote.NoteContent = endNote.Campaign.Name + " ended.";
        }

        public void startCampaign()
        {
            Note endNote = findNote(Name + " ended.");
            if (endNote == null)
                return;
            endNote.NoteContent = "Current Date";
        }

        public bool isEnded()
        {
            if (findNote("Current Date") == null && findNote(Name + " ended.") != null)
                return true;

            else return false;
        }

        public void toggleEnded()
        {
            if (isEnded())
                startCampaign();
            else
                endCampaign();
        }
        #endregion

        public void addNote(string date, AlertScope importance, string note)
        {
            notes.Add(new Note(date, importance, note, this));
            sortNotes();
        }

        public void addNote(Note noteToAdd)
        {
            addNote(noteToAdd.Date, noteToAdd.Importance, noteToAdd.NoteContent);
        }

        public bool deleteNote(Note noteToDelete)
        {
            return notes.Remove(notes.Find(x => x.Equals(noteToDelete)));
        }

        public static string fixTag(string t)
        {
            if (t.Length > 10)
                return t.Substring(0, 10).ToUpper();
            else
                return t.ToUpper();
        }

        // Returns the note "Current Date" or "ended"
        public Note getCurrentDateOrEndNote()
        {
            if (isEnded())
                return findNote(Name + " ended.");
            else
                return findNote("Current Date");
        }


        public void sortNotes()
        {
            notes.Sort(delegate (Note x, Note y)
            {
                return Note.compareNotes(x, y);
            });
        }

        public Note findNote(string content)
        {
            foreach (Note n in notes)
                if (n.NoteContent == content)
                    return n;
            return null;
        }

        public void addTimer(Timer t)
        {
            timers.Add(t);
        }

        public int hiddenTimerCount()
        {
            int count = 0;
            foreach (Timer t in timers)
                if (t.keepTrack == false)
                    count++;
            return count; ;
        }

    }

    public class Note
    {
        string date;  //MMDDYYYY
        AlertScope importance; // who should be notified when this date is reached?
        string noteContent; // Note contents
        Campaign campaign;

        public Note(string d, AlertScope imp, string n, Campaign c)
        {
            editDate(d);
            importance = imp;
            editNoteContent(n);
            campaign = c;
        }

        public Note(string d, AlertScope imp, string n)
        {
            date = d;
            importance = imp;
            noteContent = n;
        }

        public Note(dynamic noteJson)
        {
            Date = noteJson["Date"];
            NoteContent = noteJson["NoteContent"];
            Importance = noteJson["Importance"];
        }

        public string Date
        {
            get { return date; }
            set { editDate(value); }
        }

        public string NoteContent
        {
            get { return noteContent; }
            set { editNoteContent(value); }
        }

        public Campaign Campaign
        {
            get { return campaign; }
            set { campaign = value; }
        }

        public void editDate(string newDate)
        {
            if (VerifyDate(newDate))
                date = newDate;
            else
                date = "00000000";
        }

        public AlertScope Importance
        {
            get { return importance; }
            set { importance = value; }
        }

        public void editDate(int d, int m, int y)
        {
            // TODO: fix this
        }

        public bool isGeneral()
        {
            if (Campaign == null)
                return true;
            else
                return false;
        }

        public static bool VerifyDate(string dateString)
        {

            if (dateString == null || dateString.Length != 8) // Date must be 8 characters: DDMMYYYY
                return false;

            int result;                                                                           // Date is bad...
            if (Int32.TryParse(dateString.Substring(0, 2), out result) == false || result > 12)   // if day above 32
                return false;

            if (Int32.TryParse(dateString.Substring(2, 2), out result) == false || result > 32)   // if year above 12
                return false;

            if (Int32.TryParse(dateString.Substring(4, 4), out result) == false || result > 1600) // if year above 1600
                return false;

            return true; // If you reach this point, date must be good

            // Note this function could return a false date as true, a 32 day month only happens on leap year

        }

        public void editNoteContent(string newNote)
        {
            noteContent = newNote;
        }

        // Returns 1 if x happened after y, -1 if y happened after x, 0 if same date
        public static int compareNotes(Note x, Note y)
        {
            return HarptosCalendar.FarthestInTime(x.date, y.date);
        }

        public int compareTo(Note n)
        {
            return compareNotes(this, n);
        }
    }

    public class Timer
    {
        public int month;              // Month timer occurs;
        public int day;                // day timer occurs;
        public int year;               // year timer occurs;
        public bool keepTrack;         // should this timer be displayed continually until occurrence
        public string message;  // What the timer shows when it occurs

        [Newtonsoft.Json.JsonConstructor]
        public Timer(int m, int d, int y, bool track, string msg)
        {
            month = m;
            day = d;
            year = y;
            keepTrack = track;
            message = msg;
        }

        public Timer(dynamic timerJson)
        {
            month = timerJson["month"];
            day = timerJson["day"];
            year = timerJson["year"];
            keepTrack = timerJson["keepTrack"];
            message = timerJson["message"];
        }

        public Timer(string dateString, bool track, string msg)
        {
            month = Int32.Parse(dateString.Substring(0, 2));
            day = Int32.Parse(dateString.Substring(2, 2));
            year = Int32.Parse(dateString.Substring(4, 4));
            keepTrack = track;
            message = msg;
        }

        public string returnDateString()
        {
            string monthString = HarptosCalendar.enforceMonthFormat(month.ToString());
            string yearString = HarptosCalendar.enforceYearFormat(year.ToString());
            string dayString = HarptosCalendar.enforceDayFormat(monthString, day.ToString(), yearString);
            return monthString + dayString + yearString;
        }

    }
}
