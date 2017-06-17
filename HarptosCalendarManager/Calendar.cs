using System;
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
        public HarptosCalendar calendar;
        List<Campaign> campaignList;
        List<Note> generalNoteList;
        public Campaign activeCampaign;

        public Calendar()
        {
            activeCampaign = null;
            calendar = new HarptosCalendar();
            campaignList = new List<Campaign>();
            generalNoteList = new List<Note>();
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

        public List<Note> GeneralNoteList
        {
            get { return generalNoteList; }
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
                GeneralNoteList.Sort(delegate (Note x, Note y)
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

        public List<Campaign> CampaignList
        {
            get {return campaignList; }
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

        public void advanceDay()
        {
        }

        public void advanceTenday()
        {
        }

        public void advanceMonth()
        {
        }

        public void advance()
        {
        }

        public Note findNote(string content, bool general)
        {
            if (general)
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

        public void deleteNote(string content, bool general)
        {
            Note noteToDelete = findNote(content, general);
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

        public string Tag
        {
            get { return tag; }
            set { tag = fixTag(value); }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string CurrentDate
        {
            get { return currentDate; }
            set { setCurrentDate(value); }
        }

        public void setCurrentDate(string newCurrDate)
        {
            foreach (Note n in notes)
                if (n.NoteContent == "Current Date")
                    n.Date = newCurrDate;
            currentDate = newCurrDate;
            sortNotes();
        }

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
            notes.Sort(delegate(Note x, Note y)
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
            return count;
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
        public Note(string d, AlertScope imp, string n) : this(d, imp, n, null)
        {

        }

        public string Date
        {
            get {return date;}
            set {editDate(value); }
        }
        
        public string NoteContent
        {
            get { return noteContent; }
            set { editNoteContent(value); }
        }

        public Campaign Campaign
        {
            get {return campaign; }
            set {campaign = value; }
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
            get {return importance; }
            set {importance = value; }
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
        int month;              // Month timer occurs;
        int day;                // day timer occurs;
        int year;               // year timer occurs;
        public bool keepTrack;         // should this timer be displayed continually until occurrence
        public string message;  // What the timer shows when it occurs

        public Timer(int m, int d, int y, bool track, string msg)
        {
            month = m;
            day = d;
            year = y;
            keepTrack = track;
            message = msg;
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
