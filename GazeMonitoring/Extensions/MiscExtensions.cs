using System;
using System.Collections.Generic;
using System.Linq;

namespace GazeMonitoring.Extensions
{
    public static class MiscExtensions
    {
        /// <summary>
        /// Determines whether an object is equal to any of the elements in a sequence.
        /// </summary>
        public static bool IsEither<T>(this T obj, IEnumerable<T> variants,
            IEqualityComparer<T> comparer)
        {
            if (variants == null) throw new ArgumentNullException(nameof(variants));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            return variants.Contains(obj, comparer);
        }

        /// <summary>
        /// Determines whether an object is equal to any of the elements in a sequence.
        /// </summary>
        public static bool IsEither<T>(this T obj, IEnumerable<T> variants)
            => IsEither(obj, variants, EqualityComparer<T>.Default);

        /// <summary>
        /// Determines whether the object is equal to any of the parameters.
        /// </summary>
        public static bool IsEither<T>(this T obj, params T[] variants) => IsEither(obj, (IEnumerable<T>)variants);
    }
}
