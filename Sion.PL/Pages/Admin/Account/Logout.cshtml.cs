using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Sion.PL.Pages.Admin.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("Admin {Email} cerró sesión.", User.Identity?.Name);
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Admin/Account/Login");
        }
    }
}