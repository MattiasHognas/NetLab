namespace Content.Service.Repositories
{
    using System;
    using System.Linq;

    /// <summary>
    /// The enumerable extensions.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>If.</summary>
        /// <typeparam name="T">The generic type parameter.</typeparam>
        /// <param name="queryable">Queryable.</param>
        /// <param name="condition">Condition.</param>
        /// <param name="action">Action.</param>
        /// <returns>A list of <typeparamref name="T"/>.</returns>
        public static IQueryable<T> If<T>(
            this IQueryable<T> queryable,
            bool condition,
            Func<IQueryable<T>, IQueryable<T>> action)
        {
            if (queryable is null)
            {
                throw new ArgumentNullException(nameof(queryable));
            }

            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (condition)
            {
                return action(queryable);
            }

            return queryable;
        }
    }
}
