using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sion.BLL.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Sion.BLL.Services
{
    public class CorreoService : ICorreoService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<CorreoService> _logger;

        public CorreoService(IConfiguration config, ILogger<CorreoService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task EnviarContactoAsync(string nombre, string email, string? telefono, string mensaje)
        {
            var host = _config["Smtp:Host"]!;
            var puerto = int.Parse(_config["Smtp:Puerto"]!);
            var usuario = _config["Smtp:Usuario"]!;
            var contrasena = _config["Smtp:Contrasena"]!;
            var remitente = _config["Smtp:NombreRemitente"]!;
            var destinatario = _config["Smtp:DestinatarioContacto"]!;

            var cuerpo = $"""
                Nuevo mensaje de contacto — Proyecto Sion
                
                Nombre:    {nombre}
                Correo:    {email}
                Teléfono:  {(string.IsNullOrEmpty(telefono) ? "No indicado" : telefono)}
                
                Mensaje:
                {mensaje}
                """;

            using var smtp = new SmtpClient(host, puerto)
            {
                Credentials = new NetworkCredential(usuario, contrasena),
                EnableSsl = true
            };

            var correo = new MailMessage
            {
                From = new MailAddress(usuario, remitente),
                Subject = $"Contacto web: {nombre}",
                Body = cuerpo,
                IsBodyHtml = false
            };

            correo.To.Add(destinatario);
            correo.ReplyToList.Add(new MailAddress(email, nombre));

            await smtp.SendMailAsync(correo);

            _logger.LogInformation("Correo de contacto enviado de {Email}", email);
        }
    }
}