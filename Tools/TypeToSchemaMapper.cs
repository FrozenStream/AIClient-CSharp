using System;
using System.Collections.Generic;

namespace AIExtension.Tools;

/// <summary>
/// Provides mapping from .NET types to JSON Schema types
/// </summary>
public static class TypeToSchemaMapper
{
    private static readonly Dictionary<Type, string> TypeMappings = new Dictionary<Type, string>
    {
        // String types
        { typeof(string), "string" },
        { typeof(char), "string" },
        { typeof(Guid), "string" },
        
        // Integer types
        { typeof(int), "integer" },
        { typeof(long), "integer" },
        { typeof(short), "integer" },
        { typeof(byte), "integer" },
        { typeof(uint), "integer" },
        { typeof(ulong), "integer" },
        { typeof(ushort), "integer" },
        { typeof(sbyte), "integer" },
        
        // Number types
        { typeof(float), "number" },
        { typeof(double), "number" },
        { typeof(decimal), "number" },
        
        // Boolean types
        { typeof(bool), "boolean" },
        
        // Array/List types
        { typeof(Array), "array" },
        { typeof(List<>), "array" },
        { typeof(IEnumerable<>), "array" },
        
        // Object types
        { typeof(object), "object" }
    };

    /// <summary>
    /// Maps a .NET Type to its corresponding JSON Schema type
    /// </summary>
    /// <param name="type">The .NET type to map</param>
    /// <returns>The corresponding JSON Schema type as string</returns>
    public static string GetJsonSchemaType(Type type)
    {
        // Handle nullable types
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            type = Nullable.GetUnderlyingType(type);
        }
        
        // Handle arrays and collections
        if (type.IsArray)
        {
            return "array";
        }
        
        if (type.IsGenericType)
        {
            var genericType = type.GetGenericTypeDefinition();
            if (TypeMappings.ContainsKey(genericType))
            {
                return TypeMappings[genericType];
            }
        }
        
        // Direct mapping
        if (TypeMappings.ContainsKey(type))
        {
            return TypeMappings[type];
        }
        
        // For complex types, default to object
        return "object";
    }
}