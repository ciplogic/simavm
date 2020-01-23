using System.IO;

namespace SimaVmCore.Resolver
{
    public class ClassResolver
    {
        private readonly SimaVmConfig _vmConfig;

        public ClassResolver(SimaVmConfig vmConfig)
        {
            _vmConfig = vmConfig;
        }

        public static string[] ResolveClass(string className)
        {
            var cacheFileName = Path.Join(ConstStrings.CachedDir, className + ".txt");
            if (File.Exists(cacheFileName)) return File.ReadAllLines(cacheFileName);
            var outProc = Utilities.Execute("javap", "-p -c -v " + className);
            if (outProc.exitCode != 0)
                return new string[0];
            var outProcLines = outProc.code.Split('\n');
            File.WriteAllLines(cacheFileName, outProcLines);
            return outProcLines;
        }

        private static string ExtractPackage(string className)
        {
            var idx = className.LastIndexOf('.');
            if (idx == -1)
                return "";
            return className.Substring(0, idx - 1);
        }
    }
}