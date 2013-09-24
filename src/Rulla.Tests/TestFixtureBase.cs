using NUnit.Framework;

namespace ByteCarrot.Rulla.Tests
{
    [TestFixture]
    public abstract class TestFixtureBase
    {
        [SetUp]
        public void SetUpBase()
        {
            SetUp();
        }

        protected virtual void SetUp()
        {
        }

        [TearDown]
        public void TearDownBase()
        {
            TearDown();
        }

        protected virtual void TearDown()
        {
        }

        [TestFixtureSetUp]
        public void TestFixtureSetUpBase()
        {
            TestFixtureSetUp();
        }

        protected virtual void TestFixtureSetUp()
        {
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDownBase()
        {
            TestFixtureTearDown();
        }

        protected virtual void TestFixtureTearDown()
        {
        }
    }
}