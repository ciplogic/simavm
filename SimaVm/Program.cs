using System.Diagnostics;
using System.IO;

namespace SimaVm
{
    internal class Program
    {        
        public static (int exitCode, string code) Execute(string fileName, string arguments
             )
        {
            string output = "";
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.UseShellExecute = false;
            psi.RedirectStandardError = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardInput = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.CreateNoWindow = true;
            psi.ErrorDialog = false;
            psi.FileName = fileName;
            psi.Arguments = arguments;

            using (Process process = Process.Start(psi))
            {
                process.StandardInput.Close();
                StreamReader sOut = process.StandardOutput;
                StreamReader sErr = process.StandardError;
                output = sOut.ReadToEnd() + sErr.ReadToEnd();
                sOut.Close();
                sErr.Close();
                process.WaitForExit();
                return (process.ExitCode, output);
            }
        }

        
        public static void Main(string[] args)
        {
        }
    }
}