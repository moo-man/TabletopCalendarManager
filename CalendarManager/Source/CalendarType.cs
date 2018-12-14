using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace CalendarManager
{
    public enum moonPhase {newMoon, waxingCrsec, firstQuarter, waxingGib, full, waningGib, lastQuarter, waningCresc};
    public class CalendarType
    {


        #region jsonData
        [JsonProperty]
        public string calendarName { get; set; }

        [JsonProperty]
        static int numDaysInYear;
        [JsonProperty]
        static bool celestialEvents;
        [JsonProperty]
        static int numMonthsInYear;
        [JsonProperty]
        static string[] monthNames;
        [JsonProperty]
        static int[] numDaysInMonth;
        [JsonProperty]
        static int numDaysInWeek;
        [JsonProperty]
        static string[] weekdayNames;
        [JsonProperty]
        static int numOfMoons;
        [JsonProperty]
        static string[] moonNames;
        [JsonProperty]
        static int[] moonCycle;
        [JsonProperty]
        static int[] moonShift;
        [JsonProperty]
        static List<moonPhase[]> moonPhases;
        [JsonProperty]
        static int startYear;
        [JsonProperty]
        static int startDay;

        [JsonProperty]
        static string[] UniversalNoteDates;
        [JsonProperty]
        static string[] UniversalNoteContents;
        #endregion

        #region Current data
        int month;
        int day;
        int year;
        int dayOfWeek;
        int[] moonCounters;
        #endregion

        #region Accessors
        public int CurrentMonth
        {
            get
            {
                return month;
            }
        }

        public int CurrentDay
        {
            get
            {
                return day;
            }
        }

        public int CurrentYear
        {
            get
            {
                return year;
            }
        }

        public static int NumMonthsInYear
        {
            get
            {
                return numMonthsInYear;
            }
        }

        public static int[] NumDaysInMonth
        {
            get
            {
                return numDaysInMonth;
            }
        }

        public static int NumDaysInWeek
        {
            get
            {
                return numDaysInWeek;
            }
        }
        #endregion


        public CalendarType(string json, string name)
        {

            LoadFromDonjonJSON(JsonConvert.DeserializeObject(json));
            createMoonPhaseArray();
            setDate(1, 1, startYear);
            calendarName = name;
        }

        public CalendarType(System.IO.StreamReader sr)
        {
            //loadData(sr);
            setDate(1, 1, startYear);
        }

        /// <summary>
        /// Load data FROM SAVE FILE
        /// </summary>
        /// <param name="json"></param>
        public CalendarType(dynamic json)
        {
            LoadFromJSON(json);
            createMoonPhaseArray();
            setDate(1, 1, startYear);
        }



        public string LoadFromJSON(dynamic json)
        {
            try
            {
                calendarName = json["calendar"]["calendarName"];
                numDaysInYear = json["calendar"]["numDaysInYear"];
                celestialEvents = json["calendar"]["celestialEvents"];

                numMonthsInYear = json["calendar"]["numMonthsInYear"];

                monthNames = new string[numMonthsInYear + 1];
                numDaysInMonth = new int[numMonthsInYear + 1];
                for (int i = 0; i < numMonthsInYear; i++)
                {
                    monthNames[i + 1] = json["calendar"]["monthNames"][i+1];

                    try
                    {
                        numDaysInMonth[i + 1] = json["calendar"]["numDaysInMonth"][i+1];
                    }
                    catch
                    {
                        numDaysInMonth[i + 1] = numDaysInYear / numMonthsInYear;
                    }
                }

                numDaysInWeek = json["calendar"]["numDaysInWeek"];
                weekdayNames = new string[numDaysInWeek];
                if (weekdayNames == null)
                {
                    weekdayNames = new string[numDaysInWeek];
                    for (int i = 0; i < numDaysInWeek; i++)
                        weekdayNames[i] = null;
                }

                for (int i = 0; i < numDaysInWeek; i++)
                    weekdayNames[i] = json["calendar"]["weekdayNames"][i];

                numOfMoons = json["calendar"]["numOfMoons"];
                moonNames = new string[numOfMoons];
                moonCycle = new int[numOfMoons];
                moonShift = new int[numOfMoons];
                try
                {
                    for (int i = 0; i < numOfMoons; i++)
                    {
                        moonNames[i] = json["calendar"]["moonNames"][i];
                        moonCycle[i] = json["calendar"]["moonCycle"][i];
                        moonShift[i] = json["calendar"]["moonShift"][i];
                    }
                }
                catch
                {
                    numOfMoons = 0;
                    moonNames = new string[numOfMoons];
                    moonCycle = new int[numOfMoons];
                    moonShift = new int[numOfMoons];
                }
                startYear = json["calendar"]["startYear"];
                startDay = json["calendar"]["startDay"];

                Newtonsoft.Json.Linq.JArray univContent = json["calendar"]["UniversalNoteContents"];
                Newtonsoft.Json.Linq.JArray univDates = json["calendar"]["UniversalNoteDates"];

                UniversalNoteContents = univContent.Select(x => (string)x).ToArray();
                UniversalNoteDates = univDates.Select(x => (string)x).ToArray();

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Load JSON data FROM DONJON
        /// </summary>
        /// <param name="json"></param>
        public string LoadFromDonjonJSON(dynamic json)
        {
            try
            {
                numDaysInYear = json["year_len"];
                celestialEvents = json["events"];

                numMonthsInYear = json["n_months"];
                monthNames = new string[numMonthsInYear + 1];
                numDaysInMonth = new int[numMonthsInYear + 1];
                for (int i = 0; i < numMonthsInYear; i++)
                {
                    monthNames[i + 1] = json["months"][i];

                    try
                    {
                        numDaysInMonth[i + 1] = json["month_len"][monthNames[i + 1]];
                    }
                    catch
                    {
                        numDaysInMonth[i + 1] = numDaysInYear / numMonthsInYear;
                    }
                }

                numDaysInWeek = json["week_len"];
                weekdayNames = new string[numDaysInWeek];
                if (weekdayNames == null)
                {
                    weekdayNames = new string[numDaysInWeek];
                    for (int i = 0; i < numDaysInWeek; i++)
                        weekdayNames[i] = null;
                }

                for (int i = 0; i < numDaysInWeek; i++)
                    weekdayNames[i] = json["weekdays"][i];

                numOfMoons = json["n_moons"];
                moonNames = new string[numOfMoons];
                moonCycle = new int[numOfMoons];
                moonShift = new int[numOfMoons];
                try
                {
                    for (int i = 0; i < numOfMoons; i++)
                    {
                        moonNames[i] = json["moons"][i];
                        moonCycle[i] = json["lunar_cyc"][moonNames[i]];
                        moonShift[i] = json["lunar_shf"][moonNames[i]];
                    }
                }
                catch
                {
                    numOfMoons = 0;
                    moonNames = new string[numOfMoons];
                    moonCycle = new int[numOfMoons];
                    moonShift = new int[numOfMoons];
                }
                startYear = json["year"];
                startDay = json["first_day"];


                Newtonsoft.Json.Linq.JObject djNotes = json["notes"];
                UniversalNoteContents = new string[djNotes.Count];
                UniversalNoteDates = new string[djNotes.Count];
                int noteIndex = 0;

                foreach (var note in djNotes)
                {
                    // Separate date into array, year|month|day
                    string[] date = note.Key.Split('-');
                    int noteYear = Int32.Parse(date[0]);
                    int noteMonth = Int32.Parse(date[1]);
                    int noteDay = Int32.Parse(date[2]);

                    if (noteYear == startYear)
                    {
                        UniversalNoteDates[noteIndex] = noteMonth.ToString("00") + noteDay.ToString("00");
                        UniversalNoteContents[noteIndex] = note.Value.ToString();
                        noteIndex++;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /*
        public void loadData(System.IO.StreamReader reader)
        {
            calendarName = reader.ReadLine();
            numDaysInYear = Int32.Parse(reader.ReadLine());
            celestialEvents = Convert.ToBoolean(reader.ReadLine());
            numMonthsInYear = Int32.Parse(reader.ReadLine());
            monthNames = new string[numMonthsInYear+1];
            numDaysInMonth = new int[numMonthsInYear+1];
            for (int i = 0; i < numMonthsInYear; i++)
            {
                monthNames[i+1] = reader.ReadLine();
                numDaysInMonth[i+1] = Int32.Parse(reader.ReadLine());
            }

            numDaysInWeek = Int32.Parse(reader.ReadLine());
            weekdayNames = new string[numDaysInWeek];
            for (int i = 0; i < numDaysInWeek; i++)
                weekdayNames[i] = reader.ReadLine();

            numOfMoons = Int32.Parse(reader.ReadLine());
            moonNames = new string[numOfMoons];
            moonCycle = new int[numOfMoons];
            moonShift = new int[numOfMoons];
            for (int i = 0; i < numOfMoons; i++)
            {
                moonNames[i] = reader.ReadLine();
                moonCycle[i] = (int)Math.Round(Double.Parse(reader.ReadLine()));
                moonShift[i] = Int32.Parse(reader.ReadLine());
            }
            createMoonPhaseArray();
            startYear = Int32.Parse(reader.ReadLine());
            startDay = Int32.Parse(reader.ReadLine());
        }*/


        // cycle / 8 = full moon day length (rounded to nearest)
        // waning crescent -> full moon takes 4 days
        private void createMoonPhaseArray()
        {
            moonPhases = new List<moonPhase[]>(numOfMoons);
            moonCounters = new int[numOfMoons];
            for (int i = 0; i < numOfMoons; i++)
            {
                moonPhase[] arrayToAdd = new moonPhase[moonCycle[i]]; // End result, this array holds the entire cycle of the moon
                int arrayIndex = 0; // index for adding phases to arrayToAdd

                if (moonCycle[i] >= 8)
                {
                    int extraDays = moonCycle[i] % 8;                         // If 8 doesn't evenly divide the cycle, there will be extra days for some phases (there are 8 phases)
                    bool[] phasesWithExtraDay = extraDayPlacement(extraDays); // find out which phases might have an extra day length


                    // Outer loop indexes through each phase, inner loop allocates that phase "cycle/8" times, then adds an extra day if applicable
                    for (int moonPhaseIndex = 0; moonPhaseIndex < 8; moonPhaseIndex++)
                    {
                        for (int allocater = 0; allocater < (moonCycle[i] / 8); allocater++)
                        {
                            arrayToAdd[arrayIndex++] = (moonPhase)moonPhaseIndex;
                        }
                        if (phasesWithExtraDay[moonPhaseIndex]) // add extra day
                            arrayToAdd[arrayIndex++] = (moonPhase)moonPhaseIndex;
                    }
                }
                else // cycle is less that 8, some phases won't appear
                {
                    bool[] usePhase = removedPhases(moonCycle[i]);


                    // Outer loop indexes through each phase, inner loop allocates that phase "cycle/8" times, then adds an extra day if applicable
                    for (int moonPhaseIndex = 0; i < 8 && usePhase[i]; i++)
                    {
                        arrayToAdd[arrayIndex++] = (moonPhase)moonPhaseIndex;
                    }
                }
                moonPhases.Add(arrayToAdd);
            }
        }

        /// <summary>
        /// Returns which moon phases get extra days, depending on how many extra days there are
        /// Extra days range from 0 - 7 (modulo 8, since there's 8 phases), 0 extra days = all phases same length
        /// </summary>
        /// <param name="extraDays"></param>
        private bool[] extraDayPlacement(int extraDays)
        {
            // CHECK
            //  moonPhase {newMoon, waxingCrsec, firstQuarter, waxingGib, full, waningGib, lastQuarter, waningCresc};
            // Would like to do this elegantly, with simple calculation, but this is based off data from donjon, so not sure how it exactly works
            switch (extraDays)
            {
                case 0:
                    return new bool[] { false, false, false, false,  false, false, false, false};
                case 1:
                    return new bool[] {true, false, false, false, false, false, false, false};
                case 2:
                    return new bool[] {true, false, false, false, true, false, false, false };
                case 3:
                    return new bool[] {true, false, true, false, false, true, false, false };
                case 4:
                    return new bool[] { true, false, true, false, true, false, true, false };
                case 5:
                    return new bool[] {true, true, false, true, true, false, true, false };
                case 6:
                    return new bool[] {true, true, true, false, true, true, true, false };
                case 7:
                    return new bool[] { true, true, true, true, true, true, true, false};
                default:
                    return null;
            }

        }

        /// <summary>
        /// If the moon's cycle is less than 8 days, all 8 phases won't be used, this function decides which ones to use given a cycle
        /// </summary>
        /// <param name="moonCycle"></param>
        /// <returns></returns>
        private bool[] removedPhases(int moonCycle)
        {
            // CHECK
            switch (moonCycle)
            {
                case 7:
                    return new bool[] {true, true, true, true, true, true, true, false };
                case 6:
                    return new bool[] {true, true, true, false, true, true, true, false};
                case 5:
                    return new bool[] {false, true, true, false, true, true, false, true};
                case 4:
                    return new bool[] { true, false, true, false, true, false, true, false };
                case 3:
                    return new bool[] {true, false, true, false, false, true, false, false };
                case 2:
                    return new bool[] { true, false, false, false, true, false, false, false };
                case 1:
                    return new bool[] { true, false, false, false, false, false, false, false };
                case 0:
                    return null;
                default:
                    return new bool[] { true, true, true, true, true, true, true, true };
            }
        }

        #region forward in time
        public void addDay()
        {
            day++;
            if (day > numDaysInMonth[month])
            {
                day = 1;
                month++;
                if (month > numMonthsInYear)
                {
                    month = 1;
                    year++;
                }
            }
            addDayOfWeek();
            addMoonPhase();

            if (year > 9999)
            {
                year = 9999;
                month = numMonthsInYear;
                day = numDaysInMonth[numMonthsInYear];
                // Kinda jank, if this if statement happens, the date doesnt change, so reverse the adddayofweek and addmoonphase
                subDayOfWeek();
                subMoonPhase();
            }
        }
        public void addDay(int numDays)
        {
            for (int i = 0; i < numDays; i++)
                addDay();
        }
        public void addWeek()
        {
            addDay(numDaysInWeek);
        }
        public void addMonth()
        {
            addDay(numDaysInMonth[month]);
        }
        public void addYear()
        {
            year++;
            if (year > 9999)
                year = 9999;
            determineDayOfWeek();
            determineMoonCounters();
        }
        public void addDayOfWeek()
        {
            dayOfWeek++;
            if (dayOfWeek >= numDaysInWeek - 1) // >= because dayOfWeek goes from 0 to numDaysInWeek - 1
                dayOfWeek = 0;
        }

        public void addMoonPhase()
        {
            for (int i = 0; i < moonCounters.Length; i++)
            {
                moonCounters[i]++;
                if (moonCounters[i] >= moonCycle[i])
                    moonCounters[i] = 0;
            }
        }
        #endregion

        #region backward in time
        public void subDay()
        {
            day--;
            if (day < 1)
            {
                month--;
                if (month < 1)
                {
                    year--;
                    month = numMonthsInYear;
                }
                day = numDaysInMonth[month];
            }
            subDayOfWeek();
            subMoonPhase();

            if (year < 0)
            {
                day = 1;
                month = 1;
                year = 0;
                addDayOfWeek();
                addMoonPhase();
            }


        }
        public void subDay(int numDays)
        {
            for (int i = 0; i < numDays; i++)
            {
                subDay();
            }
        }
        public void subWeek()
        {
            subDay(numDaysInWeek);
        }
        public void subMonth()
        {
            subDay(numDaysInMonth[month]);
        }
        public void subYear()
        {
            year--;
            if (year < 0)
                year = 0;
            determineDayOfWeek();
            determineMoonCounters();
        }
        public void subDayOfWeek()
        {
            dayOfWeek--;
            if (dayOfWeek < 0)
                dayOfWeek = numDaysInWeek - 1;
        }

        public void subMoonPhase()
        {
            for (int i = 0; i < moonCounters.Length; i++)
            {
                moonCounters[i]--;
                if (moonCounters[i] < 0)
                    moonCounters[i] = moonCycle[i] - 1;
            }
        }
        #endregion


        #region setDate and all functions determining day of week, moonphase, etc

        public void setDate(string dateString)
        {
            setDate(Int32.Parse(dateString.Substring(0, 2)), Int32.Parse(dateString.Substring(2, 2)), Int32.Parse(dateString.Substring(4, 4)));
        }

        public void setDate(int m, int d, int y)
        {
            if (m > numMonthsInYear)
                m = numMonthsInYear;
            else if (m <= 0)
                m = 1;

            month = m;

            if (d > numDaysInMonth[month])
                d = numDaysInMonth[month];
            else if (d <= 0)
                d = 1;

            day = d;

            if (y < 0)
                y = 1;
            else if (y > 9999)
                year = 9999;

            year = y;

            dayOfWeek = determineDayOfWeek();
            determineMoonCounters();
        }

        public int determineDayOfWeek()
        {
            return determineDayOfWeek(month, day, year);
        }

        public static int determineDayOfWeek(string currentDate)
        {
            return determineDayOfWeek(Int32.Parse(currentDate.Substring(0, 2)), Int32.Parse(currentDate.Substring(2, 2)), Int32.Parse(currentDate.Substring(4, 4)));
        }

        public static int determineDayOfWeek(int m, int d, int y)
        {
            int totalDaysPassedSinceStart = Math.Abs(y - startYear) * numDaysInYear + (determineDayOfYear(m, d, y) - 1);
            int modResult = (totalDaysPassedSinceStart) % (numDaysInWeek);
            return ((modResult + startDay) % (numDaysInWeek));
        }

        /// <summary>
        /// For each moon, use its cycle and shift to determine the current phase
        /// This is calculated from the calendar's year 0 day 1 (which is what the cycle and shift are relative to)
        /// </summary>
        public void determineMoonCounters()
        {
            int daysSinceFirstDay = 0;

            for (int i = 0; i < Math.Abs(year); i++)
                daysSinceFirstDay += numDaysInYear;

            daysSinceFirstDay += determineDayOfYear() - 1;

            for (int i = 0; i < numOfMoons; i++)
              moonCounters[i] = Math.Abs(daysSinceFirstDay - moonShift[i]) % moonCycle[i];

        }
        #endregion


        #region determineDayOfYear
        public int determineDayOfYear()
        {
            return determineDayOfYear(month, day, year);
        }

        public static int determineDayOfYear(string currentDate)
        {
            return determineDayOfYear(Int32.Parse(currentDate.Substring(0, 2)), Int32.Parse(currentDate.Substring(2, 2)), Int32.Parse(currentDate.Substring(4, 4)));
        }

        public static int determineDayOfYear(int m, int d, int y)
        {
            int dayAccumulator = 0;

            // Add the days of the month before current month
            for (int i = 0; i < m - 1; i++)
            {
                dayAccumulator += numDaysInMonth[i + 1];
            }
            dayAccumulator += d; // add current day to sum
            return dayAccumulator;
        }

#endregion

        #region returning date, moonphases, names, etc.

        public string getMonthName()
        {
            return monthNames[month];
        }

        public string returnMoonNames()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in moonNames)
            {
                sb.Append(s + "\n\n\n\n");
            }

            return sb.ToString();
        }

        public string[] currentMoonPhase()
        {
            string[] returnString = new string[numOfMoons];
            for (int i = 0; i < numOfMoons; i++)
            {
                switch (moonPhases[i][moonCounters[i]])
                {
                    case moonPhase.full:
                        returnString[i] =  "Full Moon";
                        break;
                    case moonPhase.waningGib:
                        returnString[i] =  "Waning Gibbous";
                        break;
                    case moonPhase.lastQuarter:
                        returnString[i] =  "Last Quarter";
                        break;
                    case moonPhase.waningCresc:
                        returnString[i] =  "Waning Crescent";
                        break;
                    case moonPhase.newMoon:
                        returnString[i] =  "New Moon";
                        break;
                    case moonPhase.waxingCrsec:
                        returnString[i] =  "Waxing Crescent";
                        break;
                    case moonPhase.firstQuarter:
                        returnString[i] =  "First Quarter";
                        break;
                    case moonPhase.waxingGib:
                        returnString[i] =  "Waxing Gibbous";
                        break;
                    default:
                        returnString[i] = null;
                        break;
                }
            }
            return returnString;
        }

        /// <summary>
        /// Reverse ReturnGivenDate.
        /// Give (monthName) (dayNumber) (yearNumber)
        /// Returns mmddyyyy
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ReturnGivenDateFromName(string date)
        {
            string month = null, day = null, year = null;
            try
            {
                string[] splitArray = date.Split(' ');

                for (int i = 0; i < splitArray.Length; i++)
                {
                    splitArray[i] = splitArray[i].Trim(',');
                }
                // If the length is not 3, that means the month name has a space in it
                // Copy the array, extract the month name into a string builder
                // Redo the array at size 3, assemble it as [monthName][day][year]
                if (splitArray.Length != 3)
                {
                    string[] tempArray = new string[splitArray.Length];
                    splitArray.CopyTo(tempArray, 0);
                    StringBuilder monthName = new StringBuilder();
                    for (int i = 0; i < splitArray.Length - 2; i++)
                    {
                        monthName.Append(splitArray[i] + " ");
                    }

                    splitArray = new string[3];
                    splitArray[0] = monthName.ToString();
                    splitArray[1] = tempArray[tempArray.Length - 2];
                    splitArray[2] = tempArray[tempArray.Length - 1];

                }

                if (splitArray.Length == 3)
                {

                    for (int i = 1; i < monthNames.Length; i++)
                    {
                        if (splitArray[0].Equals(monthNames[i]))
                        {
                            month = enforceMonthFormat(i.ToString());
                        }

                    }

                    year = enforceYearFormat(splitArray[2]);
                    day = enforceDayFormat(month, splitArray[1], year);
                }
            }
            catch (Exception e)
            {

            }
            return month + day + year;
        }


        public string ReturnUniversalNoteContent()
        {
            for (int i = 0; i < UniversalNoteContents.Length; i++)
            {
                if (isAnniversary(UniversalNoteDates[i] + "0000"))
                    return UniversalNoteContents[i];
            }
            return null;
        }

        #endregion


        #region Date relation functions, sameDate, isAnniversary, yearsAgo, farthestInTime

        public bool sameDate(int testM, int testD, int testY)
        {
            if (testM == month && testD == day && testY == year)
                return true;
            else
                return false;
        }

        public bool sameDate(string testDate)
        {
            if (testDate.Length != 8)
                return false;
            else
                return sameDate(Int32.Parse(testDate.Substring(0, 2)), Int32.Parse(testDate.Substring(2, 2)), Int32.Parse(testDate.Substring(4, 4)));
        }

        public bool isAnniversary(string testDate)
        {
            if (testDate.Length != 8)
                return false;
            else
                return isAnniversary(Int32.Parse(testDate.Substring(0, 2)), Int32.Parse(testDate.Substring(2, 2)));
        }

        public bool isAnniversary(int testM, int testD)
        {
            if (testM == month && testD == day)
                return true;
            else
                return false;
        }
        /// <summary>
        /// Difference between input year and current year
        /// </summary>
        /// <param name="y">Input year</param>
        /// <returns>Difference between current year and y</returns>
        public int yearsAgo(int y)
        {
            return year - y;
        }

        /// <summary>
        /// Difference in years between input date and current date
        /// </summary>
        /// <param name="inputDate">Input date in the form of MMDDYYYY</param>
        /// <returns></returns>
        public int yearsAgo(string inputDate)
        {
            return yearsAgo(this.ToString(), inputDate);
        }

        public static int yearsAgo(string initialDate, string compareDate)
        {
            return Int32.Parse(initialDate.Substring(4, 4)) - Int32.Parse(compareDate.Substring(4, 4));
        }

        public static int FarthestInTime(string date1, string date2)
        {
            int year1 = Int32.Parse(date1.Substring(4, 4));
            int year2 = Int32.Parse(date2.Substring(4, 4));
            int month1 = Int32.Parse(date1.Substring(0, 2));
            int month2 = Int32.Parse(date2.Substring(0, 2));
            int day1 = Int32.Parse(date1.Substring(2, 2));
            int day2 = Int32.Parse(date2.Substring(2, 2));

            if (year1 > year2)
                return 1;
            else if (year2 > year1)
                return -1;
            else // if year1 == year2
            {
                if (month1 > month2)
                    return 1;
                else if (month2 > month1)
                    return -1;
                else // if month1 == month2
                {
                    if (day1 > day2)
                        return 1;
                    else if (day2 > day1)
                        return -1;
                    else // day1 == day 2
                        return 0;
                }
            }
        }

        /// <summary>
        /// Returns true if testDate is between date1 and date2
        /// </summary>
        /// <param name="testDate"></param>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        //public static bool dateBetween(string testDate, string date1, string date2)
        //{
        //}

        #endregion
        /// <summary>
        /// Formats current date as MMDDYYYY
        /// </summary>
        /// <returns>Returns current date as a string in the format of MMDDYYYY</returns>
        public override string ToString()
        {
            StringBuilder stringDate = new StringBuilder();

            stringDate.Append(month.ToString("00"));
            stringDate.Append(day.ToString("00"));
            stringDate.Append(year.ToString("0000"));

            return stringDate.ToString();
        }

        public string ToString(string format, bool alt = false)
        {
            return ToString(ToString(), format, alt);
        }
        

        public static string ToString(string dateString, string format, bool alt = false)
        {
            int m = Int32.Parse(dateString.Substring(0, 2));
            int d = Int32.Parse(dateString.Substring(2, 2));
            int y = Int32.Parse(dateString.Substring(4, 4));

            format = format.ToLower();



            /// First Column    Second Column
            /// Index Marker |  Marker Length
            /// D4
            /// D
            /// M
            /// Y
            int[,] offLimitArray = new int[4, 2];

            for (int i = 0; i < offLimitArray.GetLength(0); i++)
            {
                for (int j = 0; j < offLimitArray.GetLength(1); j++)
                {
                    offLimitArray[i, j] = 0;
                }
            }


            int startIndex = 0;

            if (format.Contains("dddd"))
            {
                offLimitArray[0, 0] = format.IndexOf("dddd");
                offLimitArray[0, 1] = ReturnDayFromFormat("dddd", dateString).Length;
                format = format.Replace("dddd", ReturnDayFromFormat("dddd", dateString));
            }

            while (startIndex < format.Length)
            {
                if (format.IndexOf("ddd", startIndex) != -1 && !OffLimits(startIndex = format.IndexOf("ddd", startIndex), offLimitArray))
                {
                    offLimitArray[1, 0] = format.IndexOf("ddd");
                    offLimitArray[1, 1] = ReturnMonthFromFormat("ddd", m).Length;
                    int substringStart = format.IndexOf("ddd", startIndex);
                    string substring = format.Substring(startIndex);
                    format = format.Substring(0, substringStart) + substring.Replace("ddd", ReturnDayFromFormat("ddd", dateString));
                }
                else if (format.IndexOf("dd", startIndex) != -1 && !OffLimits(startIndex = format.IndexOf("dd", startIndex), offLimitArray))
                {
                    offLimitArray[1, 0] = format.IndexOf("dd");
                    offLimitArray[1, 1] = ReturnMonthFromFormat("dd", m).Length;
                    int substringStart = format.IndexOf("dd", startIndex);
                    string substring = format.Substring(startIndex);
                    format = format.Substring(0, substringStart) + substring.Replace("dd", ReturnDayFromFormat("dd", dateString));
                }
                else if (format.IndexOf("d", startIndex) != -1 && !OffLimits(startIndex = format.IndexOf("d", startIndex), offLimitArray))
                {
                    offLimitArray[1, 0] = format.IndexOf("d");
                    offLimitArray[1, 1] = ReturnMonthFromFormat("d", m).Length;
                    int substringStart = format.IndexOf("d", startIndex);
                    string substring = format.Substring(startIndex);
                    format = format.Substring(0, substringStart) + substring.Replace("d", ReturnDayFromFormat("d", dateString));
                }
                else
                    startIndex++;
            }

            startIndex = 0;
            while (startIndex < format.Length)
            {
                if (format.IndexOf("mmm", startIndex) != -1 && !OffLimits(startIndex = format.IndexOf("mmm", startIndex), offLimitArray))
                {
                    offLimitArray[2, 0] = format.IndexOf("mmm");
                    offLimitArray[2, 1] = ReturnMonthFromFormat("mmm", m).Length;
                    int substringStart = format.IndexOf("mmm", startIndex);
                    string substring = format.Substring(startIndex);
                    format = format.Substring(0, substringStart) + substring.Replace("mmm", ReturnMonthFromFormat("mmm", m));
                }
                else if (format.IndexOf("mm", startIndex) != -1 && !OffLimits(startIndex = format.IndexOf("mm", startIndex), offLimitArray))
                {
                    offLimitArray[2, 0] = format.IndexOf("mm");
                    offLimitArray[2, 1] = ReturnMonthFromFormat("mm", m).Length;
                    int substringStart = format.IndexOf("mm", startIndex);
                    string substring = format.Substring(startIndex);
                    format = format.Substring(0, substringStart) + substring.Replace("mm", ReturnMonthFromFormat("mm", m));
                }
                else if (format.IndexOf("m", startIndex) != -1 && !OffLimits(startIndex = format.IndexOf("m", startIndex), offLimitArray))
                {
                    offLimitArray[2, 0] = format.IndexOf("m");
                    offLimitArray[2, 1] = ReturnMonthFromFormat("m", m).Length;
                    int substringStart = format.IndexOf("m", startIndex);
                    string substring = format.Substring(startIndex);
                    format = format.Substring(0, substringStart) + substring.Replace("m", ReturnMonthFromFormat("m", m));
                }
                else
                    startIndex++;
            }

            startIndex = 0;
            while (startIndex < format.Length)
            {

                if (format.IndexOf("yyyy", startIndex) != -1 && !OffLimits(startIndex = format.IndexOf("yyyy", startIndex), offLimitArray))
                {
                    offLimitArray[3, 0] = format.IndexOf("yyyy", startIndex);
                    offLimitArray[3, 1] = ReturnYearFromFormat("yyyy", y).Length;
                    int substringStart = format.IndexOf("yyyy", startIndex);
                    string substring = format.Substring(startIndex);
                    format = format.Substring(0, substringStart) + substring.Replace("yyyy", ReturnYearFromFormat("yyyy", y));
                }
                else if (format.IndexOf("yyy", startIndex) != -1 && !OffLimits(startIndex = format.IndexOf("yyy", startIndex), offLimitArray))
                {
                    offLimitArray[3, 0] = format.IndexOf("yyy", startIndex);
                    offLimitArray[3, 1] = ReturnYearFromFormat("yyy", y).Length;
                    int substringStart = format.IndexOf("yyy", startIndex);
                    string substring = format.Substring(startIndex);
                    format = format.Substring(0, substringStart) + substring.Replace("yyy", ReturnYearFromFormat("yyy", y));
                }
                else if (format.IndexOf("yy", startIndex) != -1 && !OffLimits(startIndex = format.IndexOf("yy", startIndex), offLimitArray))
                {
                    offLimitArray[3, 0] = format.IndexOf("yy", startIndex);
                    offLimitArray[3, 1] = ReturnYearFromFormat("yy", y).Length;
                    int substringStart = format.IndexOf("yy", startIndex);
                    string substring = format.Substring(startIndex);
                    format = format.Substring(0, substringStart) + substring.Replace("yy", ReturnYearFromFormat("yy", y));
                }
                else if (format.IndexOf("y", startIndex) != -1 && !OffLimits(startIndex = format.IndexOf("y", startIndex), offLimitArray))
                {
                    offLimitArray[3, 0] = format.IndexOf("y", startIndex);
                    offLimitArray[3, 1] = ReturnYearFromFormat("y", y).Length;
                    int substringStart = format.IndexOf("y", startIndex);
                    string substring = format.Substring(startIndex);
                    format = format.Substring(0, substringStart) + substring.Replace("y", ReturnYearFromFormat("y", y));
                }
                else
                    startIndex++;
            }
            return format;
        }
        
        /// <summary>
        /// Used with ToString to prevent replacing strings inside strings that were inserted from a previous replacement
        /// </summary>
        /// <param name="indexFound">the start index of string being replaced</param>
        /// <param name="offLimitData">data to determine if that index is offlimits</param>
        /// <returns></returns>
        private static bool OffLimits(int indexFound, int[,] offLimitData)
        {
            for (int i = 0; i < offLimitData.GetLength(0); i++)
            {
                // If index is greater than the start index and less than the start index + length of marker
                // (if in the offlimit zone)
                if (indexFound > offLimitData[i, 0] && indexFound < offLimitData[i, 0] + offLimitData[i, 1])
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Input format as...
        /// dddd -> returns day of week
        /// ddd  -> "(day)st/nd/rd/th"
        /// dd   -> "01" to "31"
        /// none ->  "1" to "31"
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        private static string ReturnDayFromFormat(string format, string dateString)
        {
            int dayValue = Int32.Parse(dateString.Substring(2, 2));
            string returnString;
            switch (format)
            {
                case "dddd":
                    returnString = weekdayNames[determineDayOfWeek(dateString)];
                    break;
                case "ddd":
                    if (dayValue == 1)
                        returnString = dayValue + "st";
                    else if (dayValue == 2)
                        returnString = dayValue + "nd";
                    else if (dayValue == 3)
                        returnString = dayValue + "rd";
                    else
                        returnString = dayValue + "th";
                    break;
                case "dd":
                    returnString = dayValue.ToString("00");
                    break;

                case "d":
                    returnString = dayValue.ToString();
                    break;
                default:
                    returnString = dayValue.ToString();
                    break;
            }
            return returnString;
        }

        /// <summary>
        /// Input format as...
        /// mmm -> "(month name)"
        /// mm  -> "01" to "12"
        /// m   ->  "1" to "12" 
        /// none -> "1" to "12"
        /// </summary>
        /// <param name="format"></param>
        /// <param name="alt">if you want alternative month names</param>
        /// <returns></returns>
        private static string ReturnMonthFromFormat(string format, int monthValue)
        {
            string returnMonth;
            switch (format)
            {
                case "mmm":
                    returnMonth = monthNames[monthValue];
                    break;

                case "mm":
                    returnMonth = monthValue.ToString("00");
                    break;

                case "m":
                    returnMonth = monthValue.ToString();
                    break;
                default:
                    returnMonth = monthValue.ToString();
                    break;
            }
            return returnMonth;
        }

        private string ReturnMonthFromFormat(string format)
        {
            return ReturnMonthFromFormat(format, month);
        }

        /// <summary>
        /// Input format as...
        /// yyyy -> "0000" to "9999"
        /// yyy ->   "000" to  "999" right most 3 numbers, probably useless
        /// yy ->     "00" to   "99" right most 2 numbers, probably useless
        /// y ->       "0" to    "9" right most number, probably useless
        /// none ->    "0  to "9999"
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        private static string ReturnYearFromFormat(string format, int yearValue)
        {
            string returnYear;

            switch (format)
            {
                case "yyyy":
                    returnYear = yearValue.ToString("0000");
                    break;
                case "yyy":
                    returnYear = yearValue.ToString("000"); // TEST
                    break;
                case "yy":
                    returnYear = yearValue.ToString("00");
                    break;
                case "y":
                    returnYear = yearValue.ToString("0");
                    break;
                default:
                    returnYear = yearValue.ToString();
                    break;
            }
            return returnYear;
        }

        private string ReturnYearFromFormat(string format)
        {
            return ReturnYearFromFormat(format, year);
        }

        #region functions for format enforcement

        /// <summary>
        /// Performs all enforce functions on an entire date
        /// </summary>
        /// <param name="testMonth"></param>
        /// <param name="testDay"></param>
        /// <param name="testYear"></param>
        public static void enforceDateFormat(ref string testMonth, ref string testDay, ref string testYear)
        {
            testMonth = enforceMonthFormat(testMonth);
            testYear = enforceYearFormat(testYear);
            testDay = enforceDayFormat(testDay, testDay, testYear);
        }
        /// <summary>
        /// Takes a number and changes it to a valid month number if it is not already one
        /// (if a number is larger than 12, returns 12, for example)
        /// </summary>
        /// <param name="testMonth">input number to test as a month</param>
        /// <returns>returns a correct month number</returns>
        public static string enforceMonthFormat(string testMonth)
        {
            if (testMonth.Length < 2)
            {
                if (testMonth.Length == 0)
                    testMonth = "0" + testMonth;
                testMonth = "0" + testMonth;
            }

            if (testMonth == "00" || testMonth == "")
            {
                testMonth = "01";
            }

            if (testMonth.Length > 2 || Int32.Parse(testMonth) > numMonthsInYear)
            {
                testMonth = numMonthsInYear.ToString();
            }
            return testMonth;
        }

        /// <summary>
        /// Returns a valid day value, depending on what year and month
        /// </summary>
        /// <param name="month">month value used to determine valid day (some months have 31 days)</param>
        /// <param name="testDay">day value, being tested</param>
        /// <param name="year">year value used to determine valid day (possible leap year)</param>
        /// <returns>returns a valid day value corresponding with the month and year</returns>
        public static string enforceDayFormat(string month, string testDay, string year)
        {
            if (testDay.Length < 2)
            {
                if (testDay.Length == 0)
                    testDay = "0" + testDay;
                testDay = "0" + testDay;
            }

            if (testDay == "00" || testDay == "")
            {
                testDay = "01";
            }
            if (month != "")
            {
                testDay = verifyDay(Int32.Parse(month), Int32.Parse(testDay)).ToString();
                if (testDay.Length == 1)
                    testDay = "0" + testDay;
            }
            else
            {
                testDay = "01";
            }

            return testDay;
        }

        /// <summary>
        /// Returns year in valid format, input of 0 returns 0000, 1 returns 0001, etc.
        /// </summary>
        /// <param name="testYear">year being tested</param>
        /// <returns>formatted year</returns>
        public static string enforceYearFormat(string testYear)
        {
            if (testYear.Length > 4)
                testYear = testYear.Substring(0, 4);

            if (testYear.Length == 3)
                testYear = "0" + testYear;

            if (testYear.Length == 2)
                testYear = "00" + testYear;

            if (testYear.Length == 1)
                testYear = "000" + testYear;

            if (testYear == "")
                testYear = "0000";

            return testYear;
        }

        public static int verifyDay(int m, int d)
        {
            if (m > numMonthsInYear || m < 1)
                return d;
            if (d > 0 && d <= numDaysInMonth[m])
                return d;
            if (d <= 0)
                return 1;
            else
                return numDaysInMonth[m];
        }

        public static int verifyDay(string date)
        {
            return verifyDay(Int32.Parse(date.Substring(0, 2)), Int32.Parse(date.Substring(2, 2)));
        }
        #endregion

        #region 'dateIn' and 'daysTo' and 'daysBetween' functions

        /// <summary>
        /// Calculates the date after numDays days
        /// </summary>
        /// <param name="numDays">number of days</param>
        /// <returns></returns>
        public string dateIn(int numDays)
        {
            return dateIn(month, day, year, numDays);
        }

        /// <summary>
        /// Finds the date it will be from startDate after (numDays) days pass
        /// </summary>
        /// <param name="startDate">Starting date, MMDDYYYY </param>
        /// <param name="numDays">Number of days that pass</param>
        /// <returns></returns>
        public static string dateIn(string startDate, int numDays)
        {
            return dateIn(Int32.Parse(startDate.Substring(0, 2)), Int32.Parse(startDate.Substring(2, 2)), Int32.Parse(startDate.Substring(4, 4)), numDays);
        }

        /// <summary>
        /// Finds the date it will be from startDate after (numDays) days pass 
        /// </summary>
        /// <param name="startMonth">Starting date's month</param>
        /// <param name="startDay">Starting date's day</param>
        /// <param name="startYear">starting date's year</param>
        /// <param name="numDays">Number of days that pass</param>
        /// <returns></returns>
        public static string dateIn(int startMonth, int startDay, int startYear, int numDays)
        {

            if (numDaysInMonth[startMonth] < startDay + numDays)
            {
                do
                {
                    numDays -= numDaysInMonth[startMonth] - startDay;
                    if (numDays > 0)
                    {
                        startMonth++;
                        if (startMonth > numMonthsInYear)
                        {
                            startMonth = 1;
                            startYear++;
                        }
                        startDay = 0;
                    }
                } while (numDays >= numDaysInMonth[startMonth]);
                // if numdays is 0, means the date landed at the very end of the month, assign d to numDays unless it's 0
                startDay = numDays != 0 ? numDays : numDaysInMonth[startMonth] - startDay;
            }
            else
            {
                startDay += numDays;
            }

            string monthString = enforceMonthFormat(startMonth.ToString());
            string yearString = enforceYearFormat(startYear.ToString());
            string dayString = enforceDayFormat(monthString, startDay.ToString(), yearString);
            return monthString + dayString + yearString;
        }

        /// <summary>
        /// returns the amount of days between current date and input date
        /// </summary>
        /// <param name="toMonth"></param>
        /// <param name="toDay"></param>
        /// <param name="toYear"></param>
        /// <returns></returns>
        public int daysTo(int toMonth, int toDay, int toYear)
        {
            return daysBetween(month, day, year, toMonth, toDay, toYear);
        }

        /// <summary>
        /// Calculates how many days there are between current date and input date
        /// </summary>
        /// <param name="dateString">Input date formatted as MMDDYYYY</param>
        /// <returns></returns>
        public int daysTo(string dateString)
        {
            return daysTo(Int32.Parse(dateString.Substring(0, 2)), Int32.Parse(dateString.Substring(2, 2)), Int32.Parse(dateString.Substring(4, 4)));
        }

        /// <summary>
        /// Calculates how many days between the 'begin' date and 'to' date
        /// </summary>
        /// <param name="beginMonth">beginning date's month</param>
        /// <param name="beginDay">beginning date's day</param>
        /// <param name="beginYear">beginning date's year</param>
        /// <param name="toMonth">target date's month</param>
        /// <param name="toDay">target date's day</param>
        /// <param name="toYear">target date's year</param>
        /// <returns>days between the input date</returns>
        public static int daysBetween(int beginMonth, int beginDay, int beginYear, int toMonth, int toDay, int toYear)
        {
            // This is pretty gross and i don't like to think about it, neither should you

            int numDays = 0; // Counter that's returned
            if (beginMonth != toMonth && toYear == beginYear)
            {
                while (toMonth != beginMonth)
                {
                    numDays += numDaysInMonth[beginMonth] - beginDay;
                    beginDay = 0;
                    if (++beginMonth > numMonthsInYear)
                    {
                        beginMonth = 1;
                        beginYear++;
                    }
                }
                numDays += toDay;
            }
            else if (beginMonth == toMonth && toYear == beginYear && toDay > beginDay)
            {
                numDays = toDay - beginDay;
            }
            else if (toYear != beginYear)
            {
                while (toYear - beginYear > 2)
                {
                    numDays += numDaysInYear;
                    beginYear++;
                }
                while (toMonth != beginMonth || toYear != beginYear)
                {
                    numDays += numDaysInMonth[beginMonth] - beginDay;
                    beginDay = 0;
                    if (++beginMonth > numMonthsInYear)
                    {
                        beginMonth = 1;
                        beginYear++;
                    }
                }
                numDays += toDay;
            }
            return numDays;
        }

        /// <summary>
        /// Calculates how many days between the 'begin' date and 'to' date
        /// </summary>
        /// <param name="beginDate">starting date formatted as MMDDYYYY</param>
        /// <param name="toDate">target date formatted as MMDDYYYY</param>
        /// <returns>days between the input date</returns>
        public static int daysBetween(string beginDate, string toDate)
        {
            return daysBetween(Int32.Parse(beginDate.Substring(0, 2)), Int32.Parse(beginDate.Substring(2, 2)), Int32.Parse(beginDate.Substring(4, 4)),
                Int32.Parse(toDate.Substring(0, 2)), Int32.Parse(toDate.Substring(2, 2)), Int32.Parse(toDate.Substring(4, 4)));
        }
        #endregion
    }
}
