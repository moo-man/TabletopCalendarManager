using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace CalendarManager
{
    public partial class ImportCalendarDialog : Form
    {
        public CalendarType newCalendar;
        public ImportCalendarDialog()
        {
            InitializeComponent();
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_Click(object sender, EventArgs e)
        {
            if (textBox.Text == "Paste calendar JSON data here.")
            {
                textBox.Text = "";
            }

        }

        private void createCalendarButton_Click(object sender, EventArgs e)
        {
            try
            {
                newCalendar = new CalendarType(textBox.Text, calendarNameBox.Text);
                MessageBox.Show(this, "Calendar imported successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error parsing: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            if (textBox.Text == "")
            {
                textBox.Text = "Paste calendar JSON data here.";
            }
        }

        private void loadJSONbutton_Click(object sender, EventArgs e)
        {
            String inputString;
            Stream input = null;
            StreamReader sr;

            OpenFileDialog jsonFile = new OpenFileDialog();
            jsonFile.InitialDirectory = "c:\\";
            jsonFile.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            jsonFile.FilterIndex = 1;
            jsonFile.RestoreDirectory = true;

            try
            {
                if (jsonFile.ShowDialog() == DialogResult.OK)
                {
                    if ((input = jsonFile.OpenFile()) != null)
                    {
                        using (input)
                        {
                            sr = new StreamReader(input);
                            inputString = sr.ReadToEnd();
                            textBox.Clear();
                            textBox.Text = inputString;
                            sr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
        }

        private void ImportCalendarDialog_Load(object sender, EventArgs e)
        {

        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox.Text = Clipboard.GetText();
        }

        private void helpLabel_Click(object sender, EventArgs e)
        {
           if ( MessageBox.Show(this, "Here is where you can import your custom calendar.\n\n" +
                "To create a compatible calendar, go to https://donjon.bin.sh and navigate to the Fantasy Calendar Generator. Fill out the calendar's information, then under the 'Save/Restore' tab, save or copy the calendar's JSON data, and paste it in the textbox below.\n\n"+
                "Once successfully imported, go back to the Main Menu and click 'New Calendar' to begin creating campaigns. Note that importing only needs to done once per 'New Calendar', as the save file stores the calendar data. This also means loading a file will also load its custom calendar data.\n\n" +
                "Open the calendar creator now? (Internet connection required)",
    "Importing a Calendar",
    MessageBoxButtons.YesNo,
    MessageBoxIcon.Information) == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("https://donjon.bin.sh/fantasy/calendar/");
            }
        }
    }
}
