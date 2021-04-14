namespace Identity.Service.ViewModels
{
    using System.Collections.Generic;

    /// <summary>
    /// The user manager response.
    /// </summary>
    public class UserManagerResponse
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; } = default!;

        /// <summary>
        /// Gets or sets a value indicating whether request was successful.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        public IEnumerable<string> Errors { get; set; } = new HashSet<string>();
    }
}
