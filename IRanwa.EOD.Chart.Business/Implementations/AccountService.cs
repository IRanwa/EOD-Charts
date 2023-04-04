using IRanwa.EOD.Chart.Core;
using IRanwa.EOD.Chart.Data;
using IRanwa.EOD.Chart.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace IRanwa.EOD.Chart.Business;

/// <summary>
/// Account service
/// <seealso cref="IAccountService"/>
/// </summary>
public class AccountService : IAccountService
{
    /// <summary>
    /// The user manager
    /// </summary>
    private readonly UserManager<ApplicationUser> userManager;

    /// <summary>
    /// The configuration
    /// </summary>
    private readonly IConfiguration configuration;

    /// <summary>
    /// The unit of work
    /// </summary>
    private readonly IUnitOfWorkAsync unitOfWork;

    /// <summary>
    /// The principal user
    /// </summary>
    private readonly IPrincipal principalUser;

    /// <summary>
    /// The user email provider
    /// </summary>
    private readonly IUserEmailProviderService userEmailProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountService"/> class.
    /// </summary>
    /// <param name="userManager">The user manager.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="principalUser">The principal user.</param>
    /// <param name="userEmailProvider">The user email provider.</param>
    public AccountService(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        IUnitOfWorkAsync unitOfWork,
        IPrincipal principalUser,
        IUserEmailProviderService userEmailProvider
    )
    {
        this.userManager = userManager;
        this.configuration = configuration;
        this.unitOfWork = unitOfWork;
        this.principalUser = principalUser;
        this.userEmailProvider = userEmailProvider;
    }

    /// <summary>
    /// Registers the user.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <returns>
    /// Returns response.
    /// </returns>
    public async Task<APIResponse> RegisterUserAsync(UserModel user)
    {
        if (user == null)
            return new APIResponse() { IsSuccess = false, Message = PortalRes.Message_UserRegistrationFailed };

        var applicationUser = new ApplicationUser()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            UserName = user.Email,
            CreatedUser = string.IsNullOrEmpty(user.CreatedUser) ? String.Empty : user.CreatedUser,
            CreatedDateTime = DateTime.Now,
            EmailConfirmed = false
        };

        var identityResult = await userManager.CreateAsync(applicationUser, user.Password);
        if (!identityResult.Succeeded)
        {
            var error = identityResult.Errors.FirstOrDefault();
            if (error != null && !string.IsNullOrEmpty(error.Description))
                return new APIResponse() { IsSuccess = false, Message = error.Description, Data = null };

            return new APIResponse() { IsSuccess = false, Message = PortalRes.Message_UserRegistrationFailed };
        }

        var roleSaveResult = await userManager.AddToRoleAsync(applicationUser, user.Role.ToString());
        if (!roleSaveResult.Succeeded)
        {
            var error = roleSaveResult.Errors.FirstOrDefault();
            if (error != null && !string.IsNullOrEmpty(error.Description))
                return new APIResponse() { IsSuccess = false, Message = error.Description, Data = null };

            return new APIResponse() { IsSuccess = false, Message = PortalRes.Message_UserRegistrationFailed };
        }

        string code = await userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
        if (string.IsNullOrEmpty(code))
            return new APIResponse() { IsSuccess = false, Message = PortalRes.Message_UserRegistrationFailed };

        userEmailProvider.UserRegistrationEmail(user.FirstName + " " + user.LastName, user.Email, code);
        await unitOfWork.GetGenericRepository<AspNetEmailVerifications>().Add(
        new AspNetEmailVerifications()
        {
            Email = user.Email,
            ExpireDateTime = DateTime.UtcNow.AddDays(int.Parse(configuration["MailSettings:EmailVerificationExpireInDays"])),
            UserId = applicationUser.Id,
            Code = code,
            IsActive = true
        });

        unitOfWork.SaveChanges();
        return new APIResponse() { IsSuccess = true, Message = PortalRes.Message_UserRegistrationSuccess, Data = null };
    }

    /// <summary>
    /// Logins the user asynchronous.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <returns>Returns response.</returns>
    public async Task<APIResponse> LoginUserAsync(UserModel user)
    {
        if (user == null)
            return new APIResponse() { IsSuccess = false, Message = PortalRes.Message_UserLoginFailed };

        var loginUser = unitOfWork.GetGenericRepository<ApplicationUser>().GetOne(userInfo => userInfo.Email == user.Email);
        if (loginUser == null)
            return new APIResponse() { IsSuccess = false, Message = PortalRes.Message_UserLoginCredentialsIncorrect };

        if (!loginUser.EmailConfirmed)
            return new APIResponse() { IsSuccess = false, Message = PortalRes.Message_UserLoginEmailNotVerified, Data = new { EmailConfirmed = false } };

        var passwordMatch = await userManager.CheckPasswordAsync(loginUser, user.Password);
        if (!passwordMatch)
            return new APIResponse() { IsSuccess = false, Message = PortalRes.Message_UserLoginCredentialsIncorrect };

        var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, $"{loginUser.FirstName} {loginUser.LastName}"),
                    new Claim(ClaimTypes.Name, loginUser.Id),
                    new Claim(ClaimTypes.Email, loginUser.Email)
                };

        var token = GetToken(authClaims);

        loginUser.LastActive = DateTime.UtcNow;
        unitOfWork.SaveChanges();
        return new APIResponse() { IsSuccess = true, Message = PortalRes.Message_UserLoginSuccess, Data = new JwtSecurityTokenHandler().WriteToken(token) };
    }

    /// <summary>
    /// Updates the user details.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns response.</returns>
    public async Task<APIResponse> UpdateUserDetails(UserModel model)
    {
        var user = unitOfWork.GetGenericRepository<ApplicationUser>().GetOne(userInfo => userInfo.UserName == model.Email);
        if (user != null)
        {
            InsertApplicationUserHistory(user);
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
                return new APIResponse() { IsSuccess = true, Message = PortalRes.Message_UserProfileUpdateSuccess };
            else
            {
                var error = result.Errors.FirstOrDefault();
                if (error != null && !string.IsNullOrEmpty(error.Description))
                    return new APIResponse() { IsSuccess = false, Message = error.Description, Data = null };
            }
        }
        return new APIResponse() { IsSuccess = false, Message = PortalRes.Message_UserProfileUpdateFailed };
    }

    /// <summary>
    /// Updates the password.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>
    /// Returns response.
    /// </returns>
    public async Task<APIResponse> UpdatePassword(UserPasswordModel model)
    {
        var user = unitOfWork.GetGenericRepository<ApplicationUser>().GetOne(userInfo => userInfo.UserName == model.Username);
        if (user != null)
        {
            InsertApplicationUserHistory(user);
            var result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.Password);
            if (result.Succeeded)
            {
                userEmailProvider.UserPasswordChangeEmail($"{user.FirstName} {user.LastName}", user.Email);
                return new APIResponse() { IsSuccess = true, Message = PortalRes.Message_UserProfileUpdateSuccess };
            }
            else
            {
                var error = result.Errors.FirstOrDefault();
                if (error != null && !string.IsNullOrEmpty(error.Description))
                    return new APIResponse() { IsSuccess = false, Message = error.Description, Data = null };
            }
        }
        return new APIResponse() { IsSuccess = false, Message = PortalRes.Message_UserProfileUpdateFailed };

    }

    /// <summary>
    /// Gets the user details.
    /// </summary>
    /// <param name="userName">Name of the user.</param>
    /// <returns>Returns response.</returns>
    public APIResponse GetUserDetails(string userName)
    {
        var user = unitOfWork.GetGenericRepository<ApplicationUser>().GetOne(userInfo => userInfo.UserName == userName);
        if (user != null)
        {
            var applicationUser = new UserViewModel()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserImageContent = user.UserImageContent
            };
            return new APIResponse() { IsSuccess = true, Message = string.Empty, Data = applicationUser };
        }
        return new APIResponse() { IsSuccess = false, Message = PortalRes.Message_UserDetailsRetrieveFailed, Data = null };
    }

    /// <summary>
    /// Gets the token.
    /// </summary>
    /// <param name="authClaims">The authentication claims.</param>
    /// <returns>Returns JWT token.</returns>
    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

        var token = new JwtSecurityToken(
            issuer: configuration["JWT:ValidIssuer"],
            audience: configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddMinutes(int.Parse(configuration["JWT:ExpiresInMinutes"])),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
        return token;
    }

    /// <summary>
    /// Inserts the application user history.
    /// </summary>
    /// <param name="user">The user.</param>
    private void InsertApplicationUserHistory(ApplicationUser user)
    {
        var userHistory = new AspNetUsersHistory()
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserId = user.Id,
            UserImageContent = user.UserImageContent,
            CreatedDateTime = DateTime.Now,
            CreatedUser = principalUser.Identity?.Name,
            PasswordHash = user.PasswordHash,
        };
        unitOfWork.GetGenericRepository<AspNetUsersHistory>().Add(userHistory);
        unitOfWork.SaveChanges();
    }

    /// <summary>
    /// Sends the forget password code asynchronous.
    /// </summary>
    /// <param name="email">The email.</param>
    /// <returns>Returns response.</returns>
    public async Task<APIResponse> SendForgetPasswordCodeAsync(string email)
    {
        var applicationUser = unitOfWork.GetGenericRepository<ApplicationUser>().GetOne(userInfo => userInfo.Email == email);
        if (applicationUser == null)
            return new APIResponse() { IsSuccess = false, Message = "User forget password request failed", Data = null };

        var forgetPasswordRequests = unitOfWork.GetGenericRepository<AspNetForgetPassword>()
            .GetQueryable(x => x.UserId == applicationUser.Id && x.IsActive, null);
        foreach (var request in forgetPasswordRequests)
        {
            request.IsActive = false;
            request.ModifiedDateTime = DateTime.Now;
            unitOfWork.GetGenericRepository<AspNetForgetPassword>().Update(request);
        }

        string code = await userManager.GeneratePasswordResetTokenAsync(applicationUser);
        if (string.IsNullOrEmpty(code))
            return new APIResponse() { IsSuccess = false, Message = "User forget password request failed" };

        userEmailProvider.UserPasswordResetEmail(applicationUser.FirstName + " " + applicationUser.LastName, applicationUser.Email, code);
        await unitOfWork.GetGenericRepository<AspNetForgetPassword>().Add(
        new AspNetForgetPassword()
        {
            Email = applicationUser.Email,
            ExpireDateTime = DateTime.UtcNow.AddMinutes(int.Parse(configuration["MailSettings:PasswordResetExpireInMins"])),
            UserId = applicationUser.Id,
            Code = code,
            IsActive = true
        });
        unitOfWork.SaveChanges();

        return new APIResponse() { IsSuccess = true, Message = "User forget password request sent", Data = null };
    }

    /// <summary>
    /// Verifies the password reset eligible.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns response.</returns>
    public APIResponse VerifyPasswordResetEligible(PasswordVerificationModel model)
    {
        var forgetPasswordRequests = unitOfWork.GetGenericRepository<AspNetForgetPassword>()
            .GetQueryable(x => x.Code == model.Code, null).FirstOrDefault();
        if (forgetPasswordRequests != null)
        {
            if (forgetPasswordRequests.IsUsed)
                return new APIResponse() { IsSuccess = false, Message = "Forget password request already used", Data = null };
            if (forgetPasswordRequests.ExpireDateTime < DateTime.UtcNow)
                return new APIResponse() { IsSuccess = false, Message = "Forget password request already expired", Data = null };

            return new APIResponse() { IsSuccess = true, Data = new { Username = forgetPasswordRequests.Email } };
        }
        return new APIResponse() { IsSuccess = false, Message = "Invalid forget password request", Data = null };
    }

    /// <summary>
    /// Resets the password asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns response.</returns>
    public async Task<APIResponse> ResetPasswordAsync(PasswordVerificationModel model)
    {
        var forgetPasswordRequests = unitOfWork.GetGenericRepository<AspNetForgetPassword>()
            .GetQueryable(x => x.Code == model.Code, null).FirstOrDefault();
        if (forgetPasswordRequests != null)
        {
            var isEligibleRes = VerifyPasswordResetEligible(model);
            if (!isEligibleRes.IsSuccess)
                return isEligibleRes;

            forgetPasswordRequests.IsActive = false;
            forgetPasswordRequests.IsUsed = true;
            unitOfWork.SaveChanges();

            var user = unitOfWork.GetGenericRepository<ApplicationUser>().GetOne(userInfo => userInfo.UserName == forgetPasswordRequests.Email);
            if (user != null)
            {
                InsertApplicationUserHistory(user);
                var result = await userManager.ResetPasswordAsync(user, model.Code, model.Password);
                if (result.Succeeded)
                {
                    userEmailProvider.UserPasswordChangeEmail(user.FirstName + " " + user.LastName, user.Email);
                    return new APIResponse() { IsSuccess = true, Message = "User password reset success" };
                }
                else
                {
                    var error = result.Errors.FirstOrDefault();
                    if (error != null && !string.IsNullOrEmpty(error.Description))
                        return new APIResponse() { IsSuccess = false, Message = error.Description, Data = null };
                }
            }
            return new APIResponse() { IsSuccess = false, Message = "User password reset failed" };
        }
        return new APIResponse() { IsSuccess = false, Message = "User password reset failed" };
    }

    /// <summary>
    /// Verifies the email.
    /// </summary>
    /// <param name="verificationModel">The verification model.</param>
    /// <returns>
    /// Returns response.
    /// </returns>
    public async Task<APIResponse> VerifyEmailAsync(EmailVerificationModel verificationModel)
    {
        var emailVerification = unitOfWork.GetGenericRepository<AspNetEmailVerifications>().GetOne(email => email.Code == verificationModel.Code && email.IsActive);
        if (emailVerification == null)
            return new APIResponse() { IsSuccess = false, Message = PortalRes.Message_UserEmailVerificationFailed, Data = null };

        if (emailVerification.ExpireDateTime < DateTime.UtcNow)
        {
            return await ResendEmailVerificationCodeAsync(emailVerification.Email);
            //return new APIResponse() { IsSuccess = false, Message = PortalRes.Message_UserEmailVerificationFailed, Data = new { VerificationExpired = true, Email = emailVerification.Email } };
        }

        if (emailVerification.IsVerified)
            return new APIResponse() { IsSuccess = false, Message = PortalRes.Message_UserEmailAlreadyVerified, Data = new { AlreadyVerified = true } };

        //var applicationUser = await userManager.FindByIdAsync(emailVerification.UserId);
        var applicationUser = unitOfWork.GetGenericRepository<ApplicationUser>().GetOne(userInfo => userInfo.Id == emailVerification.UserId);
        applicationUser.EmailConfirmed = true;
        await userManager.UpdateAsync(applicationUser);

        emailVerification.IsVerified = true;
        unitOfWork.GetGenericRepository<AspNetEmailVerifications>().Update(emailVerification);
        unitOfWork.SaveChanges();

        return new APIResponse() { IsSuccess = true, Message = PortalRes.Message_UserEmailVerificationSuccess, Data = null };
    }

    /// <summary>
    /// Resends the email verification code asynchronous.
    /// </summary>
    /// <param name="email">The email.</param>
    /// <returns>Returns response.</returns>
    public async Task<APIResponse> ResendEmailVerificationCodeAsync(string email)
    {
        //var applicationUser = await userManager.FindByEmailAsync(email);
        var applicationUser = unitOfWork.GetGenericRepository<ApplicationUser>().GetOne(userInfo => userInfo.Email == email);
        if (applicationUser == null)
            return new APIResponse() { IsSuccess = false, Message = PortalRes.Message_UserEmailResendFailed, Data = null };

        var emailVerificationRequests = unitOfWork.GetGenericRepository<AspNetEmailVerifications>()
            .GetQueryable(x => x.UserId == applicationUser.Id && x.IsActive, null);
        foreach (var verification in emailVerificationRequests)
        {
            verification.IsActive = false;
            verification.ModifiedDateTime = DateTime.Now;
            unitOfWork.GetGenericRepository<AspNetEmailVerifications>().Update(verification);
        }

        string code = await userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
        if (string.IsNullOrEmpty(code))
            return new APIResponse() { IsSuccess = false, Message = PortalRes.Message_UserEmailResendFailed };

        userEmailProvider.UserRegistrationEmail(applicationUser.FirstName + " " + applicationUser.LastName, applicationUser.Email, code);
        await unitOfWork.GetGenericRepository<AspNetEmailVerifications>().Add(
        new AspNetEmailVerifications()
        {
            Email = applicationUser.Email,
            ExpireDateTime = DateTime.UtcNow.AddDays(int.Parse(configuration["MailSettings:EmailVerificationExpireInDays"])),
            UserId = applicationUser.Id,
            Code = code,
            IsActive = true
        });
        unitOfWork.SaveChanges();

        return new APIResponse() { IsSuccess = true, Message = PortalRes.Message_UserEmailResendSuccess, Data = null };
    }
}

