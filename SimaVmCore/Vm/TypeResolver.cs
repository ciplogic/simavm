using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SimaVmCore.Vm
{
    public class TypeResolver
    {
        public static  TypeResolver Instance { get; } = new TypeResolver();
        public int Count => Classes.Count();

        public IEnumerable<TypeDef> Classes => Defs.Values.Where(isClass);

        private bool isClass(TypeDef it)
        {
            if (it.IsArray || it.IsPrimtive)
                return false;
            return true;
        }

        Dictionary<string, TypeDef> Defs = new Dictionary<string, TypeDef>();
        private TypeResolver()
        {
            AddPrimitive("void");
            AddPrimitive("int");
            AddPrimitive("char");
            AddPrimitive("short");
            AddPrimitive("long");
            AddPrimitive("float");
            AddPrimitive("double");
        }

        void AddPrimitive(string typeName)
        {
            var primitive = new TypeDef() {Name = typeName, ElementType = null, IsArray = false, IsPrimtive = true};
            Defs[typeName] = primitive;
        }

        static bool IsArray(string typeName)
        {
            return typeName.EndsWith("[]");
        }

        public  TypeDef Resolve(string typeName)
        {
            var defs = Defs;
            if (defs.TryGetValue(typeName, out var result))
                return result;
            if (IsArray(typeName))
                return Instance.Resolved(typeName, ResolveArray(typeName));
            var typeDef= new TypeDef()
            {
                Name = typeName
            };
            return Instance.Resolved(typeName, typeDef);
        }

        private TypeDef ResolveArray(string typeName)
        {
            var typeNameElement = typeName.Substring(0, typeName.Length - 2);
            var resolvedElement = Resolve(typeNameElement);
            var fullType = new TypeDef()
            {
                ElementType = resolvedElement,
                IsArray = true,
                IsPrimtive = false,
                Name = typeName
            };
            return fullType;
        }

        private TypeDef Resolved(string typeName, TypeDef resolveArray)
        {
            Debug.Assert(typeName.Length > 0);
            Defs[typeName] = resolveArray;
            return resolveArray;
        }

        public int Summary()
        {
            foreach (var def in Defs)
            {
                Console.WriteLine(def.Key);
            }

            return Defs.Count;
        }
    }
}