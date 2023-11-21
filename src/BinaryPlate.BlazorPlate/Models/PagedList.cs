namespace BinaryPlate.BlazorPlate.Models;

public class PagedList<T>
{
    #region Public Properties

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalRowsPerPage { get; set; }
    public int TotalRows { get; set; }
    public IList<T> Items { get; set; } = new List<T>();

    #endregion Public Properties
}