namespace BinaryPlate.Infrastructure.Persistence;

/// <summary>
/// Provides a method for seeding the application database with default data.
/// </summary>
public static class ApplicationDbContextSeeder
{
    #region Public Methods

    /// <summary>
    /// Seeds the application database with default data.
    /// </summary>
    /// <param name="appSeederService">The application seeder service used to seed the database.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static async Task SeedAsync(IAppSeederService appSeederService)
    {
        // Seed the application.
        await appSeederService.SeedApp();
    }

    #endregion Public Methods
}