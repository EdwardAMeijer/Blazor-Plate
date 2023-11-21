namespace BinaryPlate.Application.Common.Extensions;

public static class IdentityResultExtensions
{
    #region Public Methods

    /// <summary>
    /// Converts the SignInResult to a dictionary of key value pairs where the key is the name of.
    /// the property and the value is a localized error message.
    /// </summary>
    /// <param name="signInResult">The result of a sign-in attempt.</param>
    /// <returns>
    /// A dictionary of key value pairs where the key is the name of the property and the value is
    /// a. localized error message.
    /// </returns>
    public static Dictionary<string, string> ToApplicationResult(this SignInResult signInResult)
    {
        var keyValuePairs = new Dictionary<string, string>();

        if (signInResult.IsLockedOut)
            keyValuePairs.Add(nameof(signInResult.IsLockedOut), Resource.You_are_locked_out);

        if (signInResult.IsNotAllowed)
            keyValuePairs.Add(nameof(signInResult.IsNotAllowed), Resource.Please_confirm_your_email);

        if (signInResult.RequiresTwoFactor)
            keyValuePairs.Add(nameof(signInResult.RequiresTwoFactor), Resource.Two_factor_authentication_required);

        if (!signInResult.Succeeded)
            keyValuePairs.Add("Failed", Resource.Invalid_login_attempt);
        return keyValuePairs;
    }

    /// <summary>
    /// Converts a collection of <see cref="IdentityError"/> instances to a <see
    /// cref="Dictionary{TKey,TValue}"/> instance.
    /// </summary>
    /// <param name="identityErrors">
    /// The collection of <see cref="IdentityError"/> instances to convert.
    /// </param>
    /// <returns>
    /// A <see cref="Dictionary{TKey,TValue}"/> instance representing the <paramref name="identityErrors"/>.
    /// </returns>

    public static Dictionary<string, string> ToApplicationResult(this IEnumerable<IdentityError> identityErrors)
    {
        return identityErrors.ToDictionary(identityError => identityError.Code, identityError => identityError.Description);
    }

    #endregion Public Methods
}