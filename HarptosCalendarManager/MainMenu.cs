using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Text;

namespace HarptosCalendarManager
{
    public partial class MainMenu : Form
    {
        HelpBox help;
        CalendarMenu currentCalendar;
        PrivateFontCollection fonts;

        public MainMenu()
        {
            //fonts = new PrivateFontCollection();
           // fonts.AddFontFile(@"C:\Users\Moo Man\OneDrive\HarptosCalendarManager\HarptosCalendarManager\Resources\Ozymandias Solid WBW.ttf");
            InitializeComponent();
            currentCalendar = null;
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {

        }

        private void howToUseButton_Click(object sender, EventArgs e)
        {
            if (help != null)
                help.Close();
            help = new HelpBox();
            help.ShowDialog(this);
        }

        private void newCalendarButton_Click(object sender, EventArgs e)
        {
            loadCalendarMenu(new Calendar());
        }

        public void loadCalendarMenu(Calendar calendarToUse)
        {
            currentCalendar = new CalendarMenu(this, calendarToUse);
            currentCalendar.Show(this);
            this.Hide();
        }

        private void loadCalendarButton_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog();
            openFileDialog1.ShowDialog();
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            try
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);
                readInFile(sr);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not read file. Original error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void readInFile(System.IO.StreamReader sr)
        {
            Calendar loadedCalendar = new Calendar();
            
            int numOfCampaigns = Int32.Parse(sr.ReadLine());
            for (int i = 0; i < numOfCampaigns; i++)
            {
                Campaign loadedCampaign = new Campaign();

                loadedCampaign.Name = sr.ReadLine();
                loadedCampaign.Tag = sr.ReadLine();
                loadedCampaign.CurrentDate= sr.ReadLine();

                int numOfNotes = Int32.Parse(sr.ReadLine());
                for (int j = 0; j < numOfNotes; j++)
                {
                    string noteDate = sr.ReadLine();
                    int numImportance = Int32.Parse(sr.ReadLine());
                    int general = Int32.Parse(sr.ReadLine());
                    string content = sr.ReadLine();
                    alertLevel importance;
                    switch (numImportance)
                    {
                        case 2:
                            importance = alertLevel.alertAll;
                            break;
                        case 1:
                            importance = alertLevel.alertCampaign;
                            break;
                        case 0:
                            importance = alertLevel.dontAlert;
                            break;
                        default:
                            importance = alertLevel.dontAlert;
                            break;
                    }

                    loadedCampaign.addNote(noteDate, importance, content);

                }

                loadedCalendar.addCampaign(loadedCampaign);
            }
            loadCalendarMenu(loadedCalendar);
        }

        private void MainMenu_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}
