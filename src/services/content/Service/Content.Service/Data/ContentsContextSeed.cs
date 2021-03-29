namespace Content.Service.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Content.Service.Models;

    /// <summary>
    /// Content context seed.
    /// </summary>
    public class ContentsContextSeed
    {
        /// <summary>
        /// Seeds the context.
        /// </summary>
        /// <param name="contentsContext">The contents context.</param>
        /// <returns>The number of items seeded.</returns>
        public static int Seed(ContentsContext contentsContext)
        {
            if (!contentsContext.Content.Any())
            {
                var contents = new List<Content>
                {
                    new Content()
                    {
                        ContentId = 1,
                        PageId = 1,
                        BookId = 1,
                        Created = DateTimeOffset.UtcNow.AddDays(-8),
                        Value = "x",
                        Modified = DateTimeOffset.UtcNow.AddDays(-8),
                    },
                    new Content()
                    {
                        ContentId = 2,
                        PageId = 1,
                        BookId = 1,
                        Created = DateTimeOffset.UtcNow.AddDays(-8),
                        Value = "x",
                        Modified = DateTimeOffset.UtcNow.AddDays(-8),
                    },
                    new Content()
                    {
                        ContentId = 3,
                        PageId = 1,
                        BookId = 1,
                        Created = DateTimeOffset.UtcNow.AddDays(-8),
                        Value = "x",
                        Modified = DateTimeOffset.UtcNow.AddDays(-8),
                    },
                };
                contentsContext.Content.AddRange(contents);
                return contentsContext.SaveChanges();
            }

            return 0;
        }
    }
}
