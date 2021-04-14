namespace Identity.Service.Services
{
    /// <summary>
    /// Retrieves the current user. Helps with unit testing by letting you mock the user.
    /// </summary>
    public interface IPrincipalService
    {
        /// <summary>
        /// Gets the name identifier.
        /// </summary>
        string NameIdentifier { get; }
    }
}
