namespace BinaryPlate.Application.Features.Reports.GetReportForEdit;

public class GetReportForEditQuery : IRequest<Envelope<GetReportForEditResponse>>
{
    #region Public Properties

    public string Id { get; set; }

    #endregion Public Properties

    public class GetReportForEditQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetReportForEditQuery, Envelope<GetReportForEditResponse>>
    {
        #region Public Methods

        #region Public Methods

        public async Task<Envelope<GetReportForEditResponse>> Handle(GetReportForEditQuery request, CancellationToken cancellationToken)
        {
            // Check if the request id is valid.
            if (!Guid.TryParse(request.Id, out var reportId))
                return Envelope<GetReportForEditResponse>.Result.BadRequest(Resource.Invalid_report_Id);

            // Find the report by its id.
            var report = await dbContext.Reports.Where(a => a.Id == reportId).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // If the report is not found, return a not found error response.
            if (report == null)
                return Envelope<GetReportForEditResponse>.Result.NotFound(Resource.Unable_to_load_report);

            // Map the report entity to response DTO.
            var reportForEditResponse = GetReportForEditResponse.MapFromEntity(report);

            return Envelope<GetReportForEditResponse>.Result.Ok(reportForEditResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Methods
}