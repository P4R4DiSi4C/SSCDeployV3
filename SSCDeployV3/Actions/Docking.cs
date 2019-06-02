using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;
using System.Windows;

namespace SSCDeploy.Actions
{
    public static class Docking
    {
        private static string[] user_pinned = { Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft", "Internet Explorer", "Quick Launch", "User Pinned","Taskbar" };
        private static DirectoryInfo dir_user_pinned = new DirectoryInfo(Path.Combine(user_pinned));

        class PinnedDir
        {
            public string DirToPin { get; set; }
            public string DirToUnpin { get; set; }
        }

        static List<string> WinAppsToUnpin = new List<string>
        {
            "Microsoft Edge",
            "Microsoft Store"
        };

        static Dictionary<string, PinnedDir> AppsToPin = new Dictionary<string, PinnedDir>()
        {
            {
                "IE", new PinnedDir()
                {
                    DirToPin = @"C:\Program Files\internet explorer\iexplore.exe",
                    DirToUnpin = Path.Combine(dir_user_pinned.FullName,"Internet Explorer.lnk")
                }
            },
            {
                "FIREFOX", new PinnedDir()
                {
                    DirToPin = @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe",
                    DirToUnpin = Path.Combine(dir_user_pinned.FullName,"Firefox.lnk")
                }
            },
            {
                "OUTLOOK", new PinnedDir()
                {
                    DirToPin = @"C:\Program Files (x86)\Microsoft Office\Office16\OUTLOOK.EXE",
                    DirToUnpin = Path.Combine(dir_user_pinned.FullName,"Outlook 2016.lnk")
                }
            },
            {
                "WORD", new PinnedDir()
                {
                    DirToPin = @"C:\Program Files (x86)\Microsoft Office\Office16\WINWORD.EXE",
                    DirToUnpin = Path.Combine(dir_user_pinned.FullName,"Word 2016.lnk")
                }
            },
            {
                "EXCEL", new PinnedDir()
                {
                    DirToPin = @"C:\Program Files (x86)\Microsoft Office\Office16\EXCEL.EXE",
                    DirToUnpin = Path.Combine(dir_user_pinned.FullName,"Excel 2016.lnk")
                }
            },
            {
                "POWERPOINT", new PinnedDir()
                {
                    DirToPin = @"C:\Program Files (x86)\Microsoft Office\Office16\POWERPNT.EXE",
                    DirToUnpin = Path.Combine(dir_user_pinned.FullName,"Powerpoint 2016.lnk")
                }
            },
            {
                "ANYCONNECT", new PinnedDir()
                {
                    DirToPin =  @"C:\Program Files (x86)\Cisco\Cisco AnyConnect Secure Mobility Client\vpnui.exe",
                    DirToUnpin = Path.Combine(dir_user_pinned.FullName,"Cisco AnyConnect User Interface.lnk")
                }
            }
        };

        public static void Pin()
        {
            try
            {
                Process p = new Process();
                ProcessStartInfo cmd = new ProcessStartInfo();
                cmd.FileName = "cmd.exe";
                cmd.RedirectStandardInput = true;
                cmd.UseShellExecute = false;
                cmd.CreateNoWindow = true;

                p.StartInfo = cmd;
                p.Start();

                using (StreamWriter sw = p.StandardInput)
                {
                    if (sw.BaseStream.CanWrite)
                    {
                        foreach (KeyValuePair<string, PinnedDir> app in AppsToPin)
                        {
                            
                            sw.WriteLine("Files\\syspin \"" + app.Value.DirToUnpin + "\" c:5387"); //Unpin from taskbar
                            sw.WriteLine("Files\\syspin \"" + app.Value.DirToPin + "\" c:51394"); //Unpin from start

                            // Peut aussi check if Files.Exist
                            sw.WriteLine("Files\\syspin \"" + app.Value.DirToPin + "\" c:5386"); //Pin to taskbar
                            sw.WriteLine("Files\\syspin \"" + app.Value.DirToPin + "\" c:51201"); //Pin to start
                        }
                    }
                }

                p.WaitForExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("L'épinglage des applications Microsoft s'est mal déroulé: " + ex.Message);
            }
        }

        public static void Unpin()
        {
            try
            {
                using (PowerShell powershell_Inst = PowerShell.Create())
                {
                    foreach (string appName in WinAppsToUnpin)
                    {
                        powershell_Inst.AddScript("((New-Object -Com Shell.Application).NameSpace('shell:::{4234d49b-0245-4df3-b780-3893943456e1}').Items() | ?{$_.Name -eq '" + appName + "'}).Verbs() | ?{$_.Name.replace('&','') -match 'Désépingler de la barre des tâches'} | %{$_.DoIt(); $exec = $true}");
                    }

                    Collection<PSObject> PSOutput = powershell_Inst.Invoke();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Le désépinglage des applications Microsoft s'est mal déroulé: " + ex.ToString());
            }
        }
    }
}
