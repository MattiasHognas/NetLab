namespace Workspace.Service.Constants
{
    /// <summary>
    /// Workspace controller routes.
    /// </summary>
    public static class WorkspaceControllerRoute
    {
        /// <summary>
        /// The delete workspace name.
        /// </summary>
        public const string DeleteWorkspace = ControllerName.Workspace + nameof(DeleteWorkspace);

        /// <summary>
        /// The det workspace name.
        /// </summary>
        public const string GetWorkspace = ControllerName.Workspace + nameof(GetWorkspace);

        /// <summary>
        /// The head workspace name.
        /// </summary>
        public const string HeadWorkspace = ControllerName.Workspace + nameof(HeadWorkspace);

        /// <summary>
        /// The options workspace name.
        /// </summary>
        public const string OptionsWorkspace = ControllerName.Workspace + nameof(OptionsWorkspace);

        /// <summary>
        /// The options workspaces name.
        /// </summary>
        public const string OptionsWorkspaces = ControllerName.Workspace + nameof(OptionsWorkspaces);

        /// <summary>
        /// The patch workspace name.
        /// </summary>
        public const string PatchWorkspace = ControllerName.Workspace + nameof(PatchWorkspace);

        /// <summary>
        /// The post workspace name.
        /// </summary>
        public const string PostWorkspace = ControllerName.Workspace + nameof(PostWorkspace);

        /// <summary>
        /// The put workspace name.
        /// </summary>
        public const string PutWorkspace = ControllerName.Workspace + nameof(PutWorkspace);
    }
}
