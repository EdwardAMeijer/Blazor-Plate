namespace BinaryPlate.Infrastructure.Persistence;

/// <summary>
/// A static class with helper methods for configuring Entity Framework Core DbContext instances.
/// </summary>
public sealed class DbContextHelper
{
    #region Public Methods

    /// <summary>
    /// Configures the given <paramref name="builder"/> to use the "Settings" schema for. entities
    /// that implement the <see cref="ISettingsSchema"/> interface.
    /// </summary>
    /// <param name="builder">The <see cref="ModelBuilder"/> instance to configure.</param>
    public static void ConfigureSettingsSchemaEntities(ModelBuilder builder)
    {
        // Create SQL database schema for the settings tables.
        foreach (var entityType in builder.Model.GetEntityTypes().Where(e => typeof(ISettingsSchema).IsAssignableFrom(e.ClrType)))
            builder.Entity(entityType.ClrType).ToTable(entityType.ClrType.Name, "Settings");
    }

    /// <summary>
    /// Configures the given model builder to add soft delete functionality to all entities.
    /// implementing the <see cref="ISoftDeletable"/> interface.
    /// </summary>
    /// <param name="builder">The <see cref="ModelBuilder"/> instance to configure.</param>
    public static void ConfigureSoftDeletableEntities(ModelBuilder builder)
    {
        //Creating navigation or shadow properties for all entity.
        foreach (var entityType in builder.Model.GetEntityTypes().Where(e => typeof(ISoftDeletable).IsAssignableFrom(e.ClrType)))
            builder.Entity(entityType.ClrType).Property<bool>("IsDeleted").IsRequired();

        //Filter out soft-deleted entities by default.
        builder.SetQueryFilter<ISoftDeletable>(p => EF.Property<bool>(p, "IsDeleted") == false);
    }

    /// <summary>
    /// Configures a concurrency token property named "ConcurrencyStamp" for entities that don't
    /// have it.
    /// </summary>
    /// <param name="builder">The model builder to configure.</param>
    public static void ConfigureConcurrencyTokenForEntities(ModelBuilder builder)
    {
        // Loop through all entity types.
        foreach (var entityType in builder.Model.GetEntityTypes().Where(b => b.ClrType != typeof(Dictionary<string, object>) && b.GetProperties().All(p => p.Name != "ConcurrencyStamp")))
        {
            // Add the "ConcurrencyStamp" property to the entity with the appropriate configuration
            builder.Entity(entityType.ClrType)
                .Property<string>("ConcurrencyStamp")
                .IsConcurrencyToken();
        }
    }

    #endregion Public Methods
}