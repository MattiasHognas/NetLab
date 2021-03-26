namespace Workspace.Service.Options
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Server.Kestrel.Core;

    /// <summary>
    /// All options for the application.
    /// </summary>
    public class ApplicationOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationOptions"/> class.
        /// </summary>
        public ApplicationOptions() => this.CacheProfiles = new CacheProfileOptions();

        /// <summary>
        /// Gets the cache profiles options.
        /// </summary>
        [Required]
        public CacheProfileOptions CacheProfiles { get; }

        /// <summary>
        /// Gets or sets the compression options.
        /// </summary>
        [Required]
        public CompressionOptions Compression { get; set; } = default!;

        /// <summary>
        /// Gets or sets the forwarded headers options.
        /// </summary>
        [Required]
        public ForwardedHeadersOptions ForwardedHeaders { get; set; } = default!;

        /// <summary>
        /// Gets or sets the kestrel options.
        /// </summary>
        public KestrelServerOptions Kestrel { get; set; } = default!;
    }
}
