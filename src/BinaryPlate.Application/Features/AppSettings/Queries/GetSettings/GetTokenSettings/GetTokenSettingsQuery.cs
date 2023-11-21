namespace BinaryPlate.Application.Features.AppSettings.Queries.GetSettings.GetTokenSettings;

public class GetTokenSettingsQuery : IRequest<Envelope<GetTokenSettingsForEditResponse>>
{
    public class GetTokenSettingsQueryHandler(IAppSettingsService appSettingsService) : IRequestHandler<GetTokenSettingsQuery, Envelope<GetTokenSettingsForEditResponse>>
    {
        #region Public Constructors



        #region Public Methods

        public async Task<Envelope<GetTokenSettingsForEditResponse>> Handle(GetTokenSettingsQuery request, CancellationToken cancellationToken)
        {
            return await appSettingsService.GetTokenSettings();
        }

        #endregion Public Methods
    }

    #endregion Public Constructors
}