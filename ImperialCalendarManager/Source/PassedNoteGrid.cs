using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WarhammerCalendarManager
{
    public partial class PassedNoteGrid : Form
    {
        List<Tuple<Note, string>> passedNotes;
        public delegate void GoToEventHandler(object sender, GoToEventArgs args);
        public event GoToEventHandler GoToDate;
        //public event EventHandler<GoToEventArgs> GoToDate;

        public PassedNoteGrid(List<Tuple<Note, string>> _passedNotes, string currDate)
        {
            InitializeComponent();
            passedNotes = _passedNotes;

            if (passedNotes.Count == 0)
                this.Close();

            string[] rowToAdd;
            string dateString;
            string tag;
            foreach (Tuple<Note, string> t in passedNotes)
            {
                if (t.Item1.Campaign == null)
                    tag = "GEN";
                else
                    tag = t.Item1.Campaign.Tag;
                dateString = t.Item2.Insert(2, "/").Insert(5, "/");
                rowToAdd = new string[] { dateString, t.Item1.DisplayString, "Go" };
                noteGrid.Rows.Add(rowToAdd);
            }

            currentDate.Text = currDate.Insert(2, "/").Insert(5, "/");
            goButton.Hide();
        }

        private void noteGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != noteGrid.Columns["gotoColumn"].Index)
                return;

            goButton.Show();
            string date = noteGrid[0, e.RowIndex].Value.ToString();
            date = date.Replace("/", "");
            GoToDate_clicked(date);

        }

        protected virtual void GoToDate_clicked(string dateToGoTo)
        {
            GoToDate(this, new GoToEventArgs() { date = dateToGoTo });
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            GoToDate_clicked(currentDate.Text.Replace("/", ""));
        }
    }

    public class GoToEventArgs : EventArgs
    {
        public string date { get; set; }
    }
}
