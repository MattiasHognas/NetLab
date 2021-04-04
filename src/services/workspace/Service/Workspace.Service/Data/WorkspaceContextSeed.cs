namespace Workspace.Service.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Workspace.Service.Models;

    /// <summary>
    /// Workspace context seed.
    /// </summary>
    public class WorkspaceContextSeed
    {
        /// <summary>
        /// Seeds the context.
        /// </summary>
        /// <param name="workspaceContext">The workspace context.</param>
        /// <returns>The number of items seeded.</returns>
        public static int Seed(WorkspaceContext workspaceContext)
        {
            if (!workspaceContext.Books.Any())
            {
                var books = new List<Book>
                {
                    new Book()
                    {
                        Name = "x",
                        Description = "x",
                    },
                    new Book()
                    {
                        Name = "y",
                        Description = "y",
                    },
                    new Book()
                    {
                        Name = "z",
                        Description = "z",
                    },
                };
                workspaceContext.Books.AddRange(books);
            }

            if (!workspaceContext.Pages.Any())
            {
                var pages = new List<Page>
                {
                    new Page()
                    {
                        BookId = 1,
                        Name = "x",
                        Description = "x",
                    },
                    new Page()
                    {
                        BookId = 1,
                        Name = "y",
                        Description = "y",
                    },
                    new Page()
                    {
                        BookId = 1,
                        Name = "z",
                        Description = "z",
                    },
                };
                workspaceContext.Pages.AddRange(pages);
            }

            return workspaceContext.SaveChanges();
        }
    }
}
