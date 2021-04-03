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
                        ContentId = 1,
                        PageId = 1,
                        BookId = 1,
                        Value = "x",
                    },
                    new Content()
                    {
                        ContentId = 2,
                        PageId = 1,
                        BookId = 1,
                        Value = "x",
                    },
                    new Content()
                    {
                        ContentId = 3,
                        PageId = 1,
                        BookId = 1,
                        Value = "x",
                    },
                };
                contentContext.Contents.AddRange(contents);
            }

            return contentContext.SaveChanges();
        }
    }
}
