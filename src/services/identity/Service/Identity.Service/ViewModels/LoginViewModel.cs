namespace Identity.Service.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The login viewmodel.
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; } = default!;

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; } = default!;
    }
}
