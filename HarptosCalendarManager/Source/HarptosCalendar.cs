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

        readonly static string[] holidayNames = { "Shieldmeet", "Midwinter", null, null, "Greengrass", null, null, "Midsummer", null, "Highharvestide", null, "Feast of the Moon"};
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

            if (day > 32)
            {
                day = 1;
                month++;
            }

            if (day == 31)
            {
                if (month != 1 && month != 4 && month != 7 && month != 9 && month != 11)
                {
                    day = 1;
                    month++;
                    if (month == 13)
                    {
                        month = 1;
                        year++;
                    }
                } // end switch(month)
            } // end if day == 31

            else if (day == 32)
            {
                if (!(year % 4 == 0 && month == 7))
                {
                    day = 1;
                    month++;
                }
            }

            else // if day is not 31 or 32
            {
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
                        break;
                    case 4:
                        day = 31;
                        break;
                    case 7:
                        if (year % 4 == 0)
                        {
                            day = 32;
                        }
                        else
                        {
                            day = 31;
                        }
                        break;
                    case 9:
                        day = 31;
                        break;
                    case 11:
                        day = 31;
                        break;
                    default:
                        day = 30;
                        break;
                } // end switch(month)
            } // end if day == 0

            else if (day == 31) // Very specific case: should only happen if leap year and subtract a day on shieldmeet
            {
            }

            else // if day is not 0
            {
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

        public void setDate(string dateString)
        {
            if (dateString.Length == 8)
            {
                setDate(Int32.Parse(dateString.Substring(0, 2)), Int32.Parse(dateString.Substring(2, 2)), Int32.Parse(dateString.Substring(4, 4)));
            }
        }

        /// <summary>
        /// Sets the date to the input values
        /// </summary>
        /// <param name="m">month</param>
        /// <param name="d">day</param>
        /// <param name="y">year</param>
        /// <returns></returns>
        public void setDate(int m, int d, int y)
        {
            switch (d)
            {
                case 31:
                    if (m == 1 || m == 4 || m == 7 || m == 9 || m == 11)
                    {
                        day = 31;
                        month = m;
                        year = y;
                    }
                    else
                    {
                        day = 30;
                        month = m;
                        year = y;
                    }
                    break;

                case 32:
                    if (m == 7 && ((y % 4) == 0))
                    {
                        month = m;
                        day = d;
                        year = y;
                    }
                    else if (month == 7)
                    {
                        day = 31;
                        month = m;
                        year = y;
                    }
                    else
                    {
                        day = 30;
                        month = m;
                        year = y;
                    }
                    break;

                default:
                    if (d > 32 || d <= 0)
                    {
                        d = 1;
                        m = 1;
                        y = 1491;
                    }
                    else
                    {
                        day = d;
                        month = m;
                        year = y;
                    }
                    break;
            }
            determineMoonCounter();
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

        /// <summary>
        /// Return current date in format(monthName) (dayNumber) (yearNumber) and holiday, if present
        /// </summary>
        /// <returns></returns>

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
            return yearsAgo(this.ToString(), inputDate);
        }

        public static int yearsAgo(string initialDate, string compareDate)
        {
            return Int32.Parse(initialDate.Substring(4, 4)) - Int32.Parse(compareDate.Substring(4, 4));
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

        public static string ToString(string dateString, string format, bool alt = false)
        {
            int m = Int32.Parse(dateString.Substring(0, 2));
            int d = Int32.Parse(dateString.Substring(2, 2));
            int y = Int32.Parse(dateString.Substring(4, 4));

            format = format.ToLower();

            if (format.Contains("ddd"))
            {
                format = format.Replace("ddd", ReturnDayFromFormat("ddd", d));
            }
            else if (format.Contains("dd"))
            {
                format = format.Replace("dd", ReturnDayFromFormat("dd", d));

            }
            else if (format.Contains("d"))
            {
                format = format.Replace("d", ReturnDayFromFormat("d", d));
            }


            if (format.Contains("mmm"))
            {
                if (IsHolidayAt(dateString) != -1)
                {
                    bool yearPresent = format.Contains("yyyy");

                    if (yearPresent)
                        format = HolidayAt(dateString) + ", " + y.ToString("0000");
                    else
                        format = HolidayAt(dateString);
                    //int mIndex = format.IndexOf("mmm");
                    //int dIndex = format.IndexOf("ddd");
                    //if (dIndex == -1)
                    //{
                    //    dIndex = format.IndexOf("dd");
                    //}
                    //if (dIndex == -1)
                    //{
                    //    dIndex = format.IndexOf("d");
                    //}
                    //if (dIndex != -1)
                    //{
                    //    int startIndex;
                    //    int endIndex;

                    //    if (mIndex > dIndex)
                    //    {
                    //        startIndex = dIndex;
                    //        endIndex = mIndex + 3;
                    //    }
                    //    else
                    //    {
                    //        startIndex = mIndex;
                    //        endIndex = dIndex + 3;
                    //    }

                    //    format.Replace(format.Substring(startIndex, endIndex - startIndex), 
                    //}
                    //else // day is not in the format
                    //{

                    //}
                }
                else
                {
                    format = format.Replace("mmm", ReturnMonthFromFormat("mmm", m));
                }
            }
            else if (format.Contains("mm"))
            {
                format = format.Replace("mm", ReturnMonthFromFormat("mm", m));
            }
            else if (format.Contains("m"))
            {
                format = format.Replace("m", ReturnMonthFromFormat("m", m));
            }


            if (format.Contains("yyyy"))
            {
                format = format.Replace("yyyy", ReturnYearFromFormat("yyyy", y));
            }
            else if (format.Contains("yyy"))
            {
                format = format.Replace("yyy", ReturnYearFromFormat("yyy", y));
            }
            else if (format.Contains("yy"))
            {
                format = format.Replace("yy", ReturnYearFromFormat("yy", y));
            }
            else if (format.Contains("y"))
            {
                format = format.Replace("y", ReturnYearFromFormat("y", y));
            }
            return format;
        }

        public string ToString(string format, bool alt = false)
        {
            return ToString(this.ToString(), format, alt);
        }

        /// <summary>
        /// Input format as...
        /// dddd -> "(day)st/nd/rd/th of "
        /// ddd  -> "(day)st/nd/rd/th"
        /// dd   -> "01" to "31"
        /// none ->  "1" to "31"
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        private static string ReturnDayFromFormat(string format, int dayValue)
        {
            string returnString;
            switch (format)
            {
                case "dddd":
                    if (dayValue == 1)
                        returnString = dayValue + "st of ";
                    else if (dayValue == 2)
                        returnString = dayValue + "nd of ";
                    else if (dayValue == 3)
                        returnString = dayValue + "rd of ";
                    else
                        returnString = dayValue + "th of ";
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

        private string ReturnDayFromFormat(string format)
        {
            return ReturnDayFromFormat(format, day);
        }

        /// <summary>
        /// Input format as...
        /// mmm -> "(month name)" or if intercalary holiday, return holiday name
        /// mm  -> "01" to "12"
        /// m   ->  "1" to "12" 
        /// none -> "1" to "12"
        /// </summary>
        /// <param name="format"></param>
        /// <param name="alt">if you want alternative month names</param>
        /// <returns></returns>
        private static string ReturnMonthFromFormat(string format, int monthValue, bool alt = false)
        {
            string returnMonth;
            switch (format)
            {
                case "mmm":
                    if (alt)
                        returnMonth = altMonthNames[monthValue];
                    else
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

        private string ReturnMonthFromFormat(string format, bool alt = false)
        {
            return ReturnMonthFromFormat(format, month, alt);
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

        /// <summary>
        /// Input format as...
        /// hh ->
        /// Two cases:
        /// Intercalary holiday -> "It is (holidayName)!"
        /// intracalary holiday -> ", the (holidayName)" (equinoxes/solstices)
        /// h -> just the holiday name
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        private string ReturnHolidayFromFormat(string format)
        {
            if (day == 31) // if is intercalary holiday
            {
                if (format == "hh")
                {
                    if (month != 12)
                        return "It is " + CurrentHolidayName() + "!";
                    else
                        return "It is the " + CurrentHolidayName() + "!";
                }
                else if (format == "h")
                    return CurrentHolidayName();
            }
            else if (day == 32)
            {
                if (format == "hh")
                    return "It is " + CurrentHolidayName() + "!";
                else if (format == "h")
                    return CurrentHolidayName();
            }

            switch (format)
            {
                case "hh":
                    return ", the " + CurrentHolidayName();
                case "h":
                    return CurrentHolidayName();
            }
            return null;
        }


        #region Holiday Determination

        public string CurrentHolidayName()
        {
            return HolidayAt(month, day, year);
        }


        public bool IsHoliday()
        {
            if (IsHolidayAt(month, day, year) != -1)
            {
                return true;
            }
            else
                return false;

        }

        public static string HolidayAt(string dateString)
        {
            string m = dateString.Substring(0, 2);
            string d = dateString.Substring(2, 2);
            string y = dateString.Substring(4, 4);

            enforceDateFormat(ref m, ref d, ref y);
            return HolidayAt(Int32.Parse(m), Int32.Parse(d), Int32.Parse(y));

        }

        public static string HolidayAt(int m, int d, int y)
        {
            int holidayStatus = IsHolidayAt(m, d, y);

            if (holidayStatus != -1)
            {
                return holidayNames[holidayStatus];
            }
            else
            {
                return null;
            }
        }


        public static int IsHolidayAt(string dateString)
        {
            string m = dateString.Substring(0, 2);
            string d = dateString.Substring(2, 2);
            string y = dateString.Substring(4, 4);

            enforceDateFormat(ref m, ref d, ref y);
            return IsHolidayAt(Int32.Parse(m), Int32.Parse(d), Int32.Parse(y));
        }


        public static int IsHolidayAt(int m, int d, int y)
        {
            if (d == 31)
            {
                if (m == 1 || m == 4 || m == 7 || m == 9 || m == 11)
                {
                    return m;
                }
            }

            else if (d == 32)
            {
                if (m == 7 && y % 4 == 0)
                    return 0;
            }

            return -1;
        }

        #endregion


        /// <summary>
        /// Returns current year name
        /// </summary>
        /// <returns></returns>
        public string ReturnYearName()
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