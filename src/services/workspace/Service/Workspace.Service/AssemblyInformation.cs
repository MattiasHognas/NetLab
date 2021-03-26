namespace Workspace.Service
{
    using System.Reflection;

    public record AssemblyInformation(string Product, string Description, string Version)
    {
        /// <summary>
        /// The current assembly information.
        /// </summary>
        public static readonly AssemblyInformation Current = new(typeof(AssemblyInformation).Assembly);

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyInformation"/> class.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public AssemblyInformation(Assembly assembly)
            : this(
                assembly.GetCustomAttribute<AssemblyProductAttribute>()!.Product,
                assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()!.Description,
                assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()!.Version)
        {
        }
    }
}
