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
    }
}
