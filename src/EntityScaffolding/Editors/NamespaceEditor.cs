using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using EntityScaffolding.Elements;

namespace EntityScaffolding.Editors
{
    public class NamespaceEditor : EntityEditor<IWritableElement>
    {
        /*
         * This doesn't account for the types within the Generics.
         * I don't feel like doing it yet.
         *
         */

        private bool _requiresFullyQualifiedNames;

        private IEnumerable<Type> AllTypes => AllElements.SelectMany(x => x.UsedTypes);

        protected override string EditEntityBase(string entitySource)
        {
            //If two types have the same name in different namespaces, then use fully qualified names
            _requiresFullyQualifiedNames = AllTypes.GroupBy(x => x.Name)
                .Any(g => g.Select(i => i.Namespace).Distinct().Count() > 1);

            if (!_requiresFullyQualifiedNames)
            {
                entitySource = WriteNameSpaces(entitySource, AllTypes);
            }

            TypeNameWriter = new TypeNameWriter(_requiresFullyQualifiedNames);

            return entitySource;
        }

        private static string WriteNameSpaces(string entitySource, IEnumerable<Type> types)
        {
            var currentNameSpaces = FindCurrentNameSpaces(entitySource);

            var newUsingStrings = types.Select(x => x.Namespace)
                .Distinct()
                .Select(x => $"using {x};")
                .Except(currentNameSpaces).ToList();

            if (!newUsingStrings.Any()) return entitySource;

            var insertPoint = $"{Environment.NewLine}namespace";

            var newNameSpaces = $"{string.Join(Environment.NewLine, newUsingStrings)}{Environment.NewLine}";

            return entitySource.Replace(insertPoint, newNameSpaces + insertPoint);
        }


        private static IEnumerable<string> FindCurrentNameSpaces(string entitySource)
        {
            //Finding namespaces from using statements
            var usingRegex = new Regex("using .*;");
            var currentNameSpaces = usingRegex.Matches(entitySource).Select(x => x.Value).ToList();

            //Finding parent namespaces from current namespace
            var namespaceRegex = new Regex($"namespace .*{Environment.NewLine}");
            var ns = namespaceRegex.Match(entitySource).Value.Substring(10).Trim();
            var currentNamespace = ns.Split('.').ToList();

            currentNameSpaces.AddRange(currentNamespace.Select((t, i) =>
                $"using {string.Join('.', currentNamespace.Take(i + 1))};"));
            return currentNameSpaces;
        }
    }
}