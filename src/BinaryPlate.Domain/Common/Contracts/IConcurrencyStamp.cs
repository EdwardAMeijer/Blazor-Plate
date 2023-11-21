namespace BinaryPlate.Domain.Common.Contracts;

/// <summary>
/// Represents an entity with a concurrency token property for optimistic concurrency control.
/// </summary>
public interface IConcurrencyStamp
{
    /// <summary>
    /// Gets or sets the concurrency token used for optimistic concurrency control.
    /// </summary>
    string ConcurrencyStamp { get; set; }
}