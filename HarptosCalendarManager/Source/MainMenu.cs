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
using System.Threading;

public enum noteType { note, generalNote, timer, universal };


namespace HarptosCalendarManager
{
    public partial class MainMenu : Form
    {
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
            new HelpBox().ShowDialog(this);
        }

        private void NewCalendarButton_Click(object sender, EventArgs e)
        {
            LoadCalendarMenu(new CalendarContents());
        }

        public void LoadCalendarMenu(CalendarContents calendarToUse)
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
            CalendarContents loadedCalendar = Utility.Load();
            LoadCalendarMenu(loadedCalendar);
        }

        private void MainMenu_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void changelogPicture_Click(object sender, EventArgs e)
        {
            new ChangelogForm().Show();
   
        }

        private void changelogPicture_MouseUp(object sender, MouseEventArgs e)
        {
           
        }

        private void moveButton(PictureBox picture)
        {

        }

        private void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            moveButton(e.Argument as PictureBox);
        }
    }
}
