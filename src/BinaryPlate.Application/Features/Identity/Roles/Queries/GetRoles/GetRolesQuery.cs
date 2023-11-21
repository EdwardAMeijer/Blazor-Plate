namespace BinaryPlate.Application.Features.Identity.Roles.Queries.GetRoles;

public class GetRolesQuery : FilterableQuery, IRequest<Envelope<GetRolesResponse>>
{
    #region Public Classes

    public class GetRolesQueryHandler(ApplicationRoleManager roleManager) : IRequestHandler<GetRolesQuery, Envelope<GetRolesResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetRolesResponse>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            // Get the roles from the role manager.
            var query = roleManager.Roles.AsQueryable();

            // Filter by search text, if any.
            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query = query.Where(r => r.Name.Contains(request.SearchText));

            // Sort the roles by the given sort column, if any; otherwise, sort by name.
            query = !string.IsNullOrWhiteSpace(request.SortBy) ? query.SortBy(request.SortBy) : query.OrderBy(r => r.Name);

            // Get the role items in paged list format.
            var roleItems = await query.Select(q => RoleItem.MapFromEntity(q))
                                       .AsNoTracking()
                                       .ToPagedListAsync(request.PageNumber, request.RowsPerPage);

            // Create the response with the role items.
            var response = new GetRolesResponse
            {
                Roles = roleItems
            };

            // Return the response in an Envelope with a success status.
            return Envelope<GetRolesResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}