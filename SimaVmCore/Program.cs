using System;
using System.IO;
using System.Linq;
using SimaVmCore.Resolver;
using SimaVmCore.Vm;

namespace SimaVmCore
{
    class Program
    {
       
        static void Main(string[] args)
        {
            var config = "simavm.json".DeserializeFile<SimaVmConfig>();
            Directory.SetCurrentDirectory(config.startDir);
            
            if (!Directory.Exists(ConstStrings.CachedDir))
            {
                Directory.CreateDirectory(ConstStrings.CachedDir);
            }
            
            var resolver = new ClassResolver(config);
            var outProcLines = resolver.ResolveClass(config.startClass);
            var parser = new ClassDefinitionParser(outProcLines);
            var def = parser.ParseDefinition();
            Console.WriteLine("Output:\n "+string.Join("\n", outProcLines));
        }
    }
}