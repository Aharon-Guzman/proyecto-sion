using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sion.BLL.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Sion.PL.Pages
{
    public class ContactoModel : PageModel
    {
        private readonly ICorreoService _correo;
        private readonly ILogger<ContactoModel> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public ContactoInput Input { get; set; } = new();

        [BindProperty]
        public string? ReCaptchaToken { get; set; }

        public bool MensajeEnviado { get; set; } = false;
        public bool ErrorEnvio { get; set; } = false;

        public ContactoModel(
            ICorreoService correo,
            ILogger<ContactoModel> logger,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory)
        {
            _correo = correo;
            _logger = logger;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // Validar reCAPTCHA
            if (!await ValidarReCaptchaAsync(ReCaptchaToken))
            {
                _logger.LogWarning("reCAPTCHA rechazado para envío de contacto desde {Email}.", Input.Email);
                ErrorEnvio = true;
                return Page();
            }

            await _correo.EnviarContactoAsync(
                Input.Nombre,
                Input.Email,
                Input.Telefono,
                Input.Mensaje
            );

            MensajeEnviado = true;
            ModelState.Clear();
            Input = new();
            return Page();
        }

        private async Task<bool> ValidarReCaptchaAsync(string? token)
        {
            if (string.IsNullOrEmpty(token)) return false;

            var secretKey = _configuration["ReCaptcha:SecretKey"];
            var client = _httpClientFactory.CreateClient();

            var response = await client.PostAsync(
                "https://www.google.com/recaptcha/api/siteverify",
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "secret", secretKey! },
                    { "response", token }
                })
            );

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var success = root.GetProperty("success").GetBoolean();
            var score = root.TryGetProperty("score", out var scoreProp) ? scoreProp.GetDouble() : 0;

            _logger.LogInformation("reCAPTCHA — success: {Success}, score: {Score}", success, score);

            return success && score >= 0.5;
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