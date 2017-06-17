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
using System.Runtime.InteropServices;

namespace HarptosCalendarManager
{
    public partial class MainMenu : Form
    {
        HelpBox help;
        CalendarMenu currentCalendar;

        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResouceEx(IntPtr pbfont, uint cbfont, IntPtr pdv, [In] ref uint pcFonts);

        static FontFamily ff;
        public static PrivateFontCollection pfc;
        public static Font font;

        public MainMenu()
        {
            InitializeComponent();
            pfc = new PrivateFontCollection();
            initalizeFont(Properties.Resources.Minion_Pro);
            initalizeFont(Properties.Resources.Ozymandias_Solid_WBW);
            applyFont(titleText, 1);
            currentCalendar = null;
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
        }

        #region font stuff that is basically magic and DOES work
        private void initalizeFont(byte[] fontData )
        {
            //Select your font from the resources.
            int fontLength = fontData.Length;

            // create an unsafe memory block for the font data
            IntPtr data = Marshal.AllocCoTaskMem(fontLength);

            try
            {
                // copy the bytes to the unsafe memory block
                Marshal.Copy(fontData, 0, data, fontLength);

                // pass the font to the font collection
                pfc.AddMemoryFont(data, fontLength);
            }
            catch
            {

            }

            finally
            {
                // free up the unsafe memory
                Marshal.FreeCoTaskMem(data);
            }
        }


        public static void applyFont(Control c, int whichFont) // 0: Title (Ozy) 1: non-title (Minion pro)
        {
            applyFont(c, whichFont, FontStyle.Regular);
        }

        public static void applyFont(Control c, int whichFont, FontStyle style) // 0: Title (Ozy) 1: non-title (Minion pro)
        {
            c.Font = new Font(pfc.Families[whichFont], c.Font.Size, style);
        }
        #endregion

        #region font stuff that is basically magic and doesnt work
        private void loadFont()
        {

            byte[] fontArray = Properties.Resources.Ozymandias_Solid_WBW;
            int dataLength = Properties.Resources.Ozymandias_Solid_WBW.Length;

            IntPtr ptrData = Marshal.AllocCoTaskMem(dataLength);

            Marshal.Copy(fontArray, 0, ptrData, dataLength);

            uint cFonts = 0;
            PrivateFontCollection pfc = new PrivateFontCollection();
            
            try
            {
                AddFontMemResouceEx(ptrData, (uint)fontArray.Length, IntPtr.Zero, ref cFonts);
                pfc.AddMemoryFont(ptrData, dataLength);
            }
            catch (Exception e)
            {

            }
            finally
            {
                Marshal.FreeCoTaskMem(ptrData);
            }
            ff = pfc.Families[0];
            font = new Font(ff, 22, FontStyle.Regular);
        }

        public static void AllocFont(Font f, Control c, float size)
        {
            FontStyle fontStyle = FontStyle.Regular;
            c.Font = new Font(ff, size, fontStyle);
        }
        #endregion

        private void howToUseButton_Click(object sender, EventArgs e)
        {
            if (help != null)
                help.Close();
            help = new HelpBox();
            help.ShowDialog(this);
        }

        private void NewCalendarButton_Click(object sender, EventArgs e)
        {
            LoadCalendarMenu(new Calendar());
        }

        public void LoadCalendarMenu(Calendar calendarToUse)
        {
            currentCalendar = new CalendarMenu(this, calendarToUse);
            currentCalendar.Show(this);
            this.Hide();
        }

        private void LoadCalendarButton_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "Harptos Calendar files (*.hcal)|*.hcal|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                try
                {
                    bool oldFile = !openFileDialog1.FileName.Contains(".cal");
                    System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);
                    ReadInFile(sr, oldFile);
                    sr.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file. Original error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public void ReadInFile(System.IO.StreamReader sr)
        {
            Calendar loadedCalendar = new Calendar();
            int numOfGenNotes;
            if (Int32.TryParse(sr.ReadLine(), out numOfGenNotes))
            {
                for (int i = 0; i < numOfGenNotes; i++)
                    loadedCalendar.AddNote(ReadInNote(sr));

                int numOfCampaigns;
                if (Int32.TryParse(sr.ReadLine(), out numOfCampaigns))
                {
                    //StartLoadingBar(numOfCampaigns);
                    for (int i = 0; i < numOfCampaigns; i++)
                    {
                        Campaign loadedCampaign = new Campaign();

                        loadedCampaign.Name = sr.ReadLine();
                        loadedCampaign.Tag = sr.ReadLine();
                        loadedCampaign.CurrentDate = sr.ReadLine();

                        int numOfTimers = Int32.Parse(sr.ReadLine());
                        for (int j = 0; j < numOfTimers; j++)
                        {
                            string timerMessage = sr.ReadLine();
                            bool trackTimer = Convert.ToBoolean(sr.ReadLine());
                            string timerDate = sr.ReadLine();
                            loadedCampaign.addTimer(new Timer(timerDate, trackTimer, timerMessage));
                        }

                        int numOfNotes = Int32.Parse(sr.ReadLine());
                        for (int j = 0; j < numOfNotes; j++)
                            loadedCampaign.addNote(ReadInNote(sr));

                        loadedCalendar.AddCampaign(loadedCampaign);
                        //loadingBar.Increment(1);
                    }
                }
                LoadCalendarMenu(loadedCalendar);
            }
        }

        public void ReadInFile(System.IO.StreamReader sr, bool oldFile)
        {
            if (oldFile)
            {
                Calendar loadedCalendar = new Calendar();
                int numOfGenNotes;
                if (Int32.TryParse(sr.ReadLine(), out numOfGenNotes))
                {
                    for (int i = 0; i < numOfGenNotes; i++)
                        loadedCalendar.AddNote(ReadInNote(sr));

                    int numOfCampaigns;
                    if (Int32.TryParse(sr.ReadLine(), out numOfCampaigns))
                    {
                        //StartLoadingBar(numOfCampaigns);
                        for (int i = 0; i < numOfCampaigns; i++)
                        {
                            Campaign loadedCampaign = new Campaign();

                            loadedCampaign.Name = sr.ReadLine();
                            loadedCampaign.Tag = sr.ReadLine();
                            loadedCampaign.CurrentDate = sr.ReadLine();

                            int numOfTimers = Int32.Parse(sr.ReadLine());
                            for (int j = 0; j < numOfTimers; j++)
                            {
                                string timerMessage = sr.ReadLine();
                                bool trackTimer = Convert.ToBoolean(sr.ReadLine());
                                string timerDate = sr.ReadLine();
                                loadedCampaign.addTimer(new Timer(timerDate, trackTimer, timerMessage));
                            }

                            int numOfNotes = Int32.Parse(sr.ReadLine());
                            for (int j = 0; j < numOfNotes; j++)
                                loadedCampaign.addNote(ReadInNote(sr));

                            loadedCalendar.AddCampaign(loadedCampaign);
                            //loadingBar.Increment(1);
                        }
                    }
                    LoadCalendarMenu(loadedCalendar);
                }
            }
            else ReadInFile(sr);
        }

        public Note ReadInNote(System.IO.StreamReader sr)
        {
            string noteDate = sr.ReadLine();
            int numImportance = Int32.Parse(sr.ReadLine());
            string content = sr.ReadLine();
            AlertScope importance;
            switch (numImportance)
            {
                case 2:
                    importance = AlertScope.global;
                    break;
                case 1:
                    importance = AlertScope.campaign;
                    break;
                case 0:
                    importance = AlertScope.dontAlert;
                    break;
                default:
                    importance = AlertScope.dontAlert;
                    break;
            }

            return new Note(noteDate, importance, content);
        }

        private void MainMenu_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void changelogPicture_Click(object sender, EventArgs e)
        {
            new ChangelogForm().Show();
        }

        /*public void StartLoadingBar(int numItems)
        {
            loadingBar.Visible = false;
            loadingBar.Minimum = 1;
            loadingBar.Maximum = numItems;
            loadingBar.Value = 1;
            loadingBar.Step = 1;
        }*/
    }
}
