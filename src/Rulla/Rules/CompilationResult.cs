using System.Collections.Generic;

namespace ByteCarrot.Rulla.Rules
{
    public class CompilationResult<TRule, TModel> where TRule : Rule<TModel>
    {
        public CompilationResult(List<CompilationError> errors)
        {
            Success = false;
            Errors = errors;
        }

        public CompilationResult(List<TRule> rules)
        {
            Success = true;
            Rules = rules;
        }

        public List<TRule> Rules { get; private set; }

        public List<CompilationError> Errors { get; private set; }

        public bool Success { get; private set; }
    }
}