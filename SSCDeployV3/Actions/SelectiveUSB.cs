/* Classe SelectiveUSB
 * Permet l'activation ou désactivation de la mise en veille séléctive USB
 */
using System.Windows;

namespace SSCDeploy.Actions
{
    public static class SelectiveUSB
    {
        private const string SCHEMA         = "SCHEME_CURRENT 2a737441-1930-4402-8d77-b2bebba308a3 48e6b7a6-50f5-4782-a5d4-53bb8f07e226 ";
        private const string DISABLE        = "0";
        private const string ENABLE         = "1";
        private const string COMMAND_DC     = "/SETDCVALUEINDEX ";
        private const string COMMAND_AC     = "/SETACVALUEINDEX ";
        private const string DC_DISABLE     = COMMAND_DC + SCHEMA + DISABLE;
        private const string AC_DISABLE     = COMMAND_AC + SCHEMA + DISABLE;
        private const string DC_ENABLE      = COMMAND_DC + SCHEMA + ENABLE;
        private const string AC_ENABLE      = COMMAND_AC + SCHEMA + ENABLE;


        public static void Disable()
        {
            try
            {
                //DC = Battery
                PowerCFG.Power_CFG(DC_DISABLE);

                //AC = Plugged
                PowerCFG.Power_CFG(AC_DISABLE);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("La désactivation de la mise en veille USB séléctive s'est mal déroulée: " + ex.Message);
            }
        }

        public static void Enable()
        {
            //DC = Battery
            PowerCFG.Power_CFG(DC_ENABLE);

            //AC = Plugged
            PowerCFG.Power_CFG(AC_ENABLE);
        }
    }
}
