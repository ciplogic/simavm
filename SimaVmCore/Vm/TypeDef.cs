namespace SimaVmCore.Vm
{
    public class TypeDef
    {
        public TypeDef ElementType;
        public bool IsArray;
        public bool IsPrimtive;
        public string Name;

        public override string ToString()
        {
            return Name;
        }
    }
}