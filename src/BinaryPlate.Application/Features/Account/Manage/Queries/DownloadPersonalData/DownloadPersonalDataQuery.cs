﻿namespace BinaryPlate.Application.Features.Account.Manage.Queries.DownloadPersonalData;

public class DownloadPersonalDataQuery : IRequest<Envelope<DownloadPersonalDataResponse>>
{
    #region Public Classes

    public class DownloadPersonalDataQueryHandler(ApplicationUserManager userManager, 
                                                  IHttpContextAccessor httpContextAccessor) : IRequestHandler<DownloadPersonalDataQuery, Envelope<DownloadPersonalDataResponse>>
    {
        #region Public Methods

        public async Task<Envelope<DownloadPersonalDataResponse>> Handle(DownloadPersonalDataQuery request, CancellationToken cancellationToken)
        {
            // Get the current user ID from the HttpContext.
            var userId = httpContextAccessor.GetUserId();

            // If the user ID is missing or invalid, return a bad request response.
            if (string.IsNullOrEmpty(userId))
                return Envelope<DownloadPersonalDataResponse>.Result.BadRequest(Resource.Invalid_user_Id);

            // Find the user with the specified ID.
            var user = await userManager.FindByIdAsync(userId);

            // If the user cannot be found, return an unauthorized response.
            if (user == null)
                return Envelope<DownloadPersonalDataResponse>.Result.Unauthorized(Resource.Unable_to_load_user);

            // Only include personal data for download.
            var personalDataProps = typeof(IdentityUser).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));

            // Create a dictionary containing the user's personal data.
            var personalData = personalDataProps.ToDictionary(p => p.Name, p => p.GetValue(user)?.ToString() ?? "null");

            // Include external login provider keys in the personal data.
            var logins = await userManager.GetLoginsAsync(user);

            foreach (var loginInfo in logins)
                personalData.Add($"{loginInfo.LoginProvider} external login provider key", loginInfo.ProviderKey);

            // Generate a file name for the downloaded personal data based on the user's email address.
            var username = user.Email.ToUrlFriendlyString();
            var fileName = $"PersonalData-{username}";

            // Serialize the personal data to JSON and create a download response.
            var response = new DownloadPersonalDataResponse
            {
                PersonalData = JsonSerializer.SerializeToUtf8Bytes(personalData),
                FileName = fileName,
                ContentType = "application/json"
            };

            // Return a success response with the download personal data.
            return Envelope<DownloadPersonalDataResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}