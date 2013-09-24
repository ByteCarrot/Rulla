
namespace ByteCarrot.Rulla.Rules
{
    public interface IRulesCompiler
    {
        CompilationResult Compile(string rules);
    }
}