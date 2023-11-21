namespace BinaryPlate.BlazorPlate.Features.POC.Applicants.Commands.UpdateApplicant;

public class UpdateApplicantCommand
{
    #region Public Properties

    public string Id { get; set; }
    public int Ssn { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public decimal? Height { get; set; }
    public decimal? Weight { get; set; }

    public decimal? Bmi
    {
        get => Height != 0 ? Weight / (Height / 100 * 2) : 0;
        set { if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value)); }
    }

    public string ConcurrencyStamp { get; set; }

    public List<ReferenceItemForAdd> NewApplicantReferences { get; set; } = new();
    public List<ReferenceItemForEdit> ModifiedApplicantReferences { get; set; } = new();
    public List<string> RemovedApplicantReferences { get; set; } = new();

    #endregion Public Properties
}