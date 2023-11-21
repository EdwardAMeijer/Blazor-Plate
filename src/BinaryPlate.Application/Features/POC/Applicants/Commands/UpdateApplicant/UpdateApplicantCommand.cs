namespace BinaryPlate.Application.Features.POC.Applicants.Commands.UpdateApplicant;

public class UpdateApplicantCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public string Id { get; set; }
    public int Ssn { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public decimal Height { get; set; }
    public decimal Weight { get; set; }

    public decimal Bmi
    {
        get => Height != 0 ? Weight / (Height / 100 * 2) : 0;
        set { if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value)); }
    }

    public string ConcurrencyStamp { get; set; }

    public List<ReferenceItemForAdd> NewApplicantReferences { get; set; } = new();
    public List<ReferenceItemForEdit> ModifiedApplicantReferences { get; set; } = new();
    public List<string> RemovedApplicantReferences { get; set; } = new();

    #endregion Public Properties

    #region Public Methods

    public void MapToEntity(Applicant applicant)
    {
        if (applicant == null)
            throw new ArgumentNullException(nameof(applicant));

        applicant.Ssn = Ssn;
        applicant.FirstName = FirstName;
        applicant.LastName = LastName;
        applicant.DateOfBirth = DateOfBirth;
        applicant.Height = Height;
        applicant.Weight = Weight;
        applicant.ConcurrencyStamp = ConcurrencyStamp;

        UpdateApplicantReferences(applicant);
    }

    #endregion Public Methods

    #region Private Methods

    private void UpdateApplicantReferences(Applicant applicant)
    {
        AddReferences(applicant);

        ModifyReferences(applicant);

        RemoveReferences(applicant);
    }

    private void RemoveReferences(Applicant applicant)
    {
        foreach (var removedReferenceId in RemovedApplicantReferences)
        {
            var reference = applicant.References.FirstOrDefault(r => r.Id == Guid.Parse(removedReferenceId));

            if (reference != null)
                applicant.References.Remove(reference);
        }
    }

    private void ModifyReferences(Applicant applicant)
    {
        foreach (var modifiedReference in ModifiedApplicantReferences)
        {
            var reference = applicant.References.FirstOrDefault(r => r.Id == Guid.Parse(modifiedReference.Id));

            if (reference != null)
            {
                reference.Name = modifiedReference.Name;
                reference.JobTitle = modifiedReference.JobTitle;
                reference.Phone = modifiedReference.Phone;
            }
        }
    }

    private void AddReferences(Applicant applicant)
    {
        NewApplicantReferences?.ForEach(r => applicant.References.Add(new Reference
        {
            Name = r.Name,
            JobTitle = r.JobTitle,
            Phone = r.Phone
        }));
    }

    #endregion Private Methods

    #region Public Classes

    public class UpdateApplicantCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<UpdateApplicantCommand, Envelope<string>>
    {
        #region Public Methods

        public async Task<Envelope<string>> Handle(UpdateApplicantCommand request, CancellationToken cancellationToken)
        {
            // Check if the request has a valid ID.
            if (string.IsNullOrEmpty(request.Id))
                return Envelope<string>.Result.BadRequest(Resource.Invalid_applicant_Id);

            // Try to parse the ID as a GUID.
            if (!Guid.TryParse(request.Id, out var applicantId))
                return Envelope<string>.Result.BadRequest(Resource.Invalid_applicant_Id);

            // Load the applicant from the database context and include their references.
            var applicant = await dbContext.Applicants.Include(a => a.References)
                                            .Where(a => a.Id == applicantId)
                                            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // Return an error response if the applicant is not found.
            if (applicant == null)
                return Envelope<string>.Result.NotFound(Resource.Unable_to_load_applicant);

            // Map the request to the loaded entity.
            request.MapToEntity(applicant);

            // Update the entity in the database context.
            dbContext.Applicants.Update(applicant);

            // Save changes to the database.
            await dbContext.SaveChangesAsync(cancellationToken);

            // Return a success response with a message.
            return Envelope<string>.Result.Ok(Resource.Applicant_has_been_updated_successfully);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}