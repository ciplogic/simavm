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
            var cd = new ClassDefinition();
            cd.Name = GetClassName();

            ExtractMembers(cd);

            return cd;
        }

        private void ExtractMembers(ClassDefinition cd)
        {
            var startMethodBlock = this.RowStartsWith("{");
            var endMethodBlock = this.RowStartsWith("}");
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

            foreach (var rowWithField in rowsWithFields)
            {
                var declaration = ExtractModifiers(rowWithField);
                var lastSpaceIndex = declaration.remainder.LastIndexOf(' ');
                var name = declaration.remainder.Substring(lastSpaceIndex + 1);
                name = name.Substring(0, name.Length - 1);

                var field = new FieldDefinition(classDefinition, name)
                {
                    Modifiers = declaration.modifiers
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
                bool found = false;
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