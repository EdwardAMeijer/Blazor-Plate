namespace BinaryPlate.Application.Features.POC.Applicants.Queries.GetApplicantsReferences;

public class GetApplicantReferencesQuery : FilterableQuery, IRequest<Envelope<GetApplicantReferencesResponse>>
{
    #region Public Properties

    public Guid ApplicantId { get; set; }

    #endregion Public Properties

    public class GetApplicantReferencesQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetApplicantReferencesQuery, Envelope<GetApplicantReferencesResponse>>
    {
        #region Public Methods

        #region Public Methods

        public async Task<Envelope<GetApplicantReferencesResponse>> Handle(GetApplicantReferencesQuery request, CancellationToken cancellationToken)
        {
            // Start with a query that retrieves all references for the specified applicant from the database.
            var query = dbContext.References.Where(a => a.ApplicantId == request.ApplicantId
                                                         && (a.Name.Contains(request.SearchText)
                                                             || a.JobTitle.Contains(request.SearchText)
                                                             || a.Phone.Contains(request.SearchText)
                                                             || string.IsNullOrEmpty(request.SearchText)));

            // If a sort by field is provided, sort the query by that field; otherwise, sort by name.
            query = !string.IsNullOrWhiteSpace(request.SortBy)
                ? query.SortBy(request.SortBy)
                : query.OrderBy(a => a.Name);

            // Convert the query to a paged list of applicant reference item DTOs.
            var applicantReferenceItems = await query.Select(q => ApplicantReferenceItem.MapFromEntity(q)).
                                                      ToPagedListAsync(request.PageNumber, request.RowsPerPage);

            // Create an applicant references response DTO with the list of applicant reference item DTOs.
            var response = new GetApplicantReferencesResponse
            {
                ApplicantReferences = applicantReferenceItems
            };

            // Return a success response with the applicant references response DTO as the payload.
            return Envelope<GetApplicantReferencesResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Methods
}