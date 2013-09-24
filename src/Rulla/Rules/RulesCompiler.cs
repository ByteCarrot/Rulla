using Irony.Parsing;
using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ByteCarrot.Rulla.Rules
{
    public class RulesCompiler<TRule, TModel> : IRulesCompiler<TRule, TModel> where TRule : Rule<TModel>
    {
        private readonly IRulesCodeGenerator _generator;
        private readonly List<string> _references = new List<string>();

        public bool DebugMode { get; set; }

        public RulesCompiler(IRulesCodeGenerator generator)
        {
            _generator = generator;
            _references.Add(GetType().Assembly.GetName().Name + ".dll");
            _references.Add(typeof(TRule).Assembly.GetName().Name + ".dll");
            _references.Add(typeof(TModel).Assembly.GetName().Name + ".dll");
        }

        public CompilationResult<TRule, TModel> Compile(string rules)
        {
            var interfaceName = typeof(Rule<TRule>).Name;
            var tree = RulesGrammar.Parse(rules);
            if (tree.Status == ParseTreeStatus.Error)
            {
                return Errors(tree);
            }

            var unit = _generator.GenerateCode<TRule, TModel>(tree);

            if (DebugMode)
            {
                Debug.WriteLine(unit.ToCSharp());
            }

            var result = Compile(unit);
            if (result.Errors.Count > 0)
            {
                return Errors(result);
            }

            var assembly = result.CompiledAssembly;
            var list = assembly.GetExportedTypes()
                .Where(x => x.IsClass && x.GetInterfaces().Any(i => i.Name == interfaceName))
                .Select(x => (TRule)assembly.CreateInstance(x.ToString()))
                .ToList();
            return new CompilationResult<TRule, TModel>(list);
        }

        private static CompilationResult<TRule, TModel> Errors(CompilerResults result)
        {
            var errors = result.Errors.Cast<CompilerError>().Select(x => new CompilationError(x)).ToList();
            return new CompilationResult<TRule, TModel>(errors);
        }

        private static CompilationResult<TRule, TModel> Errors(ParseTree tree)
        {
            return new CompilationResult<TRule, TModel>(tree.ParserMessages.Select(x => new CompilationError(x)).ToList());
        }

        private CompilerResults Compile(CodeCompileUnit unit)
        {
            var lib = GetType().Assembly.GetName().CodeBase;
            var options = new CompilerParameters
            {
                GenerateInMemory = true,
                GenerateExecutable = false,
                OutputAssembly = Path.GetTempFileName(),
                CompilerOptions = String.Format("/lib:\"{0}\"", lib)
            };
            options.ReferencedAssemblies.Add("System.dll");
            _references.ForEach(x => options.ReferencedAssemblies.Add(x));

            return new CSharpCodeProvider().CompileAssemblyFromDom(options, unit);
        }
    }
}