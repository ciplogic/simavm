namespace SimaVmCore.Vm
{
    public class ConstructorDefinition : MemberDefinition
    {
        public TypeDef[] Parameters;

        public ConstructorDefinition(ClassDefinition parentClass, string name) : base(parentClass, name)
        {
        }
    }
}