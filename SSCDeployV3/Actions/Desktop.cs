/*
 * Classe Desktop
 * Contient les actions faisables sur le bureau telles que:
 * - Suppression de l'icone Edge sur le bureau * 
 */
using System;
using System.IO;
using System.Windows;

namespace SSCDeploy.Actions
{
    public class Desktop
    {
        // Définit le dossier du bureau
        private static DirectoryInfo desktop_dir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

        /// <summary>
        /// Méthode permettant la suppresion de l'icone Edge du bureau
        /// </summary>
        public static void Delete_Edge_Icon()
        {
            try
            {
                // Définit le nom du raccourci Edge
                FileInfo edge_lnk = new FileInfo(Path.Combine(desktop_dir.FullName, "Microsoft Edge.lnk"));

                // Si le raccourci existe => Suppression
                if (edge_lnk.Exists)
                {
                    edge_lnk.Delete();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("La suppression du raccourci Edge du bureau s'est mal déroulée: " + ex.Message);
            }
        }
    }
}
