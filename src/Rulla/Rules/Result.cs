using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ByteCarrot.Rulla.Rules
{
    public class Result<T>
    {
        public ReadOnlyCollection<string> Messages { get; set; }

        public static implicit operator bool(Result<T> result)
        {
            return result.Success;
        }

        public Result(T value)
        {
            Success = true;
            Value = value;
        }

        public Result(params string[] messages)
        {
            Success = false;
            Messages = new List<string>(messages).AsReadOnly();
        }

        protected bool Success { get; private set; }

        public T Value { get; private set; }
    }
}