using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Ximo.Validation
{
    /// <summary>
    ///     Class providing validation methods for properties.
    /// </summary>
    [DebuggerStepThrough]
    public static class FieldCheck
    {
        /// <summary>
        ///     Enforces that an <see cref="object" /> value is not null.
        /// </summary>
        /// <param name="value">The field value to be checked.</param>
        /// <param name="propertyName">
        ///     The name of the property where the field is being set.
        ///     If the <paramref name="propertyName" /> is <c>null</c> the property name is automatically used.
        /// </param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        public static object NotNull(object value, [CallerMemberName] string propertyName = "")
        {
            return Check.NotNull(value, propertyName);
        }

        /// <summary>
        ///     Enforces that a <see cref="string" /> value is not <c>null</c> or empty.
        /// </summary>
        /// <param name="value">The field value to be checked.</param>
        /// <param name="propertyName">
        ///     The name of the property where the field is being set.
        ///     If the <paramref name="propertyName" /> is <c>null</c> the property name is automatically used.
        /// </param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentException"><paramref name="value" /> is <c>empty</c>.</exception>
        public static string NotNullOrEmpty(string value, [CallerMemberName] string propertyName = "")
        {
            return Check.NotNullOrEmpty(value, propertyName);
        }

        /// <summary>
        ///     Enforces that a <see cref="string" /> value is not <c>null</c>, empty or consists only of whitespace characters.
        /// </summary>
        /// <param name="value">The field value to be checked.</param>
        /// <param name="propertyName">
        ///     The name of the property where the field is being set.
        ///     If the <paramref name="propertyName" /> is <c>null</c> the property name is automatically used.
        /// </param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentException">
        ///     <paramref name="value" /> is <c>empty</c> or consists only of whitespace
        ///     characters.
        /// </exception>
        public static string NotNullOrWhitespace(string value, [CallerMemberName] string propertyName = "")
        {
            return Check.NotNullOrWhitespace(value, propertyName);
        }

        /// <summary>
        ///     Enforces that the length of a <see cref="string" /> value does not exceed a specified maximum
        ///     length.
        /// </summary>
        /// <param name="value">The field value to be checked.</param>
        /// <param name="maximumLength">The maximum length allowed for the <see cref="string" /> value.</param>
        /// <param name="propertyName">
        ///     The name of the property where the field is being set.
        ///     If the <paramref name="propertyName" /> is <c>null</c> the property name is automatically used.
        /// </param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="System.InvalidOperationException">
        ///     The <paramref name="maximumLength" /> parameter is less than or equal to
        ///     0.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        ///     <paramref name="value" /> length exceeds the <paramref name="maximumLength" />
        ///     specified.
        /// </exception>
        public static string MaxLength(string value, int maximumLength, [CallerMemberName] string propertyName = "")
        {
            return Check.MaxLength(value, maximumLength, propertyName);
        }

        /// <summary>
        ///     Enforces that the length of a <see cref="string" /> value has at least a specified minimum
        ///     length.
        /// </summary>
        /// <param name="value">The field value to be checked.</param>
        /// <param name="minimumLength">The minimum length allowed for the <see cref="string" /> value.</param>
        /// <param name="propertyName">
        ///     The name of the property where the field is being set.
        ///     If the <paramref name="propertyName" /> is <c>null</c> the property name is automatically used.
        /// </param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <remarks><c>null</c> values will pass the test.</remarks>
        /// <exception cref="System.InvalidOperationException">The <paramref name="minimumLength" /> value is less than 0.</exception>
        /// <exception cref="System.ArgumentException">
        ///     <paramref name="value" /> length is less than the <paramref name="minimumLength" />
        ///     specified.
        /// </exception>
        public static string MinLength(string value, int minimumLength, [CallerMemberName] string propertyName = "")
        {
            return Check.MinLength(value, minimumLength, propertyName);
        }

        /// <summary>
        ///     Enforces that the length of a <see cref="string" /> value has at least a specified minimum
        ///     length and does not exceed a specified maximum length.
        /// </summary>
        /// <param name="value">The value to be checked.</param>
        /// <param name="minimumLength">The minimum length allowed for the <see cref="string" /> value.</param>
        /// <param name="maximumLength">The maximum length allowed for the <see cref="string" /> value.</param>
        /// <param name="propertyName">
        ///     The name of the property where the field is being set.
        ///     If the <paramref name="propertyName" /> is <c>null</c> the property name is automatically used.
        /// </param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <remarks><c>null</c> values will pass the test.</remarks>
        /// <exception cref="System.InvalidOperationException">
        ///     The <paramref name="minimumLength" /> value is less than 0 or the
        ///     <paramref name="maximumLength" /> value is less than or equal to 0.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        ///     <paramref name="value" /> length is less than the
        ///     <paramref name="minimumLength" /> specified or exceeds the <paramref name="maximumLength" /> specified.
        /// </exception>
        public static string StringLength(string value, int minimumLength, int maximumLength,
            [CallerMemberName] string propertyName = "")
        {
            return Check.StringLength(value, minimumLength, maximumLength, propertyName);
        }

        /// <summary>
        ///     Enforces that a <see cref="string" /> value is a valid email.
        /// </summary>
        /// <param name="value">The value to be checked.</param>
        /// <param name="propertyName">
        ///     The name of the property where the field is being set.
        ///     If the <paramref name="propertyName" /> is <c>null</c> the property name is automatically used.
        /// </param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="System.ArgumentException"><paramref name="value" /> is not a valid email.</exception>
        public static string Email(string value, [CallerMemberName] string propertyName = "")
        {
            return Check.Email(value, propertyName);
        }

        /// <summary>
        ///     Enforces that a value falls within a specified range.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the value to compare.
        /// </typeparam>
        /// <param name="value">
        ///     The value to be checked. Must implement <see cref="System.IComparable" /> or
        ///     <see cref="IComparable{T}" />
        /// </param>
        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        /// <param name="propertyName">
        ///     The name of the property where the field is being set.
        ///     If the <paramref name="propertyName" /> is <c>null</c> the property name is automatically used.
        /// </param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="System.ArgumentException">
        ///     <paramref name="value" /> does not fall within the specified range. It is
        ///     either less than <paramref name="minimum" /> value or larger than <paramref name="maximum" /> value specified.
        /// </exception>
        public static T Range<T>(T value, T minimum, T maximum, [CallerMemberName] string propertyName = "")
            where T : IComparable, IComparable<T>
        {
            return Check.Range(value, minimum, maximum, propertyName);
        }

        /// <summary>
        ///     Enforces that a <see cref="string" /> value matches a specified regular expression pattern.
        /// </summary>
        /// <param name="value">The value to be checked.</param>
        /// <param name="regularExpression">The regular expression used for evaluation of the value.</param>
        /// <param name="propertyName">
        ///     The name of the property where the field is being set.
        ///     If the <paramref name="propertyName" /> is <c>null</c> the property name is automatically used.
        /// </param>
        /// <param name="regexOptions">Provides enumerated values to use to set regular expression options.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        /// ///
        /// <exception cref="System.ArgumentNullException"><paramref name="regularExpression" /> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentException">
        ///     <paramref name="value" /> does not match the
        ///     <paramref name="regularExpression" /> pattern.
        /// </exception>
        public static string Regex(string value, string regularExpression,
            [CallerMemberName] string propertyName = "",
            RegexOptions regexOptions = RegexOptions.None)
        {
            return Check.Regex(value, regularExpression, propertyName, regexOptions);
        }

        /// <summary>
        ///     Enforces that a <see cref="string" /> value is a valid url.
        /// </summary>
        /// <param name="value">The value to be checked.</param>
        /// <param name="propertyName">
        ///     The name of the property where the field is being set.
        ///     If the <paramref name="propertyName" /> is <c>null</c> the property name is automatically used.
        /// </param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="value" /> is not a valid URL.</exception>
        public static string Url(string value, [CallerMemberName] string propertyName = "")
        {
            return Check.Url(value, propertyName);
        }

        /// <summary>
        ///     Enforces that a <see cref="Guid" /> value is not <see cref="Guid.Empty" />.
        /// </summary>
        /// <param name="value">The value to be checked.</param>
        /// <param name="propertyName">
        ///     The name of the property where the field is being set.
        ///     If the <paramref name="propertyName" /> is <c>null</c> the property name is automatically used.
        /// </param>
        /// <returns>The value that has been successfully checked.</returns>
        public static Guid NotEmpty(Guid value, [CallerMemberName] string propertyName = "")
        {
            return Check.NotEmpty(value, propertyName);
        }

        /// <summary>
        ///     Enforces that a value does not exceed a specified maximum.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the value to compare.
        /// </typeparam>
        /// <param name="value">
        ///     The value to be checked. Must implement <see cref="System.IComparable" /> or
        ///     <see cref="IComparable{T}" />
        /// </param>
        /// <param name="maximum">The maximum value.</param>
        /// <param name="propertyName">
        ///     The name of the property where the field is being set.
        ///     If the <paramref name="propertyName" /> is <c>null</c> the property name is automatically used.
        /// </param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="System.ArgumentException">
        ///     <paramref name="value" /> exceeds the specified <paramref name="maximum" /> value.
        /// </exception>
        public static T Maximum<T>(T value, T maximum, [CallerMemberName] string propertyName = "")
            where T : IComparable, IComparable<T>
        {
            return Check.Maximum(value, maximum, propertyName);
        }

        /// <summary>
        ///     Enforces that a value is not less than a specified minimum.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the value to compare.
        /// </typeparam>
        /// <param name="value">
        ///     The value to be checked. Must implement <see cref="System.IComparable" /> or
        ///     <see cref="IComparable{T}" />
        /// </param>
        /// <param name="minimum">The minimum value allowed.</param>
        /// <param name="propertyName">
        ///     The name of the property where the field is being set.
        ///     If the <paramref name="propertyName" /> is <c>null</c> the property name is automatically used.
        /// </param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="System.ArgumentException">
        ///     <paramref name="value" /> is less than the specified <paramref name="minimum" /> value.
        /// </exception>
        public static T Minimum<T>(T value, T minimum, [CallerMemberName] string propertyName = "")
            where T : IComparable, IComparable<T>
        {
            return Check.Minimum(value, minimum, propertyName);
        }

        /// <summary>
        ///     Requires the provided value to satisfy the specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of the value to be tested.</typeparam>
        /// <param name="value">The value to be checked.</param>
        /// <param name="predicate">The predicate which the value has to satisfy.</param>
        /// <param name="propertyName">
        ///     The name of the property where the field is being set.
        ///     If the <paramref name="propertyName" /> is <c>null</c> the property name is automatically used.
        /// </param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="System.ArgumentNullException">The <paramref name="predicate" /> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentException">The value did not satisfy the specified predicate.</exception>
        public static T Requires<T>([NoEnumeration] T value, Predicate<T> predicate,
            [CallerMemberName] string propertyName = "")
        {
            return Check.Requires(value, predicate, propertyName);
        }
    }
}