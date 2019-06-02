using Microsoft.Win32;
using System;
using System.Windows;

namespace SSCDeploy.Actions
{
    public class Regional
    {

        public static void Set_Thousands_Separator()
        {
            try
            {
                using (var regional_key = Registry.CurrentUser.OpenSubKey(@"Control Panel\International", true))
                {
                    regional_key.SetValue("sThousand", "'");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("L'application des paramètres régionaux pour les milliers s'est mal déroulé: " + ex.Message);
            }
        }

        public static void Set_Decimal_Separator()
        {
            try
            {
                using (var regional_key = Registry.CurrentUser.OpenSubKey(@"Control Panel\International", true))
                {
                    regional_key.SetValue("sDecimal", ".");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("L'application des paramètres régionaux pour les décimales s'est mal déroulé: " + ex.Message);
            }
        }
    }
}
