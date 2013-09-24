using Irony.Parsing;
using System.CodeDom;

namespace ByteCarrot.Rulla.Rules
{
    public class RulesCodeGenerator : IRulesCodeGenerator
    {
        public CodeCompileUnit GenerateCode(ParseTree tree)
        {
            var generator = new RuleCodeGenerator();
            var unit = new CodeCompileUnit();
            var ns = new CodeNamespace(GetType().Namespace);
            ns.Imports.Add(new CodeNamespaceImport(GetType().Namespace));

            var rules = tree.Root.ChildNodes[0].ChildNodes;
            var count = 1;
            foreach (var rule in rules)
            {
                ns.Types.Add(generator.GenerateRuleClass("Rule" + count, rule));
                count++;
            }

            unit.Namespaces.Add(ns);
            return unit;
        }
    }
}