using System.Collections.Generic;

namespace SimaVmCore.Vm
{
    public class MemberDefinition
    {
        public Modifier[] Modifiers = new Modifier[0];
        public string Name;
        public ClassDefinition ParentClass;

        public MemberDefinition(ClassDefinition parentClass, string name)
        {
            ParentClass = parentClass;
            Name = name;
        }
    }

    public class ModifiersDef
    {
        public bool IsStatic;
        public List<Modifier> Modifiers = new List<Modifier>();
    }

    public enum Modifier
    {
        Static,
        Public,
        Private,
        Protected,
        Package
    }

    public class ClassDefinition
    {
        public string BaseClass;
        public List<string> Interfaces;
        public string Name;
        public List<ClassPoolEntry> ClassPool { get; set; } = new List<ClassPoolEntry>();
        public List<MemberDefinition> Members { get; } = new List<MemberDefinition>();
    }
}