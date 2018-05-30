using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace WarhammerCalendarManager
{
    public enum moonPhase {newMoon, waxingCrsec, firstQuarter, waxingGib, full, waningGib, lastQuarter, waningCresc};
    public class CalendarType
    {


        #region jsonData

        public string calendarName { get; set; }

        readonly static int numDaysInYear = 400;


        readonly static int numMonthsInYear = 12;

        readonly static string[] intercalaryHolidays = { null, "Hexentag", null, "Mitterfruhl", null, null, "Sonnstill", "Geheimnistag", null, "Mittherbst", null, null, "Monstille" };
        readonly static string[] intercalaryAltHolidays = { null, "Witching Day", null, "Start Growth", null, null, "Sun Still", "Day of Mystery", null, "Less Growth", null, null, "World Still" };

        readonly static string[] monthNames = { null, "Nachexen", "Jahrdrung", "Pflugzeit", "Sigmarzeit", "Sommerzeit", "Vorgeheim", "Nachgeheim", "Erntezeit", "Brauzeit", "Kaldezeit", "Ulriczeit", "Vorhexen" };
        readonly static string[] altMonthNames = { null, "After-Witching", "Year-Turn", "Ploughtide", "Sigmartide", "Summertide", "Fore-Mystery", "After-Mystery", "Harvest-Tide", "Brewmonth", "Chillmonth", "Ulrictide", "Fore-Witching" };


        readonly static int[] numDaysInMonth = {0, 32, 33, 33, 33, 33, 33, 32, 33, 33, 33, 33, 33 };
        readonly static int[] numDaysInMonthIncludingHolidays = { 0, 33, 33, 34, 33, 33, 34, 33, 33, 34, 33, 33, 34 };

        readonly static int numDaysInWeek = 8;

        readonly static string[] weekdayNames = {"Wellentag", "Aubentag", "Marktag", "Backertag", "Bezahltag", "Konistag", "Angestag", "Festag"};
        readonly static string[] altWeekdayNames = { "Workday", "Levyday", "Marketday", "Bakeday", "Taxday", "Kingday", "Startweek", "Holiday" };

        readonly static int mann_Cycle = 25;
        readonly static int mann_Shift = 12;
        static moonPhase[] mann_Phases;

        static moonPhase[] morr_Phases;
        static Random morrslieb;

        readonly static string[] phase_strings = { "New Moon", "Waxing Crescent", "First Quarter","Waxing Gibbous", "Full Moon", "Waning Gibbous", "Last Quarter", "Waning Crescent" };


        readonly static int startYear = 0;

        readonly static int startDay = 0;

        static string[] UniversalNoteDates = {
            "0101",
            "0300",
            "0333",
            "0418",
            "0600",
            "0633",
            "0801",
            "0900",
            "0933",
            "1200",
            "1233" };
        static string[] UniversalNoteContents = {
            "Year Blessing",
            "Holy day for Manaan, Taal, and Ulric",
            "First Quaff (Dwarfs)",
            "Sigmarsfest",
            "Holy day for Taal, Rhya, and Elf gods",
            "Saga (Dwarfs)",
            "Start of Pie Week (Halflings)",
            "Holy day for Rhya, Taal, and Ulric",
            "Second Breech (Dwarfs)",
            "Holy day for Ulric, Taal, an Rhya",
            "Keg End (Dwarfs)" };

        #endregion

        #region Current data
        int month;
        int day;
        int year;
        int dayOfWeek;
        int mannCounter;
        int morrCounter;
        int currentMorrSeed;
        int morrSize;
        public int MorrSize
        {
            get
            {
                return morrSize;
            }
        }
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


        public CalendarType()
        {
            setDate(1, 1, 2522);
            createMoonPhaseArray();
        }

        // cycle / 8 = full moon day length (rounded to nearest)
        // waning crescent -> full moon takes 4 days
        private void createMoonPhaseArray()
        {
            mann_Phases = new moonPhase[mann_Cycle];

            int arrayIndex = 0; // index for adding phases to arrayToAdd


            int extraDays = mann_Cycle % 8;                         // If 8 doesn't evenly divide the cycle, there will be extra days for some phases (there are 8 phases)
            bool[] phasesWithExtraDay = extraDayPlacement(extraDays); // find out which phases might have an extra day length


            // Outer loop indexes through each phase, inner loop allocates that phase "cycle/8" times, then adds an extra day if applicable
            for (int moonPhaseIndex = 0; moonPhaseIndex < 8; moonPhaseIndex++)
            {
                for (int allocater = 0; allocater < (mann_Cycle / 8); allocater++)
                {
                    mann_Phases[arrayIndex++] = (moonPhase)moonPhaseIndex;
                }
                if (phasesWithExtraDay[moonPhaseIndex]) // add extra day
                    mann_Phases[arrayIndex++] = (moonPhase)moonPhaseIndex;
            }
            // TODO: CORRECT FOR ACTUAL CALENDAR

            morr_Phases = new moonPhase[8];
            currentMorrSeed = 0;
            for (int moonPhaseIndex = 0; moonPhaseIndex < 8; moonPhaseIndex++)
            {
                morr_Phases[moonPhaseIndex] = (moonPhase)moonPhaseIndex;
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
                    day = 0; // Hexentag
                    month = 1;
                    year++;
                    subDayOfWeek(); // Intercalary days are not considered a weekday, cancel out the addDayofWeek() below
                                    // Jank implementation but whatever
                }
                else if (month == 3 || month == 6 || month == 7 || month == 9 || month == 12)
                {
                    day = 0;
                    subDayOfWeek();// Intercalary days are not considered a weekday
                                   // Jank implementation but whatever
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
            mannCounter++;
            if (mannCounter >= mann_Cycle)
                mannCounter = 0;

            morrsliebNextPhase();

        }
        #endregion

        #region backward in time
        public void subDay()
        {
            day--;
            if (day < 1)
            {
                if (month == 1 || month == 3 || month == 6 || month == 7 || month == 9 || month == 12)
                {
                    if (day < 0)
                    {
                        month--;
                        if (month < 1)
                        {
                            year--;
                            month = numMonthsInYear;
                            day = numDaysInMonth[month];
                        }
                        day = numDaysInMonth[month];
                    }
                    else // if day == 0 and is intercalary day
                        addDayOfWeek(); // jank but whatever, offset subdayofweek(), intercalary days are not considered a weekday

                }
                else
                {
                    month--;
                    if (month < 1)
                    {
                        year--;
                        month = numMonthsInYear;
                        day = numDaysInMonth[month];
                    }
                    day = numDaysInMonth[month];
                }


            }
            subDayOfWeek();
            subMoonPhase();

            if (year < 0)
            {
                day = 0;
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
            mannCounter--;
            if (mannCounter < 0)
                mannCounter = mann_Cycle - 1;

            determineMorrslieb();

        }
        #endregion


        #region setDate and all functions determining day of week, moonphase, etc

        public void setDate(string dateString)
        {
            setDate(Int32.Parse(dateString.Substring(0, 2)), Int32.Parse(dateString.Substring(2, 2)), Int32.Parse(dateString.Substring(4, 4)));
        }
        // ********************************** START HERE

        public void setDate(int m, int d, int y)
        {
            if (m > numMonthsInYear)
                m = numMonthsInYear;
            else if (m <= 0)
                m = 1;

            month = m;

            if (d > numDaysInMonth[month])
                d = numDaysInMonth[month];

            else if (d < 1)
            {
                if (month == 1 || month == 3 || month == 6 || month == 7 || month == 9 || month == 12)
                {
                    day = 0;
                }
                else
                {
                    day = 1;
                }
            }
            else
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
            int yearDifference = Math.Abs(y - startYear);
            int daysToSubtract = yearDifference * 6;

            // Since intercalary days are not considered weekdays, we have to subtract however have passed since the start date and the input date
            switch (m)
            {
                case 1:
                    if (d > 0)
                        daysToSubtract++;
                    break;
                case 2:
                    daysToSubtract++;
                    break;
                case 3:
                    if (d > 0)
                        daysToSubtract += 2;
                    else
                        daysToSubtract++;
                    break;
                case 4:
                    daysToSubtract += 2;
                    break;
                case 5:
                    daysToSubtract += 2;

                    break;
                case 6:
                    if (d > 0)
                        daysToSubtract += 3;
                    else
                        daysToSubtract += 2;
                    break;
                case 7:
                    if (d > 0)
                        daysToSubtract += 4;
                    else
                        daysToSubtract += 3;
                    break;
                case 8:
                    daysToSubtract += 4;
                    break;
                case 9:
                    if (d > 0)
                        daysToSubtract += 5;
                    else
                        daysToSubtract += 4;
                    break;
                case 10:
                    daysToSubtract += 5;
                    break;
                case 11:
                    daysToSubtract += 5;
                    break;
                case 12:
                    if (d > 0)
                        daysToSubtract += 6;
                    else
                        daysToSubtract += 5;
                    break;
            }

            int totalDaysPassedSinceStart = yearDifference * numDaysInYear + (determineDayOfYear(m, d, y) - 1);
            int modResult = (totalDaysPassedSinceStart - daysToSubtract) % (numDaysInWeek);
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

              mannCounter = Math.Abs(daysSinceFirstDay - mann_Shift) % mann_Cycle;

            // For some reason, if the year is zero, the phases are off by one, not sure why
            // Has to do with determineDayofYear adding 1 if month starts at 0, but that's necessary
            if (year == 0)
                addMoonPhase();

            determineMorrslieb();

        }

        /// <summary>
        /// Determines the next morrslieb phase
        /// Calls a new Random.Next() for every day
        /// </summary>
        private void morrsliebNextPhase()
        {
            if (year != currentMorrSeed)
            {
                morrslieb = new Random(year);
                currentMorrSeed = year;
                int days = determineDayOfYear();
                for (int i = 0; i < days - 1; i++)
                {
                    morrslieb.Next();
                }
            }
            morrSize = morrslieb.Next(50, 200);
            morrCounter = morrSize % 8;

            // If it is geheimnistag or hexentag
            if ((day == 0 && (month == 1 || month == 7)))
            {
                morrSize = 200;
                morrCounter = 4;
            }
        }

        /// <summary>
        /// Since morrsliebNextPhase calls Random.Next() for every day, going backwards doesn't work (a new Next() would not be the same as the previous)
        /// Therefore, have to redo, call Next() for every day so far this year, stop at the current day
        /// </summary>
        private void determineMorrslieb()
        {
            currentMorrSeed = -1;
            morrsliebNextPhase();
        }

        public string determineCurrentStarSign()
        {
            return determineStarSignFromDate(month, day);

        }

        public static string determineStarSignFromDate(int m, int d)
        {
            switch (m)
            {
                case 1:
                    if (d <= 7)
                        return "Wymund the Anchorite";
                    else if (d <= 27)
                        return "The Big Cross";
                    else
                        return "The Limner's Line";
                case 2:
                    if (d <= 15)
                        return "The Limner's Line";
                    else
                        return "Gnuthus the Ox";
                case 3:
                    if (d <= 1)
                        return "Gnuthus the Ox";
                    else if (d <= 21)
                        return "Dragomas the Drake";
                    else
                        return "The Gloaming";
                case 4:
                    if (d <= 8)
                        return "The Gloaming";
                    else if (d <= 28)
                        return "Grungi's Baldrick";
                    else
                        return "Mammit the Wise";
                case 5:
                    if (d <= 15)
                        return "Mammit the Wise";
                    else
                        return "Mummit the Fool";
                case 6:
                    if (d <= 1)
                        return "Mummit the Fool";
                    else if (d <= 21)
                        return "The Two Bullocks";
                    else
                        return "The Dancer";
                case 7:
                    if (d <= 7)
                        return "The Dancer";
                    else if (d <= 27)
                        return "The Drummer";
                    else
                        return "The Piper";
                case 8:
                    if (d <= 15)
                        return "The Piper";
                    else
                        return "Vobist the Faint";
                case 9:
                    if (d <= 1)
                        return "Vobist the Faint";
                    else if (d <= 21)
                        return "The Broken Cart";
                    else
                        return "The Greased Goat";
                case 10:
                    if (d <= 8)
                        return "The Greased Goat";
                    else if (d <= 28)
                        return "Rhya's Cauldron";
                    else
                        return "Cacklefax the Cockerel";
                case 11:
                    if (d <= 15)
                        return "Cacklefax the Cockerel";
                    else
                        return "The Bonesaw";
                case 12:
                    if (d <= 1)
                        return "The Bonesaw";
                    else if (d <= 21)
                        return "The Witchling Star";
                    else
                        return "Wymund the Anchorite";

            }
            return null;
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
                dayAccumulator += numDaysInMonthIncludingHolidays[i + 1];
            }
            dayAccumulator += d; // add current day to sum
            if (isMonthWithHoliday(m))
                dayAccumulator++;
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

            return sb.ToString();
        }

        public string[] currentMoonPhases()
        {
            return new string[] { phase_strings[(int)(mann_Phases[mannCounter])], phase_strings[(int)(morr_Phases[morrCounter])]};
        }

        public static string returnGivenDateWithWeekday(int m, int d, int y)
        {
            StringBuilder dateString = new StringBuilder();
            if (m > numMonthsInYear || m <= 0 || d > numDaysInMonth[m] || d < 0)
                return null;
            else if (d == 0 && (m == 1 || m == 3 || m == 6 || m == 7 || m == 9 || m == 12))
            {
                dateString.Append(intercalaryHolidays[m] + ", " + y);
            }
            else
            {
                dateString.Append(weekdayNames[determineDayOfWeek(m, d, y)]);
                dateString.Append(", " + monthNames[m] + " " + d);
                dateString.Append(", " + y);
            }
            return dateString.ToString();
        }

        public static string returnGivenDateWithWeekday(string dateString)
        {
            if (dateString.Length == 8)
            {
                return returnGivenDateWithWeekday(Int32.Parse(dateString.Substring(0, 2)), Int32.Parse(dateString.Substring(2, 2)), Int32.Parse(dateString.Substring(4, 4)));
            }

            else
                return null;
        }

        // TODO: organize date formats
        public string returnConciseCurrentDate()
        {
            string dateWithWeekday = returnGivenDateWithWeekday(month, day, year);
            for (int i = 0; i < dateWithWeekday.Length; i++)
            {
                if (dateWithWeekday.ElementAt(i) == ' ')
                    return dateWithWeekday.Substring(i + 1).Trim(',');
            }
            return null;
        }

        public static string returnConciseGivenDate(int month, int day, int year)
        {
            string dateWithWeekday = returnGivenDateWithWeekday(month, day, year);
                        for (int i = 0; i < dateWithWeekday.Length; i++)
            {
                if (dateWithWeekday.ElementAt(i) == ' ')
                    return dateWithWeekday.Substring(i + 1).Replace(",", "");
            }
            return null;
        }

        public static string returnConciseGivenDate(string dateString)
        {
            if (dateString.Length == 8)
            {
                return returnConciseGivenDate(Int32.Parse(dateString.Substring(0, 2)), Int32.Parse(dateString.Substring(2, 2)), Int32.Parse(dateString.Substring(4, 4)));
            }

            else
                return null;
        }

        public string returnCurrentDateWithWeekday()
        {
            return returnGivenDateWithWeekday(month, day, year);
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

                // If the length is 2, intercalary holiday
                if (splitArray.Length == 2)
                {
                    for (int i = 0; i < numMonthsInYear; i++)
                    {
                        if (intercalaryHolidays[i] == splitArray[i])
                        {
                            month = i.ToString("00");
                            year = splitArray[i + 1];
                        }
                    }
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

        // returns true if this month begins with an intercalary day (month starts at day 0)
        public static bool isMonthWithHoliday(int m)
        {
            return (m == 1 || m == 3 || m == 6 || m == 7 || m == 9 || m == 12);

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

        public override string ToString()
        {
            StringBuilder stringDate = new StringBuilder();

            stringDate.Append(month.ToString("00"));
            stringDate.Append(day.ToString("00"));
            stringDate.Append(year.ToString("0000"));

            return stringDate.ToString();
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

            if (testDay == "")
            {
                testDay = "01";
            }
            if (month != "")
            {
                testDay = verifyDay(Int32.Parse(month), Int32.Parse(testDay)).ToString("00");
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

        /// <summary>
        /// Verifies that given day is possible in given month
        /// includes day = 0 for intercalary holidays
        /// </summary>
        /// <param name="m"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static int verifyDay(int m, int d)
        {
            if (m > numMonthsInYear || m < 1) // not what month is so just return d
                return d;
            if (d > 0 && d <= numDaysInMonth[m]) // if between 1 and numdays in month
                return d;
            if (d <= 0 && (m == 1 || m == 3 || m == 6 || m == 7 || m == 9 || m == 12)) // If day == 0 possible intercalary holiday, check for that
                return 0;
            else if (d <= 0)                                                           // If not, return 1
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
        //public static string dateIn(int startMonth, int startDay, int startYear, int numDays)
        //{

        //    if (numDaysInMonth[startMonth] < startDay + numDays)
        //    {
        //        do
        //        {
        //            if (startDay == 0 && isMonthWithHoliday(startMonth))
        //                numDays -= numDaysInMonthIncludingHolidays[startMonth];
        //            else
        //                numDays -= numDaysInMonth[startMonth] - startDay;

        //            if (numDays > 0)
        //            {
        //                startMonth++;
        //                if (startMonth > numMonthsInYear)
        //                {
        //                    startMonth = 1;
        //                    startYear++;
        //                    startDay = 0;
        //                }
        //                startDay = 0;

        //            }
        //        } while (numDays >= numDaysInMonth[startMonth]);
        //        // if numdays is 0, means the date landed at the very end of the month, assign d to numDays unless it's 0
        //        startDay = numDays != 0 ? numDays : numDaysInMonth[startMonth] - startDay;
        //        if (isMonthWithHoliday(startMonth))
        //            startDay--;
        //    }
        //    else
        //    {
        //        startDay += numDays;
        //    }

        //    string monthString = enforceMonthFormat(startMonth.ToString());
        //    string yearString = enforceYearFormat(startYear.ToString());
        //    string dayString = enforceDayFormat(monthString, startDay.ToString(), yearString);
        //    return monthString + dayString + yearString;
        //}

        //public static string dateIn(int startMonth, int startDay, int startYear, int numDays)
        //{
        //    int m = startMonth;
        //    int d = startDay;
        //    int y = startYear;
        //    if (numDaysInMonth[m] < startDay + numDays)
        //    {
        //        do
        //        {
        //            numDays -= numDaysInMonth[m] - d;
        //            if (d == 0 && isMonthWithHoliday(m))
        //            {
        //                numDays--;
        //            }
        //            d = 0;

        //            if (numDays > 0)
        //            {
        //                m++;
        //                if (m > numMonthsInYear)
        //                {
        //                    m = 1;
        //                    y++;
        //                }
        //            }
        //        } while (numDays >= numDaysInMonthIncludingHolidays[m]);
        //        // if numdays is 0, means the date landed at the very end of the month, assign d to numDays unless it's 0
        //        d = numDays != 0 ? numDays : numDaysInMonthIncludingHolidays[m] - d;
        //        if (isMonthWithHoliday(m))// && (m != startMonth || y != startYear))
        //            d--;
        //    }
        //    else
        //    {
        //        d += numDays;
        //    }

        //    string monthString = enforceMonthFormat(m.ToString());
        //    string yearString = enforceYearFormat(y.ToString());
        //    string dayString = enforceDayFormat(monthString, d.ToString(), yearString);
        //    return monthString + dayString + yearString;
        //}


        public static string dateIn(int startMonth, int startDay, int startYear, int numDays)
        {
            int m = startMonth;
            int d = startDay;
            int y = startYear;


            if (d == 0 && numDays > 0)
            {
                d++;
                numDays--;
            }

            while (d + numDays > numDaysInMonth[m])
            {
                if (d == 0)
                {
                    numDays -= numDaysInMonthIncludingHolidays[m];
                }
                else
                {
                    numDays -= numDaysInMonth[m] - d;
                    d = 0;
                }

                m++;
                if (m > numMonthsInYear)
                {
                    m = 1;
                    y++;
                }

            }

            d += numDays;
            if (isMonthWithHoliday(m) && (m != startMonth || y != startYear))
                d--;

            //d = numDays == 0 ? numDaysInMonth[startMonth] - d : numDays;


            string monthString = enforceMonthFormat(m.ToString());
            string yearString = enforceYearFormat(y.ToString());
            string dayString = enforceDayFormat(monthString, d.ToString(), yearString);
            return monthString + dayString + yearString;
        }


        // TODO: if 100 days from vorhexen 33, becomes 99, if 99 from vorhexen 33, becomes 67

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
            if (isMonthWithHoliday(beginMonth))
                numDays--;

            // If dates have the same year but not same month
            if (beginMonth != toMonth && toYear == beginYear)
            {
                while (toMonth != beginMonth)
                {
                    numDays += numDaysInMonthIncludingHolidays[beginMonth] - beginDay;
                    beginDay = 0;
                    if (++beginMonth > numMonthsInYear)
                    {
                        beginMonth = 1;
                        beginYear++;
                    }
                }
                numDays += toDay;
                if (isMonthWithHoliday(toMonth))
                    numDays++;
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
                    numDays += numDaysInMonthIncludingHolidays[beginMonth] - beginDay;
                    beginDay = 0;
                    if (++beginMonth > numMonthsInYear)
                    {
                        beginMonth = 1;
                        beginYear++;
                    }
                }
                numDays += toDay;
                if (isMonthWithHoliday(toMonth))
                    numDays++;

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
