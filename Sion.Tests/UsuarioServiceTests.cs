using Microsoft.AspNetCore.Identity;
using Moq;
using Sion.BLL.Services;

namespace Sion.Tests;

public class UsuarioServiceTests
{
    private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
    private readonly UsuarioService _service;

    public UsuarioServiceTests()
    {
        _userManagerMock = new Mock<UserManager<IdentityUser>>(
            Mock.Of<IUserStore<IdentityUser>>(),
            null, null, null, null, null, null, null, null);

        _service = new UsuarioService(_userManagerMock.Object);
    }

    [Fact]
    public async Task EsAdminAsync_UsuarioAdmin_DevuelveTrue()
    {
        // Arrange
        var userId = "user-123";
        var usuario = new IdentityUser { Id = userId };

        _userManagerMock.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(usuario);
        _userManagerMock.Setup(m => m.IsInRoleAsync(usuario, "Admin")).ReturnsAsync(true);

        // Act
        var resultado = await _service.EsAdminAsync(userId);

        // Assert
        Assert.True(resultado);
    }

    [Fact]
    public async Task EsAdminAsync_UsuarioNoAdmin_DevuelveFalse()
    {
        // Arrange
        var userId = "user-456";
        var usuario = new IdentityUser { Id = userId };

        _userManagerMock.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(usuario);
        _userManagerMock.Setup(m => m.IsInRoleAsync(usuario, "Admin")).ReturnsAsync(false);

        // Act
        var resultado = await _service.EsAdminAsync(userId);

        // Assert
        Assert.False(resultado);
    }

    [Fact]
    public async Task EsAdminAsync_UsuarioNoExiste_DevuelveFalse()
    {
        // Arrange
        _userManagerMock.Setup(m => m.FindByIdAsync("no-existe")).ReturnsAsync((IdentityUser?)null);

        // Act
        var resultado = await _service.EsAdminAsync("no-existe");

        // Assert
        Assert.False(resultado);
    }
}