namespace BinaryPlate.Application.Common.Contracts.Infrastructure;

/// <summary>
/// Seeds the application database with initial data.
/// </summary>
public interface IAppSeederService
{
    #region Public Methods

    /// <summary>
    /// Seeds the application with data required for its initial setup.
    /// </summary>
    /// <returns>A boolean value indicating whether the seeding operation succeeded.</returns>
    Task<bool> SeedApp();

    #endregion Public Methods
}