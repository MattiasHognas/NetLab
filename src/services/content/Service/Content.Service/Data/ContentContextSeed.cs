namespace Content.Service.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Content.Service.Models;

    /// <summary>
    /// Content context seed.
    /// </summary>
    public class ContentContextSeed
    {
        /// <summary>
        /// Seeds the context.
        /// </summary>
        /// <param name="contentContext">The content context.</param>
        /// <returns>The number of items seeded.</returns>
        public static int Seed(ContentContext contentContext)
        {
            if (!contentContext.Contents.Any())
            {
                var contents = new List<Content>
                {
                    new Content()
                    {
                        PageId = 1,
                        BookId = 1,
                        Value = "x",
                    },
                    new Content()
                    {
                        PageId = 1,
                        BookId = 1,
                        Value = "y",
                    },
                    new Content()
                    {
                        PageId = 1,
                        BookId = 1,
                        Value = "z",
                    },
                };
                contentContext.Contents.AddRange(contents);
            }

            return contentContext.SaveChanges();
        }
    }
}
