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

namespace CalendarManager
{
    public partial class MainMenu : Form
    {
        HelpBox help;
        CalendarMenu currentCalendar;
        CalendarType calendarType;

        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResouceEx(IntPtr pbfont, uint cbfont, IntPtr pdv, [In] ref uint pcFonts);

        static FontFamily ff;
        public static PrivateFontCollection pfc;
        public static Font font;


       /*// Console used for testing
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();*/

        public MainMenu()
        {
            InitializeComponent();
            pfc = new PrivateFontCollection();
            initalizeFont(Properties.Resources.Minion_Pro);
            initalizeFont(Properties.Resources.Ozymandias_Solid_WBW);
            applyFont(titleText, 1);
            currentCalendar = null;
            newCalendarButton.Enabled = false;

            /*// TESTING
            AllocConsole();
            CalendarType testCalendar = new CalendarType(@"{""year_len"":365,""events"":1,""n_months"":12,""months"":[""Abadius"",""Calistril"",""Pharast"",""Gozran"",""Desnus"",""Sarenith"",""Erastus"",""Arodus"",""Rova"",""Lamashan"",""Neth"",""Kuthona""],""month_len"":{""Abadius"":31,""Calistril"":30,""Pharast"":31,""Gozran"":30,""Desnus"":31,""Sarenith"":30,""Erastus"":31,""Arodus"":31,""Rova"":30,""Lamashan"":31,""Neth"":30,""Kuthona"":29},""week_len"":7,""weekdays"":[""Moonday"",""Toilday"",""Wealday"",""Oathday"",""Fireday"",""Starday"",""Sunday""],""n_moons"":1,""moons"":[""Somal""],""lunar_cyc"":{""Somal"":29.53},""lunar_shf"":{""Somal"":0},""year"":4707,""first_day"":0,""notes"":{}}", "test");
            Timer testTimer;
            Console.WriteLine("BEGIN TEST");
            for (int i = 1; i <= 10000; i++)
            {
                testTimer = new Timer(testCalendar.dateIn(i), true, "test");
                Console.WriteLine("i: " + i + "\tDays till: " + testCalendar.daysTo(testTimer.returnDateString()));
                if (i != testCalendar.daysTo(testTimer.returnDateString()))
                {
                    Console.Write("\t ERROR********************************************************\n");
                }
            }
            Console.WriteLine("TEST COMPLETE");*/
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
            LoadCalendarMenu(new Calendar(calendarType));
        }

        public void LoadCalendarMenu(Calendar calendarToUse)
        {
            if (calendarToUse == null)
            {
                return;
            }

            currentCalendar = new CalendarMenu(this, calendarToUse);
            currentCalendar.Show(this);
            this.Hide();
        }

        private void LoadCalendarButton_Click(object sender, EventArgs e)
        {
            Calendar loadedCalendar = Utility.Load();
            LoadCalendarMenu(loadedCalendar);
        }


        private void MainMenu_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void changelogPicture_Click(object sender, EventArgs e)
        {
            new ChangelogForm().Show();
        }

        private void importCalButton_Click(object sender, EventArgs e)
        {
            ImportCalendarDialog importCalendar = new ImportCalendarDialog();
            importCalendar.ShowDialog(this);
            calendarType = importCalendar.newCalendar;
            if (calendarType != null)
                newCalendarButton.Enabled = true;
        }

        private void helpLabel_Click(object sender, EventArgs e)
        {
            if (help != null)
                help.Close();
            help = new HelpBox();
            help.ShowDialog(this);
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