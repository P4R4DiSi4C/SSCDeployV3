using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace SSCDeploy.Actions
{
    public class FileProps
    {
        private const int SW_SHOW = 5;
        private const uint SEE_MASK_INVOKEIDLIST = 12;

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        static extern bool ShellExecuteEx(ref SHELLEXECUTEINFO lpExecInfo);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SHELLEXECUTEINFO
        {
            public int cbSize;
            public uint fMask;
            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpVerb;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpFile;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpParameters;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpDirectory;
            public int nShow;
            public IntPtr hInstApp;
            public IntPtr lpIDList;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpClass;
            public IntPtr hkeyClass;
            public uint dwHotKey;
            public IntPtr hIcon;
            public IntPtr hProcess;
        }

        public static void OpenPDFDetails()
        {
            try
            {
                SHELLEXECUTEINFO info = new SHELLEXECUTEINFO();
                info.cbSize = Marshal.SizeOf(info);
                info.lpVerb = "properties";
                info.lpParameters = "Details";
                info.lpFile = @"Files\dummy.pdf";
                info.nShow = SW_SHOW;
                info.fMask = SEE_MASK_INVOKEIDLIST;
                ShellExecuteEx(ref info);
            }
            catch (Exception ex)
            {
                MessageBox.Show("L'ouverture de la fenêtre 'Proprietés' du fichier .PDF s'est mal déroulée: " + ex.Message);
            }
        }
    }
}
