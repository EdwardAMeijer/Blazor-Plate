﻿namespace BinaryPlate.BlazorPlate.Contracts.Services;

public interface IRefreshTokenService
{
    #region Public Methods

    Task<string> TryRefreshToken();

    #endregion Public Methods
}