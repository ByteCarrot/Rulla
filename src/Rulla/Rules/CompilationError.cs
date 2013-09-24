using Irony;
using System.CodeDom.Compiler;

namespace ByteCarrot.Rulla.Rules
{
    public class CompilationError
    {
        public CompilationError(LogMessage message)
        {
            Message = message.Message;
            Line = message.Location.Line;
            Column = message.Location.Column;
        }

        public CompilationError(CompilerError error)
        {
            Message = error.ErrorText;
            Line = error.Line;
            Column = error.Column;
        }

        public string Message { get; private set; }

        public int Line { get; private set; }

        public int Column { get; private set; }   
    }
}