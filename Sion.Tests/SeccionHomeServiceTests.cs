using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Sion.BLL.Interfaces;
using Sion.BLL.Services;
using Sion.DAL.Entities;
using Sion.DAL.Interfaces;

namespace Sion.Tests;

public class SeccionHomeServiceTests
{
    private readonly Mock<ISeccionHomeRepository> _repositoryMock;
    private readonly Mock<ILogAuditoriaService> _auditoriaMock;
    private readonly Mock<IMemoryCache> _cacheMock;
    private readonly Mock<ILogger<SeccionHomeService>> _loggerMock;
    private readonly SeccionHomeService _service;

    public SeccionHomeServiceTests()
    {
        _repositoryMock = new Mock<ISeccionHomeRepository>();
        _auditoriaMock = new Mock<ILogAuditoriaService>();
        _cacheMock = new Mock<IMemoryCache>();
        _loggerMock = new Mock<ILogger<SeccionHomeService>>();

        // Setup necesario para que _cache.Remove no explote
        _cacheMock.Setup(c => c.Remove(It.IsAny<object>()));

        _service = new SeccionHomeService(
            _repositoryMock.Object,
            _auditoriaMock.Object,
            _cacheMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task DeleteAsync_SeccionExiste_RegistraAuditoria()
    {
        // Arrange
        var seccion = new SeccionHome
        {
            Id = 1,
            Titulo = "Una comunidad unida por la fe y el servicio",
            Descripcion = "Descripción de prueba",
            Estilo = "ImagenDerecha",
            Orden = 1,
            EstaActiva = true,
            FechaModificacion = DateTime.UtcNow
        };

        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(seccion);
        _repositoryMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(1, "mauro.vega@sioncr.org");

        // Assert — verificar que se registró la auditoría con los parámetros correctos
        _auditoriaMock.Verify(a => a.RegistrarAsync(
            "Eliminar",
            "SeccionHome",
            "mauro.vega@sioncr.org",
            It.Is<string>(d => d.Contains("Una comunidad unida por la fe y el servicio"))),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_SeccionNoExiste_NoRegistraAuditoria()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((SeccionHome?)null);

        // Act
        await _service.DeleteAsync(99, "mauro.vega@sioncr.org");

        // Assert — si la sección no existe, nunca debe registrar auditoría
        _auditoriaMock.Verify(a => a.RegistrarAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()),
            Times.Never);
    }
}