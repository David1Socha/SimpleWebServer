using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    class CSharpServerCodeExecutor
    {
        private CSharpCodeProvider _provider;
        private CompilerParameters _params;

        private const string _classTemplate = "using System;" +
    "namespace Server {" +
        "public class Executor {" +
            "public void Execute(System.IO.StringWriter wout, System.Collections.Generic.Dictionary<string, string> request) {" +
                "{0}" +
            "}" +
        "}" +
    "}";

        public CSharpServerCodeExecutor()
        {
            _provider = new CSharpCodeProvider();
            _params = new CompilerParameters();
            _params.ReferencedAssemblies.Add("system.dll");
            _params.GenerateInMemory = true;
            _params.CompilerOptions = "/t:library";
        }

        private static String _buildClassString(String code)
        {
            return String.Format(_classTemplate, code);
        }

    }
}
