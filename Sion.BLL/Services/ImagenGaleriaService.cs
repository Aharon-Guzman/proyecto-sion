using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Sion.BLL.Interfaces;
using Sion.BLL.ViewModels;
using Sion.DAL.Entities;
using Sion.DAL.Interfaces;
using SixLabors.ImageSharp.Formats.Webp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;


namespace Sion.BLL.Services
{

    public class ImagenGaleriaService : IImagenGaleriaService
    {
        private readonly IImagenGaleriaRepository _repository;
        private readonly ILogAuditoriaService _auditoria;
        private readonly string _uploadsPath;
        private const int ThumbnailSize = 300;

        public ImagenGaleriaService(
            IImagenGaleriaRepository repository,
            ILogAuditoriaService auditoria,
            IWebHostEnvironment env)
        {
            _repository = repository;
            _auditoria = auditoria;
            _uploadsPath = Path.Combine(env.WebRootPath, "uploads", "galeria");
            Directory.CreateDirectory(_uploadsPath);
        }

        public async Task<IEnumerable<ImagenGaleriaViewModel>> GetAllAsync()
        {
            var entidades = await _repository.GetAllAsync();
            return entidades.Select(MapToViewModel);
        }

        public async Task<IEnumerable<ImagenGaleriaViewModel>> GetActivasAsync()
        {
            var entidades = await _repository.GetActivasAsync();
            return entidades.Select(MapToViewModel);
        }

        public async Task<ImagenGaleriaViewModel?> GetByIdAsync(int id)
        {
            var entidad = await _repository.GetByIdAsync(id);
            return entidad == null ? null : MapToViewModel(entidad);
        }

        public async Task SubirAsync(IFormFile archivo, string titulo, string usuarioEmail)
        {
            var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(archivo.FileName).ToLower();

            if (!extensionesPermitidas.Contains(extension))
                throw new InvalidOperationException("Formato de imagen no permitido.");

            if (archivo.Length > 5 * 1024 * 1024)
                throw new InvalidOperationException("La imagen no puede superar 5MB.");

            var nombreBase = $"{Guid.NewGuid()}";
            var rutaOriginal = Path.Combine(_uploadsPath, $"{nombreBase}{extension}");
            var rutaThumbnail = Path.Combine(_uploadsPath, $"{nombreBase}_thumb.webp");
            var rutaWebP = Path.Combine(_uploadsPath, $"{nombreBase}.webp");

            using var stream = archivo.OpenReadStream();
            using var imagen = await Image.LoadAsync(stream);

            // Guardar original
            await imagen.SaveAsync(rutaOriginal);

            // Generar WebP
            await imagen.SaveAsync(rutaWebP, new WebpEncoder());

            // Generar thumbnail
            imagen.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(ThumbnailSize, ThumbnailSize),
                Mode = ResizeMode.Crop
            }));
            await imagen.SaveAsync(rutaThumbnail, new WebpEncoder());

            var entidad = new ImagenGaleria
            {
                Titulo = titulo,
                RutaOriginal = $"/uploads/galeria/{nombreBase}{extension}",
                RutaWebP = $"/uploads/galeria/{nombreBase}.webp",
                RutaThumbnail = $"/uploads/galeria/{nombreBase}_thumb.webp",
                EstaActiva = true,
                FechaSubida = DateTime.UtcNow
            };

            await _repository.AddAsync(entidad);
            await _auditoria.RegistrarAsync("Subir", "ImagenGaleria", usuarioEmail, $"Imagen '{titulo}' subida");
        }

        public async Task UpdateAsync(ImagenGaleriaViewModel viewModel, string usuarioEmail)
        {
            var entidad = await _repository.GetByIdAsync(viewModel.Id);
            if (entidad == null) return;

            entidad.Titulo = viewModel.Titulo;

            await _repository.UpdateAsync(entidad);
            await _auditoria.RegistrarAsync("Actualizar", "ImagenGaleria", usuarioEmail, $"Imagen '{entidad.Titulo}' actualizada");
        }

        public async Task DeleteAsync(int id, string usuarioEmail)
        {
            var entidad = await _repository.GetByIdAsync(id);
            if (entidad == null) return;

            // Eliminar archivos físicos
            EliminarArchivo(entidad.RutaOriginal);
            EliminarArchivo(entidad.RutaWebP);
            EliminarArchivo(entidad.RutaThumbnail);

            await _repository.DeleteAsync(id);
            await _auditoria.RegistrarAsync("Eliminar", "ImagenGaleria", usuarioEmail, $"Imagen '{entidad.Titulo}' eliminada");
        }

        public async Task ToggleActivaAsync(int id, string usuarioEmail)
        {
            var entidad = await _repository.GetByIdAsync(id);
            if (entidad == null) return;

            entidad.EstaActiva = !entidad.EstaActiva;

            await _repository.UpdateAsync(entidad);
            await _auditoria.RegistrarAsync("Toggle", "ImagenGaleria", usuarioEmail, $"Imagen '{entidad.Titulo}' → activa: {entidad.EstaActiva}");
        }

        private void EliminarArchivo(string rutaRelativa)
        {
            var rutaFisica = Path.Combine(_uploadsPath, Path.GetFileName(rutaRelativa));
            if (File.Exists(rutaFisica))
                File.Delete(rutaFisica);
        }

        private static ImagenGaleriaViewModel MapToViewModel(ImagenGaleria e) => new()
        {
            Id = e.Id,
            Titulo = e.Titulo,
            RutaOriginal = e.RutaOriginal,
            RutaThumbnail = e.RutaThumbnail,
            RutaWebP = e.RutaWebP,
            EstaActiva = e.EstaActiva,
            FechaSubida = e.FechaSubida
        };
    }
}
