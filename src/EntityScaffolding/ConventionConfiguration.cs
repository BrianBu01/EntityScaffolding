using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EntityScaffolding.Editors;
using EntityScaffolding.Elements;
using EntityScaffolding.Matchers;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityScaffolding
{
    public class ConventionConfiguration : IConventionConfiguration
    {
        private static List<IConventionMatcher<IWritableElement>> _conventionMatchers;
        private static WeakReference<List<Type>> _allTypes;

        public IEnumerable<IConventionMatcher<IWritableElement>> ConventionMatchers
        {
            get { return _conventionMatchers = _conventionMatchers ?? LoadConventions(); }
        }

        public IReadOnlyList<IEntityEditor> EntityEditors =>
            new IEntityEditor[]
            {
                new NamespaceEditor(),
                new InterfaceEditor(),
                new PropertyAttributeEditor()
            };

        private static IEnumerable<Type> LoadedTypes()
        {
            if (_allTypes != null && _allTypes.TryGetTarget(out var results)) return results;

            var types = new List<Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    types.AddRange(assembly.GetTypes());
                }
                catch (ReflectionTypeLoadException e)
                {
                    Console.WriteLine($"Could not load assembly {assembly.GetName()}. {e.Message}");
                }
            }
            _allTypes = new WeakReference<List<Type>>(types);
            return types;
        }
        
        private static List<IConventionMatcher<IWritableElement>> LoadConventions()
        {
            //Interfaces that have attribute [EntityConvention] wrapped by MatchingInterfaceConvention class
            var conventions = LoadedTypes()
                .Where(x => x.GetCustomAttribute(typeof(EntityConventionAttribute)) != null)
                .Select(x => new MatchingInterfaceConventionFinder(x))
                .ToList<IConventionMatcher<IWritableElement>>();

            //Classes that implement IConventionMatcher
            conventions.AddRange(LoadedTypes()
                .Where(x => x.GetInterfaces().
                    Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConventionMatcher<>)))
                .Where(x => x.IsPublic && !x.IsInterface)
                .Select(Activator.CreateInstance)
                .Cast<IConventionMatcher<IWritableElement>>());

            return conventions;
        }


        /// <summary>
        ///     private sub-class to avoid this wrapper class being picked up by IEntityConvention search.
        /// </summary>
        private sealed class MatchingInterfaceConventionFinder : IInterfaceConventionMatcher
        {
            private readonly Type _convention;

            public MatchingInterfaceConventionFinder(Type convention)
            {
                _convention = convention;
            }
            
            public IEnumerable<InterfaceElement> GetMatchingElements(IEntityType entityType, string @namespace)
            {
                if (_convention.GetProperties().All(p => entityType.GetProperties().Any(x =>
                    x.Name == p.Name &&
                    x.ClrType.FullName == p.PropertyType.FullName)))  //entityType.GetProperties[0].PropertyInfo is coming up null;  Can't check read/write
                {
                    yield return new InterfaceElement(_convention);
                }
            }
        }
    }
}