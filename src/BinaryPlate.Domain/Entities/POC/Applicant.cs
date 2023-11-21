namespace BinaryPlate.Domain.Entities.POC;

/// <summary>
/// Represents an applicant.
/// </summary>
public class Applicant : IAuditable, IConcurrencyStamp
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the social security number.
    /// </summary>
    public int Ssn { get; set; }

    /// <summary>
    /// Gets or sets the first name.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets the date of birth.
    /// </summary>
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the height.
    /// </summary>
    public decimal Height { get; set; }

    /// <summary>
    /// Gets or sets the weight.
    /// </summary>
    public decimal Weight { get; set; }

    public string ConcurrencyStamp { get; set; }

    /// <summary>
    /// Gets or sets the references.
    /// </summary>
    public ICollection<Reference> References { get; set; } = new List<Reference>();

    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public string DeletedBy { get; set; }
    public DateTime? DeletedOn { get; set; }

    #endregion Public Properties
}