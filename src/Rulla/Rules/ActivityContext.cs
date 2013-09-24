using System.Collections.Specialized;

namespace ByteCarrot.Rulla.Rules
{
    public class ActivityContext : IActivityContext
    {
        public string Url
        {
            get { return null; }
        }

        public string Machine
        {
            get { return null; }
        }

        public int StatusCode
        {
            get { return 0; }
        }

        public int RequestSize
        {
            get { return 0; }
        }

        public int ResponseSize
        {
            get { return 0; }
        }

        public NameValueCollection RequestHeaders
        {
            get { return null; }
        }

        public NameValueCollection ResponseHeaders
        {
            get { return null; }
        }
    }
}