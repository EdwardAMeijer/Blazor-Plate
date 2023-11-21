namespace BinaryPlate.Application.Features.AppSettings.Commands.UpdateSettings.UpdateTokenSettings;

public class UpdateTokenSettingsCommand : IRequest<Envelope<GetTokenSettingsResponse>>
{
    #region Public Properties

    public string Id { get; set; }
    public int AccessTokenUoT { get; set; }
    public double? AccessTokenTimeSpan { get; set; }
    public int RefreshTokenUoT { get; set; }
    public double? RefreshTokenTimeSpan { get; set; }
    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties

    #region Public Methods

    public void MapToEntity(TokenSettings tokenSettings)
    {
        tokenSettings.Id = Guid.Parse(Id);
        tokenSettings.AccessTokenUoT = AccessTokenUoT;
        tokenSettings.AccessTokenTimeSpan = AccessTokenTimeSpan;
        tokenSettings.RefreshTokenUoT = RefreshTokenUoT;
        tokenSettings.RefreshTokenTimeSpan = RefreshTokenTimeSpan;
        tokenSettings.ConcurrencyStamp = ConcurrencyStamp;
    }

    #endregion Public Methods

    #region Public Classes

    public class UpdateTokenSettingsCommandHandler(IApplicationDbContext dbContext, 
                                                   IAppOptionsService appOptionsService) : IRequestHandler<UpdateTokenSettingsCommand, Envelope<GetTokenSettingsResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetTokenSettingsResponse>> Handle(UpdateTokenSettingsCommand request, CancellationToken cancellationToken)
        {
            // Check if the provided token settings id is valid.
            if (!Guid.TryParse(request.Id, out var tokenSettingsId))
                return Envelope<GetTokenSettingsResponse>.Result.BadRequest(Resource.Invalid_token_settings_Id);

            // Retrieve the token settings with the given id from the database, or fall back to the
            // default settings if not found.
            var tokenSettings = await dbContext.TokenSettings.FirstOrDefaultAsync(fs => fs.Id == tokenSettingsId, cancellationToken: cancellationToken)
                                ?? appOptionsService.GetAppTokenOptions().MapToEntity();

            // Map the properties of the command onto the token settings entity.
            request.MapToEntity(tokenSettings);

            // Update the token settings entity in the database.
            dbContext.TokenSettings.Update(tokenSettings);

            // Save changes to the database.
            await dbContext.SaveChangesAsync(cancellationToken);

            // Create a response with the updated token settings id and a success message.
            var response = new GetTokenSettingsResponse
            {
                Id = tokenSettings.Id,
                ConcurrencyStamp = tokenSettings.ConcurrencyStamp,
                SuccessMessage = Resource.Token_settings_have_been_updated_successfully
            };

            // Return the response envelope containing the token settings response.
            return Envelope<GetTokenSettingsResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}