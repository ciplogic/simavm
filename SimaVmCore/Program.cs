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
            
            var classDefinitionCache = new ClassDefinitionCache();

                var classDefinition = classDefinitionCache.GetClass(config.startClass);

                var classes = TypeResolver.Instance.Classes;
                foreach (var clazz in classes)
                {
                    classDefinitionCache.GetClass(clazz.Name);
                }
        }
    }
}