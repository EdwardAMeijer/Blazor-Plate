using System.Runtime.CompilerServices;

namespace BinaryPlate.Infrastructure.Services.Identity;

public class PermissionScanner(ApplicationPartManager partManager, IApplicationDbContext dbContext) : IPermissionScanner
{
    #region Public Methods

    public async Task ScanAndSeedBuiltInPermissions()
    {
        // Get all the application permissions to be deleted and remove them from the database.
        var permissionsToBeDeleted = await dbContext.ApplicationPermissions.Where(p => p.IsCustomPermission == false).IgnoreQueryFilters().ToListAsync();
        dbContext.ApplicationPermissions.RemoveRange(permissionsToBeDeleted);

        // Create a new controller feature and populate it.
        var feature = new ControllerFeature();
        partManager.PopulateFeature(feature);

        // Select all the controllers and their actions, and filter out the compiler generated ones
        // and allow anonymous actions.
        var controllers = feature.Controllers
            .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
            .Where(m => !m.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Any())
            .Select(info => new
            {
                Controller = info.DeclaringType?.Name.Replace("Controller", string.Empty),
                Action = info.Name,
                Attributes = string.Join(",", info.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))),
                TypeAttributes = string.Join(",", info.DeclaringType?.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", "")) ?? Array.Empty<string>())
            })
            .Where(c => !c.TypeAttributes.Contains("AllowAnonymous") && c.TypeAttributes.Contains("BpAuthorize"))
            .Where(c => !c.Attributes.Contains("AllowAnonymous"))
            .OrderBy(c => c.Controller).ThenBy(c => c.Action).ToList();

        // Add a root node to the database.
        var rootNode = new ApplicationPermission { Name = "Actions" };
        await dbContext.ApplicationPermissions.AddAsync(rootNode);

        // Create a new list to store root permissions for the application.
        var rootPermissions = new List<ApplicationPermission>();

        // Iterate through the controllers list and add root permissions.
        foreach (var item in controllers)
        {
            // Create a string representing the name of the root permission based on the controller name.
            var rootPermissionName = $"{item.Controller}";

            // Check if the root permission has not already been added to the list of root permissions.
            if (rootPermissions.All(p => p.Name != rootPermissionName))
            {
                // Create a new application permission object.
                var rootPermission = new ApplicationPermission
                {
                    Name = rootPermissionName,
                    ParentId = rootNode.Id == Guid.Empty ? (await dbContext.ApplicationPermissions.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Name == "Actions"))?.Id : rootNode.Id,
                };

                // Add the root permission to the list of root permissions.
                rootPermissions.Add(rootPermission);
            }
        }

        // Iterate through all root permissions.
        foreach (var rootPermission in rootPermissions)
        {
            // Set the parent ID for the root permission.
            rootPermission.ParentId = rootNode.Id == Guid.Empty ? dbContext.ApplicationPermissions.FirstOrDefault(p => p.Name == "Actions")?.Id : rootNode.Id;

            // Add the root permission to the database.
            await dbContext.ApplicationPermissions.AddAsync(rootPermission);

            // Iterate through each controller associated with the root permission.
            foreach (var type in controllers.Where(ct => ct.Controller == rootPermission.Name))
            {
                // Create a child permission for the controller action.
                var childPermissionName = $"{type.Controller}.{type.Action}";

                // Get the controller name.
                var controllerName = childPermissionName.Split(".")[0];

                // Add the child permission to the database.
                await dbContext.ApplicationPermissions.AddAsync(new ApplicationPermission
                {
                    Name = childPermissionName,
                    ParentId = rootPermission.Id == Guid.Empty ? (await dbContext.ApplicationPermissions.FirstOrDefaultAsync(p => p.Name == controllerName))?.Id : rootPermission.Id,
                });
            }
        }

        try
        {
            // Save all changes made to the underlying database.
            await dbContext.SaveChangesAsync();
        }
        catch
        {
            throw new DbUpdateException();
        }
    }

    #endregion Public Methods
}