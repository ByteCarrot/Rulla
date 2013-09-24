namespace ByteCarrot.Rulla.Rules
{
    public abstract class Rule : IRule
    {
        public string Text { get; protected set; }

        public bool? IgnoreActivity { get; protected set; }

        public bool? IgnoreRequestBody { get; protected set; }

        public bool? IgnoreResponseBody { get; protected set; }

        public bool? IgnoreServerVariables { get; protected set; }
        
        public bool? IgnoreRouteData { get; protected set; }

        public abstract bool Apply(IActivityContext context);

        public const string Default =
@"WHEN * THEN LOG ACTIVITY;

WHEN RESPONSE HEADER 'Content-Type' = '%image/png%' OR
     RESPONSE HEADER 'Content-Type' = '%image/jpg%' OR
     RESPONSE HEADER 'Content-Type' = '%image/gif%' OR
     RESPONSE HEADER 'Content-Type' = '%application/x-javascript%' OR
     RESPONSE HEADER 'Content-Type' = '%text/css%' OR
     URL = '%favicon.ico%'
THEN IGNORE ACTIVITY;

WHEN STATUS CODE < 500 OR REQUEST SIZE > 1048576 OR RESPONSE SIZE > 1048576
THEN IGNORE REQUEST BODY, IGNORE RESPONSE BODY, IGNORE SERVER VARIABLES";
    }
}