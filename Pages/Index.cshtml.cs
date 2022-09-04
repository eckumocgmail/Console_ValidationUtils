using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Console8_ValidationUtils.Pages
{
    public class IndexModel : PageModel
    {

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public List<NavLink> NavMenu { get; set; } = new List<NavLink>() {
            new NavLink(){ Label="Авторизация", IsActive=true },
            new NavLink(){ Label="Авторизация", IsActive=false },
        };
        public class NavLink
        {
            public string Label { get; set; }
            public bool IsActive { get; set; }
        }
       

        [TempData]
        public string ErrorMessage { get; set; }
        public string ReturnUrl { get; set; }
        public string ViewName { get; set; } = "Login";

        public class InputModel
        {
            [Required]
            [Label("Электронная почта")]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [Label("Пароль")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Запомнить меня")]
            public bool RememberMe { get; set; }
        } 

        public async Task OnGetAsync( string returnUrl = null )
        {
            await Task.CompletedTask;
            
            ReturnUrl = returnUrl;
            await Task.CompletedTask;
            if (ModelState.IsValid)
            {
                RedirectToPage(returnUrl, Input);
            }
            Page();
        }
 
    }
}
