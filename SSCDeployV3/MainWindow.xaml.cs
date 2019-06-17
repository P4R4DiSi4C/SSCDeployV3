using SSCDeploy.Actions;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
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

        /// <summary>
        /// Initialisation de la fenêtre principale
        /// </summary>
        public MainWindow()
        {
            // Initialisation de la vue
            InitializeComponent();

            // Obtient la version actuel de l'application
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString(2);

            // Affiche la version sur l'application
            version_title.Content = "Version " + version;
            version_paragraph.Inlines.Add("Version actuelle: " + version);
        }

        /// <summary>
        /// Événement lors du click sur le bouton de déploiement
        /// </summary>
        private async void Btn_deploy_Click(object sender, RoutedEventArgs e)
        {
            // Compteur d'actions
            int count = 0;

            // Ouvre le panneau de déploiement
            deploy_progress_flyout.IsOpen = true;

            // Active l'îcone de déploiement
            progress_ring.IsActive = true;

            // Méthode multitâches
            var progress = new Progress<string>(update =>
            {
                if (count != 0)
                {
                    text_deploy_progress.AppendText(Environment.NewLine);
                }
                text_deploy_progress.AppendText(update);
                count++;
            });

            // Execute la fonction progress dans un autre thread
            await Task.Run(() => Deploy(progress));

            // Désactive l'îcone de déploiement en cours
            progress_ring.IsActive = false;

            // Cache l'icone de déploiement en cours
            progress_ring.Visibility = Visibility.Collapsed;

            // Affiche l'icone de déploiement réussi
            check_icon.Visibility = Visibility.Visible;

            // Défini le statut du déploiement en terminé
            deploy_status_textblock.Text = "DÉPLOIEMENT TERMINÉ";

            // Affiche le bouton de fermeture
            btn_close.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Méthode de déploiement dans un thread à part
        /// </summary>
        private void Deploy(IProgress<string> progress)
        {
            // Mise en place de l'auto-configuration Firefox
            if(check_firefox.Dispatcher.Invoke(() => check_firefox.IsChecked == true))
            {
                progress?.Report("Mise en place de la config Firefox...");
                Firefox.Profilize();
            }

            // Désactivation de la suspension sélective USB
            if (check_select_usb.Dispatcher.Invoke(() => check_select_usb.IsChecked == true))
            {
                progress?.Report("Désactivation suspension sélective USB...");
                SelectiveUSB.Disable();
            }

            // Désactivation mise en veille sous secteur
            if (check_sleep.Dispatcher.Invoke(() => check_sleep.IsChecked == true))
            {
                progress?.Report("Désactivation mise en veille sous secteur...");
                Sleep.Disable();
            }

            // Désactivation de l'IPV6
            if (check_ipv6.Dispatcher.Invoke(() => check_ipv6.IsChecked == true))
            {
                progress?.Report("Désactivation de l'IPV6...");
                IPV6.Disable();
            }

            // Désépinglage des applications Windows de la barre des tâches
            if (check_unpin.Dispatcher.Invoke(() => check_unpin.IsChecked == true))
            {
                progress?.Report("Désépinglage des applications Microsoft...");
                Docking.Unpin();
            }

            // Épinglage des applications par défaut
            if (check_pin.Dispatcher.Invoke(() => check_pin.IsChecked == true))
            {
                progress?.Report("Épinglage des applications par défaut...");
                Docking.Pin();
            }

            // Désactivation de la mise en veille des cartes réseau
            if (check_nic_sleep.Dispatcher.Invoke(() => check_nic_sleep.IsChecked == true))
            {
                progress?.Report("Désactivation mise en veille cartes réseau...");
                NICPowerSave.Disable();
            }

            // Désactivation de la mise en veille USB
            if (check_usb_sleep.Dispatcher.Invoke(() => check_usb_sleep.IsChecked == true))
            {
                progress?.Report("Désactivation mise en veille USB...");
                USBPowerSave.Disable();
            }

            // Désinstallation de Onedrive
            if (check_onedrive.Dispatcher.Invoke(() => check_onedrive.IsChecked == true))
            {
                progress?.Report("Désinstallation de Onedrive...");
                Onedrive.Uninstall();
            }

            // Application des options régionales par défaut
            if (check_region.Dispatcher.Invoke(() => check_region.IsChecked == true))
            {
                progress?.Report("Application options régionales...");
                Regional.Set_Thousands_Separator();
                Regional.Set_Decimal_Separator();
            }

            // Suppresion de l'icone Edge du bureau
            if (check_edge_desk.Dispatcher.Invoke(() => check_edge_desk.IsChecked == true))
            {
                progress?.Report("Supression îcone Edge du bureau...");
                Desktop.Delete_Edge_Icon();
            }

            // Ouvre le fichier dummy pour définir Adobe par défaut
            if (check_adobe.Dispatcher.Invoke(() => check_adobe.IsChecked == true))
            {
                progress?.Report("Adobe par défaut S.V.P...");
                FileProps.OpenPDFDetails();
            }

        }

        /// <summary>
        /// Événement lorsque un radio boutton est coché/décoché
        /// </summary>
        private void Radiobuttons_checked_changed(object sender, RoutedEventArgs e)
        {
            // Uniquement si l'application a déjà chargé
            if (window_loaded)
            {
                // Définit la variable à true pour éviter d'activer l'événement du changement de checkbox
                ignore_check = true;

                // Définit le radiobouton utilisé 
                RadioButton clicked_radio = (RadioButton)sender;

                // Bloque le radiobouton coché et active les autres
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

                // Si le preset "nouveau poste" est coché
                if (radio_new.IsChecked == true)
                {
                    // Active toutes les options
                    foreach (ToggleSwitch action_checkbox in actions_groupbox_grid.Children.OfType<ToggleSwitch>())
                    {
                        action_checkbox.IsChecked = true;
                    }
                }
                // Si le preset "mise à jour" est coché
                else if (radio_update.IsChecked == true)
                {
                    // Active toutes les options
                    foreach (ToggleSwitch action_checkbox in actions_groupbox_grid.Children.OfType<ToggleSwitch>())
                    {
                        action_checkbox.IsChecked = true;
                    }

                    // Décoche ces 3 options
                    check_firefox.IsChecked = false;
                    check_unpin.IsChecked = false;
                    check_pin.IsChecked = false;
                }
                // Si le preset "custom" est coché
                else if (radio_custom.IsChecked == true && !ignore_radio_custom)
                {
                    // Décoche toutes les coches
                    foreach (ToggleSwitch action_checkbox in actions_groupbox_grid.Children.OfType<ToggleSwitch>())
                    {
                        action_checkbox.IsChecked = false;
                    }
                }

                // Remet la variable ignore a false
                ignore_check = false;
            }
        }

        /// <summary>
        /// Événement lorsque une coche est cochée/décochée
        /// </summary>
        private void Checkbox_actions_checked_changed(object sender, RoutedEventArgs e)
        {
            // Uniquement lorsque l'application est chargée
            if (window_loaded && !ignore_check)
            {
                // Si toutes les coches sont cochées == Preset nouveau poste
                if (actions_groupbox_grid.Children.OfType<ToggleSwitch>().Count() == actions_groupbox_grid.Children.OfType<ToggleSwitch>().Count(c => c.IsChecked == true))
                {
                    radio_new.IsChecked = true;
                }
                else
                {
                    //Sinon preset custom
                    ignore_radio_custom = true;
                    radio_custom.IsChecked = true;
                    ignore_radio_custom = false;
                }
            }
        }

        /// <summary>
        /// Définit la variable lorsque l'application a fini de charger
        /// </summary>
        private void Main_form_ContentRendered(object sender, System.EventArgs e)
        {
            window_loaded = true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {
            // Nettoie les étapes de déploiement
            text_deploy_progress.Document.Blocks.Clear();

            // Cache l'icone de déploiement terminé
            check_icon.Visibility = Visibility.Collapsed;

            // Affiche l'icone de déploiement en cours
            progress_ring.Visibility = Visibility.Visible;

            // Définit le statut de déploiement en cours
            deploy_status_textblock.Text = "EN COURS DE DÉPLOIEMENT";

            // Cache le bouton de fermeture
            btn_close.Visibility = Visibility.Collapsed;

            // Ferme le panneau de déploiement
            deploy_progress_flyout.IsOpen = false;
        }

        /// <summary>
        /// Ouvre IE lors de l'appui sur le bouton
        /// </summary>
        private void Btn_ie_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("iexplore");
        }

        /// <summary>
        /// Ouvre la page confidentialité des paramatères Windows 10
        /// </summary>
        private void Btn_privacy_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("ms-settings:privacy-general");
        }

        /// <summary>
        /// Ouvre la page des applications par défaut Windows 10
        /// </summary>
        private void Btn_default_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("ms-settings:defaultapps");
        }
    }
}
