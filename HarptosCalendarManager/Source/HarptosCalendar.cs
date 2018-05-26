using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HarptosCalendarManager
{
    public enum moonPhase { full, waningGib, lastQuarter, waningCresc, newMoon, waxingCrsec, firstQuarter, waxingGib };
    public class HarptosCalendar
    {
        [Newtonsoft.Json.JsonProperty]
        int day;
        [Newtonsoft.Json.JsonProperty]
        int month;
        [Newtonsoft.Json.JsonProperty]
        int year;

        readonly static string[] UniversalNoteDates = { "0319", "0620", "0921", "1220" };
        readonly static string[] UniversalNoteContents = { "Spring Equinox", "Summer Solstice", "Autumn Equinox", "Winter Solstice" };

        bool[] holidays; // Array of bools for each holiday, if it is a holiday, find the true value for which one
        public bool isHoliday { get; set; } //Is current day a holiday?       // 0 1 3 5 7: locations of holidays that do not occur on days of a month 
        readonly static string[] holidayNames = { "Shieldmeet", "Midwinter", "Spring Equinox", "Greengrass", "Summer Solstice", "Midsummer", "Autumn Equinox", "Highharvestide", "Feast of the Moon", "Winter Solstice" };
        readonly static string[] monthNames = { null, "Hammer", "Alturiak", "Ches", "Tarsakh", "Mirtul", "Kythorn", "Flamerule", "Eleasis", "Eleint", "Merpenoth", "Uktar", "Nightal" };
        readonly static string[] altMonthNames = { null, "Deepwinter", "The Claw of Winter", "The Claw of Sunsets", "The Claw of Storms", "The Melting", "The Time of Flowers", "Summertide", "Highsun", "The Fading", "Leaffall", "The Rotting", "The Drawing Down" };
        string[] yearNames;
        public readonly static moonPhase[] moonPhases = {
            moonPhase.full,
            moonPhase.waningGib,
            moonPhase.waningGib,
            moonPhase.waningGib,
            moonPhase.waningGib,
            moonPhase.waningGib,
            moonPhase.waningGib,
            moonPhase.lastQuarter,
            moonPhase.waningCresc,
            moonPhase.waningCresc,
            moonPhase.waningCresc,
            moonPhase.waningCresc,
            moonPhase.waningCresc,
            moonPhase.waningCresc,
            moonPhase.waningCresc,
            moonPhase.newMoon,
            moonPhase.waxingCrsec,
            moonPhase.waxingCrsec,
            moonPhase.waxingCrsec,
            moonPhase.waxingCrsec,
            moonPhase.waxingCrsec,
            moonPhase.waxingCrsec,
            moonPhase.waxingCrsec,
            moonPhase.firstQuarter,
            moonPhase.waxingGib,
            moonPhase.waxingGib,
            moonPhase.waxingGib,
            moonPhase.waxingGib,
            moonPhase.waxingGib,
            moonPhase.waxingGib,
            moonPhase.waxingGib,
            moonPhase.full,
            moonPhase.waningGib,
            moonPhase.waningGib,
            moonPhase.waningGib,
            moonPhase.waningGib,
            moonPhase.waningGib,
            moonPhase.waningGib,
            moonPhase.lastQuarter,
            moonPhase.waningCresc,
            moonPhase.waningCresc,
            moonPhase.waningCresc,
            moonPhase.waningCresc,
            moonPhase.waningCresc,
            moonPhase.waningCresc,
            moonPhase.newMoon,
            moonPhase.waxingCrsec,
            moonPhase.waxingCrsec,
            moonPhase.waxingCrsec,
            moonPhase.waxingCrsec,
            moonPhase.waxingCrsec,
            moonPhase.waxingCrsec,
            moonPhase.waxingCrsec,
            moonPhase.firstQuarter,
            moonPhase.waxingGib,
            moonPhase.waxingGib,
            moonPhase.waxingGib,
            moonPhase.waxingGib,
            moonPhase.waxingGib,
            moonPhase.waxingGib,
            moonPhase.waxingGib };
        int moonCounter;

        public HarptosCalendar() : this(1, 1, 1491)
        {

        }

        public HarptosCalendar(int y) : this(1, 1, y)
        {
        }

        public HarptosCalendar(int m, int d, int y)
        {
            holidays = new bool[10];
            setDate(m, d, y);
            readInYears();
        }

        public int getDay()
        {
            return day;
        }

        public int getMonth()
        {
            return month;
        }

        public string getMonthName()
        {
            return monthNames[month];
        }

        public string getMonthAltName()
        {
            return altMonthNames[month];
        }

        public int getYear()
        {
            return year;
        }

        #region Forward in time
        /// <summary>
        /// Move to the next day in the calendar
        /// </summary>
        public void addDay()
        {

            day++;
            addMoonPhase();
            if (isHoliday)
                clearHolidays();

            if (day > 32)
            {
                day = 1;
                month++;
            }

            if (day == 31)
            {
                switch (month)
                {
                    case 1:
                        isHoliday = true;
                        holidays[1] = true;
                        break;
                    case 4:
                        isHoliday = true;
                        holidays[3] = true;
                        break;
                    case 7:
                        isHoliday = true;
                        holidays[5] = true;
                        break;
                    case 9:
                        isHoliday = true;
                        holidays[7] = true;
                        break;
                    case 11:
                        isHoliday = true;
                        holidays[8] = true;
                        break;
                    default:
                        day = 1;
                        month++;
                        if (month == 13)
                        {
                            month = 1;
                            year++;
                        }
                        break;
                } // end switch(month)
            } // end if day == 31

            else if (day == 32)
            {
                if (year % 4 == 0 && month == 7)
                {
                    isHoliday = true;
                    holidays[0] = true;
                }
                else
                {
                    day = 1;
                    month++;
                }
            }

            else // if day is not 31 or 32
            {
                // check if in-month holiday (equinoxes)

                if (day == 19 && month == 3)
                {
                    isHoliday = true;
                    holidays[2] = true;
                }

                if (day == 20 && month == 6)
                {
                    isHoliday = true;
                    holidays[4] = true;
                }

                if (day == 21 && month == 9)
                {
                    isHoliday = true;
                    holidays[6] = true;
                }

                if (day == 20 && month == 12)
                {
                    isHoliday = true;
                    holidays[9] = true;
                }
            }
        }

        /// <summary>
        /// Move to the next n days in the calendar
        /// </summary>
        /// <param name="num">The number of days passing</param>
        public void addDay(int num)
        {
            for (int i = 0; i < num; i++)
                addDay();
        }

        public void addTenday()
        {
            addDay(10);
        }

        public void addMonth()
        {
            addDay(30);
        }

        public void addMonth(int num)
        {
            for (int i = 0; i < num; i++)
                addMonth();
        }

        public void addYear()
        {
            if (year % 4 == 0 && day == 32)
                setDate(month, day - 1, year + 1);
            else
                year++;
            determineMoonCounter();
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
            day--;
            subMoonPhase();
            if (isHoliday)
                clearHolidays();

            if (day == 0)
            {
                month--;
                switch (month)
                {
                    case 0:
                        month = 12;
                        day = 30;
                        year--;
                        break;
                    case 1:
                        day = 31;
                        isHoliday = true;
                        holidays[1] = true;
                        break;
                    case 4:
                        day = 31;
                        isHoliday = true;
                        holidays[3] = true;
                        break;
                    case 7:
                        if (year % 4 == 0)
                        {
                            day = 32;
                            isHoliday = true;
                            holidays[0] = true;
                        }
                        else
                        {
                            day = 31;
                            isHoliday = true;
                            holidays[5] = true;
                        }
                        break;
                    case 9:
                        day = 31;
                        isHoliday = true;
                        holidays[7] = true;
                        break;
                    case 11:
                        day = 31;
                        isHoliday = true;
                        holidays[8] = true;
                        break;
                    default:
                        day = 30;
                        break;
                } // end switch(month)
            } // end if day == 0

            else if (day == 31) // Very specific case: should only happen if leap year and subtract a day on shieldmeet
            {
                isHoliday = true;
                holidays[5] = true;
            }

            else // if day is not 0
            {
                // check if in-month holiday (equinoxes)

                if (day == 19 && month == 3)
                {
                    isHoliday = true;
                    holidays[2] = true;
                }

                if (day == 20 && month == 6)
                {
                    isHoliday = true;
                    holidays[4] = true;
                }

                if (day == 21 && month == 9)
                {
                    isHoliday = true;
                    holidays[6] = true;
                }

                if (day == 20 && month == 12)
                {
                    isHoliday = true;
                    holidays[9] = true;
                }
            }

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
            if (year % 4 == 0 && day == 32)
                setDate(month, day - 1, year - 1);
            else
                year--;
            determineMoonCounter();
        }
        #endregion

        public bool setDate(string dateString)
        {
            if (dateString.Length == 8)
            {
                return setDate(Int32.Parse(dateString.Substring(0, 2)), Int32.Parse(dateString.Substring(2, 2)), Int32.Parse(dateString.Substring(4, 4)));
            }

            else
                return false;
        }

        /// <summary>
        /// Sets the date to the input values
        /// </summary>
        /// <param name="m">month</param>
        /// <param name="d">day</param>
        /// <param name="y">year</param>
        /// <returns></returns>
        public bool setDate(int m, int d, int y)
        {
            clearHolidays();
            switch (d)
            {
                case 31:
                    if (m == 1 || m == 4 || m == 7 || m == 9 || m == 11)
                    {
                        isHoliday = true;
                        switch (m)
                        {
                            case 1:
                                holidays[1] = true;
                                break;
                            case 4:
                                holidays[3] = true;
                                break;
                            case 7:
                                holidays[5] = true;
                                break;
                            case 9:
                                holidays[7] = true;
                                break;
                            case 11:
                                holidays[8] = true;
                                break;
                        }
                        day = 31;
                        month = m;
                        year = y;
                    }
                    else
                        return false;
                    break;

                case 32:
                    if (m == 7 && ((y % 4) == 0))
                    {
                        isHoliday = true;
                        holidays[0] = true;
                        month = m;
                        day = d;
                        year = y;
                    }
                    else
                        return false;
                    break;

                default:
                    if (d > 32 || d <= 0)
                        return false;
                    else
                    {
                        day = d;
                        month = m;
                        year = y;

                        if (day == 19 && month == 3)
                        {
                            isHoliday = true;
                            holidays[2] = true;
                        }
                        else if (day == 20 && month == 6)
                        {
                            isHoliday = true;
                            holidays[4] = true;
                        }
                        else if (day == 21 && month == 9)
                        {
                            isHoliday = true;
                            holidays[6] = true;
                        }
                        else if (day == 20 && month == 12)
                        {
                            isHoliday = true;
                            holidays[9] = true;
                        }
                    }
                    break;
            }
            determineMoonCounter();
            return true; // if the date was false, the switch statement would have returned false
        }                // so if it gets to this point, it's true

        /// <summary>
        /// Returns date in format (monthName) (dayNumber) (yearNumber) and holiday, if present
        /// </summary>
        /// <param name="dateString">String representing date, in format MMDDYYYY</param>
        /// <returns></returns>
        public static string returnGivenDate(string dateString)
        {
            if (dateString.Length == 8)
            {
                return returnGivenDate(Int32.Parse(dateString.Substring(0, 2)), Int32.Parse(dateString.Substring(2, 2)), Int32.Parse(dateString.Substring(4, 4)));
            }

            else
                return null;
        }

        /// <summary>
        /// Returns date in format (monthName) (dayNumber) (yearNumber) and holiday, if present
        /// </summary>
        /// <param name="m">Month number</param>
        /// <param name="d">Day number</param>
        /// <param name="y">Year number</param>
        /// <returns></returns>
        public static string returnGivenDate(int m, int d, int y)
        {
            StringBuilder dateString = new StringBuilder();
            switch (d)
            {
                case 31:
                    if (m == 1 || m == 4 || m == 7 || m == 9 || m == 11)
                    {
                        switch (m)
                        {
                            case 1:
                                dateString.Append(holidayNames[1] + " ");
                                break;
                            case 4:
                                dateString.Append(holidayNames[3] + " ");
                                break;
                            case 7:
                                dateString.Append(holidayNames[5] + " ");
                                break;
                            case 9:
                                dateString.Append(holidayNames[7] + " ");
                                break;
                            case 11:
                                dateString.Append(holidayNames[8] + " ");
                                break;
                        }
                    }
                    else
                        return null;
                    break;

                case 32:
                    if (m == 7 && ((y % 4) == 0))
                        dateString.Append(holidayNames[0] + " ");
                    else
                        return null;
                    break;

                default:
                    if (d >= 31 || d <= 0 || m >= 13 || m <= 0)
                        return null;
                    else
                    {
                        dateString.Append(monthNames[m] + " " + d + " ");

                        // Equinoxes, not shown on concise dates
                        /*  if (day == 19 && month == 3)
                          {
                              isHoliday = true;
                              holidays[2] = true;
                          }
                          else if (day == 20 && month == 6)
                          {
                              isHoliday = true;
                              holidays[4] = true;
                          }
                          else if (day == 21 && month == 9)
                          {
                              isHoliday = true;
                              holidays[6] = true;
                          }
                          else if (day == 20 && month == 12)
                          {
                              isHoliday = true;
                              holidays[9] = true;
                          }
                      }*/

                    }
                    break;
            }
            dateString.Append(y);
            return dateString.ToString();
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

                if (splitArray.Length == 3)
                {

                    for (int i = 1; i < monthNames.Length; i++)
                    {
                        if (splitArray[0].Equals(monthNames[i]))
                        {
                            month = HarptosCalendar.enforceMonthFormat(i.ToString());
                        }

                    }

                    year = HarptosCalendar.enforceYearFormat(splitArray[2]);
                    day = HarptosCalendar.enforceDayFormat(month, splitArray[1], year);
                }
                else if (splitArray.Length == 2) // holiday (except feast of the moon, else if below this for that case)
                {
                    switch (splitArray[0])
                    {
                        case "Shieldmeet":
                            month = "07";
                            day = "32";
                            break;
                        case "Midwinter":
                            month = "01";
                            day = "31";
                            break;
                        case "Greengrass":
                            month = "04";
                            day = "31";
                            break;
                        case "Midsummer":
                            month = "07";
                            day = "31";
                            break;
                        case "Highharvestide":
                            month = "09";
                            day = "31";
                            break;
                    }
                    year = HarptosCalendar.enforceYearFormat(splitArray[1]);
                }
                // The fact that feast of the moon is multiple words screws up the above method, so hard coded else if for it
                else if (splitArray.Contains("Feast") && splitArray.Contains("of") && splitArray.Contains("the") && splitArray.Contains("Moon"))
                {
                    month = "11";
                    day = "31";
                    year = HarptosCalendar.enforceYearFormat(splitArray[4]);
                }
            }
            catch (Exception e)
            {

            }
            return month + day + year;
        }


        /// <summary>
        /// Returns current date as (dayNumber)st/rd/th of (monthName) (yearNumber) and holiday, if present
        /// </summary>
        /// <returns></returns>
        public string returnDateWithHolidays()
        {
            string returnString = null;
            if (isHoliday)
            {
                bool b = false; // locates holiday in for loop
                int i;
                for (i = -1; b == false && i < 10;) // Find location of true in holiday array, location = what holiday
                    b = holidays[++i];              // once holiday is found, location is i
                if (day == 31)
                {
                    if (i == 0 || i == 1 || i == 3 || i == 5 || i == 7)
                        returnString = "It is " + holidayNames[i] + "!";
                    else
                        returnString = "It is the " + holidayNames[i] + "!";
                }
                else if (day == 32)
                {
                    if (holidays[0])
                        returnString = "It is " + holidayNames[0] + "!";
                }
                else
                {
                    switch (day)
                    {
                        case 1:
                            returnString = day + "st of " + monthNames[month] + " " + year;
                            break;
                        case 2:
                            returnString = day + "nd of " + monthNames[month] + " " + year;
                            break;
                        case 3:
                            returnString = day + "rd of " + monthNames[month] + " " + year;
                            break;
                        default:
                            returnString = day + "th of " + monthNames[month] + " " + year;
                            break;
                    }
                }
            }

            else
                switch (day)
                {
                    case 1:
                        returnString = day + "st of " + monthNames[month] + " " + year;
                        break;
                    case 2:
                        returnString = day + "nd of " + monthNames[month] + " " + year;
                        break;
                    case 3:
                        returnString = day + "rd of " + monthNames[month] + " " + year;
                        break;
                    default:
                        returnString = day + "th of " + monthNames[month] + " " + year;
                        break;
                }
            return returnString;
        }

        /// <summary>
        /// Returns current date as (dayNumber)st/rd/th of (alternateMonthName) (yearNumber), and holiday, if present
        /// </summary>
        /// <returns></returns>
        public string returnAltDateWithHolidays()
        {
            string returnString = null;
            if (isHoliday)
            {
                bool b = false;
                int i;
                for (i = -1; b == false && i < 10;)
                    b = holidays[++i];
                if (day == 31)
                {
                    if (i == 0 || i == 1 || i == 3 || i == 5 || i == 7)
                        returnString = "It is " + holidayNames[i] + "!";
                    else
                        returnString = "It is the " + holidayNames[i] + "!";
                }
                else if (day == 32)
                {
                    if (holidays[0])
                        returnString = "It is " + holidayNames[0] + "!";
                }

                else
                {
                    switch (day)
                    {
                        case 1:
                            returnString = day + "st of " + monthNames[month] + " " + year;
                            break;
                        case 2:
                            returnString = day + "nd of " + monthNames[month] + " " + year;
                            break;
                        case 3:
                            returnString = day + "rd of " + monthNames[month] + " " + year;
                            break;
                        default:
                            returnString = day + "th of " + monthNames[month] + " " + year;
                            break;
                    }
                }
            }

            else
                switch (day)
                {
                    case 1:
                        returnString = day + "st of " + altMonthNames[month] + " " + year;
                        break;
                    case 2:
                        returnString = day + "nd of " + altMonthNames[month] + " " + year;
                        break;
                    case 3:
                        returnString = day + "rd of " + altMonthNames[month] + " " + year;
                        break;
                    default:
                        returnString = day + "th of " + altMonthNames[month] + " " + year;
                        break;
                }
            return returnString;
        }

        public string currentMoonPhase()
        {
            switch (moonPhases[moonCounter])
            {
                case moonPhase.full:
                    return "Full Moon";
                case moonPhase.waningGib:
                    return "Waning Gibbous";
                case moonPhase.lastQuarter:
                    return "Last Quarter";
                case moonPhase.waningCresc:
                    return "Waning Crescent";
                case moonPhase.newMoon:
                    return "New Moon";
                case moonPhase.waxingCrsec:
                    return "Waxing Crescent";
                case moonPhase.firstQuarter:
                    return "First Quarter";
                case moonPhase.waxingGib:
                    return "Waxing Gibbous";
                default:
                    return null;
            }

        }

        /// <summary>
        /// Returns a value for day that's correct for the month (30, 31, or 32 if the input day was greater than those, depending on the month
        /// </summary>
        /// <param name="m">number of month being tested</param>
        /// <param name="d">number of day being tested</param>
        /// <param name="y">number of year being tested</param>
        /// <returns>Returns a value for day that's correct for the month (30, 31, or 32 if the input day was greater than those, depending on the month</returns>
        public static int verifyDay(int m, int d, int y)
        {
            if (m > 12 || m < 1)
                return d;
            if (d <= 30)
                return d;
            if (d > 30)
            {
                switch (m)
                {
                    case 1:
                        return 31;
                    case 4:
                        return 31;
                    case 7:
                        if (y % 4 == 0 && d >= 32)
                            return 32;
                        else
                            return 31;
                    case 9:
                        return 31;
                    case 11:
                        return 31;
                    default:
                        return 30;
                }
            }
            return -1;
        }

        public static int verifyDay(string date)
        {
            return verifyDay(Int32.Parse(date.Substring(0, 2)), Int32.Parse(date.Substring(2, 2)), Int32.Parse(date.Substring(4, 4)));
        }

        private void clearHolidays()
        {
            isHoliday = false;
            for (int i = 0; i < 10; i++)
                holidays[i] = false;
        }

        /// <summary>
        /// Return current date in format(monthName) (dayNumber) (yearNumber) and holiday, if present
        /// </summary>
        /// <returns></returns>
        public string returnConciseDate()
        {
            return returnGivenDate(month, day, year);
        }

        /// <summary>
        /// Returns only holiday string, such as "Autumn Equinox" or "Midwinter", null if current date is not a holiday
        /// </summary>
        /// <returns></returns>
        public string returnJustHoliday()
        {
            if (isHoliday)
            {
                int holidayIndex;
                for (holidayIndex = 0; holidayIndex < holidays.Length && holidays[holidayIndex] == false; holidayIndex++)
                {
                    // move to correct holiday index
                }
                return holidayNames[holidayIndex];
            }
            return null;
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


        /// <summary>
        /// returns true if input string (MMDDYYYY) is the same as the current calendar date
        /// </summary>
        /// <param name="testDate">Input date in the form of MMDDYYYY</param>
        /// <returns></returns>
        public bool sameDate(string testDate)
        {
            if (testDate.Length != 8)
                return false;
            else
                return sameDate(Int32.Parse(testDate.Substring(0, 2)), Int32.Parse(testDate.Substring(2, 2)), Int32.Parse(testDate.Substring(4, 4)));
        }

        /// <summary>
        /// returns true if input values is the same as the current calendar date
        /// </summary>
        /// <param name="testM">month value</param>
        /// <param name="testD">day value</param>
        /// <param name="testY">year value</param>
        /// <returns>true if test values match current date</returns>
        public bool sameDate(int testM, int testD, int testY)
        {
            if (testM == month && testD == day && testY == year)
                return true;
            else
                return false;
        }

        /// <summary>
        /// tests if input date is an anniversary of the current date
        /// </summary>
        /// <param name="testDate">Input date string, MMDDYYYY</param>
        /// <returns></returns>
        public bool isAnniversary(string testDate)
        {
            if (testDate.Length != 8)
                return false;
            else
                return isAnniversary(Int32.Parse(testDate.Substring(0, 2)), Int32.Parse(testDate.Substring(2, 2)));
        }

        /// <summary>
        /// tests if input date is an anniversary of the current date
        /// </summary>
        /// <param name="testM">month value</param>
        /// <param name="testD">year value</param>
        /// <returns>true if testM and testD are the same as current month and day (anniversary)</returns>
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
            return yearsAgo(Int32.Parse(inputDate.Substring(4, 4)));
        }

        /// <summary>
        /// Determines whcih date is farthest in time.
        /// returns 1 if date 1 happened later than date 2,
        ///         0 if same,
        ///        -1 if date 2 happened later than date 1.
        ///        
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
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
        // public static bool dateBetween(string testDate, string date1, string date2)
        //{
        // }

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

        /// <summary>
        /// Returns current year name
        /// </summary>
        /// <returns></returns>
        public string returnYear()
        {
            if (year >= 1601)
                return "";
            else
                return yearNames[year];
        }

        #region Static functions for format enforcement

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

            if (testMonth.Length > 2 || Int32.Parse(testMonth) > 12)
            {
                testMonth = "12";
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
            if (year != "" && month != "")
            {
                testDay = HarptosCalendar.verifyDay(month + testDay + year).ToString();
                if (testDay.Length == 1)
                    testDay = "0" + testDay;
            }
            else if (Int32.Parse(testDay) > 30)
            {
                testDay = "30";
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
        #endregion

        /// <summary>
        /// Reads in the names of 1601 years, from DR 0 to DR 1600
        /// </summary>
        private void readInYears()
        {
            yearNames = new string[1601];
            String yearFile = Properties.Resources.Roll_of_Years;
            for (int endIndex = 0, beginIndex = 3, yearNum = 0; endIndex < yearFile.Length; endIndex++)
            {

                if (yearFile[endIndex] == '\n')
                {
                    yearNames[yearNum] = yearFile.Substring(beginIndex, endIndex - 1 - beginIndex);
                    beginIndex = endIndex + 1;
                    for (; Char.IsLetter(yearFile[beginIndex]) == false; beginIndex++)
                    {
                    }
                    yearNum++;
                }
            }
            yearNames[1600] = "The Year of Unseen Enemies";
        }

        public void addMoonPhase()
        {
            moonCounter++;
            if (moonCounter == moonPhases.Length)
                moonCounter = 0;
        }
        public void subMoonPhase()
        {
            moonCounter--;
            if (moonCounter < 0)
                moonCounter = moonPhases.Length - 1;
        }

        /// <summary>
        /// Determines what phase selune is current in
        /// full moon every 30.4375, approximate with 30.5
        /// Full moon happens EVERY Leap year on the first day of that year.
        /// </summary>
        public void determineMoonCounter()
        {
            int lastLeapYear = year;
            int daysSinceFirstDay = 0;

            while (lastLeapYear % 4 != 0)
                lastLeapYear--;

            switch (year - lastLeapYear)
            {
                case 0:
                    daysSinceFirstDay = determineDayOfYear();
                    break;
                case 1:
                    daysSinceFirstDay = 366 + determineDayOfYear();
                    break;
                case 2:
                    daysSinceFirstDay = 366 + 365 + determineDayOfYear();
                    break;
                case 3:
                    daysSinceFirstDay = 366 + 365 + 365 + determineDayOfYear();
                    break;
            }

            moonCounter = daysSinceFirstDay % 61;
        }

        /// <summary>
        /// How many days have passed in the current year
        /// </summary>
        /// <returns></returns>
        public int determineDayOfYear()
        {
            return determineDayOfYear(month, day, year);
        }

        public static int determineDayOfYear(int m, int d, int y)
        {
            if (y % 4 == 0)
            {
                switch (m)
                {
                    case 1:
                        return d - 1;
                    case 2:
                        return 31 + d - 1;
                    case 3:
                        return 61 + d - 1;
                    case 4:
                        return 91 + d - 1;
                    case 5:
                        return 122 + d - 1;
                    case 6:
                        return 152 + d - 1;
                    case 7:
                        return 182 + d - 1;
                    case 8:
                        return 214 + d - 1;
                    case 9:
                        return 244 + d - 1;
                    case 10:
                        return 275 + d - 1;
                    case 11:
                        return 305 + d - 1;
                    case 12:
                        return 336 + d - 1;
                }
            }
            else
            {
                switch (m)
                {
                    case 1:
                        return d - 1;
                    case 2:
                        return 31 + d - 1;
                    case 3:
                        return 61 + d - 1;
                    case 4:
                        return 91 + d - 1;
                    case 5:
                        return 122 + d - 1;
                    case 6:
                        return 152 + d - 1;
                    case 7:
                        return 182 + d - 1;
                    case 8:
                        return 213 + d - 1;
                    case 9:
                        return 243 + d - 1;
                    case 10:
                        return 274 + d - 1;
                    case 11:
                        return 304 + d - 1;
                    case 12:
                        return 335 + d - 1;
                }
            }
            return -1;
        }

        public static int daysInMonth(int m, int y)
        {
            if (m == 1 || m == 4 || m == 7 || m == 9 || m == 11)
            {
                if (m == 7 && y % 4 == 0)
                    return 32;
                else
                    return 31;
            }
            else
                return 30;
        }

        /// <summary>
        /// Calculates the date after numDays days
        /// </summary>
        /// <param name="numDays">number of days</param>
        /// <returns></returns>
        public string dateIn(int numDays)
        {
            int d = day;
            int m = month;
            int y = year;

            if (daysInMonth(m, y) < d + numDays)
            {
                do
                {
                    numDays -= (daysInMonth(m, y) - d);
                    if (numDays > 0)
                    {
                        m++;
                        if (m > 12)
                        {
                            m = 1;
                            y++;
                        }
                        d = 0;
                    }
                } while (numDays >= daysInMonth(m, y));
                // if numdays is 0, means the date landed at the very end of the month, assign d to numDays unless it's 0
                d = numDays != 0 ? numDays : daysInMonth(m, y) - d;
            }
            else
            {
                d += numDays;
            }

            string monthString = HarptosCalendar.enforceMonthFormat(m.ToString());
            string yearString = HarptosCalendar.enforceYearFormat(y.ToString());
            string dayString = HarptosCalendar.enforceDayFormat(monthString, d.ToString(), yearString);
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
            // TODO: should produce same result even if begin is later than end date

            // This is pretty gross and i don't like to think about it, neither should you

            int numDays = 0; // Counter that's returned
            if (beginMonth != toMonth && toYear == beginYear)
            {
                while (toMonth != beginMonth)
                {
                    numDays += daysInMonth(beginMonth, beginYear) - beginDay;
                    beginDay = 0;
                    if (++beginMonth > 12)
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
                    if (beginYear % 4 == 0)
                        numDays += 366;
                    else
                        numDays += 365;
                    beginYear++;
                }
                while (toMonth != beginMonth || toYear != beginYear)
                {
                    numDays += daysInMonth(beginMonth, beginYear) - beginDay;
                    beginDay = 0;
                    if (++beginMonth > 12)
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

    }
}