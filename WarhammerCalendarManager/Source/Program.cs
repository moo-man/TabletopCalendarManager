using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace WarhammerCalendarManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [DllImport("Shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (IsAssociated() == false)
                Associate();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            if (args.Length == 0)
            {
                Application.Run(new MainMenu());
            }
            else
            {
                string filename = "";
                foreach (string str in args)
                {
                    filename = filename + " " + str;
                }

                MessageBox.Show(filename);
                Application.Run(new MainMenu(filename));
            }
        }

        private static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            using (var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("WarhammerCalendarManager.Newtonsoft.Json.dll"))
            {
                byte[] assemblyData = new byte[stream.Length];
                stream.Read(assemblyData, 0, assemblyData.Length);
                return System.Reflection.Assembly.Load(assemblyData);
            }
        }

        public static bool IsAssociated()
        {
            return (Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\.whc", false) == null);
        }

        public static void Associate()
        {
            RegistryKey FileReg = Registry.CurrentUser.CreateSubKey("Software\\Classes\\.whc");
            RegistryKey AppReg = Registry.CurrentUser.CreateSubKey("Software\\Classes\\Applications\\WarhammerCalendarManager.exe");
            RegistryKey AppAssoc = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\.whc");

            FileReg.CreateSubKey("DefaultIcon").SetValue("", Properties.Resources.ResourceManager.GetObject("whicon2"));
            FileReg.CreateSubKey("PerceivedType").SetValue("", "Warhammer Calendar File");

            AppReg.CreateSubKey("shell\\open\\command").SetValue("", "\"" + Application.ExecutablePath + "\" %1");
            AppReg.CreateSubKey("DefaultIcon").SetValue("", Properties.Resources.ResourceManager.GetObject("whicon2"));

            //AppAssoc.CreateSubKey("UserChoice").SetValue("Progid", "Applications\\HarptosCalendarManager.exe");
            SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
        }

    }
    // creating branch
}
