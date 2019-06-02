using System.Diagnostics;

namespace SSCDeploy.Actions
{
    public static class PowerCFG
    {
        private const string POWER_CFG = "powercfg";

        public static void Power_CFG(string args)
        {
            Process p = new Process();
            ProcessStartInfo cmd = new ProcessStartInfo();
            cmd.FileName = "cmd.exe";
            cmd.RedirectStandardInput = true;
            cmd.UseShellExecute = false;
            cmd.CreateNoWindow = true;
            cmd.Arguments = "/C " + POWER_CFG + args;

            p.StartInfo = cmd;
            p.Start();
            p.WaitForExit();
        }
    }
}
