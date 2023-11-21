namespace BinaryPlate.Application.Features.POC.Applicants.Queries.GetApplicants;

public class GetApplicantsResponse
{
    #region Public Properties

    public PagedList<ApplicantItem> Applicants { get; set; }

    #endregion Public Properties
}