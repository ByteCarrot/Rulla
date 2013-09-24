using Irony.Parsing;
using System.CodeDom;

namespace ByteCarrot.Rulla.Rules
{
    public interface IRulesCodeGenerator
    {
        CodeCompileUnit GenerateCode(ParseTree tree);
    }
}