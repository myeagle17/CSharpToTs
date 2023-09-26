using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Terry
{
    public class PrintTree
    {
        private SyntaxTree tree;

        public void Print(SyntaxTree tree)
        {
            this.tree = tree;
            try
            {
                var root = tree.GetCompilationUnitRoot();
                //while(root.Members.Count == 0)
                //{
                //    Thread.Yield();
                //}
                PrintMember(root.Members);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
        }

        private void PrintMember(SyntaxList<MemberDeclarationSyntax> members)
        {
            for (int i = 0; i < members.Count; i++)
            {
                var member = members[i];
                PrintMember(member);
            }
        }

        private void PrintMember(MemberDeclarationSyntax firstMember)
        {
            if (firstMember is ClassDeclarationSyntax)
            {
                PrintClass((ClassDeclarationSyntax)firstMember);
            }
            else if (firstMember is ConstructorDeclarationSyntax)
            {
                PrintConstruct((ConstructorDeclarationSyntax)firstMember);
            }
            else if (firstMember is FieldDeclarationSyntax)
            {
                PrintField((FieldDeclarationSyntax)firstMember);
            }
            else if (firstMember is MethodDeclarationSyntax)
            {
                PrintMethod((MethodDeclarationSyntax)firstMember);
            }
            else if (firstMember is PropertyDeclarationSyntax)
            {
                PrintPropertyDeclarationSyntax((PropertyDeclarationSyntax)firstMember);
            }

            else if( firstMember is NamespaceDeclarationSyntax)
            {
                PrintMember(((NamespaceDeclarationSyntax)firstMember).Members);
            }
            else
            {
                FileWrite.WriteLine("// 未处理的代码");
                FileWrite.WriteLine(firstMember.ToFullString());
            }
        }

        private void PrintClass(ClassDeclarationSyntax classDeclaration)
        {
            FileWrite.WriteLine($"export class {classDeclaration.Identifier}{GetExtends(classDeclaration)} {{");
            PrintMember(classDeclaration.Members);
            FileWrite.WriteLine($"}}");
        }

        private void PrintConstruct(ConstructorDeclarationSyntax constructorDeclaration)
        {
            FileWrite.WriteLine($"public constructor{GetParameterList(constructorDeclaration.ParameterList)}{{");
            FileWrite.WriteLine($"{GetConstructorInitializer(constructorDeclaration.Initializer)};");
            PrintStatements(constructorDeclaration.Body.Statements);
            FileWrite.WriteLine($"}}");
        }

        private void PrintField(FieldDeclarationSyntax filed)
        {
            FileWrite.WriteLine("");
            FileWrite.WriteLine($"{GetModifier(filed.Modifiers)}{GetFiledVariableDeclarationSyntax(filed.Declaration)}");
        }

        private string GetFiledVariableDeclarationSyntax(VariableDeclarationSyntax variableDeclarationSyntax)
        {
            var str = $"{variableDeclarationSyntax.Variables[0].Identifier}:{GetType(variableDeclarationSyntax.Type)}";
            if (variableDeclarationSyntax.Variables.Count > 0)
            {
                var v = variableDeclarationSyntax.Variables[0];
                if (null != v.Initializer)
                {
                    str += $"{v.Initializer.ToString()}";
                }
            }
            str +=";";
            return str;
        }
        
        private void PrintMethod(MethodDeclarationSyntax methodDeclarationSyntax)
        {
            FileWrite.WriteLine($"{methodDeclarationSyntax.Identifier}{GetParameterList(methodDeclarationSyntax.ParameterList)}{{");
            // 写变量
            
            // 写语句
            PrintStatements(methodDeclarationSyntax.Body.Statements);

            FileWrite.WriteLine($"}}");
        }

        private void PrintPropertyDeclarationSyntax(PropertyDeclarationSyntax propertyDeclaration)
        {
            if (propertyDeclaration.ExpressionBody != null)
            {
                return;
            }

            //if (propertyDeclaration.DescendantNodesAndTokensAndSelf().Any(x => x.GetLeadingTrivia().Concat(x.GetTrailingTrivia()).Any(y => !y.i)))
            //{
            //    return;
            //}
            
            foreach (var item in propertyDeclaration.AccessorList.Accessors)
            {
                var isGet = item.Keyword.ValueText == "get";
                var paramStr = isGet ? "" : $"value:{this.GetType(propertyDeclaration.Type)}";
                var firstStr = $"public {item.Keyword.ValueText} {propertyDeclaration.Identifier}({paramStr})";
                if (isGet)
                {
                    firstStr += $":{GetType(propertyDeclaration.Type)}";
                }
                FileWrite.WriteLine(firstStr);
                FileWrite.WriteLine(item.Body.ToString());
                //item.Body.Statements
                //FileWrite.WriteLine($"}}");
            }
        }


        private void PrintStatements(SyntaxList<StatementSyntax> statementSyntaxs)
        {
            foreach (var item in statementSyntaxs)
            {
                if (PrintExpressionStatementSyntax(item)) continue;
                if (PrintLocalDeclarationStatement(item)) continue;
                if(PrintIfStatement(item)) continue;
                if(PrintForStatement(item)) continue;
                if (PrintForeachStatement(item)) continue;
                if (PrintReturnStatement(item)) continue;
                FileWrite.WriteLine("// 未处理代码");
                FileWrite.WriteLine(item.ToFullString());
            }
        }

        private bool PrintExpressionStatementSyntax(StatementSyntax statementSyntax)
        {
            if (!(statementSyntax is ExpressionStatementSyntax)) return false;
            var express = (ExpressionStatementSyntax)statementSyntax;
            FileWrite.WriteLine(RegexEx.RpExpressionStatement(statementSyntax.ToFullString()));
            return true;
        }

        private bool PrintLocalDeclarationStatement(StatementSyntax statementSyntax)
        {
            if(!(statementSyntax is LocalDeclarationStatementSyntax))
            {
                return false;
            }

            var localDeclaration = (LocalDeclarationStatementSyntax)statementSyntax;
            var variable = localDeclaration.Declaration.Variables[0];
            var str = "";
            if (null != variable.Initializer)
            {
                var strType = "";
                if(localDeclaration.Declaration.Type.ToString() == "var")
                {
                    strType = "/* var */";
                    //strType =$": {variable.Initializer.Value.ToString()}";
                }
                else
                {
                    strType = $": {GetType(localDeclaration.Declaration.Type)}";
                }
                str = $"let {variable.Identifier}{strType} {variable.Initializer}";
            }
            else
            {
                str = $"let {variable.Identifier}:{GetType(localDeclaration.Declaration.Type)}";
            }
               
            FileWrite.WriteLine(str);
            return true;
        }

        private bool PrintIfStatement(StatementSyntax statementSyntax, bool isElse = false)
        {
            if (!(statementSyntax is IfStatementSyntax))
            {
                return false;
            }
            IfStatementSyntax ifStatement = (IfStatementSyntax)statementSyntax;
            string ifTag =(isElse?"else ":"") +  ifStatement.IfKeyword.Text;
            string ifTagCondition = ifStatement.Condition.ToString();
            FileWrite.WriteLine($"{ifTag}({ifTagCondition}){{");
            if(ifStatement.Statement is BlockSyntax)
            {
                PrintStatements((ifStatement.Statement as BlockSyntax).Statements);
            }
            else
            {
                FileWrite.WriteLine(ifStatement.Statement.ToString());
            }
            
            FileWrite.WriteLine("}");
            if(null != ifStatement.Else)
            {
                if (PrintIfStatement(ifStatement.Else.Statement,true)) return true;
                if (ifStatement.Else.Statement is BlockSyntax)
                {
                    FileWrite.WriteLine("else{");
                    PrintStatements((ifStatement.Else.Statement as BlockSyntax).Statements);
                    FileWrite.WriteLine("}");
                }
            }
            return true;
        }

        private bool PrintForStatement(StatementSyntax statementSyntax, bool isElse = false)
        {
            if (!(statementSyntax is ForStatementSyntax))
            {
                return false;
            }
            ForStatementSyntax forStatement = (ForStatementSyntax)statementSyntax;
            var decalrationStr = "";
            if (null != forStatement.Declaration)
            {
                decalrationStr = $"let {forStatement.Declaration.Variables[0].Identifier}:{GetType(forStatement.Declaration.Type)}{forStatement.Declaration.Variables[0].Initializer}";
            }
            var conditionStr = "";
            if (null != forStatement.Condition)
            {
                var condition = forStatement.Condition;
                conditionStr = condition.ToString();
            }
            var incrementStr = "";
            if (null != incrementStr)
            {
                incrementStr = forStatement.Incrementors[0].ToString();
            }
            FileWrite.WriteLine($"for({decalrationStr};{conditionStr};{incrementStr}){{");
            PrintStatements((forStatement.Statement as BlockSyntax).Statements);
            FileWrite.WriteLine("}");    
            return true;
        }

            private string GetParameterList(ParameterListSyntax parameterList)
        {
            var result = "(";
            for(int i = 0;i<parameterList.Parameters.Count;i++)
            {
                var parameter = parameterList.Parameters[i];
                result += parameter.Identifier + ":" + GetType(parameter.Type);
                if (i == parameterList.Parameters.Count - 1)
                {

                }
                else
                {
                    result += ", ";
                }
            }
            result += ")";
            return result;
        }

        private bool PrintForeachStatement(StatementSyntax statementSyntax) {
            if (!(statementSyntax is ForEachStatementSyntax))
            {

                return false;
            }
   
            var foreachStatement = (ForEachStatementSyntax)statementSyntax;
            FileWrite.WriteLine($"for(let {foreachStatement.Identifier} of {foreachStatement.Expression.ToString()}){{");
            PrintStatements((foreachStatement.Statement as BlockSyntax).Statements);
            FileWrite.WriteLine("}");
            return true;
        }

            private string GetExtends(ClassDeclarationSyntax classDeclarationSyntax)
        {
            var result = "";
            //foreach (var ancestorNode in classDeclarationSyntax.BaseList)
            if (classDeclarationSyntax.BaseList == null) return result;
            for (int i = 0; i < classDeclarationSyntax.BaseList.Types.Count; i++)
            {
                result += " extends " +GetType(classDeclarationSyntax.BaseList.Types[i].Type);
                break;
            }
            return result;
        }

        private bool PrintReturnStatement(StatementSyntax statementSyntax)
        {
            if (!(statementSyntax is ReturnStatementSyntax))
            {

                return false;
            }


            return true;
        }
          private string GetConstructorInitializer(ConstructorInitializerSyntax constructorInitializerSyntax)
        {
            var result = "super(";
            bool first = true;
            foreach (var expression in constructorInitializerSyntax.ArgumentList.Arguments)
            {
                if (first)
                    first = false;
                else
                    result +=", ";

                result += expression.Expression;
            }
            result += ")";
            return result;
        }

        private string GetModifier(SyntaxTokenList tokenList)
        {
            var result = "";
            foreach (var item in tokenList)
            {
                result +=item.Value+" ";
            }
            return result;
        }

        private string GetType(string type)
        {
            return TypeModify.Trans(type);
        }
        private string GetType(object type)
        {
            return TypeModify.Trans(type.ToString());
        }
    }
}
