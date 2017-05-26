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
    public enum alertScope { dontAlert, alertCampaign, alertAll }

    public class Calendar
    {
        public HarptosCalendar calendar;
        List<Campaign> campaignList;
        public Campaign activeCampaign;

        public Calendar()
        {
            activeCampaign = null;
            calendar = new HarptosCalendar();
            campaignList = new List<Campaign>();
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

        public void addCampaign(Campaign c)
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

        public Note findNote(string content)
        {
            foreach (Campaign c in campaignList)
                foreach (Note n in c.notes)
                    if (n.NoteContent == content)
                        return n;
            return null;
        }
    }

    public class Campaign
    {
        string tag;
        string name;
        public List<Note> notes;
        string currentDate;

        public Campaign(string n, string t, string startDate) : this(n, t, startDate, startDate)
        {

        }

        public Campaign(string n, string t, string startDate, string currDate)
        {
            notes = new List<Note>();
            name = n;
            tag = t;
            if (Note.VerifyDate(startDate))
            {
                string msg = name + " began!";
                addNote(startDate, alertScope.alertAll, msg);
                currentDate = currDate;
            }
            if (Note.VerifyDate(currDate))
            {
                string msg = "Current Date";
                addNote(currentDate, alertScope.alertAll, msg);
                currentDate = currDate;
            }
        }

        public Campaign()
        {
            notes = new List<Note>();
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

        public void addNote(string date, alertScope importance, string note)
        {
            notes.Add(new Note(date, importance, note, this));
            sortNotes();
        }

        public bool deleteNote(Note noteToDelete)
        {
            return notes.Remove(notes.Find(x => x.Equals(noteToDelete)));
        }

        public static string fixTag(string t)
        {
            if (t.Length > 5)
                return t.Substring(0, 5).ToUpper();
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

    }

    public class Note
    {
        string date;  //MMDDYYYY
        alertScope importance; // who should be notified when this date is reached?
        string noteContent; // Note contents
        Campaign campaign;

        public Note(string d, alertScope imp, string n, Campaign c)
        {
            editDate(d);
            importance = imp;
            editNoteContent(n);
            campaign = c;
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

        public alertScope Importance
        {
            get {return importance; }
            set {importance = value; }
        }

        public void editDate(int d, int m, int y)
        {
            // TODO: fix this
        }

        public static bool VerifyDate(string dateString)
        {

            if (dateString.Length != 8) // Date must be 8 characters: DDMMYYYY
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
            /*int lineSize = 20; // When should endlines be put in the note?
            StringBuilder sb = new StringBuilder(newNote);
            for(int i = 0; i < newNote.Length; i++)
            {
                if (i % lineSize == 0) // if the line should break on a space, do so
                {
                    if (newNote.ElementAt(i) == ' ')
                    {
                        newNote = newNote.Insert(i, "\n");
                    }
                    else // if the line would break in the middle of a word, back up to a space, then insert
                    {
                        int j = i;

                        while (newNote.ElementAt(j) != ' ')
                            j--;

                        newNote = newNote.Insert(j, "\n");
                        i = j;
                    }
                }
            }*/ // TODO: fix editnotecontent

            noteContent = newNote;
        }

        // Returns 1 if x happened after y, -1 if y happened after x, 0 if same date
        public static int compareNotes(Note x, Note y)
        {
            int xDate = Int32.Parse(x.Date);
            int yDate = Int32.Parse(y.Date);

            if (xDate == yDate)
                return 0;
            else if(xDate > yDate)
                return 1;
            else
                return -1;
        }

        public int compareTo(Note n)
        {
            return compareNotes(this, n);
        }
    }

}
