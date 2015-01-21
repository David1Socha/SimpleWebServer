using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    class CSharpCodeExecutor
    {
        private CSharpCodeProvider _provider;
        private CompilerParameters _params;

        public CSharpCodeExecutor()
        {
            _provider = new CSharpCodeProvider();
            _params = new CompilerParameters();
            _params.ReferencedAssemblies.Add("system.dll");
            _params.GenerateInMemory = true;
            _params.CompilerOptions = "/t:library";
        }

    }
}
