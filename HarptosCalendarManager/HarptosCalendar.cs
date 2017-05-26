using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HarptosCalendarManager
{
    public class HarptosCalendar
    {
        int day;
        int month;
        int year;
        bool [] holidays; // Array of bools for each holiday, if it is a holiday, find the true value for which one
        public bool isHoliday { get; set; } //Is current day a holiday?       // 0 1 3 5 7: locations of holidays that do not occur on days of a month 
        readonly static string [] holidayNames = {"Shieldmeet", "Midwinter", "Spring Equinox", "Greengrass", "Summer Solstice", "Midsummer", "Autumn Equinox", "Highharvestide", "Feast of the Moon", "Winter Solstice" };
        readonly static string [] monthNames = {null, "Hammer", "Alturiak", "Ches", "Tarsakh", "Mirtul", "Kythorn", "Flamerule", "Eleasis", "Eleint", "Merpenoth", "Uktar", "Nightal" };
        readonly static string[] altMonthNames = { null, "Deepwinter", "The Claw of Winter", "The Claw of Sunsets", "The Claw of Storms", "The Melting", "The Time of Flowers", "Summertide", "Highsun", "The Fading", "Leaffall", "The Rotting", "The Drawing Down" };
        string [] yearNames;

        public HarptosCalendar() : this (1, 1, 1491)
        {

        }

        public HarptosCalendar(int y) : this (1, 1, y)
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
        
        public void addDay()
        {

            day++;
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
        }

        public void addYear(int num)
        {
            for(int i = 0; i < num; i++)
                addYear();
        }

        public void subDay()
        {
            day--;
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

        }

        public bool setDate(string dateString)
        {
            if (dateString.Length == 8)
            {
                return setDate(Int32.Parse(dateString.Substring(0, 2)), Int32.Parse(dateString.Substring(2, 2)), Int32.Parse(dateString.Substring(4,4)));
            }

            else
                return false;
        }

        public bool setDate(int m, int d, int y)
        {
            clearHolidays();
            switch(d)
            {
                case 31:
                    if (m == 1 || m == 4 || m == 7 || m == 9 || m == 11)
                    {
                        isHoliday = true;
                        switch(m)
                        {
                            case 1:
                                holidays[1] = true;
                                break;
                            case 4:
                                holidays[3] = true;
                                break;
                            case 7:
                                holidays[5]= true;
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

                        if ( day == 19 && month == 3)
                        {
                            isHoliday = true;
                            holidays[2] = true;
                        }
                        else if ( day == 20 && month == 6)
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
            return true; // if the date was false, the switch statement would have returned false
        }                // so if it gets to this point, it's true

        public static string returnGivenDate(string dateString)
        {
            if (dateString.Length == 8)
            {
                return returnGivenDate(Int32.Parse(dateString.Substring(0, 2)), Int32.Parse(dateString.Substring(2, 2)), Int32.Parse(dateString.Substring(4, 4)));
            }

            else
                return null;
        }

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
                else
                    switch (day)
                    {
                        case 32:
                            if (holidays[0])
                                returnString = "It is " + holidayNames[0] + "!";
                            break;
                        case 1:
                            returnString = day + "st of " + monthNames[month] + " " + year + ", the " + holidayNames[i];
                            break;
                        case 2:
                            returnString = day + "nd of " + monthNames[month] + " " + year + ", the " + holidayNames[i];
                            break;
                        case 3:
                            returnString = day + "rd of " + monthNames[month] + " " + year + ", the " + holidayNames[i];
                            break;
                        default:
                            returnString = day + "th of " + monthNames[month] + " " + year + ", the " + holidayNames[i];
                            break;
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
                else
                    switch (day)
                    {
                        case 32:
                            if (holidays[0])
                                returnString = "It is " + holidayNames[0] + "!";
                            break;
                        case 1:
                            returnString = day + "st of " + altMonthNames[month] + " " + year + ", the " + holidayNames[i];
                            break;
                        case 2:
                            returnString = day + "nd of " + altMonthNames[month] + " " + year + ", the " + holidayNames[i];
                            break;
                        case 3:
                            returnString = day + "rd of " + altMonthNames[month] + " " + year + ", the " + holidayNames[i];
                            break;
                        default:
                            returnString = day + "th of " + altMonthNames[month] + " " + year + ", the " + holidayNames[i];
                            break;
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

        public void clearHolidays()
        {
            isHoliday = false;
            for (int i = 0; i < 10; i++)
                holidays[i] = false;
        }

        public string returnConciseDate()
        {
            return returnGivenDate(month, day, year);
        }

        public string returnJustHoliday()
        {
            if (isHoliday)
            {
                int holidayIndex;
                for (holidayIndex = 0; holidayIndex < holidays.Length && holidays[holidayIndex] == false; holidayIndex++)
                {
                }
                return holidayNames[holidayIndex];
            }
            return null;
        }

        public bool sameDate(string testDate)
        {
            if (testDate.Length != 8)
                return false;
            else
                return sameDate(Int32.Parse(testDate.Substring(0, 2)), Int32.Parse(testDate.Substring(2,2)), Int32.Parse(testDate.Substring(4, 4)));
        }

        public bool sameDate(int testM, int testD, int testY)
        {
            if (testM == month && testD == day && testY == year)
                return true;
            else 
                return false;
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

        public int yearsAgo(int y)
        {
            return year - y;
        }

        public int yearsAgo(string currentDate)
        {
            return yearsAgo(Int32.Parse(currentDate.Substring(4, 4)));
        }

        public override string ToString()
        {
            StringBuilder stringDate = new StringBuilder();
            if (month < 10)
                stringDate.Append("0" + month);
            else
                stringDate.Append(month);

            if (day < 10)
                stringDate.Append("0" + day);
            else
                stringDate.Append(day);

            string yString = year.ToString();
            while (yString.Length < 4)
                yString =  yString.Insert(0, "0");

            stringDate.Append(yString);

            return stringDate.ToString();
        }

        public string returnYear()
        {
            return yearNames[year];
        }

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

            /*
            try
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader(@"C:\Users\Moo Man\OneDrive\HarptosCalendarManager\HarptosCalendarManager\Roll of Years.txt"))
                {
                    string line;
                    int yearIndex = 0;
                    for (int i = 0; i < yearNames.Length && (line = file.ReadLine() )!= null; i++)
                    {
                        for (int j = 0; j < line.Length; j++)
                            if(Char.IsLetter(line.ElementAt(j)))
                            {
                                yearIndex = Convert.ToInt32(line.Substring(0, j-2));
                                line = line.Substring(j);
                                break;
                            }
                        yearNames[yearIndex] = line;
                    }
                }
            }

            catch (Exception e)
            {
                
            }*/
        }
    }
}
