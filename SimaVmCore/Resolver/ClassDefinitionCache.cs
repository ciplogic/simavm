using System.Collections.Generic;
using SimaVmCore.Vm;

namespace SimaVmCore.Resolver
{
    public class ClassDefinitionCache
    {
        private readonly Dictionary<string, ClassDefinition> _definitions = new Dictionary<string, ClassDefinition>();

        public ClassDefinition GetClass(string javaLangString)
        {
            if (_definitions.TryGetValue(javaLangString, out var result)) return result;
            var outProcLines = ClassResolver.ResolveClass(javaLangString);
            var parser = new ClassDefinitionParser(outProcLines);
            var def = parser.ParseDefinition();
            _definitions[javaLangString] = def;
            return def;
        }

        public bool HasClass(string className)
        {
            return _definitions.ContainsKey(className);
        }
    }
}