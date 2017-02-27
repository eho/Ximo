using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Ximo.Validation;

// ReSharper disable SwitchStatementMissingSomeCases

namespace Ximo.Extensions
{
    /// <summary>
    ///     Class containing <see cref="Type" /> utilities.
    /// </summary>
    public static class TypeExtensions
    {
        private static readonly ConcurrentDictionary<Type, object> DefaultValueCache =
            new ConcurrentDictionary<Type, object>();

        /// <summary>
        ///     Determines whether the specified <see cref="Type" /> implements the <see cref="IEnumerable" /> interface.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the specified type implements <see cref="IEnumerable" />; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="type" /> is <c>null</c>.</exception>
        public static bool IsEnumerableType(this Type type)
        {
            Check.NotNull(type, nameof(type));
            return type.Implements(typeof(IEnumerable));
        }

        /// <summary>
        ///     Returns true of the supplied <paramref name="type" /> implements the given interface
        ///     <paramref name="interfaceType" />. If the given
        ///     interface type is a generic type definition this method will use the generic type definition of any implemented
        ///     interfaces
        ///     to determine the result.
        /// </summary>
        /// <param name="interfaceType">The interface type to check for.</param>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the given type implements the specified interface.</returns>
        /// <remarks>
        ///     This method is for interfaces only. Use <seealso cref="Inherits" /> for classes and
        ///     <seealso cref="InheritsOrImplements" /> to check both interfaces and classes.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException"><paramref name="type" /> is <c>null</c>.</exception>
        /// ///
        /// <exception cref="System.ArgumentNullException"><paramref name="interfaceType" /> is <c>null</c>.</exception>
        public static bool Implements(this Type type, Type interfaceType)
        {
            Check.NotNull(type, nameof(type));
            Check.NotNull(interfaceType, nameof(interfaceType));
            if (type == interfaceType)
            {
                return false;
            }

            var interfaces = type.GetTypeInfo().GetInterfaces();
            if (interfaceType.GetTypeInfo().IsGenericTypeDefinition &&
                interfaces.Where(t => t.GetTypeInfo().IsGenericType)
                    .Select(t => t.GetGenericTypeDefinition())
                    .Any(gt => gt == interfaceType))
            {
                return true;
            }
            return interfaceType.GetTypeInfo().IsAssignableFrom(type);
        }

        /// <summary>
        ///     Returns <c>true</c> if the supplied <paramref name="type" /> implements the given interface
        ///     <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The type (interface) to check for.</typeparam>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the given type implements the specified interface.</returns>
        /// <remarks>
        ///     This method is for interfaces only. Use <seealso cref="Inherits" /> for class types and
        ///     <seealso cref="InheritsOrImplements" /> to check both interfaces and classes.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException"><paramref name="type" /> is <c>null</c>.</exception>
        public static bool Implements<T>(this Type type)
        {
            return type.Implements(typeof(T));
        }

        /// <summary>
        ///     Returns <c>true</c> if the supplied <paramref name="type" /> inherits from the given class
        ///     <paramref name="baseType" />.
        /// </summary>
        /// <param name="baseType">The type (class) to check for.</param>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the given type inherits from the specified class.</returns>
        /// <remarks>
        ///     This method is for classes only. Use <seealso cref="Implements" /> for interface types and
        ///     <seealso cref="InheritsOrImplements" /> to check both interfaces and classes.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException"><paramref name="type" /> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="baseType" /> is <c>null</c>.</exception>
        public static bool Inherits(this Type type, Type baseType)
        {
            Check.NotNull(type, nameof(type));
            Check.NotNull(baseType, nameof(baseType));
            if (type == baseType)
            {
                return false;
            }
            var rootType = typeof(object);
            if (baseType == rootType)
            {
                return true;
            }
            while (type != null && type != rootType)
            {
                var typeInfo = type.GetTypeInfo();
                var current = typeInfo.IsGenericType && baseType.GetTypeInfo().IsGenericTypeDefinition
                    ? type.GetGenericTypeDefinition()
                    : type;
                if (baseType == current)
                {
                    return true;
                }
                type = typeInfo.BaseType;
            }
            return false;
        }

        /// <summary>
        ///     Returns <c>true</c> if the supplied <paramref name="type" /> inherits from the given class
        ///     <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The type (class) to check for.</typeparam>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the given type inherits from the specified class.</returns>
        /// <remarks>
        ///     This method is for classes only. Use <seealso cref="Implements" /> for interface types and
        ///     <seealso cref="InheritsOrImplements" /> to check both interfaces and classes.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException"><paramref name="type" /> is <c>null</c>.</exception>
        public static bool Inherits<T>(this Type type)
        {
            return type.Inherits(typeof(T));
        }

        /// <summary>
        ///     Returns <c>true</c> of the supplied <paramref name="type" /> inherits from or implements the type
        ///     <paramref name="baseType" />.
        /// </summary>
        /// <param name="baseType">The base type to check for.</param>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the given type inherits from or implements the specified base type.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="type" /> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="baseType" /> is <c>null</c>.</exception>
        public static bool InheritsOrImplements(this Type type, Type baseType)
        {
            Check.NotNull(type, nameof(type));
            Check.NotNull(baseType, nameof(baseType));

            return baseType.GetTypeInfo().IsInterface ? type.Implements(baseType) : type.Inherits(baseType);
        }

        /// <summary>
        ///     Returns <c>true</c> if the supplied <paramref name="type" /> inherits from or implements the type
        ///     <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The base type to check for.</typeparam>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the given type inherits from or implements the specified base type.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="type" /> is <c>null</c>.</exception>
        public static bool InheritsOrImplements<T>(this Type type)
        {
            return type.InheritsOrImplements(typeof(T));
        }

        /// <summary>
        ///     Determines whether the specified type is string.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the specified type is string; otherwise, <c>false</c>.</returns>
        public static bool IsString(this Type type)
        {
            return type == typeof(string);
        }

        /// <summary>
        ///     Determines whether the specified type is DateTime.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if specified type is DateTime; otherwise, <c>false</c>.</returns>
        public static bool IsDateTime(this Type type)
        {
            return type == typeof(DateTime) || type == typeof(DateTime?);
        }

        /// <summary>
        ///     Returns the default value for a specified <see cref="Type" />.
        /// </summary>
        /// <param name="type">The <see cref="Type" /> to return the default value for.</param>
        /// <returns>The default value.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="type" /> is <c>null</c>.</exception>
        public static object DefaultValue(this Type type)
        {
            Check.NotNull(type, nameof(type));

            if (!type.GetTypeInfo().IsValueType)
            {
                return null;
            }

            if (!DefaultValueCache.ContainsKey(type))
            {
                DefaultValueCache[type] = Activator.CreateInstance(type);
            }

            return DefaultValueCache[type];
        }

        /// <summary>
        ///     Determines whether the specified type is a dynamic object.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the specified type is a dynamic object; otherwise, <c>false</c>.</returns>
        internal static bool IsDynamicObject(this Type type)
        {
            return type == typeof(object) || type.IsCompatibleWith(typeof(IDynamicMetaObjectProvider));
        }

        /// <summary>
        ///     Determines whether the specified type is nullable.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the specified type is nullable; otherwise, <c>false</c>.</returns>
        public static bool IsNullable(this Type type)
        {
            Check.NotNull(type, nameof(type));

            var typeInfo = type.GetTypeInfo();
            if (!typeInfo.IsValueType ||
                typeInfo.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return true;
            }
            return false;
        }

        internal static Type GetNonNullableType(this Type type)
        {
            return !type.IsNullable() ? type : type.GetTypeInfo().GetGenericArguments()[0];
        }

        internal static int GetNumericTypeKind(this Type type)
        {
            if (type != null)
            {
                type = type.GetNonNullableType();
                if (type.GetTypeInfo().IsEnum)
                {
                    return 0;
                }
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Char:
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                        return 1;

                    case TypeCode.SByte:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                        return 2;

                    case TypeCode.Byte:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                        return 3;
                }
            }
            return 0;
        }

        internal static bool IsCompatibleWith(this Type source, Type target)
        {
            if (source == target)
            {
                return true;
            }
            var typeInfo = target.GetTypeInfo();
            if (!typeInfo.IsValueType)
            {
                return typeInfo.IsAssignableFrom(source);
            }
            var nonNullableType = source.GetNonNullableType();
            var type = target.GetNonNullableType();
            if (nonNullableType == source || type != target)
            {
                var code = nonNullableType.GetTypeInfo().IsEnum ? TypeCode.Object : Type.GetTypeCode(nonNullableType);
                var code2 = type.GetTypeInfo().IsEnum ? TypeCode.Object : Type.GetTypeCode(type);
                switch (code)
                {
                    case TypeCode.SByte:
                        switch (code2)
                        {
                            case TypeCode.SByte:
                            case TypeCode.Int16:
                            case TypeCode.Int32:
                            case TypeCode.Int64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                return true;
                        }
                        break;
                    case TypeCode.Byte:
                        switch (code2)
                        {
                            case TypeCode.Byte:
                            case TypeCode.Int16:
                            case TypeCode.UInt16:
                            case TypeCode.Int32:
                            case TypeCode.UInt32:
                            case TypeCode.Int64:
                            case TypeCode.UInt64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                return true;
                        }
                        break;

                    case TypeCode.Int16:
                        switch (code2)
                        {
                            case TypeCode.Int16:
                            case TypeCode.Int32:
                            case TypeCode.Int64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                return true;
                        }
                        break;

                    case TypeCode.UInt16:
                        switch (code2)
                        {
                            case TypeCode.UInt16:
                            case TypeCode.Int32:
                            case TypeCode.UInt32:
                            case TypeCode.Int64:
                            case TypeCode.UInt64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                return true;
                        }
                        break;

                    case TypeCode.Int32:
                        switch (code2)
                        {
                            case TypeCode.Int32:
                            case TypeCode.Int64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                return true;
                        }
                        break;

                    case TypeCode.UInt32:
                        switch (code2)
                        {
                            case TypeCode.UInt32:
                            case TypeCode.Int64:
                            case TypeCode.UInt64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                return true;
                        }
                        break;

                    case TypeCode.Int64:
                        switch (code2)
                        {
                            case TypeCode.Int64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                return true;
                        }
                        break;

                    case TypeCode.UInt64:
                        switch (code2)
                        {
                            case TypeCode.UInt64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                return true;
                        }
                        break;

                    case TypeCode.Single:
                        switch (code2)
                        {
                            case TypeCode.Single:
                            case TypeCode.Double:
                                return true;
                        }
                        break;

                    default:
                        if (nonNullableType == type)
                        {
                            return true;
                        }
                        break;
                }
            }
            return false;
        }
    }
}