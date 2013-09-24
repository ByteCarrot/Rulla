using Irony.Parsing;

namespace ByteCarrot.Rulla.Rules
{
    public class RulesGrammar : Grammar
    {
        public RulesGrammar()
        {
            var rule = _("Rule", WhenGrammar() + ThenGrammar());
            Root = _("Root", MakeListRule(_("Rules"), ToTerm(";"), rule, TermListOptions.StarList));
        }

        private NonTerminal WhenGrammar()
        {
            var starOperator = _("StarOperator", ToTerm("*"));
            var stringOperator = _("StringOperator", ToTerm("=") | "!=");
            var numberOperator = _("NumberOperator", ToTerm("=") | "!=" | ">" | ">=" | "<" | "<=");

            var headerName = new StringLiteral("Header ExpressionExtensions", "'", StringOptions.NoEscapes);
            var headerValue = new StringLiteral("Header Value", "'", StringOptions.NoEscapes);
            var headerCondition = _("HeaderCondition", ToTerm("HEADER") + headerName + stringOperator + headerValue);
            var requestHeaderCondition = _("RequestHeaderCondition", ToTerm("REQUEST") + headerCondition);
            var responseHeaderCondition = _("ResponseHeaderCondition", ToTerm("RESPONSE") + headerCondition);

            var statusCodeValue = new NumberLiteral("Status Code Value", NumberOptions.IntOnly);
            var statusCodeCondition = _("StatusCodeCondition", ToTerm("STATUS") + "CODE" + numberOperator + statusCodeValue);

            var requestSizeValue = new NumberLiteral("Request Size Value", NumberOptions.IntOnly);
            var requestSizeCondition = _("RequestSizeCondition", ToTerm("REQUEST") + "SIZE" + numberOperator + requestSizeValue);

            var responseSizeValue = new NumberLiteral("Response Size Value", NumberOptions.IntOnly);
            var responseSizeCondition = _("ResponseSizeCondition", ToTerm("RESPONSE") + "SIZE" + numberOperator + responseSizeValue);

            var urlValue = new StringLiteral("Url Value", "'", StringOptions.NoEscapes);
            var urlCondition = _("UrlCondition", ToTerm("URL") + stringOperator + urlValue);

            var machineValue = new StringLiteral("Machine Value", "'", StringOptions.NoEscapes);
            var machineCondition = _("MachineCondition", ToTerm("MACHINE") + stringOperator + machineValue);

            var expression = _("Expression");

            var logicalOperator = _("LogicalOperator", ToTerm("AND") | ToTerm("OR"));
            var logicalOperation = _("LogicalOperation", expression + logicalOperator + expression);
            var parentheses = _("Parentheses", "(" + logicalOperation + ")");

            _(expression, responseSizeCondition | requestSizeCondition | statusCodeCondition | requestHeaderCondition | responseHeaderCondition | urlCondition | machineCondition | logicalOperation | parentheses);

            return _("When", ToTerm("WHEN") + expression | ToTerm("WHEN") + starOperator);
        }

        private NonTerminal ThenGrammar()
        {
            var ignoreRequestBody = _("IgnoreRequestBody", ToTerm("IGNORE") + "REQUEST" + "BODY");
            var ignoreResponseBody = _("IgnoreResponseBody", ToTerm("IGNORE") + "RESPONSE" + "BODY");
            var ignoreServerVariables = _("IgnoreServerVariables", ToTerm("IGNORE") + "SERVER" + "VARIABLES");
            var ignoreRouteData = _("IgnoreRouteData", ToTerm("IGNORE") + "ROUTE" + "DATA");
            var ignoreActivity = _("IgnoreActivity", ToTerm("IGNORE") + "ACTIVITY");

            var logRequestBody = _("LogRequestBody", ToTerm("LOG") + "REQUEST" + "BODY");
            var logResponseBody = _("LogResponseBody", ToTerm("LOG") + "RESPONSE" + "BODY");
            var logServerVariables = _("LogServerVariables", ToTerm("LOG") + "SERVER" + "VARIABLES");
            var logRouteData = _("LogRouteData", ToTerm("LOG") + "ROUTE" + "DATA");
            var logActivity = _("LogActivity", ToTerm("LOG") + "ACTIVITY");

            var @switch = _("Switch", ignoreRequestBody | ignoreResponseBody | ignoreServerVariables | ignoreRouteData | logRequestBody | logResponseBody | logServerVariables | logRouteData);
            var switches = _("Switches", MakeListRule(_("SwitchesList"), ToTerm(","), @switch, TermListOptions.StarList) | ignoreActivity | logActivity);

            return _("Then", ToTerm("THEN") + switches);
        }

        private NonTerminal _(string name, BnfExpression rule)
        {
            return new NonTerminal(name) {Rule = rule};
        }

        private void _(NonTerminal nonTerminal, BnfExpression rule)
        {
            nonTerminal.Rule = rule;
        }

        private NonTerminal _(string name)
        {
            return new NonTerminal(name);
        }

        public static ParseTree Parse(string rules)
        {
            return new Parser(new LanguageData(new RulesGrammar())).Parse(rules);
        }
    }
}
