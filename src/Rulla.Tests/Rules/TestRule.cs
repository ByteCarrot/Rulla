using ByteCarrot.Rulla.Rules;

namespace ByteCarrot.Rulla.Tests.Rules
{
    public abstract class TestRule : Rule<TestModel>
    {
        public bool? IgnoreActivity { get; protected set; }

        public bool? IgnoreRequestBody { get; protected set; }

        public bool? IgnoreResponseBody { get; protected set; }

        public bool? IgnoreServerVariables { get; protected set; }

        public bool? IgnoreRouteData { get; protected set; }
    }
}