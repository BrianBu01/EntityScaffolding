using System;
using System.Linq;
using System.Reflection;
using EntityScaffolding.Matchers;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityScaffolding.DefaultConventions.Matchers
{
    public class KeyLocationChangedWarning
    {
        public static void DetectKeyLocationChanges(IEntityType entityType, string @namespace)
        {
            var existingType = ExistingEntityFinder.FindExistingType(entityType, @namespace);

            if (existingType == null) return;

            var previousKeys = (from property in existingType.GetProperties()
                let attribute = property.GetCustomAttribute<PrimaryKeyAttribute>()
                where attribute != null
                select new
                {
                    property.Name,
                    property.PropertyType,
                    attribute.Index
                }).OrderBy(x=>x.Index).ToArray();

            var newKeys = entityType.FindPrimaryKey()?.Properties;

            if (newKeys == null || previousKeys.Length != newKeys.Count)
            {
                //New Length will get compile errors with FindEntity
                return;
            }

            for (var index = 0; index < newKeys.Count; index++)
            {
                var newKey = newKeys[index];
                var previousKey = previousKeys[index];
                if (newKey.ClrType == previousKey.PropertyType && newKey.Name != previousKey.Name) //TODO implicit cast
                {
                    Console.WriteLine(
                        $"EntityScaffolding: {entityType.Name} Primary Key index {index} was changed from {previousKey.Name} to {newKey.Name}.");
                }

            }
        }
    }
}