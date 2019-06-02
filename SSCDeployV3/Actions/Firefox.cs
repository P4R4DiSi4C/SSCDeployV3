using System;
using System.IO;
using System.Windows;
using System.Diagnostics;

namespace SSCDeploy.Actions
{
    public class Firefox
    {
        private static string PROGRAM_FILES = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
        private static DirectoryInfo mozillaDir = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Mozilla", "Firefox"));
        private static string JS_PATH = PROGRAM_FILES + @"\Mozilla Firefox\defaults\pref\all-epflssc.js";
        private static string CFG_PATH = PROGRAM_FILES + @"\Mozilla Firefox\firefoxssc.cfg";
        private static string DISTRIB_PATH = PROGRAM_FILES + @"\Mozilla Firefox\distribution";
        private static string POLICIES_PATH = PROGRAM_FILES + @"\Mozilla Firefox\distribution\policies.json";


        private static void CopyConfigFiles()
        {
            Directory.CreateDirectory(DISTRIB_PATH);
            File.Copy("Files\\policies.json", POLICIES_PATH, true);
            File.Copy("Files\\all-epflssc.js", JS_PATH, true);
            File.Copy("Files\\firefoxssc.cfg", CFG_PATH, true);
        }

        private static void CopyHandlers()
        {
            DirectoryInfo profilesDir = new DirectoryInfo(Path.Combine(mozillaDir.FullName, "Profiles"));

            foreach (var dir in profilesDir.EnumerateDirectories("*release"))
            {
                string ext_path = dir.FullName + "\\extensions";

                File.Copy("Files\\handlers.json", Path.Combine(dir.FullName, "handlers.json"), true);

                if (!Directory.Exists(ext_path))
                {
                    Directory.CreateDirectory(ext_path);
                }

                foreach (var file in Directory.GetFiles("Files", "*.xpi", SearchOption.AllDirectories))
                {
                    File.Copy(file, Path.Combine(ext_path, Path.GetFileName(file)), true);
                }
            }
        }

        private static void KillFirefox()
        {
            bool iskilled = false;

            foreach (var process in Process.GetProcessesByName("firefox"))
            {
                process.Kill();
                iskilled = true;
            }

            if(iskilled)
                System.Threading.Thread.Sleep(5000);
        }

        private static void OpenFirefox()
        {
            using (Process firefox_p = new Process())
            {
                firefox_p.StartInfo.FileName = "firefox.exe";
                firefox_p.Start();
                System.Threading.Thread.Sleep(10000);
                try
                {
                    firefox_p.CloseMainWindow();
                }
                catch(Exception)
                {

                }
            };
        }

        private static void ClearProfiles()
        {
            if (mozillaDir.Exists)
            {
                foreach (FileInfo file in mozillaDir.GetFiles())
                {
                    file.Delete();
                }

                foreach (DirectoryInfo dir in mozillaDir.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
        }

        private static void CheckArch()
        {
            string arch64_firefox_path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Mozilla Firefox\";

            if (Directory.Exists(arch64_firefox_path) && Environment.Is64BitOperatingSystem)
            {
                if (Directory.GetFiles(arch64_firefox_path).Length > 0)
                {
                    PROGRAM_FILES = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                }
            }
        }
        
        public static void Profilize()
        {
            try
            {
                CheckArch();
                KillFirefox();
                ClearProfiles();
                CopyConfigFiles();
                OpenFirefox();
                CopyHandlers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'application du profil Firefox: " + ex.Message);
            }
        }
    }
}
