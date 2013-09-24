using System;
using System.Collections.Generic;
using System.Reflection;

namespace ByteCarrot.Rulla.Rules
{
    public interface IModelAnalyzer
    {
        Result<Dictionary<string, PropertyInfo>> Analyze(Type t);
    }
}