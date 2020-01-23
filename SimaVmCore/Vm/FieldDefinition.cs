namespace SimaVmCore.Vm
{
    public class FieldDefinition : MemberDefinition
    {
        public TypeDef Type;

        public FieldDefinition(ClassDefinition parentClass, string name) : base(parentClass, name)
        {
        }
    }
}