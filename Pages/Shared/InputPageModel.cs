using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using Mvc_WwwLogin.Pages;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Console8_ValidationUtils.Pages.Shared
{
    public abstract class InputPageModel<InputModel>: PageModel
    {
        private readonly ILogger<InputPageModel<InputModel>> _logger;
        private readonly APIAuthorization _signinManager;

        public InputPageModel(
                ILogger<InputPageModel<InputModel>> logger, 
                 APIAuthorization signinManager)
        {
            this._logger = logger;
            this._signinManager = signinManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }


        [TempData]
        public string ErrorMessage { get; set; }
        public string ReturnUrl { get; set; }
         
        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");
             
            ReturnUrl = returnUrl;
        }


        public async Task<IActionResult> OnPostAsync(

            [FromForm(Name = "g-recaptcha-response")]
            [Bind("g-recaptcha-response")]
            string Recaptcha,
            string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                return OnModelValidated();
            }
            return Page();
        }

        public abstract IActionResult OnModelValidated();
    }
}
