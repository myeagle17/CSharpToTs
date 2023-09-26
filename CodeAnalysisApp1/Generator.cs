using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Terry
{
    public class Generator
    {
        public void GeneratorFile(string filename,string output) {

            SyntaxTree tree = CSharpSyntaxTree.ParseText(ReadFile(filename));
            PrintTree(tree);
            FileWrite.End(output);
        }

        private string ReadFile(string filename)
        {
            var fileStr = "";
            using (var fs = new StreamReader(filename))
            {

                //string s = "";
                //while((s= fs.ReadLine()) != null) {
                //    fileStr += s;
                //}
               fileStr= fs.ReadToEnd();
            }
            return fileStr;
        }

        private void PrintTree(SyntaxTree tree)
        {
            new PrintTree().Print(tree);
        }
        
    }
}
