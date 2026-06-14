using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sion.BLL.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Sion.PL.Pages.Admin.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ICorreoService _correo;
        private readonly ILogger<ForgotPasswordModel> _logger;

        [BindProperty]
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public bool Enviado { get; set; } = false;
        public string? ErrorMessage { get; set; }

        public ForgotPasswordModel(
            UserManager<IdentityUser> userManager,
            ICorreoService correo,
            ILogger<ForgotPasswordModel> logger)
        {
            _userManager = userManager;
            _correo = correo;
            _logger = logger;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = await _userManager.FindByEmailAsync(Email);

            // Siempre mostramos "enviado" aunque el email no exista (seguridad)
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var link = Url.Page(
                    "/Admin/Account/ResetPassword",
                    pageHandler: null,
                    values: new { token, email = Email },
                    protocol: Request.Scheme)!;

                await _correo.EnviarResetPasswordAsync(Email, link);
                _logger.LogInformation("Reset de contraseña solicitado para {Email}", Email);
            }

            Enviado = true;
            return Page();
        }
    }
}