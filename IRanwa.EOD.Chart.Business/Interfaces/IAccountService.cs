using IRanwa.EOD.Chart.Core;
using IRanwa.EOD.Chart.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRanwa.EOD.Chart.Business;

/// <summary>
/// Account service.
/// </summary>
public interface IAccountService
{
    /// <summary>
    /// Registers the user.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <returns>Returns response.</returns>
    Task<APIResponse> RegisterUserAsync(UserModel user);

    /// <summary>
    /// Logins the user asynchronous.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <returns>Returns response.</returns>
    Task<APIResponse> LoginUserAsync(UserModel user);

    /// <summary>
    /// Updates the user details.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns response.</returns>
    Task<APIResponse> UpdateUserDetails(UserModel model);

    /// <summary>
    /// Updates the password.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns response.</returns>
    Task<APIResponse> UpdatePassword(UserPasswordModel model);

    /// <summary>
    /// Gets the user details asynchronous.
    /// </summary>
    /// <param name="userName">Name of the user.</param>
    /// <returns>Returns response.</returns>
    APIResponse GetUserDetails(string userName);

    /// <summary>
    /// Verifies the email.
    /// </summary>
    /// <param name="verificationModel">The verification model.</param>
    /// <returns>Returns response.</returns>
    Task<APIResponse> VerifyEmailAsync(EmailVerificationModel verificationModel);

    /// <summary>
    /// Resends the email verification code asynchronous.
    /// </summary>
    /// <param name="email">The email.</param>
    /// <returns>Returns response.</returns>
    Task<APIResponse> ResendEmailVerificationCodeAsync(string email);

    /// <summary>
    /// Sends the forget password code asynchronous.
    /// </summary>
    /// <param name="email">The email.</param>
    /// <returns></returns>
    Task<APIResponse> SendForgetPasswordCodeAsync(string email);

    /// <summary>
    /// Verifies the password reset eligible.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    APIResponse VerifyPasswordResetEligible(PasswordVerificationModel model);

    /// <summary>
    /// Resets the password asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    Task<APIResponse> ResetPasswordAsync(PasswordVerificationModel model);
}
