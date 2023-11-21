namespace BinaryPlate.Application.Features.AppSettings.Queries.GetSettings.GetFileStorageSettings;

public class GetFileStorageSettingsQuery : IRequest<Envelope<GetFileStorageSettingsForEditResponse>>
{
    #region Public Classes

    public class GetFileStorageSettingsQueryHandler(IAppSettingsService appSettingsService) : IRequestHandler<GetFileStorageSettingsQuery, Envelope<GetFileStorageSettingsForEditResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetFileStorageSettingsForEditResponse>> Handle(GetFileStorageSettingsQuery request, CancellationToken cancellationToken)
        {
            return await appSettingsService.GetFileStorageSettings();
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}