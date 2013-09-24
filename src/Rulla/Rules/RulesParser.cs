using Irony.Parsing;

namespace ByteCarrot.Rulla.Rules
{
    public class RulesParser : IRulesParser
    {
        public ParseResult Parse(string rules)
        {
            var tree = RulesGrammar.Parse(rules);
            if (tree.Status == ParseTreeStatus.Parsed)
            {
                return new ParseResult(tree);
            }

            var message = tree.ParserMessages[0];
            return new ParseResult(message.Message, message.Location.Line, message.Location.Column);
        }
    }
}