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
    public partial class PassedNoteGrid : Form
    {
        List<Note> passedNotes;
        public delegate void GoToEventHandler(object sender, GoToEventArgs args);
        public event GoToEventHandler GoToDate;
        //public event EventHandler<GoToEventArgs> GoToDate;

        public PassedNoteGrid(List<Note> _passedNotes)
        {
            InitializeComponent();
            passedNotes = _passedNotes;

            if (passedNotes.Count == 0)
                this.Close();

            string[] rowToAdd;
            string dateString;
            foreach (Note n in passedNotes)
            {
                dateString = n.Date.Insert(2, "/").Insert(5, "/");
                rowToAdd = new string[] { n.Campaign.Tag, dateString, n.NoteContent, "Go" };
                noteGrid.Rows.Add(rowToAdd);
            }
        }

        private void noteGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != noteGrid.Columns["gotoColumn"].Index)
                return;
            string date = noteGrid[1, e.RowIndex].Value.ToString();
            date = date.Replace("/", "");

            GoToDate_clicked(date);

        }

        protected virtual void GoToDate_clicked(string dateToGoTo)
        {
            GoToDate(this, new GoToEventArgs() { date = dateToGoTo });
        }
    }
}
