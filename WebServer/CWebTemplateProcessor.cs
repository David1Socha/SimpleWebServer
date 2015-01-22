using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebServer
{
    class CWebTemplateProcessor : IScriptProcessor
    {

        private static Regex _OutputRegex = new Regex("@{(?<code>[^}])*}"); //and now I have 2 problems..
        private const String _WriteHtmlString = "wout.WriteLine(\"{0}\");",
                             _WriteCodeString = "{wout.WriteLine(${code});}";
        private CSharpServerCodeExecutor _executor;

        public CWebTemplateProcessor()
        {
            _executor = new CSharpServerCodeExecutor();
        }

        private static int _FindClosingBracket(String str, int openingBracketIndex)
        {
            int leftRightDifference = 1;
            int index = openingBracketIndex;
            while (leftRightDifference > 0)
            {
                index++;
                if (str[index] == '}')
                {
                    leftRightDifference--;
                }
                else if (str[index] == '{')
                {
                    leftRightDifference++;
                }
            }
            return index;
        }

        /// <summary>
        /// Returns index of next opening bracket, or -1 if no opening bracket is left
        /// </summary>
        /// <param name="str"></param>
        /// <param name="searchStartIndex"></param>
        /// <returns></returns>
        private static int _FindOpeningBracket(String str, int searchStartIndex)
        {
            return str.IndexOf('{', searchStartIndex);
        }

        public ScriptResult ProcessScript(Stream stream, IDictionary<string, string> requestParameters)
        {
            StringBuilder code = new StringBuilder();
            String script = _BuildScript(stream);
            script = _ReplaceOutputLines(script);
            int i = 0;
            while (i < script.Length)
            {
                int openingIndex = _FindOpeningBracket(script, i);
                if (openingIndex == -1)
                {
                    String remainingHtml = script.Substring(i);
                    code.AppendFormat(_WriteHtmlString, remainingHtml);
                }
                else
                {
                    String htmlSoFar = script.Substring(i, openingIndex - i);
                    if (htmlSoFar.Length > 0)
                    {
                        code.AppendFormat(_WriteHtmlString, htmlSoFar);
                    }
                    int closingIndex = _FindClosingBracket(script, openingIndex);
                    if (openingIndex + 1 > closingIndex) //check if code exists inbetween brackets
                    {
                        String codeInBrackets = script.Substring(openingIndex + 1, closingIndex - openingIndex - 1);
                        code.Append(codeInBrackets);
                    }

                    i = closingIndex + 1;
                }

            }
            ScriptResult result = _executor.ExecuteCode(code.ToString(), requestParameters);
            return result;
        }

        private static String _ReplaceOutputLines(String script)
        {
            return _OutputRegex.Replace(script, _WriteCodeString);
        }

        private static String _BuildScript(Stream scriptStream)
        {
            StringBuilder scriptBuilder = new StringBuilder();
            StreamReader reader = new StreamReader(scriptStream);
            while (!reader.EndOfStream)
            {
                scriptBuilder.Append(reader.ReadLine());
            }
            String script = scriptBuilder.ToString();
            return script;
        }

    }
}
