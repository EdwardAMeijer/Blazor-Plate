namespace BinaryPlate.Application.Features.AppSettings.Commands.UpdateSettings.UpdateTokenSettings
{
    public class GetTokenSettingsResponse
    {
        #region Public Properties

        public Guid Id { get; set; }
        public string SuccessMessage { get; set; }
        public string ConcurrencyStamp { get; set; }

        #endregion Public Properties
    }
}