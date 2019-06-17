/* Classe NICPowerSave
 * Gère la mise en veille des cartes réseau
 */
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Windows;

namespace SSCDeploy.Actions
{
    public class NICPowerSave
    {
        public static void Disable()
        {
            try
            {
                using (PowerShell ps = PowerShell.Create())
                {
                    ps.AddCommand("Set-ExecutionPolicy")
                      .AddParameter("ExecutionPolicy", "Bypass")
                      .AddParameter("Scope", "Process")
                      .AddParameter("Force");

                    ps.AddScript(@"Files\NICPowerSave.ps1");

                    Collection<PSObject> result = ps.Invoke();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("La désactivation de la mise en veille des cartes réseau s'est mal déroulée: " + ex.Message);
            }
        }
    }
}
