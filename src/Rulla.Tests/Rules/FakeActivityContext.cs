using ByteCarrot.Rulla.Rules;
using System;
using System.Collections.Specialized;

namespace ByteCarrot.Rulla.Tests.Rules
{
    public class FakeActivityContext : IActivityContext
    {
        public FakeActivityContext()
        {
            Url = String.Empty;
            RequestHeaders = new NameValueCollection();
            ResponseHeaders = new NameValueCollection();
        }

        public string Url { get; set; }

        public string Machine { get; set; }

        public NameValueCollection RequestHeaders { get; set; }

        public NameValueCollection ResponseHeaders { get; set; }

        public int StatusCode { get; set; }

        public int ResponseSize { get; set; }

        public int RequestSize { get; set; }
    }
}