namespace BinaryPlate.Application.Features.POC.Applicants.Commands.DeleteApplicant;

public class DeleteApplicantCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public string Id { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class DeleteApplicantCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<DeleteApplicantCommand, Envelope<string>>
    {
        #region Public Methods

        public async Task<Envelope<string>> Handle(DeleteApplicantCommand request, CancellationToken cancellationToken)
        {
            // Check if the applicant ID is null or empty.
            if (string.IsNullOrEmpty(request.Id))
                return Envelope<string>.Result.BadRequest(Resource.Invalid_applicant_Id);

            // Check if the applicant ID is a valid GUID format.
            if (!Guid.TryParse(request.Id, out var applicantId))
                return Envelope<string>.Result.BadRequest(Resource.Invalid_applicant_Id);

            // Find the applicant with the given ID.
            var applicant = await dbContext.Applicants.Where(a => a.Id == applicantId).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // If no applicant found with the given ID, return 404 NotFound response.
            if (applicant == null)
                return Envelope<string>.Result.NotFound(Resource.The_applicant_is_not_found);

            // Remove the applicant from the Applicants table.
            dbContext.Applicants.Remove(applicant);

            // Save the changes in the database.
            await dbContext.SaveChangesAsync(cancellationToken);

            // Return a success response.
            return Envelope<string>.Result.Ok(Resource.Applicant_has_been_deleted_successfully);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}