using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks; 

using ApplicationDb.Entities;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

using ReCaptcha;

namespace Mvc_WwwLogin.Pages
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        public ReCaptchaService _reCaptchaService { get; }

        private readonly APIAuthorization _authorization;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            APIAuthorization authorization,
            ReCaptcha.ReCaptchaService reCaptchaService,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _reCaptchaService = reCaptchaService;
            _authorization = authorization;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<string> ExternalLogins { get; set; }

        public class InputModel
        {           
            [Required(ErrorMessage = "Введите электронную почту")]
            [Label("Электронная почта")]
            [EmailAddress]
            public string Email { get; set; }

            [Required(ErrorMessage = "Введите пароль")]
            [Label("Пароль")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Label("Подтверждение")]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _authorization.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(

            [FromForm(Name = "g-recaptcha-response")]
            [Bind("g-recaptcha-response")]
            string Recaptcha, 
            string returnUrl = null)
        {            
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _authorization.GetExternalAuthenticationSchemesAsync()).ToList();
            bool reCaptchaValidated = _reCaptchaService.Validate(Recaptcha);
            if (ModelState.IsValid && reCaptchaValidated)
            {
                var user = new Account(Input.Password) { Email = Input.Email };
                var result = _authorization.Signup(user );
                if (result.Succeeded)
                {

                    _authorization.Signin(user.Email, user.Password, true);
                    return LocalRedirect(returnUrl);
                }
                foreach (DetailsMessage error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private async Task ConfirmEmail(string returnUrl, Account user)
        {
            var code = await _authorization.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = user.ID, code, returnUrl },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
        }
    }
}
