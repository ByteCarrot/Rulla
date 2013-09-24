using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ByteCarrot.Rulla.Rules
{
    public class ModelAnalyzer : IModelAnalyzer
    {
        public Result<Dictionary<string, PropertyInfo>> Analyze(Type t)
        {
            if (t == null)
            {
                throw new ArgumentNullException();
            }

            if (!t.IsClass || t == typeof(String))
            {
                return new Result<Dictionary<string, PropertyInfo>>(
                    String.Format("'{0}' does not satisfy model requirements", t.Name));
            }

            var meta = t.GetProperties()
                .Where(mi => mi.GetGetMethod() != null && (mi.PropertyType == typeof(int) || mi.PropertyType == typeof(string)))
                .ToDictionary(mi => mi.Name);

            return new Result<Dictionary<string, PropertyInfo>>(meta);
        }
    }
}