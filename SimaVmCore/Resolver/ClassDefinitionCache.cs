using System;
using System.Collections.Generic;
using SimaVmCore.Vm;

namespace SimaVmCore.Resolver
{
    public class ClassDefinitionCache
    {
        private readonly Dictionary<string, ClassDefinition> _definitions = new Dictionary<string, ClassDefinition>();

        static string SplitAndDrop(string text, char cut)
        {
            var split = text.Split(cut, StringSplitOptions.RemoveEmptyEntries);
            return split[0];
        }
        public ClassDefinition GetClass(string javaLangString)
        {
            javaLangString = SplitAndDrop(javaLangString, '<');
            javaLangString = SplitAndDrop(javaLangString, ')');
            if (_definitions.TryGetValue(javaLangString, out var result)) return result;
            var outProcLines = ClassResolver.ResolveClass(javaLangString);
            if (outProcLines.Length == 0)
                return null;
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