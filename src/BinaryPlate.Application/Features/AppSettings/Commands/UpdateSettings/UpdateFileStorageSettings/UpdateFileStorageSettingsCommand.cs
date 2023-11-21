namespace BinaryPlate.Application.Features.AppSettings.Commands.UpdateSettings.UpdateFileStorageSettings;

public class UpdateFileStorageSettingsCommand : IRequest<Envelope<GetFileStorageSettingsResponse>>
{
    #region Public Properties

    public string Id { get; set; }
    public int StorageType { get; set; }
    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties

    #region Public Methods

    public void MapToEntity(FileStorageSettings fileStorageSettings)
    {
        fileStorageSettings.Id = Guid.Parse(Id);
        fileStorageSettings.StorageType = (StorageTypes)Enum.Parse(typeof(StorageTypes), StorageType.ToString(), true);
        fileStorageSettings.ConcurrencyStamp = ConcurrencyStamp;
    }

    #endregion Public Methods

    #region Public Classes

    public class UpdateFileStorageSettingsCommandHandler(IApplicationDbContext dbContext,
                                                         IAppOptionsService appOptionsService) : IRequestHandler<UpdateFileStorageSettingsCommand, Envelope<GetFileStorageSettingsResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetFileStorageSettingsResponse>> Handle(UpdateFileStorageSettingsCommand request, CancellationToken cancellationToken)
        {
            // Parse file storage settings ID from request.
            if (!Guid.TryParse(request.Id, out var fileStorageSettingsId))
                return Envelope<GetFileStorageSettingsResponse>.Result.BadRequest(Resource.Invalid_file_storage_Id);

            // Get the file storage settings entity from the database, or create a new one from the
            // app settings.
            var fileStorageSettings = await dbContext.FileStorageSettings.FirstOrDefaultAsync(fs => fs.Id == fileStorageSettingsId, cancellationToken: cancellationToken)
                                      ?? appOptionsService.GetAppFileStorageOptions().MapToEntity();

            // Map the request data to the entity.
            request.MapToEntity(fileStorageSettings);

            // Update the entity in the database.
            dbContext.FileStorageSettings.Update(fileStorageSettings);
            await dbContext.SaveChangesAsync(cancellationToken);

            // Create a response containing the ID of the updated file storage settings and a
            // success message.
            var response = new GetFileStorageSettingsResponse
            {
                Id = fileStorageSettings.Id,
                ConcurrencyStamp = fileStorageSettings.ConcurrencyStamp,
                SuccessMessage = Resource.File_storage_settings_have_been_updated_successfully
            };
            return Envelope<GetFileStorageSettingsResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}