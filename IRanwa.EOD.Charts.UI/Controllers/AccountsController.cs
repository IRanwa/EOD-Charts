using IRanwa.EOD.Chart.Business;
using IRanwa.EOD.Chart.Model;
using Microsoft.AspNetCore.Mvc;

namespace IRanwa.EOD.Charts.UI.Controllers
{
    [Route("api/v1/account")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        /// <summary>
        /// The account service
        /// </summary>
        private readonly IAccountService accountService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountsController"/> class.
        /// </summary>
        /// <param name="accountService">The account service.</param>
        public AccountsController(
             IAccountService accountService
            )
        {
            this.accountService = accountService;
        }

        /// <summary>
        /// Registers the asynchronous.
        /// </summary>
        /// <param name="userModel">The user model.</param>
        /// <returns>Returns response.</returns>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> RegisterAsync(UserModel userModel)
        {
            try
            {
                var response = await accountService.RegisterUserAsync(userModel);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetAllMessages());
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(UserModel userModel)
        {
            try
            {
                var response = await accountService.LoginUserAsync(userModel);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetAllMessages());

            }
        }

        [HttpPost]
        [Route("user-details")]
        public async Task<IActionResult> UpdateUserDetailsAsync(UserModel model)
        {
            try
            {
                var response = await accountService.UpdateUserDetails(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetAllMessages());

            }
        }

        [HttpPost]
        [Route("user-password")]
        public async Task<IActionResult> UpdateUserPasswordAsync(UserPasswordModel model)
        {
            try
            {
                var response = await accountService.UpdatePassword(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetAllMessages());

            }
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetUserDetailsAsync(string username)
        {
            try
            {
                var response = accountService.GetUserDetails(username);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetAllMessages());

            }
        }

        [HttpPost]
        [Route("email-verify")]
        public async Task<IActionResult> EmailVerifyAsync(EmailVerificationModel verificationModel)
        {
            try
            {
                var response = await accountService.VerifyEmailAsync(verificationModel);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetAllMessages());

            }
        }

        [HttpGet]
        [Route("email-verification-resend")]
        public async Task<IActionResult> EmailVerificationResendAsync(string email)
        {
            try
            {
                var response = await accountService.ResendEmailVerificationCodeAsync(email);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetAllMessages());

            }
        }

        [HttpGet]
        [Route("password-reset")]
        public async Task<IActionResult> PasswordResetRequestAsync(string email)
        {
            try
            {
                var response = await accountService.SendForgetPasswordCodeAsync(email);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetAllMessages());

            }
        }

        [HttpPost]
        [Route("password-reset-eligible")]
        public IActionResult PasswordResetEligible(PasswordVerificationModel model)
        {
            try
            {
                var response = accountService.VerifyPasswordResetEligible(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetAllMessages());
            }
        }

        [HttpPost]
        [Route("password-reset")]
        public async Task<IActionResult> PasswordResetAsync(PasswordVerificationModel model)
        {
            try
            {
                var response = await accountService.ResetPasswordAsync(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetAllMessages());
            }
        }
    }
}
