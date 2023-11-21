﻿namespace BinaryPlate.Application.Features.Identity.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<Envelope<CreateUserResponse>>
{
    #region Public Properties

    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }

    public string AvatarUri { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string PhoneNumber { get; set; }
    public int NumberOfAttachments { get; set; }
    public bool IsAvatarAdded { get; set; }
    public bool SetRandomPassword { get; set; }
    public bool MustSendActivationEmail { get; set; }
    public bool IsSuperAdmin { get; set; }
    public bool IsSuspended { get; set; }
    public bool IsStatic { get; set; }
    public IReadOnlyList<string> AssignedRoleIds { get; set; }
    public IReadOnlyList<string> AttachmentUris { get; set; }
    public string JobTitle { get; set; }

    #endregion Public Properties

    #region Public Methods

    public ApplicationUser MapToEntity()
    {
        var user = new ApplicationUser
        {
            UserName = Email,
            Email = Email,
            AvatarUri = AvatarUri,
            Name = Name,
            Surname = Surname,
            JobTitle = JobTitle,
            PhoneNumber = PhoneNumber,
            IsSuperAdmin = IsSuperAdmin,
            IsSuspended = IsSuspended,
            IsStatic = IsStatic,
            UserAttachments = AttachmentUris.Select(fileUri => new ApplicationUserAttachment
            {
                FileName = fileUri.Split("/").Last(),
                FileUri = fileUri
            }).ToList()
        };
        return user;
    }

    #endregion Public Methods

    public class CreateUserCommandHandler(ApplicationUserManager userManager, ApplicationRoleManager roleManager) : IRequestHandler<CreateUserCommand, Envelope<CreateUserResponse>>
    {
        #region Public Methods

        #region Public Methods

        public async Task<Envelope<CreateUserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Map the CreateUserCommand to a user entity.
            var user = request.MapToEntity();

            // If the SetRandomPassword flag is set, generate a random password for the user.
            if (request.SetRandomPassword)
                request.Password = userManager.GenerateRandomPassword(6);

            // Assign roles to the user.
            await AssignRolesToUser(request.AssignedRoleIds, user);

            // Create the user with the UserManager.
            var identityResult = await userManager.CreateAsync(user, request.Password);

            // If the create user operation fails, return an error envelope.
            if (!identityResult.Succeeded)
                return Envelope<CreateUserResponse>.Result.AddErrors(identityResult.Errors.ToApplicationResult(), HttpStatusCode.InternalServerError);

            // If the MustSendActivationEmail flag is set, send an activation email to the user.
            if (request.MustSendActivationEmail)
            {
                await userManager.SendActivationEmailAsync(user);
            }
            else
            {
                // Otherwise, mark the user's email as confirmed and update the user.
                user.EmailConfirmed = true;
                await userManager.UpdateAsync(user);
            }

            // Create a CreateUserResponse object and return an OK envelope with it.
            var response = new CreateUserResponse
            {
                Id = user.Id,
                SuccessMessage = Resource.User_has_been_created_successfully
            };
            return Envelope<CreateUserResponse>.Result.Ok(response);
        }

        private async Task AssignRolesToUser(IReadOnlyList<string> assignedRoleIds, ApplicationUser user)
        {
            // Retrieve all roles that are marked as default.
            var defaultRoles = await roleManager.Roles.Where(r => r.IsDefault).Select(r => r.Id).ToListAsync();

            // Assign the given role IDs to the user, along with the default roles.
            userManager.AssignRolesToUser(assignedRoleIds, user, defaultRoles);
        }

        #endregion Public Methods
    }

    #endregion Public Methods
}