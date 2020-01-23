namespace SimaVmCore.Vm
{
    public class MethodDefinition : MemberDefinition
    {
        public TypeDef ReturnType;

        public MethodDefinition(ClassDefinition parentClass, string name) : base(parentClass, name)
        {
        }
    }
}