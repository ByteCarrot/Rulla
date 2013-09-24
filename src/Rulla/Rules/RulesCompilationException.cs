using System;

namespace ByteCarrot.Rulla.Rules
{
    [Serializable]
    public class RulesCompilationException : ApplicationException
    {
        public RulesCompilationException(string message) : base(message)
        {
        }
    }
}