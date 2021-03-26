namespace Content.Service.Repositories
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The enumerable extensions.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>If.</summary>
        /// <typeparam name="T">The generic type parameter.</typeparam>
        /// <param name="enumerable">Enumerable.</param>
        /// <param name="condition">Condition.</param>
        /// <param name="action">Action.</param>
        /// <returns>A list of <typeparamref name="T"/>.</returns>
        public static IEnumerable<T> If<T>(
            this IEnumerable<T> enumerable,
            bool condition,
            Func<IEnumerable<T>, IEnumerable<T>> action)
        {
            if (enumerable is null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (condition)
            {
                return action(enumerable);
            }

            return enumerable;
        }
    }
}
