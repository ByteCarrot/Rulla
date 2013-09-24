using Irony.Parsing;

namespace ByteCarrot.Rulla.Rules
{
    public class ParseResult
    {
        public ParseResult(string message, int line, int @char)
        {
            Success = false;
            Message = message;
            Line = line;
            Char = @char;
        }

        public ParseResult(ParseTree tree)
        {
            Success = true;
            Tree = tree;
        }

        public ParseTree Tree { get; private set; }

        public string Message { get; private set; }

        public int Line { get; private set; }

        public int Char { get; private set; }

        public bool Success { get; private set; }
    }
}