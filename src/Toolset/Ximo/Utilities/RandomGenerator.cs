using System;

namespace Ximo.Utilities
{
    public static class RandomGenerator
    {
        private static readonly Random Global = new Random();

        [ThreadStatic] private static Random _local;

        private static Random RandomSeed
        {
            get
            {
                if (_local == null)
                {
                    int seed;
                    lock (Global)
                    {
                        seed = Global.Next();
                    }
                    _local = new Random(seed);
                }
                return _local;
            }
        }

        /// <summary>
        ///     Generates a random <see cref="System.Int32" />.
        /// </summary>
        /// <param name="min">The minimum <see cref="System.Int32" /> to generate.</param>
        /// <param name="max">The maximum <see cref="System.Int32" /> to generate.</param>
        /// <returns>A random <see cref="System.Int32" />.</returns>
        public static int RandomInt(int min, int max)
        {
            return RandomSeed.Next(min, max);
        }

        /// <summary>
        ///     Generates a random <see cref="System.Double" />.
        /// </summary>
        /// <returns>A random <see cref="System.Double" />.</returns>
        public static double RandomDouble()
        {
            return RandomSeed.NextDouble();
        }

        /// <summary>
        ///     Generates a random <see cref="System.Double" />.
        /// </summary>
        /// <param name="min">The minimum <see cref="System.Double" /> to generate.</param>
        /// <param name="max">The maximum <see cref="System.Double" /> to generate.</param>
        /// <param name="digits">The number or digits to round to.</param>
        /// <returns>A random <see cref="System.Double" />.</returns>
        public static double RandomNumber(int min, int max, int digits)
        {
            return Math.Round(RandomSeed.Next(min, max - 1) + RandomSeed.NextDouble(), digits);
        }

        /// <summary>
        ///     Generates a random <see cref="System.Boolean" />.
        /// </summary>
        /// <returns>A random <see cref="System.Boolean" /></returns>
        public static bool RandomBool()
        {
            return RandomSeed.NextDouble() > 0.5;
        }

        /// <summary>
        ///     Generates a random <see cref="System.DateTime" /> object.
        /// </summary>
        /// <returns>A random <see cref="System.DateTime" />.</returns>
        public static DateTime RandomDate()
        {
            return RandomDate(new DateTime(1900, 1, 1), DateTime.Now);
        }

        /// <summary>
        ///     Generates a random <see cref="DateTime" /> object.
        /// </summary>
        /// <param name="from">The minimum <see cref="System.DateTime" /> generation boundary.</param>
        /// <param name="to">The maximum <see cref="System.DateTime" /> generation boundary.</param>
        /// <returns>A random <see cref="System.DateTime" />.</returns>
        public static DateTime RandomDate(DateTime from, DateTime to)
        {
            var range = new TimeSpan(to.Ticks - from.Ticks);
            return from + new TimeSpan((long) (range.Ticks * RandomSeed.NextDouble()));
        }

        /// <summary>
        ///     Generates a random alphanumeric string.
        /// </summary>
        /// <param name="size">The size of the string to generate.</param>
        /// <param name="lowerCase">If <c>true</c> the generated string will be in lower-case.</param>
        /// <param name="includeSpecialCharacters">
        ///     If <c>true</c> special characters !@#$%^&amp;*() will be included in the generation pool.
        /// </param>
        /// <returns>
        ///     A random generated string.
        /// </returns>
        public static string RandomString(int size, bool lowerCase = false, bool includeSpecialCharacters = false)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";
            if (lowerCase)
            {
                chars = chars.ToLowerInvariant();
            }
            if (includeSpecialCharacters)
            {
                chars = chars + "!@#$%^&*()?";
            }

            var buffer = new char[size];

            for (var i = 0; i < size; i++)
            {
                buffer[i] = chars[RandomSeed.Next(chars.Length)];
            }
            return new string(buffer);
        }

        public static string GenerateRandomEmail()
        {
            return RandomString(10, true) + "@" + RandomString(10, true) + ".com";
        }
    }
}