using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Terry
{
    public struct RegexInfo
    {
        public string Regex;
        public string Replace;
        public string NoRegex;
        public RegexInfo(string regex, string replace, string NoRegex = "")
        {
            this.Replace = replace;
            this.Regex = regex;
            this.NoRegex = NoRegex;
        }
    }
    public class RegexEx
    {
        public static List<RegexInfo> regexInfos = new List<RegexInfo>()
        {
            new RegexInfo("^[ ]*base.","super."),
            new RegexInfo("^[ ]*(\\w+)\\(","this.$1("),

        };

        public static List<RegexInfo> regexTypes = new List<RegexInfo>()
        {
            new RegexInfo("^[ ]*(List)<","Array<"),
            new RegexInfo("<int","this.$1("),

        };

        public static string RpExpressionStatement(string input)
        {

            return RpList(input, regexInfos);
        }

        //public static string RpField(string filedType)
        //{

        //}

        private static string RpList(string input,List<RegexInfo> list)
        {
            string result = input;
            foreach (var item in list)
            {
                result = Regex.Replace(result, item.Regex, item.Replace);
            }
            return result;
        }
        

        
    }
}
