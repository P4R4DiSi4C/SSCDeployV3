/* Classe Firefox
 * Gère toute l'autoconfiguration de Firefox et du profil par défaut * 
 */
using System;
using System.IO;
using System.Windows;
using System.Diagnostics;

namespace SSCDeploy.Actions
{
    public class Firefox
    {
        // Dossier "Progam Files"
        private static string PROGRAM_FILES = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);

        // Dossier Firefox dans AppData
        private static DirectoryInfo mozillaDir = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Mozilla", "Firefox"));

        // Emplacement des fichiers de config
        private static string JS_PATH = PROGRAM_FILES + @"\Mozilla Firefox\defaults\pref\all-epflssc.js";
        private static string CFG_PATH = PROGRAM_FILES + @"\Mozilla Firefox\firefoxssc.cfg";
        private static string DISTRIB_PATH = PROGRAM_FILES + @"\Mozilla Firefox\distribution";
        private static string POLICIES_PATH = PROGRAM_FILES + @"\Mozilla Firefox\distribution\policies.json";

        /// <summary>
        /// Copie des fichiers de config
        /// </summary>
        private static void CopyConfigFiles()
        {
            // Crée le dossier distribution
            Directory.CreateDirectory(DISTRIB_PATH);
   
            // Copie les fichiers de config
            File.Copy("Files\\policies.json", POLICIES_PATH, true);
            File.Copy("Files\\all-epflssc.js", JS_PATH, true);
            File.Copy("Files\\firefoxssc.cfg", CFG_PATH, true);
        }

        /// <summary>
        /// Copie du fichier handlers.json et les extensions(langues) dans le profil utilisateur => Permet d'utiliser le lecteur .pdf du système par défaut
        /// </summary>
        private static void CopyHandlers()
        {
            // Dossier où se trouve les profiles firefox
            DirectoryInfo profilesDir = new DirectoryInfo(Path.Combine(mozillaDir.FullName, "Profiles"));

            // Parcourt le profile par défaut
            foreach (var dir in profilesDir.EnumerateDirectories("*release"))
            {
                // Définit le dossier des extensions
                string ext_path = dir.FullName + "\\extensions";

                // Copy le fichers handlers
                File.Copy("Files\\handlers.json", Path.Combine(dir.FullName, "handlers.json"), true);

                // Crée le dossier des extensions s'il n'existe pas
                if (!Directory.Exists(ext_path))
                {
                    Directory.CreateDirectory(ext_path);
                }

                // Copie les extensions(langues)
                foreach (var file in Directory.GetFiles("Files", "*.xpi", SearchOption.AllDirectories))
                {
                    File.Copy(file, Path.Combine(ext_path, Path.GetFileName(file)), true);
                }
            }
        }

        /// <summary>
        /// Méthode permettant de tuer le processus Firefox et de patienter
        /// </summary>
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

        /// <summary>
        /// Ouverture + Fermeture de firefox(fonctionne rarement) permettant de générer le profil
        /// </summary>
        private static void OpenFirefox()
        {
            using (Process firefox_p = new Process())
            {
                firefox_p.StartInfo.FileName = "firefox.exe";
                firefox_p.Start();
                System.Threading.Thread.Sleep(8000);
                try
                {
                    firefox_p.CloseMainWindow();
                }
                catch(Exception)
                {

                }
            };
        }

        /// <summary>
        /// Nettoyage des profiles "déjà" existants
        /// </summary>
        private static void ClearProfiles()
        {
            // Si le dossier Firefox dans appdata est déjà présent, on le nettoie
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

        /// <summary>
        /// Vérification de l'architecture du poste pour définir le dossier où est installé Firefox
        /// </summary>
        private static void CheckArch()
        {
            // Dossier d'installation Firefox si 64bits
            string arch64_firefox_path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Mozilla Firefox\";

            // Si existe et 64bits on définit la variable sur ce dossier
            if (Directory.Exists(arch64_firefox_path) && Environment.Is64BitOperatingSystem)
            {
                if (Directory.GetFiles(arch64_firefox_path).Length > 0)
                {
                    PROGRAM_FILES = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                }
            }
        }
        
        /// <summary>
        /// Méthode principale executant les actions nécessaires
        /// </summary>
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
