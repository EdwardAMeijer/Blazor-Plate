namespace BinaryPlate.Infrastructure.Services.Reporting;

public class ReportDataProvider(IApplicationDbContext applicationDbContext) : IReportDataProvider
{
    #region Public Methods

    public async Task<Envelope<dynamic>> GetApplicants(dynamic request)
    {
        var searchText = request.SearchText as string;
        var sortBy = request.SortBy as string;

        // Start with a query that retrieves all applicants and their references from the database.
        var query = applicationDbContext.Applicants.Include(a => a.References).AsQueryable();

        // If a search text is provided, filter the applicants by their first name, last name, or SSN.
        if (!string.IsNullOrWhiteSpace(searchText))
            query = query.Where(a =>
                a.FirstName.Contains(searchText) ||
                a.LastName.Contains(searchText) ||
                a.Ssn.ToString().Contains(searchText));

        // If a sort by field is provided, sort the query by that field; otherwise, sort by first name and then last name.
        query = !string.IsNullOrWhiteSpace(sortBy)
            ? query.SortBy(sortBy)  // Ensure SortBy is a valid extension method
            : query.OrderBy(a => a.FirstName).ThenBy(a => a.LastName);

        // Convert the query to a paged list of applicant item DTOs.
        var applicantItems = await query.Select(q => new
        {
            q.Id,
            q.Ssn,
            q.FirstName,
            q.LastName,
            q.DateOfBirth,
            q.Height,
            q.Weight,
            q.CreatedOn,
            q.CreatedBy,
            q.ModifiedOn,
            q.ModifiedBy,
            References = q.References.Select(r => new
            {
                r.Name,
                r.JobTitle,
                r.Phone,
                r.CreatedBy,
                r.CreatedOn,
                r.ModifiedBy,
                r.ModifiedOn
            }).ToList()  // Materialize the References collection
        }).AsNoTracking().ToPagedListAsync();

        // Create an applicants response DTO with the list of applicant item DTOs.
        var response = new
        {
            Applicants = applicantItems
        };

        // Return a success response with the applicants response DTO as the payload.
        return Envelope<dynamic>.Result.Ok(response);
    }

    #endregion Public Methods
}