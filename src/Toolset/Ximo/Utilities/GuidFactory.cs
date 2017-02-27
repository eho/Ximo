using System;

namespace Ximo.Utilities
{
    /// <summary>
    ///     Contains utility methods for creating <see cref="Guid" />.
    /// </summary>
    public static class GuidFactory
    {
        /// <summary>
        ///     Generate a new <see cref="Guid" /> using the comb/sequential algorithm.
        /// </summary>
        /// <returns>A new <see cref="Guid" />.</returns>
        public static Guid NewGuidComb()
        {
            return SequentialGuid.NewSequentialGuid();
        }
    }
}