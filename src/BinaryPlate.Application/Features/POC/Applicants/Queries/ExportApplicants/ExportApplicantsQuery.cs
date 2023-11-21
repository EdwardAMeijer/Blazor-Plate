namespace BinaryPlate.Application.Features.POC.Applicants.Queries.ExportApplicants
{
    public class ExportApplicantsQuery : IRequest<Envelope<ExportApplicantsResponse>>
    {
        #region Public Properties

        public string SearchText { get; set; }
        public string SortBy { get; set; }

        #endregion Public Properties

        #region Public Classes

        public class ExportApplicantsHandler(IReportDataProvider reportDataProvider,
                                             IOnDemandReportingService onDemandReportingService,
                                             IHttpContextAccessor httpContextAccessor) : IRequestHandler<ExportApplicantsQuery, Envelope<ExportApplicantsResponse>>
        {
            #region Public Methods

            public async Task<Envelope<ExportApplicantsResponse>> Handle(ExportApplicantsQuery request, CancellationToken cancellationToken)
            {
                // Retrieve the list of applicants based on the provided search criteria and sorting order.
                var applicantResponse = await reportDataProvider.GetApplicants(new GetApplicantsQuery
                {
                    SearchText = request.SearchText,
                    SortBy = request.SortBy
                });

                // Get the name of the current user who initiated the export.
                var currentUserName = httpContextAccessor.GetUserName();

                // Extract the issuer name by splitting the current user's name at the '@' symbol and using the first part.
                var issuerName = currentUserName.Split("@")[0];

                // Acquire the base URL of the application from the HTTP context accessor.
                var baseUrl = $"{httpContextAccessor.HttpContext?.Request.Scheme}://{httpContextAccessor.HttpContext?.Request.Host}";

                // Export the list of applicants as a PDF file using the reporting service and retrieve file metadata.
                var fileMetaData = await onDemandReportingService.ExportDataAsPdfOnDemand(applicantResponse.Payload, issuerName, baseUrl);

                // Create an instance of ExportApplicantsResponse with file metadata.
                var payload = new ExportApplicantsResponse
                {
                    SuccessMessage = string.Format(Resource.The_report_0_is_being_downloaded, fileMetaData.FileName as string),
                    ContentType = fileMetaData.ContentType as string,
                    FileName = fileMetaData.FileName as string,
                    FileUri = fileMetaData.FileUri as string,
                };

                // Return the PDF file response within an Envelope.
                return Envelope<ExportApplicantsResponse>.Result.Ok(payload);
            }

            #endregion Public Methods
        }

        #endregion Public Classes
    }
}