using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terry
{
    public class TypeModify
    {
        public static string Trans(string type)
        {
            if (TypeModify.IsNumber(type)){
                return "number";
            }else if(type == "string")
            {
                return "string";
            }else if(type == "bool")
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
