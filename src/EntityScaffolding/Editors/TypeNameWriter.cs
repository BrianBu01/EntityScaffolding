using System;
using System.Linq;

namespace EntityScaffolding.Editors
{
    public class TypeNameWriter : ITypeNameWriter
    {
        public bool RequiresFullyQualifiedNames { get; }

        public TypeNameWriter(bool requiresFullyQualifiedNames)
        {
            RequiresFullyQualifiedNames = requiresFullyQualifiedNames;
        }

        public string GetTypeName(Type type)
        {
            //Probably not the best way to do this.
            //Could probably cache here, but there isn't a performance issue now
            //But I didn't want to use Roslyn

            var primitiveName = GetTypeNameForPrimitive(type);

            if (!string.IsNullOrEmpty(primitiveName)) return primitiveName;

            var name = RequiresFullyQualifiedNames ? type.FullName : type.Name;

            return !type.IsGenericType ? name : GetTypeNameForGeneric(type, name);
        }

        private static string GetTypeNameForPrimitive(Type type)
        {
            if (type.IsPrimitive)
            {
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Int32: return "int";
                    case TypeCode.Boolean: return "bool";
                    case TypeCode.Double: return "double";
                    case TypeCode.Int16: return "short";
                    case TypeCode.Byte: return "byte";
                    case TypeCode.Char: return "char";
                    case TypeCode.SByte: return "sbyte";
                    case TypeCode.UInt16: return "ushort";
                    case TypeCode.UInt32: return "uint";
                    case TypeCode.Int64: return "long";
                    case TypeCode.UInt64: return "ulong";
                    case TypeCode.Single: return "float";
                }
            }

            if (type == typeof(DateTime)) return "DateTime";
            if (type == typeof(decimal)) return "decimal";
            if (type == typeof(object)) return "object";
            return type == typeof(string) ? "string" : null;
        }

        private string GetTypeNameForGeneric(Type type, string name)
        {
            if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return $"{GetTypeName(type.GetGenericArguments()[0])}?";
            }

            return name?.Substring(0, name.IndexOf('`')) + '<' +
                   string.Join(", ", type.GetGenericArguments().Select(GetTypeName)) + '>';
        }
    }
}
