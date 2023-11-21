namespace BinaryPlate.Infrastructure.Persistence;

public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser,
                                                             ApplicationRole,
                                                             string,
                                                             ApplicationUserClaim,
                                                             ApplicationUserRole,
                                                             ApplicationUserLogin,
                                                             ApplicationRoleClaim,
                                                             ApplicationUserToken>, IApplicationDbContext
{
    #region Private Fields

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUtcDateTimeProvider _utcDateTimeProvider;

    #endregion Private Fields

    #region Public Constructors

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                                IHttpContextAccessor httpContextAccessor,
                                IUtcDateTimeProvider utcDateTimeProvider) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
        _utcDateTimeProvider = utcDateTimeProvider;
        Current = this;
    }

    #endregion Public Constructors

    #region Public Properties

    // Application configuration entities.
    public override DbSet<ApplicationUserRole> UserRoles { get; set; }

    public override DbSet<ApplicationUserClaim> UserClaims { get; set; }
    public override DbSet<ApplicationUserLogin> UserLogins { get; set; }
    public override DbSet<ApplicationRoleClaim> RoleClaims { get; set; }
    public override DbSet<ApplicationUserToken> UserTokens { get; set; }
    public DbSet<ApplicationUserAttachment> ApplicationUserAttachments { get; set; }
    public DbSet<ApplicationPermission> ApplicationPermissions { get; set; }

    // Application configuration entities.
    public DbSet<UserSettings> UserSettings { get; set; }

    public DbSet<PasswordSettings> PasswordSettings { get; set; }
    public DbSet<LockoutSettings> LockoutSettings { get; set; }
    public DbSet<SignInSettings> SignInSettings { get; set; }
    public DbSet<TokenSettings> TokenSettings { get; set; }
    public DbSet<FileStorageSettings> FileStorageSettings { get; set; }

    // Application-specific entities.
    public DbSet<Applicant> Applicants { get; set; }

    public DbSet<Reference> References { get; set; }

    // Application-generic entities.
    public DbSet<Report> Reports { get; set; }

    // DbContext-related properties.
    public DbContext Current { get; }

    #endregion Public Properties

    #region Public Methods

    // Override the SaveChangesAsync method of DbContext.
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        // Iterate through all the entries in the ChangeTracker and validate them.
        foreach (var entry in ChangeTracker.Entries())
        {
            var validationContext = new ValidationContext(entry);
            Validator.ValidateObject(entry, validationContext);
        }

        // Get the current user ID and current UTC utcDateTime.
        var userId = _httpContextAccessor.GetUserId();
        var utcNow = _utcDateTimeProvider.GetUtcNow();

        // Iterate through all the IAuditable entries in the ChangeTracker and set the
        // created/modified/deleted properties accordingly.
        foreach (var entry in ChangeTracker.Entries<IAuditable>())
            switch (entry)
            {
                // For added entities, set the created properties.
                case { State: EntityState.Added }:
                    entry.Property("CreatedOn").CurrentValue = utcNow;
                    entry.Property("CreatedBy").CurrentValue = userId;
                    break;

                // For modified entities, set the modified properties.
                case { State: EntityState.Modified }:
                    entry.Property("ModifiedOn").CurrentValue = utcNow;
                    entry.Property("ModifiedBy").CurrentValue = userId;
                    break;

                // For deleted entities, if they are soft deletable, set the deleted properties.
                case { State: EntityState.Deleted }:
                    if (entry.Entity is ISoftDeletable)
                    {
                        entry.Property("DeletedOn").CurrentValue = utcNow;
                        entry.Property("DeletedBy").CurrentValue = userId;
                    }
                    break;
            }

        // Iterate over all entities that implement the ISoftDeletable interface and have been
        // marked for deletion. For such entities, it sets their state to EntityState.Unchanged,
        // which ensures that only the IsDeleted flag will be updated and sent to the database
        // during the next call to SaveChangesAsync().
        foreach (var entry in ChangeTracker.Entries<ISoftDeletable>().Where(x => x.State == EntityState.Deleted))
        {
            entry.State = EntityState.Unchanged;
            entry.Property("IsDeleted").CurrentValue = true;
        }

        // Get the modified entities that have the ConcurrencyStamp property (Guid) and are not database-generated.
        var modifiedEntities = ChangeTracker.Entries<IConcurrencyStamp>().Where(e => e.State == EntityState.Modified && e.Metadata.FindProperty("ConcurrencyStamp") != null);

        foreach (var entry in modifiedEntities)
        {
            // Retrieve the original and current values of the ConcurrencyStamp property.
            var originalVersion = entry.OriginalValues.GetValue<string>("ConcurrencyStamp");
            var currentVersion = entry.CurrentValues.GetValue<string>("ConcurrencyStamp");

            // Check if the values match to detect any concurrency conflict.
            if (originalVersion != currentVersion)
            {
                // A concurrency conflict has occurred. You may handle the conflict here, such as
                // logging it or returning an error response to the user. Alternatively, throw a
                // custom DbUpdateConcurrencyException that can be caught and processed by the
                // global exception handler.
                throw new DbUpdateConcurrencyException("It appears someone else has made changes or deleted the data you're updating. Please refresh the page and try again.");
            }

            // Regenerate the ConcurrencyStamp property with a new GUID before saving the changes to
            // the database.
            entry.Property("ConcurrencyStamp").CurrentValue = Guid.NewGuid().ToString();
        }

        // Saves all changes made in this context to the database. Any validation errors will be
        // thrown as exceptions. If the operation is successful, it returns the number of entities
        // written to the database.
        var totalChanges = await base.SaveChangesAsync(cancellationToken);

        // Returns the number of entities written to the database.
        return totalChanges;
    }

    #endregion Public Methods

    #region Protected Methods

    // Configures the DbContext.
    protected override void OnConfiguring(DbContextOptionsBuilder contextOptionsBuilder)
    {
        // Calls the base method to configure other DbContext options.
        base.OnConfiguring(contextOptionsBuilder);
    }

    // Configures the entities in the DbContext.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configures entities to support soft deletion.
        DbContextHelper.ConfigureSoftDeletableEntities(modelBuilder);

        // Configures entities for support concurrency check.
        DbContextHelper.ConfigureConcurrencyTokenForEntities(modelBuilder);

        // Configures entities for storing settings in a separate schema.
        DbContextHelper.ConfigureSettingsSchemaEntities(modelBuilder);

        // Calls the base method to continue with further configuration.
        base.OnModelCreating(modelBuilder);

        // Applies entity configurations from the current assembly.
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    #endregion Protected Methods
}