namespace Identity.Service.Models
{
    /// <summary>
    /// The member model.
    /// </summary>
    public class Member : BaseEntity
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public long MemberId { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; } = default!;
    }
}
