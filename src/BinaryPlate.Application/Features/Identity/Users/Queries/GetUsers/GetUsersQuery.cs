namespace BinaryPlate.Application.Features.Identity.Users.Queries.GetUsers;

public class GetUsersQuery : FilterableQuery, IRequest<Envelope<GetUsersResponse>>
{
    #region Public Properties

    public IReadOnlyList<string> SelectedRoleIds { get; set; } = new List<string>();

    #endregion Public Properties

    #region Public Classes

    public class GetUsersQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetUsersQuery, Envelope<GetUsersResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetUsersResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            // Set the base query to retrieve all users from the database.
            var query = dbContext.Users.AsQueryable();

            // If there are any selected role ids, filter the query to only include users with those roles.
            if (request.SelectedRoleIds.Count != 0)
                query = (from ur in dbContext.UserRoles
                         where request.SelectedRoleIds.Contains(ur.RoleId)
                         select ur.User).Distinct();

            // If there is any search text, filter the query to include only users whose username or
            // email contains the search text.
            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query = query.Where(u => u.UserName.Contains(request.SearchText) && u.Email.Contains(request.SearchText));

            // Sort the query based on the requested sort column and order, or by username in
            // ascending order if no sort column was requested.
            query = !string.IsNullOrWhiteSpace(request.SortBy) ? query.SortBy(request.SortBy) : query.OrderBy(u => u.UserName);

            // Execute the query and map the resulting User entities to UserItem view models, then
            // paginate the results.
            var userItems = await query.Select(q => UserItem.MapFromEntity(q))
                                       .AsNoTracking()
                                       .ToPagedListAsync(request.PageNumber, request.RowsPerPage);

            // Create a GetUsersResponse view model and populate it with the paginated UserItem view models.
            var response = new GetUsersResponse
            {
                Users = userItems
            };

            // Return an Ok result with the GetUsersResponse view model.
            return Envelope<GetUsersResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}