
using System;
using System.IO;
using System.Windows;

namespace SSCDeploy.Actions
{
    public class Desktop
    {
        private static DirectoryInfo desktop_dir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

        public static void Delete_Edge_Icon()
        {
            try
            {
                FileInfo edge_lnk = new FileInfo(Path.Combine(desktop_dir.FullName, "Microsoft Edge.lnk"));

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
