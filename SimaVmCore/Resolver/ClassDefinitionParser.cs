using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace SimaVmCore.Vm
{
    public class ClassDefinitionParser
    {
        private readonly string[] _rows;

        public ClassDefinitionParser(string[] rows)
        {
            _rows = rows;
        }

        private int RowStartsWith(string text)
        {
            for(var i = 0;i<_rows.Length; i++)
                if (_rows[i].StartsWith(text))
                    return i;
            return -1;
        }
        public ClassDefinition ParseDefinition()
        {
            var cd = new ClassDefinition
            {
                Name = GetClassName()
            };

            ExtractMembers(cd);

            return cd;
        }

        private void ExtractMembers(ClassDefinition cd)
        {
            var startMethodBlock = RowStartsWith("{");
            var endMethodBlock = RowStartsWith("}");
            ExtractFields(cd, startMethodBlock, endMethodBlock);
        }

        int CountStartSpaces(string text)
        {
            for (var i = 0; i < text.Length; i++)
            {
                if (text[i] != ' ')
                    return i;
            }

            return 0;
        }

        private void ExtractFields(ClassDefinition classDefinition, int startMethodBlock,  int endMethodBlock)
        {
            var rowsWithFields = new List<string>();
            var rowsWithMethodsOrCtors = new List<string>();
            for (var i = startMethodBlock+1; i <= endMethodBlock; i++)
            {
                var row = _rows[i];
                if (CountStartSpaces(row) == 2)
                {
                    row = row.Substring(2);
                    if (!row.Contains('('))
                    {
                        rowsWithFields.Add(row);
                    }
                    else
                    {
                        rowsWithMethodsOrCtors.Add(row);
                    }
                }
            }

            BuildClassFields(classDefinition, rowsWithFields);
            BuildClassMethods(classDefinition, rowsWithMethodsOrCtors);
        }

        private void BuildClassMethods(ClassDefinition classDefinition, List<string> rows)
        {
            foreach (var row in rows)
            {
                var declaration = ExtractModifiers(row);
                var openParenIndex = declaration.remainder.IndexOf('(');
                var beforeParen = declaration.remainder.Substring(0, openParenIndex);
                var args = declaration.remainder.Substring(openParenIndex + 1);
                args = args.Substring(0, args.Length - 2);
                var isMethod = beforeParen.Contains(' ');
                if (!isMethod)
                {
                    var ctor = BuildConstructor(classDefinition, declaration.modifiers, args);
                    classDefinition.Members.Add(ctor);
                    continue;
                }

                var mth = BuildMethod(classDefinition, declaration.modifiers, beforeParen, args);
                classDefinition.Members.Add(mth);
            }
        }

        TypeDef[] ResolvedArguments(string args)
        {
            var argsSplit = args.Split(',', StringSplitOptions.RemoveEmptyEntries);
            var resultList = new List<TypeDef>();
            foreach (var argType in argsSplit)
            {
                var argsResult = TypeResolver.Instance.Resolve(argType);
                resultList.Add(argsResult);
            }

            return resultList.ToArray();
        }

        private MethodDefinition BuildMethod(ClassDefinition classDefinition, Modifier[] declarationModifiers, string beforeParen, string args)
        {
            var split = beforeParen.Split(' ');
            var methodName = split[1];
            var result = new MethodDefinition(classDefinition, methodName)
            {
                Modifiers = declarationModifiers,
                ReturnType = TypeResolver.Instance.Resolve(split[0]),
                Parameters = ResolvedArguments(args)
            };
            return result;
        }

        private ConstructorDefinition BuildConstructor(ClassDefinition classDefinition, Modifier[] declarationModifiers, string args)
        {
            var result = new ConstructorDefinition(classDefinition, classDefinition.Name)
            {
                Modifiers = declarationModifiers, Parameters = ResolvedArguments(args)
            };
            return result;
        }

        private static void BuildClassFields(ClassDefinition classDefinition, List<string> rowsWithFields)
        {
            foreach (var rowWithField in rowsWithFields)
            {
                var declaration = ExtractModifiers(rowWithField);
                var split = declaration.remainder.Split(' ');
                
                var name = split[1];
                name = name.Substring(0, name.Length - 1);

                var field = new FieldDefinition(classDefinition, name)
                {
                    Modifiers = declaration.modifiers,
                    Type = TypeResolver.Instance.Resolve(split[0])
                };
                classDefinition.Members.Add(field);
            }
        }

        private string GetClassName()
        {
            var classIndexRow = RowStartsWith("class ");
            return _rows[classIndexRow].Substring(6);
        }

        public static (string remainder, bool found) StartsWithCut(string text, string startsWith)
        {
            if (!text.StartsWith(startsWith))
                return (text, false);
            return (text.Substring(startsWith.Length), true);
        }

        public static (Modifier[] modifiers, string remainder) ExtractModifiers(string declaration)
        {
            var resultModifiers = new List<Modifier>();
            var modifiers = new Dictionary<string, Modifier>()
            {
                {"public", Modifier.Public
                },
                {"private", Modifier.Private},
                {"static", Modifier.Static},
                {"protected", Modifier.Protected}
            };
            do
            {
                var found = false;
                foreach (var m in modifiers)
                {
                    var startText = m.Key + " ";
                    var starts = StartsWithCut(declaration, startText);
                    if (starts.found)
                    {
                        resultModifiers.Add(m.Value);
                        declaration = starts.remainder;
                        found = true;
                        break;
                    }
                }

                if (!found)
                    break;
            } while (true);

            return (resultModifiers.ToArray(), declaration);
        }
    }
}