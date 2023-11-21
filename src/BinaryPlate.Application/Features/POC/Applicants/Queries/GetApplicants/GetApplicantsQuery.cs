namespace BinaryPlate.Application.Features.POC.Applicants.Queries.GetApplicants;

public class GetApplicantsQuery : FilterableQuery, IRequest<Envelope<GetApplicantsResponse>>
{
    #region Public Classes

    public class GetApplicantsQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetApplicantsQuery, Envelope<GetApplicantsResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetApplicantsResponse>> Handle(GetApplicantsQuery request, CancellationToken cancellationToken)
        {
            // Start with a query that retrieves all applicants from the database.
            var query = dbContext.Applicants.AsQueryable();

            // If a search text is provided, filter the applicants by their first name or last name.
            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query = query.Where(a => a.FirstName.Contains(request.SearchText) || a.LastName.Contains(request.SearchText) || a.Ssn.ToString().Contains(request.SearchText));

            // If a sort by field is provided, sort the query by that field; otherwise, sort by
            // first name and then last name.
            query = !string.IsNullOrWhiteSpace(request.SortBy)
                ? query.SortBy(request.SortBy)
                : query.OrderBy(a => a.FirstName).ThenBy(a => a.LastName);

            // Convert the query to a paged list of applicant item DTOs.
            var applicantItems = await query.Select(q => ApplicantItem.MapFromEntity(q, false))
                                            .AsNoTracking()
                                            .ToPagedListAsync(request.PageNumber, request.RowsPerPage);

            // Create an applicants response DTO with the list of applicant item DTOs.
            var response = new GetApplicantsResponse
            {
                Applicants = applicantItems
            };

            // Return a success response with the applicants response DTO as the payload.
            return Envelope<GetApplicantsResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}