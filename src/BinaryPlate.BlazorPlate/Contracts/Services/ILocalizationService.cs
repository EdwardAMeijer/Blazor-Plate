namespace BinaryPlate.BlazorPlate.Contracts.Services;

public interface ILocalizationService
{
    #region Public Methods

    string GetString(string key);

    #endregion Public Methods
}