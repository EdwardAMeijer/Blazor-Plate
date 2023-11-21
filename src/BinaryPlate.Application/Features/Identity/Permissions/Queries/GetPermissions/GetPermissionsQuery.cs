namespace BinaryPlate.Application.Features.Identity.Permissions.Queries.GetPermissions;

public class GetPermissionsQuery : IRequest<Envelope<GetPermissionsResponse>>
{
    #region Public Properties

    public Guid? Id { get; set; }
    public bool LoadingOnDemand { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class GetPermissionsQueryHandler(IPermissionService permissionService) : IRequestHandler<GetPermissionsQuery, Envelope<GetPermissionsResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetPermissionsResponse>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
        {
            if (request.LoadingOnDemand)
                return await permissionService.GetPermissionsOnDemand(request);

            return await permissionService.GetAllPermissions();
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}