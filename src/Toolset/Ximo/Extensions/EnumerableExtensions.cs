using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using Ximo.Validation;

namespace Ximo.Extensions
{
    /// <summary>
    ///     Class containing <see cref="IEnumerable" /> utilities.
    /// </summary>
    [DebuggerStepThrough]
    public static class EnumerableExtensions
    {
        /// <summary>
        ///     Checks if an <see cref="IEnumerable" /> instance has items.
        /// </summary>
        /// <param name="collection">The <see cref="IEnumerable" /> instance to be checked.</param>
        /// <returns><c>true</c> if the <see cref="IEnumerable" /> object is empty.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="collection" /> is null. </exception>
        public static bool IsEmpty([NoEnumeration] this IEnumerable collection)
        {
            Check.NotNull(collection, nameof(collection));

            var enumerator = collection.GetEnumerator();
            return !enumerator.MoveNext();
        }

        /// <summary>
        ///     Checks if an <see cref="IEnumerable" /> instance has no items.
        /// </summary>
        /// <param name="collection">The <see cref="IEnumerable" /> instance to be checked.</param>
        /// <returns><c>true</c> if the <see cref="IEnumerable" /> object is empty.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="collection" /> is null. </exception>
        public static bool IsNotEmpty([NoEnumeration] this IEnumerable collection)
        {
            return !IsEmpty(collection);
        }

        /// <summary>
        ///     Checks if an <see cref="IEnumerable" /> instance is null or has no items.
        /// </summary>
        /// <param name="collection">The <see cref="IEnumerable" /> instance to be checked.</param>
        /// <returns><c>true</c> if the <see cref="IEnumerable" /> object is empty or has no items.</returns>
        public static bool IsNullOrEmpty([NoEnumeration] this IEnumerable collection)
        {
            return collection == null || collection.IsEmpty();
        }

        /// <summary>
        ///     Checks if an <see cref="IEnumerable" /> instance is null or has no items.
        /// </summary>
        /// <param name="collection">The <see cref="IEnumerable" /> instance to be checked.</param>
        /// <returns><c>true</c> if the <see cref="IEnumerable" /> object is empty or has no items.</returns>
        public static bool IsNotNullOrEmpty([NoEnumeration] this IEnumerable collection)
        {
            return !IsNullOrEmpty(collection);
        }

        /// <summary>
        ///     Executes an action passing every item of the collection as a parameter.
        /// </summary>
        /// <typeparam name="T">The type of the item in the list.</typeparam>
        /// <param name="source">The source list instance.</param>
        /// <param name="action">The action to be executed.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     either <paramref name="action" /> or <paramref name="source" /> is <c>null</c>.
        /// </exception>
        public static void ForEach<T>([NoEnumeration] [NotNull] this IEnumerable<T> source, [NotNull] Action<T> action)
        {
            Check.NotNull(source, nameof(source));
            Check.NotNull(action, nameof(action));

            foreach (var element in source)
            {
                action(element);
            }
        }

        /// <summary>
        ///     An IEnumerable&lt;T&gt; extension method that query if '@this' contains all.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="source">The source to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool ContainsAll<T>([NoEnumeration] [NotNull] this IEnumerable<T> source,
            [NotNull] params T[] values)
        {
            Check.NotNull(source, nameof(source));
            Check.NotNull(values, nameof(values));

            var list = source.ToArray();
            return values.All(value => list.Contains(value));
        }
    }
}