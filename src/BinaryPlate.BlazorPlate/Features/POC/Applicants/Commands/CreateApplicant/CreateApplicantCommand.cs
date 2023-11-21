namespace BinaryPlate.BlazorPlate.Features.POC.Applicants.Commands.CreateApplicant;

public class CreateApplicantCommand
{
    #region Public Properties

    public int? Ssn { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public decimal? Height { get; set; }
    public decimal? Weight { get; set; }

    public decimal? Bmi
    {
        get => Height != 0 ? Weight / (Height / 100 * 2) : 0;
        set { }
    }

    public List<ReferenceItemForAdd> ReferenceItems { get; set; } = new();

    #endregion Public Properties
}