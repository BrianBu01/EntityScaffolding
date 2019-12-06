using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityScaffolding.Elements
{
    public class PropertyAttributeElement : IWritableElement
    {
        private List<string> _attributeValues;

        public IProperty Property { get; set; }

        public Type Attribute { get; set; }

        /// <summary>
        /// Each of these are written as is.  For int value of zero you would add a string "0".  For a string with a single character of zero you would add "\"0\"".
        /// </summary>
        public List<string> AttributeValues
        {
            get => _attributeValues = _attributeValues ?? new List<string>();
            set => _attributeValues = value;
        }

        public IEnumerable<Type> UsedTypes => new[] { Attribute };
    }
}