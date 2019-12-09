using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityScaffolding.Matchers
{
    public static class ExistingEntityFinder
    {
        private static WeakReference<Type[]> _typesToSearch;

        private static IEnumerable<Type> TypeToSearch(string @namespace)
        {
            //Assuming only ever one namespace per scaffolding.

            if (_typesToSearch != null && _typesToSearch.TryGetTarget(out var results))
            {
                return results;
            }

            var namespacePrefix = @namespace.Split('.').FirstOrDefault() ?? string.Empty;

            var types = new List<Type>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    if (assembly.GetName().Name.StartsWith(namespacePrefix))
                    {
                        types.AddRange(assembly.GetTypes());
                    }
                }
                catch (ReflectionTypeLoadException e)
                {
                    Console.WriteLine($"EntityScaffolding: Could not load assembly {assembly.GetName()}. {e.Message}");
                }
            }

            results = types.ToArray();

            _typesToSearch = new WeakReference<Type[]>(results);

            return results;

        }

        public static Type FindExistingType(IEntityType entityType, string @namespace)
        {
            var entityTypeName = $"{@namespace}.{entityType.Name}";

            var foundTypes = TypeToSearch(@namespace)
                .Where(x => x.FullName == entityTypeName)
                .ToArray();

            if (!foundTypes.Any()) return null;

            if (foundTypes.Length == 1) return foundTypes.Single();

            Console.WriteLine($"EntityScaffolding Warning: {entityTypeName} was found twice in your domain.");
            return null;

        }
    }
}