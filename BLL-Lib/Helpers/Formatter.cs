using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.SqlServer.Management.SqlParser.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL.Helpers
{
    public static class Formatter
    {
        public static string FormatCSharp(string code)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(code);            
            SyntaxNode root = tree.GetRoot().NormalizeWhitespace();
            return root.ToFullString();
        }

        public static string FormatSql(string code)
        {
            ParseResult result = Parser.Parse(code);
            return result.Script.Sql;
        }

        public static string GetCSharpName(this Type type)
        {
            const string NullablePattern = @"Nullable<(?<nulledType>[\w\.]+)>";
            const string NullableReplacement = @"${nulledType}?";

            Dictionary<Type, string> primitiveTypes = new()
            {
                { typeof(bool), "bool" },
                { typeof(byte), "byte" },
                { typeof(char), "char" },
                { typeof(decimal), "decimal" },
                { typeof(double), "double" },
                { typeof(float), "float" },
                { typeof(int), "int" },
                { typeof(long), "long" },
                { typeof(sbyte), "sbyte" },
                { typeof(short), "short" },
                { typeof(string), "string" },
                { typeof(uint), "uint" },
                { typeof(ulong), "ulong" },
                { typeof(ushort), "ushort" },
            };

            if (primitiveTypes.TryGetValue(type, out string result))
                return result;
            else
                result = type.Name.Replace('+', '.');

            if (!type.IsGenericType)
                return result;
            else if (type.IsNested && type.DeclaringType.IsGenericType)
                throw new NotImplementedException();

            result = result.Substring(0, result.IndexOf("`"));
            result = result + "<" + string.Join(", ", type.GetGenericArguments().Select(GetCSharpName)) + ">";

            return Regex.Replace(result, NullablePattern, NullableReplacement);
        }
    }
}
