namespace BinaryPlate.Application.Features.Identity.Roles.Queries.GetRoleForEdit;

public class GetRoleForEditQuery : IRequest<Envelope<GetRoleForEditResponse>>
{
    #region Public Properties

    public string Id { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class GetRoleForEditQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetRoleForEditQuery, Envelope<GetRoleForEditResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetRoleForEditResponse>> Handle(GetRoleForEditQuery request, CancellationToken cancellationToken)
        {
            // Checks if the Id provided in the request is valid.
            if (string.IsNullOrWhiteSpace(request.Id))
                return Envelope<GetRoleForEditResponse>.Result.BadRequest(Resource.Invalid_role_Id);

            // Retrieves the role from the database.
            var role = await dbContext.Roles.Include(r => r.RoleClaims).Where(r => r.Id == request.Id).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // Checks if the role was found.
            if (role == null)
                return Envelope<GetRoleForEditResponse>.Result.NotFound(Resource.Unable_to_load_role);

            // Maps the role to a GetRoleForEditResponse object.
            var roleForEditResponse = GetRoleForEditResponse.MapFromEntity(role);

            // Returns the mapped role in an Envelope object with the Ok status.
            return Envelope<GetRoleForEditResponse>.Result.Ok(roleForEditResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}