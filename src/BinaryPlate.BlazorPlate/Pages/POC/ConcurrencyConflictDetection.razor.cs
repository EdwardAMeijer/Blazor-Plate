namespace BinaryPlate.BlazorPlate.Pages.POC
{
    public partial class ConcurrencyConflictDetection
    {
        #region Private Properties

        [Inject] private IBreadcrumbService BreadcrumbService { get; set; }

        #endregion Private Properties

        #region Protected Methods

        protected override void OnInitialized()
        {
            BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
                                                 {
                                                     new(Resource.Home, "/"),
                                                     new(Resource.Proof_of_Concepts, "#", true),
                                                     new(Resource.ConcurrencyConflictDetection, "#", true)
                                                 });
        }

        #endregion Protected Methods
    }
}