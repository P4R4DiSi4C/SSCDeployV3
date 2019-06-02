using System.Windows;

namespace SSCDeploy.Actions
{
    public static class Sleep
    {
        private const string ENABLE         = "30";
        private const string DISABLE        = "0";
        private const string COMMAND_DC     = "/change standby-timeout-dc ";
        private const string COMMAND_AC     = "/change standby-timeout-ac ";
        private const string DC_DISABLE     = COMMAND_DC + DISABLE;
        private const string AC_DISABLE     = COMMAND_AC + DISABLE;
        private const string DC_ENABLE      = COMMAND_DC + ENABLE;
        private const string AC_ENABLE      = COMMAND_AC + ENABLE;

        public static void Disable()
        {
            try
            {
                //DC = Battery
                //PowerCFG.Power_CFG(DC_DISABLE);

                //AC = Plugged
                PowerCFG.Power_CFG(AC_DISABLE);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("La désactivation de la mise en veille sous secteur s'est mal déroulée: " + ex.Message);
            }
        }

        public static void Enable()
        {
            //DC = Battery
            //PowerCFG.Power_CFG(DC_ENABLE);

            //AC = Plugged
            PowerCFG.Power_CFG(AC_ENABLE);
        }
    }
}
