namespace ByteCarrot.Rulla.Rules
{
    public interface IRulesParser
    {
        ParseResult Parse(string rules);
    }
}