using ByteCarrot.Rulla.Rules;
using Irony.Parsing;
using NUnit.Framework;
using System;

namespace ByteCarrot.Rulla.Tests.Rules
{
    public class RulesGrammarTests : TestFixtureBase
    {
        [Test]
        public void ShouldParseStarOperator()
        {
            AssertParsed(@"WHEN * THEN IGNORE ACTIVITY");
        }

        [Test]
        public void ShouldParseUrlIsEqualTo()
        {
            AssertParsed(@"WHEN URL = 'something' THEN IGNORE ACTIVITY");
        }

        [Test]
        public void ShouldParseUrlIsNotEqualTo()
        {
            AssertParsed(@"WHEN URL != 'something' THEN IGNORE ACTIVITY");
        }

        [Test]
        public void ShouldParseStatusCodeIsEqualTo()
        {
            AssertParsed(@"WHEN STATUS CODE = 100 THEN IGNORE ACTIVITY");
        }

        [Test]
        public void ShouldParseStatusCodeIsNotEqualTo()
        {
            AssertParsed(@"WHEN STATUS CODE != 100 THEN IGNORE ACTIVITY");
        }

        [Test]
        public void ShouldParseStatusCodeIsGreaterThan()
        {
            AssertParsed(@"WHEN STATUS CODE > 100 THEN IGNORE ACTIVITY");
        }

        [Test]
        public void ShouldParseStatusCodeIsGreaterThanOrEqualTo()
        {
            AssertParsed(@"WHEN STATUS CODE >= 100 THEN IGNORE ACTIVITY");
        }

        [Test]
        public void ShouldParseStatusCodeIsLessThan()
        {
            AssertParsed(@"WHEN STATUS CODE < 100 THEN IGNORE ACTIVITY");
        }

        [Test]
        public void ShouldParseStatusCodeIsLessThanOrEqualTo()
        {
            AssertParsed(@"WHEN STATUS CODE <= 100 THEN IGNORE ACTIVITY");
        }

        [Test]
        public void ShouldAcceptOnlyIntegerStatusCodes()
        {
            AssertError(@"WHEN STATUS CODE <= 100.10 THEN IGNORE ACTIVITY");
            AssertError(@"WHEN STATUS CODE <= 100,10 THEN IGNORE ACTIVITY");
            AssertError(@"WHEN STATUS CODE = 'ala ma kota' THEN IGNORE ACTIVITY");
        }

        [Test]
        public void ShouldParseRequestSizeIsEqualTo()
        {
            AssertParsed(@"WHEN REQUEST SIZE = 100 THEN IGNORE ACTIVITY");
        }

        [Test]
        public void ShouldParseResponseSizeIsEqualTo()
        {
            AssertParsed(@"WHEN RESPONSE SIZE = 100 THEN IGNORE ACTIVITY");
        }

        [Test]
        public void ShouldParseRequestSizeIsNotEqualTo()
        {
            AssertParsed(@"WHEN REQUEST SIZE != 100 THEN IGNORE ACTIVITY");
        }

        [Test]
        public void ShouldParseResponseSizeIsNotEqualTo()
        {
            AssertParsed(@"WHEN RESPONSE SIZE != 100 THEN IGNORE ACTIVITY");
        }

        [Test]
        public void ShouldParseRequestSizeIsGreaterThan()
        {
            AssertParsed(@"WHEN REQUEST SIZE > 100 THEN IGNORE ACTIVITY");
        }

        [Test]
        public void ShouldParseResponseSizeIsGreaterThan()
        {
            AssertParsed(@"WHEN RESPONSE SIZE > 100 THEN IGNORE ACTIVITY");
        }

        [Test]
        public void ShouldParseRequestSizeIsGreaterThanOrEqualTo()
        {
            AssertParsed(@"WHEN REQUEST SIZE >= 100 THEN IGNORE ACTIVITY");
        }

        [Test]
        public void ShouldParseResponseSizeIsGreaterThanOrEqualTo()
        {
            AssertParsed(@"WHEN RESPONSE SIZE >= 100 THEN IGNORE ACTIVITY");
        }

        [Test]
        public void ShouldParseRequestSizeIsLessThan()
        {
            AssertParsed(@"WHEN REQUEST SIZE < 100 THEN IGNORE ACTIVITY");
        }

        [Test]
        public void ShouldParseResponseSizeIsLessThan()
        {
            AssertParsed(@"WHEN RESPONSE SIZE < 100 THEN IGNORE ACTIVITY");
        }

        [Test]
        public void ShouldParseRequestSizeIsLessThanOrEqualTo()
        {
            AssertParsed(@"WHEN REQUEST SIZE <= 100 THEN IGNORE ACTIVITY");
        }

        [Test]
        public void ShouldParseResponseSizeIsLessThanEqualTo()
        {
            AssertParsed(@"WHEN RESPONSE SIZE <= 100 THEN IGNORE ACTIVITY");
        }

        [Test]
        public void ShouldAcceptOnlyIntegerRequestAndResponseSize()
        {
            AssertError(@"WHEN REQUEST SIZE <= 100.10 THEN IGNORE ACTIVITY");
            AssertError(@"WHEN REQUEST SIZE <= 100,10 THEN IGNORE ACTIVITY");
            AssertError(@"WHEN REQUEST SIZE = 'ala ma kota' THEN IGNORE ACTIVITY");

            AssertError(@"WHEN RESPONSE SIZE <= 100.10 THEN IGNORE ACTIVITY");
            AssertError(@"WHEN RESPONSE SIZE <= 100,10 THEN IGNORE ACTIVITY");
            AssertError(@"WHEN RESPONSE SIZE = 'ala ma kota' THEN IGNORE ACTIVITY");
        }

        [Test]
        public void ShouldParseHeaderIsEqualTo()
        {
            AssertParsed(@"WHEN REQUEST HEADER 'Accept-Encoding' = 'something' THEN IGNORE ACTIVITY");
        }

        [Test]
        public void ShouldParseHeaderIsNotEqualTo()
        {
            AssertParsed(@"WHEN REQUEST HEADER 'Accept-Encoding' != 'something' THEN IGNORE ACTIVITY");
        }

        [Test]
        public void ShouldParseWhenUrlValueHasNotSingleQuotes()
        {
            AssertError("WHEN URL = something THEN");
        }

        [Test]
        public void ShouldParseAndLogicalOperation()
        {
            AssertParsed(@"WHEN URL != 'something' AND URL = 'something else' THEN IGNORE ACTIVITY");
        }

        [Test]
        public void ShouldParseOrLogicalOperation()
        {
            AssertParsed(@"WHEN URL != 'something' OR URL = 'something else' THEN IGNORE ACTIVITY");
        }

        [Test]
        public void ShouldParseParentheses()
        {
            AssertParsed(@"WHEN URL != 'something' OR (URL = 'something else' AND REQUEST HEADER 'Accept-Encoding' = 'some value') THEN IGNORE ACTIVITY");
        }

        [Test]
        public void ShouldParseMultipleRules()
        {
            AssertParsed(@"WHEN * THEN IGNORE ACTIVITY; WHEN URL = 'something' THEN IGNORE ACTIVITY");
        }

        [Test]
        public void ShouldParseMultipleThenElements()
        {
            AssertParsed(@"WHEN * THEN IGNORE REQUEST BODY, IGNORE RESPONSE BODY");
        }

        [Test]
        public void ShouldDistinguishRequestAndResponseHeaders()
        {
            AssertParsed(@"WHEN REQUEST HEADER 'Content-Type' = 'something' THEN IGNORE ACTIVITY");
            AssertParsed(@"WHEN RESPONSE HEADER 'Content-Type' = 'something' THEN IGNORE ACTIVITY");
        }

        [Test]
        public void ShouldParseIgnoreRouteDataSwitch()
        {
            AssertParsed(@"WHEN * THEN IGNORE ROUTE DATA");
            AssertParsed(@"WHEN * THEN IGNORE REQUEST BODY, IGNORE ROUTE DATA, IGNORE SERVER VARIABLES");
        }

        private void AssertError(string rules)
        {
            var tree = RulesGrammar.Parse(rules.Trim());
            ShowMessages(tree);
            Assert.That(tree.Status, Is.EqualTo(ParseTreeStatus.Error));
        }

        private void AssertParsed(string rules)
        {
            var tree = RulesGrammar.Parse(rules.Trim());
            ShowMessages(tree);
            Assert.That(tree.Status, Is.EqualTo(ParseTreeStatus.Parsed));
        }

        private void ShowMessages(ParseTree tree)
        {
            if (tree != null && tree.ParserMessages != null)
            {
                tree.ParserMessages.ForEach(x => Console.WriteLine("{0} {1}", x.Message, x.Location));
            }
        }
    }
}
