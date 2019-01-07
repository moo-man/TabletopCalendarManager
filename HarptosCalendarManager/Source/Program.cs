using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace HarptosCalendarManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [DllImport("Shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);


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

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("HarptosCalendarManager.Newtonsoft.Json.dll"))
            {
                byte[] assemblyData = new byte[stream.Length];
                stream.Read(assemblyData, 0, assemblyData.Length);
                return Assembly.Load(assemblyData);
            }
        }
        public static bool IsAssociated()
        {
            return (Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\.hcal", false) == null);
        }

        public static void Associate()
        {
            RegistryKey FileReg = Registry.CurrentUser.CreateSubKey("Software\\Classes\\.hcal");
            RegistryKey AppReg = Registry.CurrentUser.CreateSubKey("Software\\Classes\\Applications\\HarptosCalendarManager.exe");
            RegistryKey AppAssoc = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\.hcal");

            FileReg.CreateSubKey("DefaultIcon").SetValue("", Properties.Resources.ResourceManager.GetObject("HarptosIcon"));
            FileReg.CreateSubKey("PerceivedType").SetValue("", "Harptos Calendar Data");

            AppReg.CreateSubKey("shell\\open\\command").SetValue("", "\"" + Application.ExecutablePath + "\" %1");
            AppReg.CreateSubKey("DefaultIcon").SetValue("", Properties.Resources.ResourceManager.GetObject("HarptosIcon"));

            //AppAssoc.CreateSubKey("UserChoice").SetValue("Progid", "Applications\\HarptosCalendarManager.exe");
            SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
        }
    }
}
