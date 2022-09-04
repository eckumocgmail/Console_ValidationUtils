using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
 

using Microsoft.AspNetCore.Authentication;
 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Mvc_WwwLogin.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly APIAuthorization _signinManager;

        public LoginModel(ILogger<LoginModel> logger, APIAuthorization signinManager)
        {
            this._logger = logger;
            this._signinManager = signinManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }


        [TempData]
        public string ErrorMessage { get; set; }
        public string ReturnUrl { get; set; }
        public IEnumerable<string> ExternalLogins { get; set; }
        public AuthenticationProperties ExternalScheme { get; private set; }


        public class InputModel
        {
            [Label("Электронная почта")]
            [Required(ErrorMessage = "Подтвердите, что вы не робот")]    
            [ReCaptcha.RecaptchaValidation("Результат не подтверждён")]
 
            [FromForm(Name = "g-recaptcha-response")]
            [BindProperty(Name = "g-recaptcha-response")]
            [Display(Name = "g-Recaptcha-Response")]
            public string Recaptcha { get; set; }

            [Required(ErrorMessage = "Введите электронную почту")]
            [Label("Электронная почта")]
            [EmailAddress]
            public string Email { get; set; }

            [Required(ErrorMessage = "Введите пароль")]
            [Label("Пароль")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Запомнить меня")]
            public bool RememberMe { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");            
            //await HttpContext.SignOutAsync(ExternalScheme);
            ExternalLogins = (await _signinManager.GetExternalAuthenticationSchemesAsync()).ToList();
            ReturnUrl = returnUrl;
        }


        public async Task<IActionResult> OnPostAsync(

            [FromForm(Name = "g-recaptcha-response")]
            [Bind("g-recaptcha-response")]             
            string Recaptcha,
            string returnUrl = null)
        {
            Input.Recaptcha = Recaptcha;
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signinManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {                
                var result = _signinManager.Signin(Input.Email, Input.Password, true   );
                if (result!=null)
                {         
                    return LocalRedirect(returnUrl);
                }                
                else
                {
                    ModelState.AddModelError(string.Empty, "Учётные данные не зарегистрированы");
                    return Page();
                }
            }            
            return Page();
        }
    }
}
