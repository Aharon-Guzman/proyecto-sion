using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Sion.PL.Pages.Admin.Account
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<ResetPasswordModel> _logger;

        [BindProperty]
        public ResetInput Input { get; set; } = new();

        public bool Exitoso { get; set; } = false;
        public bool TokenInvalido { get; set; } = false;
        public string? ErrorMessage { get; set; }

        public ResetPasswordModel(
            UserManager<IdentityUser> userManager,
            ILogger<ResetPasswordModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult OnGet(string? token, string? email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                TokenInvalido = true;
                return Page();
            }

            Input.Token = token;
            Input.Email = email;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                TokenInvalido = true;
                return Page();
            }

            var result = await _userManager.ResetPasswordAsync(user, Input.Token, Input.NuevaContrasena);

            if (result.Succeeded)
            {
                _logger.LogInformation("Contraseña restablecida para {Email}", Input.Email);
                Exitoso = true;
                return Page();
            }

            ErrorMessage = "No se pudo restablecer la contraseña. El enlace puede haber expirado.";
            return Page();
        }
    }

    public class ResetInput
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mínimo 6 caracteres")]
        [DataType(DataType.Password)]
        public string NuevaContrasena { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirmá la contraseña")]
        [Compare("NuevaContrasena", ErrorMessage = "Las contraseñas no coinciden")]
        [DataType(DataType.Password)]
        public string ConfirmarContrasena { get; set; } = string.Empty;
    }
}