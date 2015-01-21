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

        private const Regex _OutputRegex = new Regex("@{(?<code>[^}])*}"); //and now I have 2 problems..
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

        public ScriptResult ProcessScript(Stream stream, IDictionary<string, string> requestParameters)
        {
            String script = _BuildScript(stream);
            return new ScriptResult()
            {
                Result = script,
                Error = true
            };
        }

        private static String _ReplaceOutputLines(String script)
        {
            return _OutputRegex.Replace(script, "{wout.WriteLine(${code});}");
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
