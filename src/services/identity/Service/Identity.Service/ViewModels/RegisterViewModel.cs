namespace Identity.Service.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The register viewmodel.
    /// </summary>
    public class RegisterViewModel
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

        /// <summary>
        /// Gets or sets the confirm password.
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string ConfirmPassword { get; set; } = default!;
    }
}
