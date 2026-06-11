using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Sion.PL.Pages
{
    public class ContactoModel : PageModel
    {
        private readonly ILogger<ContactoModel> _logger;

        [BindProperty]
        public ContactoInput Input { get; set; } = new();

        public bool MensajeEnviado { get; set; } = false;

        public ContactoModel(ILogger<ContactoModel> logger)
        {
            _logger = logger;
        }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            // SMTP va aquí en F-004.5
            _logger.LogInformation("Formulario de contacto recibido de {Email}", Input.Email);

            MensajeEnviado = true;
            ModelState.Clear();
            Input = new();
            return Page();
        }
    }

    public class ContactoInput
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Correo inválido")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Teléfono inválido")]
        [StringLength(20, ErrorMessage = "Máximo 20 caracteres")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "El mensaje es obligatorio")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Entre 10 y 1000 caracteres")]
        public string Mensaje { get; set; } = string.Empty;
    }
}