using System;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace SimaVmCore
{
    internal static class Utilities
    {
        public static (int exitCode, string code) Execute(string fileName, string arguments)
        {
            var output = "";
            var psi = new ProcessStartInfo();
            psi.UseShellExecute = false;
            psi.RedirectStandardError = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardInput = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.CreateNoWindow = true;
            psi.ErrorDialog = false;
            psi.FileName = fileName;
            psi.Arguments = arguments;

            using (var process = Process.Start(psi))
            {
                process.StandardInput.Close();
                var sOut = process.StandardOutput;
                var sErr = process.StandardError;
                output = sOut.ReadToEnd() + sErr.ReadToEnd();
                sOut.Close();
                sErr.Close();
                process.WaitForExit();
                return (process.ExitCode, output);
            }
        }

        public static T DeserializeFile<T>(this string fileName)
            where T : new()
        {
            try
            {
                var json = File.ReadAllText(fileName);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception)
            {
                return new T();
            }
        }
    }
}