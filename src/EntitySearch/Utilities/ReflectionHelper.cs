using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EntitySearch.Utilities
{
    internal static class ReflectionHelper
    {
        internal static IList<PropertyInfo> GetPropertiesFromType(IList<PropertyInfo> searchableProperties, IList<string> queryProperties = null)
        {
            searchableProperties = searchableProperties.Where(x => queryProperties == null || (queryProperties != null && queryProperties.Count == 0) || queryProperties.Any(y => y.ToLower() == x.Name.ToLower())).ToList();

            if (searchableProperties == null || searchableProperties.Count == 0)
            {
                throw new Exception("There's no searchable property found!");
            }

            return searchableProperties;
        }
        internal static MethodInfo GetMethodFromType(Type type, string methodName, int parameters, int genericArguments, List<Type> parameterTypes = null)
        {
            return type.GetMethods()
                .SingleOrDefault(method =>
                    method.Name == methodName
                    && method.GetParameters().Count() == parameters
                    && method.GetGenericArguments().Count() == genericArguments
                    && (parameterTypes == null || parameterTypes.All(x => method.GetParameters().Select(parameter => parameter.ParameterType).Contains(x)))
                );
        }
    }
}
