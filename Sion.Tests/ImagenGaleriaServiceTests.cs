using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Sion.BLL.Interfaces;
using Sion.BLL.Services;
using Sion.DAL.Interfaces;

namespace Sion.Tests;

public class ImagenGaleriaServiceTests
{
    private readonly Mock<IImagenGaleriaRepository> _repositoryMock;
    private readonly Mock<ILogAuditoriaService> _auditoriaMock;
    private readonly Mock<IWebHostEnvironment> _envMock;
    private readonly Mock<ILogger<ImagenGaleriaService>> _loggerMock;
    private readonly ImagenGaleriaService _service;

    public ImagenGaleriaServiceTests()
    {
        _repositoryMock = new Mock<IImagenGaleriaRepository>();
        _auditoriaMock = new Mock<ILogAuditoriaService>();
        _loggerMock = new Mock<ILogger<ImagenGaleriaService>>();
        _envMock = new Mock<IWebHostEnvironment>();
        _envMock.Setup(e => e.WebRootPath).Returns(Path.GetTempPath());

        _service = new ImagenGaleriaService(
            _repositoryMock.Object,
            _auditoriaMock.Object,
            _envMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task SubirAsync_ExtensionInvalida_LanzaExcepcion()
    {
        // Arrange
        var archivoMock = new Mock<IFormFile>();
        archivoMock.Setup(f => f.FileName).Returns("virus.exe");
        archivoMock.Setup(f => f.Length).Returns(1024);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.SubirAsync(archivoMock.Object, "Test", "admin@sion.cr"));
    }

    [Fact]
    public async Task SubirAsync_ArchivoMayorA5MB_LanzaExcepcion()
    {
        // Arrange
        var archivoMock = new Mock<IFormFile>();
        archivoMock.Setup(f => f.FileName).Returns("foto.jpg");
        archivoMock.Setup(f => f.Length).Returns(6 * 1024 * 1024); // 6MB

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.SubirAsync(archivoMock.Object, "Test", "admin@sion.cr"));
    }
}