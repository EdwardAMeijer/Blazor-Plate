namespace BinaryPlate.BlazorPlate.Features.POC.Applicants.Queries.ExportApplicants;

public class ExportApplicantsResponse
{
    #region Public Properties

    public string Id { get; set; }
    public string SuccessMessage { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public string FileUri { get; set; }

    #endregion Public Properties
}