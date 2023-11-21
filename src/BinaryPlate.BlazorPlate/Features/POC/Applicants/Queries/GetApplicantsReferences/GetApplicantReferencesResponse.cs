namespace BinaryPlate.BlazorPlate.Features.POC.Applicants.Queries.GetApplicantsReferences;

public class GetApplicantReferencesResponse
{
    #region Public Properties

    public PagedList<ApplicantReferenceItem> ApplicantReferences { get; set; }

    #endregion Public Properties
}