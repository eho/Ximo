using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Ximo.Extensions;

namespace Ximo.Validation
{
    /// <summary>
    ///     Class with validation utilities to be used for validating method input.
    /// </summary>
    [DebuggerStepThrough]
    public static class Check
    {
        /// <summary>
        ///     Enforces that an <see cref="object" /> value is not null.
        /// </summary>
        /// <param name="value">The value to be checked.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        [ContractAnnotation("value:null => halt")]
        public static object NotNull([NoEnumeration] object value, [InvokerParameterName] [NotNull] string argumentName)
        {
            if (value != null)
            {
                return value;
            }

            string errorMessage = $"The value of '{argumentName}' cannot be null.";
            throw new ValidationException(errorMessage, new ArgumentNullException(errorMessage, argumentName));
        }

        /// <summary>
        ///     Enforces that a <see cref="string" /> value is not <c>null</c> or empty, i.e. length is 0.
        /// </summary>
        /// <param name="value">The value to be checked.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="value" /> is empty (length is 0).</exception>
        [ContractAnnotation("value:null => halt")]
        public static string NotNullOrEmpty(string value, [InvokerParameterName] [NotNull] string argumentName)
        {
            NotNull(value, argumentName);

            if (value.Length != 0)
            {
                return value;
            }

            var errorMessage = $"The string value of '{argumentName}' cannot be empty.";
            throw new ValidationException(errorMessage, new ArgumentException(errorMessage, argumentName));
        }

        /// <summary>
        ///     Enforces that a <see cref="string" /> value is not <c>null</c>, empty or consists only of whitespace characters.
        /// </summary>
        /// <param name="value">The value to be checked.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentException"><paramref name="value" /> is empty or contains only whitespace characters.</exception>
        [ContractAnnotation("value:null => halt")]
        public static string NotNullOrWhitespace(string value, [InvokerParameterName] [NotNull] string argumentName)
        {
            NotNull(value, argumentName);

            if (value.Length != 0 && !value.All(char.IsWhiteSpace))
            {
                return value;
            }

            var errorMessage =
                $"The string value of '{argumentName}' cannot be empty or contain only whitespace characters.";
            throw new ValidationException(errorMessage, new ArgumentException(errorMessage, argumentName));
        }

        /// <summary>
        ///     Enforces that the length of a <see cref="string" /> value does not exceed a specified maximum
        ///     number of characters.
        /// </summary>
        /// <param name="value">The value to be checked.</param>
        /// <param name="maximumLength">The maximum length allowed for the <see cref="string" /> value.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="System.InvalidOperationException">
        ///     The <paramref name="maximumLength" /> value is less than or equal to
        ///     0.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        ///     the length<paramref name="value" /> exceeds the <paramref name="maximumLength" />
        ///     specified.
        /// </exception>
        public static string MaxLength(string value, int maximumLength,
            [InvokerParameterName] [NotNull] string argumentName)
        {
            if (maximumLength <= 0)
            {
                const string invalidErrorMessageText = "The maximum length specified cannot be less than or equal to 0.";
                throw new InvalidOperationException(invalidErrorMessageText);
            }

            if (value == null || value.Length <= maximumLength)
            {
                return value;
            }

            var errorMessage = $"The length of string '{argumentName}' cannot exceed {maximumLength} characters.";
            throw new ValidationException(errorMessage, new ArgumentException(errorMessage, argumentName));
        }

        /// <summary>
        ///     Enforces that the length of a <see cref="string" /> value has at least a specified minimum
        ///     number of characters.
        /// </summary>
        /// <param name="value">The value to be checked.</param>
        /// <param name="minimumLength">The minimum length.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="System.InvalidOperationException">
        ///     The <paramref name="minimumLength" /> value is less than or equal to
        ///     0.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        ///     the length of <paramref name="value" /> is less than the <paramref name="minimumLength" />
        ///     specified.
        /// </exception>
        public static string MinLength(string value, int minimumLength,
            [InvokerParameterName] [NotNull] string argumentName)
        {
            if (minimumLength < 0)
            {
                const string invalidErrorMessageText = "The minimum length specified cannot be less than or equal 0.";
                throw new InvalidOperationException(invalidErrorMessageText);
            }

            if (value == null || value.Length >= minimumLength)
            {
                return value;
            }

            var errorMessage = $"The length of '{argumentName}' cannot be less than {minimumLength} characters.";
            throw new ValidationException(errorMessage, new ArgumentException(errorMessage, argumentName));
        }

        /// <summary>
        ///     Enforces that the length of a <see cref="string" /> value has at least a specified minimum
        ///     number of characters and does not exceed a specified maximum number of characters.
        /// </summary>
        /// <param name="value">The value to be checked.</param>
        /// <param name="minimumLength">The minimum length allowed for the <see cref="string" /> value.</param>
        /// <param name="maximumLength">The maximum length allowed for the <see cref="string" /> value.</param>
        /// <param name="argumentName">Name of the parameter.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="System.InvalidOperationException">
        ///     The <paramref name="maximumLength" /> value is less than or equal to
        ///     0.
        ///     (or)
        ///     The <paramref name="minimumLength" /> value is less than or equal to
        ///     0.
        ///     (or)
        ///     The <paramref name="maximumLength" /> value is less than The <paramref name="minimumLength" /> specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        ///     <paramref name="value" /> length is less than the <paramref name="minimumLength" /> specified or exceeds the
        ///     <paramref name="maximumLength" /> specified.
        /// </exception>
        public static string StringLength(string value, int minimumLength, int maximumLength,
            [InvokerParameterName] [NotNull] string argumentName)
        {
            if (minimumLength < 0)
            {
                const string invalidMinLengthErrorText =
                    "The minimum length specified cannot be less than or equal 0.";
                throw new InvalidOperationException(invalidMinLengthErrorText);
            }

            if (maximumLength <= 0)
            {
                const string invalidMaxLengthErrorText =
                    "The maximum length specified cannot be less than or equal to 0.";
                throw new InvalidOperationException(invalidMaxLengthErrorText);
            }

            if (maximumLength < minimumLength)
            {
                const string invalidErrorText =
                    "The minimum length specified cannot be greater than the maximum length specified.";
                throw new InvalidOperationException(invalidErrorText);
            }

            if (value == null || value.Length >= minimumLength && value.Length <= maximumLength)
            {
                return value;
            }

            var errorMessage =
                $"The length of '{argumentName}' cannot be less than {minimumLength} characters and cannot exceed {maximumLength} characters.";
            throw new ValidationException(errorMessage, new ArgumentException(errorMessage, argumentName));
        }

        /// <summary>
        ///     Enforces that a <see cref="string" /> value represents a valid email.
        /// </summary>
        /// <param name="value">The value to be checked.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentException">
        ///     <paramref name="value" /> is empty, contains only whitespace characters or
        ///     not a valid email.
        /// </exception>
        public static string Email(string value, [InvokerParameterName] [NotNull] string argumentName)
        {
            NotNullOrWhitespace(value, argumentName);

            var validator = new EmailAddressAttribute();
            if (validator.IsValid(value))
            {
                return value;
            }

            var errorMessage = $"The value of '{argumentName}' is not a valid email.";
            throw new ValidationException(errorMessage, new ArgumentException(errorMessage, argumentName));
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
        /// <param name="argumentName">Name of the argument.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="System.ArgumentException">
        ///     <paramref name="value" /> does not fall within the specified range. It is
        ///     either less than <paramref name="minimum" /> value or larger than <paramref name="maximum" /> value specified.
        /// </exception>
        public static T Range<T>(T value, T minimum, T maximum, [InvokerParameterName] [NotNull] string argumentName)
            where T : IComparable, IComparable<T>
        {
            var validator = new RangeAttribute(typeof(T), minimum.ToString(), maximum.ToString());
            if (validator.IsValid(value))
            {
                return value;
            }

            var errorMessage =
                $"The value of '{argumentName}' does not fall within the range of minimum: {minimum} and maximum: {maximum}.";
            throw new ValidationException(errorMessage, new ArgumentException(errorMessage, argumentName));
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
        /// <param name="maximum">The maximum value allowed.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="System.ArgumentException">
        ///     <paramref name="value" /> exceeds the specified <paramref name="maximum" /> value.
        /// </exception>
        public static T Maximum<T>(T value, T maximum, [InvokerParameterName] [NotNull] string argumentName)
            where T : IComparable, IComparable<T>
        {
            if (value.CompareTo(maximum) <= 0)
            {
                return value;
            }

            var errorMessage = $"The value of '{argumentName}' cannot exceed {maximum}.";
            throw new ValidationException(errorMessage, new ArgumentException(errorMessage, argumentName));
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
        /// <param name="argumentName">Name of the argument.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="System.ArgumentException">
        ///     <paramref name="value" /> is less than the specified <paramref name="minimum" /> value.
        /// </exception>
        public static T Minimum<T>(T value, T minimum, [InvokerParameterName] [NotNull] string argumentName)
            where T : IComparable, IComparable<T>
        {
            if (value.CompareTo(minimum) >= 0)
            {
                return value;
            }

            var errorMessage = $"The value of '{argumentName}' cannot be less than {minimum}.";
            throw new ValidationException(errorMessage, new ArgumentException(errorMessage, argumentName));
        }

        /// <summary>
        ///     Enforces that a <see cref="string" /> value matches a specified regular expression pattern.
        /// </summary>
        /// <param name="value">The value to be checked.</param>
        /// <param name="regularExpression">The regular expression used for evaluation of the value.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <param name="regexOptions">(Optional) Provides enumerated values to use to set regular expression options.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="value" /> or <paramref name="regularExpression" /> is <c>null</c>.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        ///     <paramref name="value" /> does not match the
        ///     <paramref name="regularExpression" /> pattern.
        /// </exception>
        public static string Regex(string value, string regularExpression,
            [InvokerParameterName] [NotNull] string argumentName,
            RegexOptions regexOptions = RegexOptions.None)
        {
            NotNull(value, argumentName);
            NotNullOrWhitespace(regularExpression, nameof(regularExpression));

            var regex = new Regex(regularExpression, regexOptions);

            if (regex.IsMatch(value))
            {
                return value;
            }

            var errorMessage =
                $"The value of '{argumentName}' does not match the regular expression {regularExpression}.";
            throw new ValidationException(errorMessage, new ArgumentException(errorMessage, argumentName));
        }

        /// <summary>
        ///     Enforces that a <see cref="string" /> value is a valid url.
        /// </summary>
        /// <param name="value">The value to be checked.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentException">
        ///     <paramref name="value" /> is empty, contains only whitespace characters or
        ///     not a valid URL.
        /// </exception>
        public static string Url(string value, [InvokerParameterName] [NotNull] string argumentName)
        {
            NotNullOrWhitespace(value, argumentName);

            var validator = new UrlAttribute();

            if (validator.IsValid(value))
            {
                return value;
            }

            var errorMessage = $"The value of '{argumentName}' is not a valid URL.";
            throw new ValidationException(errorMessage, new ArgumentException(errorMessage, argumentName));
        }

        /// <summary>
        ///     Enforces that a <see cref="Guid" /> value is not <see cref="Guid.Empty" />.
        /// </summary>
        /// <param name="value">The value to be checked.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="System.ArgumentException"><paramref name="value" /> is <c>Guid.Empty</c>.</exception>
        public static Guid NotEmpty(Guid value, [InvokerParameterName] [NotNull] string argumentName)
        {
            if (!Equals(value, Guid.Empty))
            {
                return value;
            }

            var errorMessage = $"The Guid value of '{argumentName}' cannot be Guid.Empty.";
            throw new ValidationException(errorMessage, new ArgumentException(errorMessage, argumentName));
        }

        /// <summary>
        ///     Enforces that a <see cref="IEnumerable" /> value is not null or empty.
        /// </summary>
        /// <param name="value">The value to be checked.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="System.ArgumentNullException">The <paramref name="value" /> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentException">The <paramref name="value" /> is empty.</exception>
        [ContractAnnotation("value:null => halt")]
        public static IEnumerable NotNullOrEmpty([NoEnumeration] IEnumerable value,
            [InvokerParameterName] [NotNull] string argumentName)
        {
            NotNull(value, argumentName);
            // ReSharper disable once PossibleMultipleEnumeration
            if (!value.IsEmpty())
            {
                return value;
            }

            var errorMessage =
                $"The enumerable list '{argumentName}' cannot empty, i.e. has to contain at least one item.";
            throw new ValidationException(errorMessage, new ArgumentException(errorMessage, argumentName));
        }

        /// <summary>
        ///     Requires the provided value to satisfy the specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of the value to be tested.</typeparam>
        /// <param name="value">The value to be checked.</param>
        /// <param name="predicate">The predicate which the value has to satisfy.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="System.ArgumentNullException">The <paramref name="predicate" /> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentException">The value did not satisfy the specified predicate.</exception>
        public static T Requires<T>([NoEnumeration] T value, Predicate<T> predicate,
            [InvokerParameterName] [NotNull] string argumentName)
        {
            NotNull(predicate, nameof(predicate));

            if (predicate.Invoke(value))
            {
                return value;
            }

            var errorMessage = $"The value '{argumentName}' did not satisfy the specified predicate.";
            throw new ValidationException(errorMessage, new ArgumentException(errorMessage, argumentName));
        }
    }
}