namespace SimaVmCore.Vm
{
    public class MethodDefinition : MemberDefinition
    {
        public TypeDef[] Parameters;
        public TypeDef ReturnType;

        public MethodDefinition(ClassDefinition parentClass, string name) : base(parentClass, name)
        {
        }
    }
}