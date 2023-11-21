namespace BinaryPlate.Application.Features.Reports.GetReports;

public class GetReportsQuery : FilterableQuery, IRequest<Envelope<GetReportsResponse>>
{
    #region Public Properties

    public ReportStatus? SelectedReportStatus { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class GetReportsQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetReportsQuery, Envelope<GetReportsResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetReportsResponse>> Handle(GetReportsQuery request, CancellationToken cancellationToken)
        {
            // Get the reports from the database.
            var query = dbContext.Reports.AsQueryable();

            // Filter reports by search text if there is any.
            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query = query.Where(q => q.Title.Contains(request.SearchText) ||
                                         q.FileName.Contains(request.SearchText) ||
                                         q.FileUri.Contains(request.SearchText) ||
                                         q.QueryString.Contains(request.SearchText));

            // Filter reports by selected status if there is any.
            query = query.Where(q => q.Status == (int)request.SelectedReportStatus ||
                                     request.SelectedReportStatus == null ||
                                     request.SelectedReportStatus == 0);

            // Sort reports by the specified field.
            query = !string.IsNullOrWhiteSpace(request.SortBy)
                ? query.SortBy(request.SortBy)
                : query.OrderByDescending(a => a.CreatedOn);

            // Get the report items as paged list.
            var reportItems = await query.Select(q => ReportItem.MapFromEntity(q))
                                         .AsNoTracking()
                                         .ToPagedListAsync(request.PageNumber, request.RowsPerPage);

            // Map the report items to response DTO.
            var response = new GetReportsResponse
            {
                Reports = reportItems
            };

            return Envelope<GetReportsResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}