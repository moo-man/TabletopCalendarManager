using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HarptosCalendarManager
{

    public partial class TimeDifference : Form
    {
        string date1;
        string date2;
        public TimeDifference()
        {
            InitializeComponent();
            date1 = date2 = null;
        }

        public void GiveDate(string inputDate)
        {
            if (inputDate == null)
                return;

            if (date1 == null && date2 == null)
            {
                InputDate1(inputDate);

            }
            else if (date1 != null && date2 == null)
            {
                InputDate2(inputDate);
                Calculate();
            }
            else 
            {
                Clear();
                InputDate1(inputDate);

            }
        }

        private void InputDate1(string date)
        {
            date1 = date;
            date1Text.Text = AddSlash(date);
        }

        private void InputDate2(string date)
        {
            date2 = date;
            date2Text.Text = AddSlash(date);
        }

        public string AddSlash(string date)
        {
            return date.Insert(2, "/").Insert(5, "/");
        }

        private void Calculate()
        {
            if(HarptosCalendar.FarthestInTime(date1, date2) == 1)
                differenceLabel.Text = HarptosCalendar.daysBetween(date2, date1).ToString() + " days";
            else
                differenceLabel.Text = HarptosCalendar.daysBetween(date1, date2).ToString() + " days";
        }

        private void Clear()
        {
            differenceLabel.Text = "";
            date1Text.Text = "";
            date2Text.Text = "";
            date1 = null;
            date2 = null;
        }

        private void TimeDifference_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }


        private void TimeDifference_VisibleChanged(object sender, EventArgs e)
        {
            Clear();
        }
    }
}
