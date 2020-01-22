using System;
using System.IO;
using System.Linq;
using SimaVmCore.Vm;

namespace SimaVmCore
{
    class Program
    {
       
        static void Main(string[] args)
        {
            var config = "simavm.json".DeserializeFile<SimaVmConfig>();
            Directory.SetCurrentDirectory(config.startDir);
            var outProc = Utilities.Execute("javap", "-p -c -v " + config.startClass);
            var outProcLines = outProc.code.Split('\n');
            var parser = new ClassDefinitionParser(outProcLines);
            var def = parser.parseDefinition();
            Console.WriteLine("Output:\n "+string.Join("\n", outProcLines));
        }
    }
}