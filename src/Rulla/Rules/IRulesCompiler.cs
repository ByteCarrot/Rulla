
namespace ByteCarrot.Rulla.Rules
{
    public interface IRulesCompiler<TRule, TModel> where TRule : Rule<TModel>
    {
        CompilationResult<TRule, TModel> Compile(string rules);
    }
}