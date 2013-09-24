using Irony.Parsing;
using System.CodeDom;

namespace ByteCarrot.Rulla.Rules
{
    public interface IRulesCodeGenerator
    {
        CodeCompileUnit GenerateCode<TRule, TModel>(ParseTree tree) where TRule : Rule<TModel>;
    }
}