﻿using Microsoft.Build.Locator;
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
            new Generator().GeneratorFile("D:\\cocos-client\\assets\\scripts\\logic\\module\\CmdModules\\model\\frame\\Comp\\TestGenerator.as", "D:\\cocos-client\\assets\\scripts\\logic\\module\\CmdModules\\model\\frame\\Comp\\TestGenerator.ts");
            Console.WriteLine("end");
        }      
    }
}