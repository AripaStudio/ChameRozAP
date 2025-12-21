using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChameRozAP.ServiceManager
{
    public static class ShowMessageAP
    {
        public static void ShowMessageBoxAP(string message , string title)
        {
            string command =
                $"Add-Type -AssemblyName PresentationFramework; [System.Windows.MessageBox]::Show('{message}' , '{title}')";
            var processInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "powershell",
                Arguments = $"-Command \"{command}\"",
                CreateNoWindow = true,
                UseShellExecute = false
            };

            System.Diagnostics.Process.Start(processInfo);
        }
    }
}
