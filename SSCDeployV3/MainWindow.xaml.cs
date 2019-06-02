using SSCDeploy.Actions;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using System.Threading;
using System;
using System.Diagnostics;
using System.Reflection;

namespace SSCDeployV3
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private bool window_loaded;
        private bool ignore_radio_custom;
        private bool ignore_check;
        private string version;

        public MainWindow()
        {
            InitializeComponent();
            version = Assembly.GetExecutingAssembly().GetName().Version.ToString(2);

            version_title.Content = "Version " + version;
            version_paragraph.Inlines.Add("Version actuelle: " + version);
        }

        private async void Btn_deploy_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            deploy_progress_flyout.IsOpen = true;
            progress_ring.IsActive = true;

            var progress = new Progress<string>(update =>
            {
                if (count != 0)
                {
                    text_deploy_progress.AppendText(Environment.NewLine);
                }
                text_deploy_progress.AppendText(update);
                count++;
            });
            await Task.Run(() => Deploy(progress));

            progress_ring.IsActive = false;

            progress_ring.Visibility = Visibility.Collapsed;
            check_icon.Visibility = Visibility.Visible;

            deploy_status_textblock.Text = "DÉPLOIEMENT TERMINÉ";

            btn_close.Visibility = Visibility.Visible;
        }

        private void Deploy(IProgress<string> progress)
        {
            if(check_firefox.Dispatcher.Invoke(() => check_firefox.IsChecked == true))
            {
                progress?.Report("Mise en place de la config Firefox...");
                Thread.Sleep(2000);
                //Firefox.Profilize();
            }

            if (check_select_usb.Dispatcher.Invoke(() => check_select_usb.IsChecked == true))
            {
                progress?.Report("Désactivation suspension sélective USB...");
                Thread.Sleep(2000);
                //SelectiveUSB.Disable();
            }

            if(check_sleep.Dispatcher.Invoke(() => check_sleep.IsChecked == true))
            {
                progress?.Report("Désactivation mise en veille sous secteur...");
                Thread.Sleep(2000);
                //Sleep.Disable();
            }

            if (check_ipv6.Dispatcher.Invoke(() => check_ipv6.IsChecked == true))
            {
                progress?.Report("Désactivation de l'IPV6...");
                Thread.Sleep(2000);
                //IPV6.Disable();
            }

            if (check_unpin.Dispatcher.Invoke(() => check_unpin.IsChecked == true))
            {
                progress?.Report("Désépinglage des applications Microsoft...");
                Thread.Sleep(2000);
                //Docking.Unpin();
            }

            if (check_pin.Dispatcher.Invoke(() => check_pin.IsChecked == true))
            {
                progress?.Report("Épinglage des applications par défaut...");
                Thread.Sleep(2000);
                //Docking.Pin();
            }

            if (check_nic_sleep.Dispatcher.Invoke(() => check_nic_sleep.IsChecked == true))
            {
                progress?.Report("Désactivation mise en veille cartes réseau...");
                Thread.Sleep(2000);
                //Sleep.Disable();
            }

            if (check_usb_sleep.Dispatcher.Invoke(() => check_usb_sleep.IsChecked == true))
            {
                progress?.Report("Désactivation mise en veille USB...");
                Thread.Sleep(2000);
                //NICPowerSave.Disable();
            }

            if (check_onedrive.Dispatcher.Invoke(() => check_usb_sleep.IsChecked == true))
            {
                progress?.Report("Désinstallation de Onedrive...");
                Thread.Sleep(2000);
                //Onedrive.Uninstall();
            }

            if (check_region.Dispatcher.Invoke(() => check_usb_sleep.IsChecked == true))
            {
                progress?.Report("Application options régionales...");
                Thread.Sleep(2000);
                //Regional.Set_Thousands_Separator();
                //Regional.Set_Decimal_Separator();
            }

            if (check_edge_desk.Dispatcher.Invoke(() => check_usb_sleep.IsChecked == true))
            {
                progress?.Report("Supression îcone Edge du bureau...");
                Thread.Sleep(2000);
                //Desktop.Delete_Edge_Icon();
            }

            if (check_adobe.Dispatcher.Invoke(() => check_usb_sleep.IsChecked == true))
            {
                progress?.Report("Adobe par défaut S.V.P...");
                Thread.Sleep(2000);
                //FileProps.OpenPDFDetails();
            }

        }

        private void Radiobuttons_checked_changed(object sender, RoutedEventArgs e)
        {
            // Uniquement si l'application a déjà charger
            if (window_loaded)
            {
                ignore_check = true;
                RadioButton clicked_radio = (RadioButton)sender;

                foreach (RadioButton radiobutton in presets_group_grid.Children.OfType<RadioButton>())
                {
                    if (radiobutton == clicked_radio)
                    {
                        radiobutton.IsEnabled = false;
                    }
                    else
                    {
                        radiobutton.IsEnabled = true;
                    }
                }

                if (radio_new.IsChecked == true)
                {
                    foreach (ToggleSwitch action_checkbox in actions_groupbox_grid.Children.OfType<ToggleSwitch>())
                    {
                        action_checkbox.IsChecked = true;
                    }
                }
                else if (radio_update.IsChecked == true)
                {
                    foreach (ToggleSwitch action_checkbox in actions_groupbox_grid.Children.OfType<ToggleSwitch>())
                    {
                        action_checkbox.IsChecked = true;
                    }

                    check_firefox.IsChecked = false;
                    check_unpin.IsChecked = false;
                    check_pin.IsChecked = false;
                }
                else if (radio_custom.IsChecked == true && !ignore_radio_custom)
                {
                    foreach (ToggleSwitch action_checkbox in actions_groupbox_grid.Children.OfType<ToggleSwitch>())
                    {
                        action_checkbox.IsChecked = false;
                    }
                }

                ignore_check = false;
            }
        }

        private void Checkbox_actions_checked_changed(object sender, RoutedEventArgs e)
        {
            // Uniquement lorsque l'application est chargée
            if (window_loaded && !ignore_check)
            {
                if (actions_groupbox_grid.Children.OfType<ToggleSwitch>().Count() == actions_groupbox_grid.Children.OfType<ToggleSwitch>().Count(c => c.IsChecked == true))
                {
                    radio_new.IsChecked = true;
                }
                else
                {
                    ignore_radio_custom = true;
                    radio_custom.IsChecked = true;
                    ignore_radio_custom = false;
                }
            }
        }

        private void Main_form_ContentRendered(object sender, System.EventArgs e)
        {
            window_loaded = true;
        }

        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {
            text_deploy_progress.Document.Blocks.Clear();

            check_icon.Visibility = Visibility.Collapsed;
            progress_ring.Visibility = Visibility.Visible;

            deploy_status_textblock.Text = "EN COURS DE DÉPLOIEMENT";

            btn_close.Visibility = Visibility.Collapsed;
            deploy_progress_flyout.IsOpen = false;
        }

        private void Btn_ie_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("iexplore");
        }

        private void Btn_privacy_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("ms-settings:privacy-general");
        }

        private void Btn_default_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("ms-settings:defaultapps");
        }
    }
}
