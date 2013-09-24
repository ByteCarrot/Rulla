using ByteCarrot.Rulla.Rules;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ByteCarrot.Rulla.Tests.Rules
{
    public class RulesCompilerTests : TestFixtureBase
    {
        [Test]
        public void ShouldCompileRuleWithIgnoreRequest()
        {
            var list = Compile(@"WHEN * THEN IGNORE ACTIVITY");

            Assert.That(list.Count, Is.EqualTo(1));
            Assert.That(list[0].IgnoreActivity, Is.True);
        }

        [Test]
        public void ShouldCompileRuleWithSkipRequestBody()
        {
            var list = Compile(@"WHEN * THEN IGNORE REQUEST BODY");

            Assert.That(list.Count, Is.EqualTo(1));
            Assert.That(list[0].IgnoreRequestBody, Is.True);
        }

        [Test]
        public void ShouldCompileRuleWithSkipResponseBody()
        {
            var list = Compile(@"WHEN * THEN IGNORE RESPONSE BODY");

            Assert.That(list.Count, Is.EqualTo(1));
            Assert.That(list[0].IgnoreResponseBody, Is.True);
        }

        [Test]
        public void ShouldCompileRuleWithSkipServerVariables()
        {
            var list = Compile(@"WHEN * THEN IGNORE SERVER VARIABLES");

            Assert.That(list.Count, Is.EqualTo(1));
            Assert.That(list[0].IgnoreServerVariables, Is.True);
        }

        [Test]
        public void ShouldCompileRuleWithMultipleSkips()
        {
            var list = Compile(@"WHEN * THEN IGNORE REQUEST BODY, IGNORE SERVER VARIABLES");

            Assert.That(list.Count, Is.EqualTo(1));
            Assert.That(list[0].IgnoreRequestBody, Is.True);
            Assert.That(list[0].IgnoreServerVariables, Is.True);
        }

        [Test]
        public void CompiledRuleShouldNotTouchNotSpecifiedSettings()
        {
            var list = Compile(@"WHEN * THEN IGNORE REQUEST BODY, IGNORE SERVER VARIABLES");

            Assert.That(list[0].IgnoreActivity, Is.Null);
            Assert.That(list[0].IgnoreResponseBody, Is.Null);
        }

        [Test]
        public void GeneratedRuleShouldBeAbleToHandleEmptyUrl()
        {
            var context = new FakeActivityContext{ Url = String.Empty };
            var list = Compile("WHEN URL = '%something' THEN IGNORE ACTIVITY");
            
            Assert.That(list[0].Apply(context), Is.False);
        }

        [Test]
        public void GeneratedRuleShouldBeAbleToHandleEmptyMachine()
        {
            var context = new FakeActivityContext { Machine = String.Empty };
            var list = Compile("WHEN MACHINE = '%something' THEN IGNORE ACTIVITY");

            Assert.That(list[0].Apply(context), Is.False);
        }

        [Test]
        public void GeneratedRuleShouldBeAbleToHandleEmptyHeaders()
        {
            var context = new FakeActivityContext { RequestHeaders = new NameValueCollection() };
            var list = Compile("WHEN REQUEST HEADER 'Accept-Encoding' = '%something' THEN IGNORE ACTIVITY");

            Assert.That(list[0].Apply(context), Is.False);
        }

        [Test]
        public void ShouldBeAbleToCompileMultipleRules()
        {
            var list = Compile(@"WHEN * THEN IGNORE REQUEST BODY; WHEN URL = 'something' THEN IGNORE ACTIVITY; WHEN * THEN IGNORE SERVER VARIABLES");

            Assert.That(list.Count, Is.EqualTo(3));
        }

        [Test]
        public void ShouldBeAbleToCompileComplexMultilineRules()
        {
            var list = Compile(@"
WHEN    REQUEST HEADER 'h1' = '%one%' AND
        RESPONSE HEADER 'h2' = 'two%' OR (
            REQUEST HEADER 'h3' = '%three' AND
            REQUEST HEADER 'h4' = 'four' OR (
                URL = '%five%' OR
                URL = '%six')) AND
        URL = 'seven%' OR
        URL = 'eight' OR
        MACHINE = 'nine'
THEN    IGNORE REQUEST BODY,
        IGNORE RESPONSE BODY,
        IGNORE SERVER VARIABLES; 

WHEN URL = 'something' OR URL = 'something else' OR URL = 'blah blah'
THEN IGNORE ACTIVITY; 

WHEN * 
THEN IGNORE SERVER VARIABLES
");

            Assert.That(list.Count, Is.EqualTo(3));
        }

        [Test]
        public void ShouldCompileRuleWithStatusCodeConditionEqualTo()
        {
            var list = Compile(@"WHEN STATUS CODE = 200 THEN IGNORE ACTIVITY");
            
            Assert.That(list[0].Apply(new FakeActivityContext { StatusCode = 200 }), Is.True);
            Assert.That(list[0].Apply(new FakeActivityContext { StatusCode = 300 }), Is.False);
        }

        [Test]
        public void ShouldCompileRuleWithStatusCodeConditionNotEqualTo()
        {
            var list = Compile(@"WHEN STATUS CODE != 200 THEN IGNORE ACTIVITY");

            Assert.That(list[0].Apply(new FakeActivityContext { StatusCode = 200 }), Is.False);
            Assert.That(list[0].Apply(new FakeActivityContext { StatusCode = 300 }), Is.True);
        }

        [Test]
        public void ShouldCompileRuleWithStatusCodeConditionGreaterThan()
        {
            var list = Compile(@"WHEN STATUS CODE > 200 THEN IGNORE ACTIVITY");

            Assert.That(list[0].Apply(new FakeActivityContext { StatusCode = 200 }), Is.False);
            Assert.That(list[0].Apply(new FakeActivityContext { StatusCode = 300 }), Is.True);
        }

        [Test]
        public void ShouldCompileRuleWithStatusCodeConditionGreaterThanOrEqualTo()
        {
            var list = Compile(@"WHEN STATUS CODE >= 200 THEN IGNORE ACTIVITY");

            Assert.That(list[0].Apply(new FakeActivityContext { StatusCode = 199 }), Is.False);
            Assert.That(list[0].Apply(new FakeActivityContext { StatusCode = 200 }), Is.True);
            Assert.That(list[0].Apply(new FakeActivityContext { StatusCode = 201 }), Is.True);
        }

        [Test]
        public void ShouldCompileRuleWithStatusCodeConditionLessThan()
        {
            var list = Compile(@"WHEN STATUS CODE < 200 THEN IGNORE ACTIVITY");

            Assert.That(list[0].Apply(new FakeActivityContext { StatusCode = 200 }), Is.False);
            Assert.That(list[0].Apply(new FakeActivityContext { StatusCode = 199 }), Is.True);
        }

        [Test]
        public void ShouldCompileRuleWithStatusCodeConditionLessThanOrEqualTo()
        {
            var list = Compile(@"WHEN STATUS CODE <= 200 THEN IGNORE ACTIVITY");

            Assert.That(list[0].Apply(new FakeActivityContext { StatusCode = 199 }), Is.True);
            Assert.That(list[0].Apply(new FakeActivityContext { StatusCode = 200 }), Is.True);
            Assert.That(list[0].Apply(new FakeActivityContext { StatusCode = 201 }), Is.False);
        }

        [Test]
        public void ShouldCompileRuleWithUrlConditionNotEqualTo()
        {
            var context = new FakeActivityContext { Url = "notsomething" };
            var list = Compile("WHEN URL != 'something' THEN IGNORE ACTIVITY");
            Assert.That(list[0].Apply(context), Is.True);
        }

        [Test]
        public void ShouldCompileRuleWithIgnoreRouteDataSwitchSetToTrue()
        {
            var rule = Compile(@"WHEN * THEN IGNORE ROUTE DATA")[0];
            Assert.That(rule.IgnoreRouteData, Is.True);

            Assert.That(rule.IgnoreActivity, Is.Null);
            Assert.That(rule.IgnoreRequestBody, Is.Null);
            Assert.That(rule.IgnoreResponseBody, Is.Null);
            Assert.That(rule.IgnoreServerVariables, Is.Null);
        }

        [Test]
        public void ShouldCompileRuleWithIgnoreRouteDataSwitchSetToFalse()
        {
            var rule = Compile(@"WHEN * THEN LOG ROUTE DATA")[0];
            Assert.That(rule.IgnoreRouteData, Is.False);

            Assert.That(rule.IgnoreActivity, Is.Null);
            Assert.That(rule.IgnoreRequestBody, Is.Null);
            Assert.That(rule.IgnoreResponseBody, Is.Null);
            Assert.That(rule.IgnoreServerVariables, Is.Null);
        }

        [Test]
        public void ShouldCompileRuleWithRequestSizeConditionEqualTo()
        {
            var list = Compile(@"WHEN REQUEST SIZE = 100 THEN IGNORE ACTIVITY");

            Assert.That(list[0].Apply(new FakeActivityContext { RequestSize = 100 }), Is.True);
            Assert.That(list[0].Apply(new FakeActivityContext { RequestSize = 300 }), Is.False);
        }

        [Test]
        public void ShouldCompileRuleWithRequestSizeConditionNotEqualTo()
        {
            var list = Compile(@"WHEN REQUEST SIZE != 100 THEN IGNORE ACTIVITY");

            Assert.That(list[0].Apply(new FakeActivityContext { RequestSize = 100 }), Is.False);
            Assert.That(list[0].Apply(new FakeActivityContext { RequestSize = 300 }), Is.True);
        }

        [Test]
        public void ShouldCompileRuleWithRequestSizeConditionGreaterThan()
        {
            var list = Compile(@"WHEN REQUEST SIZE > 100 THEN IGNORE ACTIVITY");

            Assert.That(list[0].Apply(new FakeActivityContext { RequestSize = 100 }), Is.False);
            Assert.That(list[0].Apply(new FakeActivityContext { RequestSize = 300 }), Is.True);
        }

        [Test]
        public void ShouldCompileRuleWithRequestSizeConditionGreaterThanOrEqualTo()
        {
            var list = Compile(@"WHEN REQUEST SIZE >= 100 THEN IGNORE ACTIVITY");

            Assert.That(list[0].Apply(new FakeActivityContext { RequestSize = 100 }), Is.True);
            Assert.That(list[0].Apply(new FakeActivityContext { RequestSize = 300 }), Is.True);
            Assert.That(list[0].Apply(new FakeActivityContext { RequestSize = 50 }), Is.False);
        }

        [Test]
        public void ShouldCompileRuleWithRequestSizeConditionLessThan()
        {
            var list = Compile(@"WHEN REQUEST SIZE < 100 THEN IGNORE ACTIVITY");

            Assert.That(list[0].Apply(new FakeActivityContext { RequestSize = 100 }), Is.False);
            Assert.That(list[0].Apply(new FakeActivityContext { RequestSize = 50 }), Is.True);
        }

        [Test]
        public void ShouldCompileRuleWithRequestSizeConditionLessThanOrEqualTo()
        {
            var list = Compile(@"WHEN REQUEST SIZE <= 100 THEN IGNORE ACTIVITY");

            Assert.That(list[0].Apply(new FakeActivityContext { RequestSize = 100 }), Is.True);
            Assert.That(list[0].Apply(new FakeActivityContext { RequestSize = 50 }), Is.True);
            Assert.That(list[0].Apply(new FakeActivityContext { RequestSize = 300 }), Is.False);
        }

        [Test]
        public void ShouldCompileRuleWithResponseSizeConditionEqualTo()
        {
            var list = Compile(@"WHEN RESPONSE SIZE = 100 THEN IGNORE ACTIVITY");

            Assert.That(list[0].Apply(new FakeActivityContext { ResponseSize = 100 }), Is.True);
            Assert.That(list[0].Apply(new FakeActivityContext { ResponseSize = 300 }), Is.False);
        }

        [Test]
        public void ShouldCompileRuleWithResponseSizeConditionNotEqualTo()
        {
            var list = Compile(@"WHEN RESPONSE SIZE != 100 THEN IGNORE ACTIVITY");

            Assert.That(list[0].Apply(new FakeActivityContext { ResponseSize = 100 }), Is.False);
            Assert.That(list[0].Apply(new FakeActivityContext { ResponseSize = 300 }), Is.True);
        }

        [Test]
        public void ShouldCompileRuleWithResponseSizeConditionGreaterThan()
        {
            var list = Compile(@"WHEN RESPONSE SIZE > 100 THEN IGNORE ACTIVITY");

            Assert.That(list[0].Apply(new FakeActivityContext { ResponseSize = 100 }), Is.False);
            Assert.That(list[0].Apply(new FakeActivityContext { ResponseSize = 300 }), Is.True);
        }

        [Test]
        public void ShouldCompileRuleWithResponseSizeConditionGreaterThanOrEqualTo()
        {
            var list = Compile(@"WHEN RESPONSE SIZE >= 100 THEN IGNORE ACTIVITY");

            Assert.That(list[0].Apply(new FakeActivityContext { ResponseSize = 100 }), Is.True);
            Assert.That(list[0].Apply(new FakeActivityContext { ResponseSize = 300 }), Is.True);
            Assert.That(list[0].Apply(new FakeActivityContext { ResponseSize = 50 }), Is.False);
        }

        [Test]
        public void ShouldCompileRuleWithResponseSizeConditionLessThan()
        {
            var list = Compile(@"WHEN RESPONSE SIZE < 100 THEN IGNORE ACTIVITY");

            Assert.That(list[0].Apply(new FakeActivityContext { ResponseSize = 100 }), Is.False);
            Assert.That(list[0].Apply(new FakeActivityContext { ResponseSize = 50 }), Is.True);
        }

        [Test]
        public void ShouldCompileRuleWithResponseSizeConditionLessThanOrEqualTo()
        {
            var list = Compile(@"WHEN RESPONSE SIZE <= 100 THEN IGNORE ACTIVITY");

            Assert.That(list[0].Apply(new FakeActivityContext { ResponseSize = 100 }), Is.True);
            Assert.That(list[0].Apply(new FakeActivityContext { ResponseSize = 50 }), Is.True);
            Assert.That(list[0].Apply(new FakeActivityContext { ResponseSize = 300 }), Is.False);
        }

        private static List<IRule> Compile(string rules)
        {
            var generator = new RulesCodeGenerator();
            var compiler = new RulesCompiler(generator) {DebugMode = true};

            var result = compiler.Compile(rules);
            if (result.Success)
            {
                return result.Rules;
            }

            ShowMessages(result.Errors);
            throw new ApplicationException();
        }

        private static void ShowMessages(List<CompilationError> errors)
        {
            if (errors != null)
            {
                errors.ForEach(x => Console.WriteLine("{0} [{1},{2}]", x.Message, x.Line, x.Column));
            }
        }
    }
}
