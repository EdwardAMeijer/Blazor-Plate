namespace BinaryPlate.Application.Features.POC.Applicants.Queries.GetApplicantForEdit;

public class GetApplicantForEditQuery : IRequest<Envelope<GetApplicantForEditResponse>>
{
    #region Public Properties

    public string Id { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class GetApplicantForEditQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetApplicantForEditQuery, Envelope<GetApplicantForEditResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetApplicantForEditResponse>> Handle(GetApplicantForEditQuery request, CancellationToken cancellationToken)
        {
            // Check if the ID provided in the request is a valid GUID.
            if (!Guid.TryParse(request.Id, out var applicantId))
                return Envelope<GetApplicantForEditResponse>.Result.BadRequest(Resource.Invalid_applicant_Id);

            // Retrieve the applicant from the database using the ID.
            var applicant = await dbContext.Applicants.Where(a => a.Id == applicantId).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // If the applicant is not found, return a not found response.
            if (applicant == null)
                return Envelope<GetApplicantForEditResponse>.Result.NotFound(Resource.Unable_to_load_applicant);

            // Map the applicant entity to an applicant response DTO.
            var applicantForEditResponse = GetApplicantForEditResponse.MapFromEntity(applicant);

            // Return a success response with the applicant response DTO as the payload.
            return Envelope<GetApplicantForEditResponse>.Result.Ok(applicantForEditResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}