using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.SqlServer.Management.SqlParser.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
