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
    "}",
                            _compilationErrorHeader = "<html><body><h1>Script Compilation Errors</h1><p>The following errors occurred processing the requested resource</p><ul>",
                            _compilationErrorLine = "<li>{0}:{1} - Error: {2}</li>",
                            _compilationErrorFooter = "</ul></body></html>",
                            _runtimeErrorMessage = "<html><body><h1>Runtime Error</h1><p>The following runtime error occurred: {0}</p>";

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

        private static ScriptResult _buildCompilationErrorResponse(CompilerErrorCollection compilerErrors)
        {
            StringBuilder response = new StringBuilder();
            response.Append(_compilationErrorHeader);
            foreach (CompilerError e in compilerErrors)
            {
                String errorLine = String.Format(_compilationErrorLine, e.Line, e.Column, e.ErrorText);
                response.Append(errorLine);
            }
            response.Append(_compilationErrorFooter);
            return new ScriptResult()
            {
                Error = true,
                Result = response.ToString()
            };
        }

        private static ScriptResult _buildRuntimeErrorResponse(Exception runtimeException)
        {
            String response = String.Format(_runtimeErrorMessage, runtimeException.Message);
            return new ScriptResult()
            {
                Error = true,
                Result = response
            };
        }

    }
}
