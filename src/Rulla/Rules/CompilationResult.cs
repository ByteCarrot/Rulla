using System.Collections.Generic;

namespace ByteCarrot.Rulla.Rules
{
    public class CompilationResult
    {
        public CompilationResult(List<CompilationError> errors)
        {
            Success = false;
            Errors = errors;
        }

        public CompilationResult(List<IRule> rules)
        {
            Success = true;
            Rules = rules;
        }

        public List<IRule> Rules { get; private set; }

        public List<CompilationError> Errors { get; private set; }

        public bool Success { get; private set; }
    }
}