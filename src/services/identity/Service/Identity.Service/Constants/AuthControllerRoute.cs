namespace Identity.Service.Constants
{
    /// <summary>
    /// Auth controller routes.
    /// </summary>
    public static class AuthControllerRoute
    {
        /// <summary>
        /// The register name.
        /// </summary>
        public const string Register = ControllerName.Auth + nameof(Register);

        /// <summary>
        /// The login name.
        /// </summary>
        public const string Login = ControllerName.Auth + nameof(Login);

        /// <summary>
        /// The logout name.
        /// </summary>
        public const string Logout = ControllerName.Auth + nameof(Logout);
    }
}
