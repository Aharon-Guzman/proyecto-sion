using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Sion.PL.Pages.Admin.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<LoginModel> _logger;

        [BindProperty]
        public LoginInput Input { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public LoginModel(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _signInManager.PasswordSignInAsync(
                Input.Email,
                Input.Password,
                Input.RememberMe,
                lockoutOnFailure: true);

            if (result.Succeeded)
            {
                _logger.LogInformation("Admin {Email} inició sesión.", Input.Email);
                return RedirectToPage("/Admin/Index");
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("Cuenta {Email} bloqueada por intentos fallidos.", Input.Email);
                ErrorMessage = "Cuenta bloqueada temporalmente por demasiados intentos. Intentá de nuevo en 15 minutos.";
                return Page();
            }

            // Feedback de intentos restantes antes del bloqueo
            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user != null && await _userManager.GetLockoutEnabledAsync(user))
            {
                var maximo    = _userManager.Options.Lockout.MaxFailedAccessAttempts;
                var fallidos  = await _userManager.GetAccessFailedCountAsync(user);
                var restantes = maximo - fallidos;
                ErrorMessage = restantes > 0
                    ? $"Correo o contraseña incorrectos. Te queda{(restantes == 1 ? "" : "n")} {restantes} intento{(restantes == 1 ? "" : "s")} antes del bloqueo."
                    : "Correo o contraseña incorrectos.";
            }
            else
            {
                ErrorMessage = "Correo o contraseña incorrectos.";
            }

            return Page();
        }
    }

    public class LoginInput
    {
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}
