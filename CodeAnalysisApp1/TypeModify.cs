using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terry
{
    public class TypeModify
    {


        public static string Trans(TypeSyntax type)
        {
            //if(type)
            var genericNameSyntax = type as GenericNameSyntax;
            var str = TransBaseType(type.ToString());
            if(null != genericNameSyntax)
            {
                var typeFirst = "";
                if(genericNameSyntax.Identifier.Text == "List")
                {
                    typeFirst = "Array";
                }
                else
                {
                    typeFirst = genericNameSyntax.Identifier.Text;
                }

                var argueStr = "";
                var isFirst = true;
                foreach (var argus in genericNameSyntax.TypeArgumentList.Arguments)
                {
                    if (!isFirst)
                    {
                        argueStr += ", ";
                    }
                    argueStr += TransBaseType(argus.ToString());
                    isFirst = false;
                }
                str = $"{typeFirst}<{argueStr}>";
            }
            return str;
        }

        private static string TransBaseType(string type)
        {
            if (TypeModify.IsNumber(type))
            {
                return "number";
            }
            else if (type == "string")
            {
                return "string";
            }
            else if (type == "bool")
            {
                return "boolean";
            }
            else
            {
                return RegexEx.RpExpressionStatement(type); ;
            }
        }

        private static bool IsNumber(string tag)
        {
            return tag == "int" || tag == "float" || tag == "double";
        }
    }
}
