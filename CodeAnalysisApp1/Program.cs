using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terry;

namespace CodeAnalysisApp1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length >= 2)
            {
                new Generator().GeneratorFile(args[0], args[1]);

            }
            else
            {
            new Generator().GeneratorFile("D:\\TOP\\client\\Assets\\Deer\\Scripts\\Hotfix\\HotFixBusiness\\UI\\UIModelProperty\\editAttrComItem.cs", "D:\\cocos-client\\assets\\scripts\\logic\\module\\CmdModules\\model\\frame\\Comp\\TestGenerator.ts");
            }
            Console.ReadKey();
            Console.WriteLine("end");
        }      
    }
}
