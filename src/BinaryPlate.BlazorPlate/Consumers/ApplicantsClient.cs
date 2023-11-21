namespace BinaryPlate.BlazorPlate.Consumers;

public class ApplicantsClient(IHttpService httpService) : IApplicantsClient
{
    #region Public Methods

    public async Task<ApiResponseWrapper<ApplicantForEdit>> GetApplicant(GetApplicantForEditQuery request)
    {
        return await httpService.Post<GetApplicantForEditQuery, ApplicantForEdit>("applicants/GetApplicant", request);
    }

    public async Task<ApiResponseWrapper<GetApplicantReferencesResponse>> GetApplicantReferences(GetApplicantReferencesQuery getApplicantReferencesQuery)
    {
        return await httpService.Post<GetApplicantReferencesQuery, GetApplicantReferencesResponse>("applicants/GetApplicantReferences", getApplicantReferencesQuery);
    }

    public async Task<ApiResponseWrapper<GetApplicantsResponse>> GetApplicants(GetApplicantsQuery request)
    {
        return await httpService.Post<GetApplicantsQuery, GetApplicantsResponse>("applicants/GetApplicants", request);
    }

    public async Task<ApiResponseWrapper<CreateApplicantResponse>> CreateApplicant(CreateApplicantCommand request)
    {
        return await httpService.Post<CreateApplicantCommand, CreateApplicantResponse>("applicants/CreateApplicant", request);
    }

    public async Task<ApiResponseWrapper<string>> UpdateApplicant(UpdateApplicantCommand request)
    {
        return await httpService.Put<UpdateApplicantCommand, string>("applicants/UpdateApplicant", request);
    }

    public async Task<ApiResponseWrapper<string>> DeleteApplicant(string id)
    {
        return await httpService.Delete<string>($"applicants/DeleteApplicant?id={id}");
    }

    public async Task<ApiResponseWrapper<ExportApplicantsResponse>> ExportAsPdf(ExportApplicantsQuery request)
    {
        return await httpService.Post<ExportApplicantsQuery, ExportApplicantsResponse>("applicants/ExportAsPdf", request);
    }

    #endregion Public Methods
}