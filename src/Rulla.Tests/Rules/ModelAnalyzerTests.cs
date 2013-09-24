using ByteCarrot.Rulla.Rules;
using NUnit.Framework;
using System;

namespace ByteCarrot.Rulla.Tests.Rules
{
    public class ModelAnalyzerTests : TestFixtureBase
    {
        private IModelAnalyzer _analyzer;
        protected override void SetUp()
        {
            _analyzer = new ModelAnalyzer();
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowExceptionIfTypeIsNull()
        {
            _analyzer.Analyze(null);
        }

        [Test]
        public void ShouldReturnErrorIfTypeCannotBeAModel()
        {
            Action<Type> assert = type =>
            {
                var result = _analyzer.Analyze(type);
                Assert.That(!result, type.ToString());
                Assert.That(result.Messages[0], 
                    Is.EqualTo(String.Format("'{0}' does not satisfy model requirements", type.Name)));
            };

            assert(typeof(int));
            assert(typeof(int?));
            assert(typeof(string));
            assert(typeof(DateTime));
            assert(typeof(DateTime?));
        }

        [Test]
        public void ShouldReturnAListOfAllMetadata()
        {
            var result = _analyzer.Analyze(typeof (TestModel));
            Assert.That(result);
            var meta = result.Value;
            Assert.That(meta.Count, Is.EqualTo(2));
            Assert.That(meta["StringProperty"].PropertyType, Is.EqualTo(typeof(string)));
            Assert.That(meta["Int32Property"].PropertyType, Is.EqualTo(typeof(int)));
        }
    }
}
