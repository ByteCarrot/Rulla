using System.Collections.Specialized;

namespace ByteCarrot.Rulla.Rules
{
    public interface IActivityContext
    {
        string Url { get; }

        string Machine { get; }

        int StatusCode { get; }

        int RequestSize { get; }

        int ResponseSize { get; }

        NameValueCollection RequestHeaders { get; }

        NameValueCollection ResponseHeaders { get; }
    }
}