using System.Collections.Generic;

namespace SimaVmCore.Vm
{
    public class ClassDefinition
    {
        public string Name;
        public string BaseClass;
        public List<string> Interfaces;
        public List<ClassPoolEntry> ClassPool { get; set; } = new List<ClassPoolEntry>();
        
    }

    public class ClassDefinitionParser
    {
        private readonly string[] _rows;

        public ClassDefinitionParser(string[] rows)
        {
            _rows = rows;
        }

        int RowStartsWith(string text)
        {
            for(var i = 0;i<_rows.Length; i++)
                if (_rows[i].StartsWith(text))
                    return i;
            return -1;
        }
        public ClassDefinition parseDefinition()
        {
            var cd = new ClassDefinition();
            string className = GetClassName();
            cd.Name = className;

            return cd;
        }

        private string GetClassName()
        {
            var classIndexRow = RowStartsWith("class ");
            return _rows[classIndexRow].Substring(6);
        }
    }
}