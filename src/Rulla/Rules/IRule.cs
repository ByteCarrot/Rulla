namespace ByteCarrot.Rulla.Rules
{
    public interface IRule
    {
        string Text { get; }

        bool? IgnoreActivity { get; }

        bool? IgnoreRequestBody { get; }

        bool? IgnoreResponseBody { get; }

        bool? IgnoreServerVariables { get; }
        
        bool? IgnoreRouteData { get; }

        bool Apply(IActivityContext context);
    }
}