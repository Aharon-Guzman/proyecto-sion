using Moq;
using Sion.BLL.Interfaces;
using Sion.BLL.Services;
using Sion.BLL.ViewModels;
using Sion.DAL.Entities;
using Sion.DAL.Interfaces;

namespace Sion.Tests;

public class DonacionServiceTests
{
    private readonly Mock<IDonacionRepository> _repoMock;
    private readonly Mock<ILogAuditoriaService> _auditoriaMock;
    private readonly DonacionService _service;

    public DonacionServiceTests()
    {
        _repoMock = new Mock<IDonacionRepository>();
        _auditoriaMock = new Mock<ILogAuditoriaService>();
        _service = new DonacionService(_repoMock.Object, _auditoriaMock.Object);
    }

    private static DonacionViewModel NuevaDonacion() => new()
    {
        TransaccionPaypalId = "ORDER-123",
        Monto = 50m,
        Moneda = "USD",
        NombreDonante = "John Doe",
        EmailDonante = "john@test.com",
        Estado = "COMPLETED",
        EsRecurrente = false
    };

    [Fact]
    public async Task RegistrarAsync_TransaccionNueva_GuardaYRegistraAuditoria()
    {
        // Arrange: la transacción NO existe todavía
        _repoMock.Setup(r => r.GetByPaypalIdAsync("ORDER-123"))
                 .ReturnsAsync((Donacion?)null);

        // Act
        await _service.RegistrarAsync(NuevaDonacion());

        // Assert: se guardó una vez con los datos correctos
        _repoMock.Verify(r => r.AddAsync(It.Is<Donacion>(d =>
            d.TransaccionPaypalId == "ORDER-123" &&
            d.Monto == 50m &&
            d.Estado == "COMPLETED")), Times.Once);
        _auditoriaMock.Verify(a => a.RegistrarAsync(
            "Registrar", "Donacion", It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task RegistrarAsync_TransaccionDuplicada_NoGuardaDeNuevo()
    {
        // Arrange: la transacción YA existe
        _repoMock.Setup(r => r.GetByPaypalIdAsync("ORDER-123"))
                 .ReturnsAsync(new Donacion { TransaccionPaypalId = "ORDER-123" });

        // Act
        await _service.RegistrarAsync(NuevaDonacion());

        // Assert: NO se guarda ni se audita de nuevo
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Donacion>()), Times.Never);
        _auditoriaMock.Verify(a => a.RegistrarAsync(
            It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetByFechasAsync_MapeaEntidadesAViewModels()
    {
        // Arrange
        var desde = DateTime.UtcNow.AddDays(-7);
        var hasta = DateTime.UtcNow;
        _repoMock.Setup(r => r.GetByFechasAsync(desde, hasta))
                 .ReturnsAsync(new List<Donacion>
                 {
                     new() { Id = 1, TransaccionPaypalId = "A",
                             Monto = 10m, Moneda = "USD", Estado = "COMPLETED" }
                 });

        // Act
        var result = (await _service.GetByFechasAsync(desde, hasta)).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("A", result[0].TransaccionPaypalId);
        Assert.Equal(10m, result[0].Monto);
    }
}