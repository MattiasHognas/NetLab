namespace Workspace.Service.Services
{
    using System;

    /// <summary>
    /// Retrieves the current date and/or time. Helps with unit testing by letting you mock the system clock.
    /// </summary>
    public interface IClockService
    {
        /// <summary>
        /// Gets the current UTC time offset.
        /// </summary>
        DateTimeOffset UtcNow { get; }
    }
}
