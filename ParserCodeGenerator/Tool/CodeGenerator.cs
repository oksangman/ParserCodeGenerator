using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserCodeGenerator.Tool
{
    public class CodeGenerator
    {
        public static string CSharpCodeGenerate(List<Models.GrammarModel> tokenList, string _namespace)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Properties.Resources.CSharp_BeforNamespace);
            sb.Append(_namespace);
            sb.Append(Properties.Resources.CSharp_AfterNamespace);
            for (int i = 0; i < tokenList.Count; ++i)
            {
                if (string.IsNullOrWhiteSpace(tokenList[i].Comment))
                    sb.AppendLine($"\t\t{tokenList[i].Name} = {i+1},");
                else
                    sb.AppendLine($"\t\t{tokenList[i].Name} = {i + 1},\t//\t{tokenList[i].Comment}");
            }
            sb.Length -= 2;
            sb.Append(Properties.Resources.CSharp_AfterEnum);
            for (int i=0; i<tokenList.Count; ++i)
            {
                sb.AppendLine($"\t\t\tAddRegExToken(\"{tokenList[i].RegularExpression.Replace("\\","\\\\").Replace("\"", "\\\"")}\", TokenType.{tokenList[i].Name});");
            }
            sb.Append(Properties.Resources.CSharp_AfterAddToken);
            return sb.ToString();
        }
    }
}
