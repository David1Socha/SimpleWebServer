using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    class CWebTemplateProcessor : IScriptProcessor
    {

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
            StringBuilder scriptBuilder = new StringBuilder();
            StreamReader reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                scriptBuilder.Append(reader.ReadLine());
            }
            String script = scriptBuilder.ToString();
            return new ScriptResult()
            {
                Result = script,
                Error = true
            };
        }

    }
}
