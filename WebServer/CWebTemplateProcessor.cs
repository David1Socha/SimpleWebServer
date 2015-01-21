using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    class CWebTemplateProcessor
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

    }
}
