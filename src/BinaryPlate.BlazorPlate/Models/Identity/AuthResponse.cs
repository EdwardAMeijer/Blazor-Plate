﻿namespace BinaryPlate.BlazorPlate.Models.Identity;

public class AuthResponse
{
    #region Public Properties

    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }

    #endregion Public Properties
}