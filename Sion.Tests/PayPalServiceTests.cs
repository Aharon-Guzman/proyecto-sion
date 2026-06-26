using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Sion.BLL.Services;

namespace Sion.Tests;

public class PayPalServiceTests
{
    // Config falsa con credenciales de prueba
    private static IConfiguration BuildConfig()
    {
        var mock = new Mock<IConfiguration>();
        mock.Setup(c => c["PayPal:ClientId"]).Returns("test-client");
        mock.Setup(c => c["PayPal:ClientSecret"]).Returns("test-secret");
        mock.Setup(c => c["PayPal:Mode"]).Returns("sandbox");
        return mock.Object;
    }

    // Handler que responde distinto según la URL (token vs orders)
    private static Mock<HttpMessageHandler> BuildHandler(
        string tokenJson, HttpStatusCode tokenStatus,
        string ordersJson, HttpStatusCode ordersStatus)
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync((HttpRequestMessage req, CancellationToken _) =>
            {
                var url = req.RequestUri!.ToString();
                if (url.Contains("/oauth2/token"))
                    return new HttpResponseMessage(tokenStatus)
                    { Content = new StringContent(tokenJson) };
                return new HttpResponseMessage(ordersStatus)
                { Content = new StringContent(ordersJson) };
            });
        return handler;
    }

    private static PayPalService BuildService(Mock<HttpMessageHandler> handler)
    {
        var factory = new Mock<IHttpClientFactory>();
        factory.Setup(f => f.CreateClient(It.IsAny<string>()))
               .Returns(() => new HttpClient(handler.Object));
        return new PayPalService(factory.Object, BuildConfig(),
            new Mock<ILogger<PayPalService>>().Object);
    }

    [Fact]
    public async Task CrearOrdenAsync_RespuestaOk_RetornaOrderId()
    {
        var handler = BuildHandler(
            "{\"access_token\":\"tok123\"}", HttpStatusCode.OK,
            "{\"id\":\"ORDER-XYZ\"}", HttpStatusCode.Created);
        var service = BuildService(handler);

        var orderId = await service.CrearOrdenAsync(50m);

        Assert.Equal("ORDER-XYZ", orderId);
    }

    [Fact]
    public async Task CapturarOrdenAsync_StatusCompleted_RetornaExitosoConDatos()
    {
        var capturaJson =
            "{\"status\":\"COMPLETED\"," +
            "\"purchase_units\":[{\"payments\":{\"captures\":[{\"amount\":" +
            "{\"value\":\"50.00\",\"currency_code\":\"USD\"}}]}}]," +
            "\"payer\":{\"email_address\":\"john@test.com\"," +
            "\"name\":{\"given_name\":\"John\",\"surname\":\"Doe\"}}}";

        var handler = BuildHandler(
            "{\"access_token\":\"tok123\"}", HttpStatusCode.OK,
            capturaJson, HttpStatusCode.Created);
        var service = BuildService(handler);

        var result = await service.CapturarOrdenAsync("ORDER-XYZ");

        Assert.True(result.Exitoso);
        Assert.Equal(50m, result.Monto);
        Assert.Equal("USD", result.Moneda);
        Assert.Equal("John Doe", result.NombreDonante);
    }

    [Fact]
    public async Task CapturarOrdenAsync_HttpError_RetornaNoExitoso()
    {
        var handler = BuildHandler(
            "{\"access_token\":\"tok123\"}", HttpStatusCode.OK,
            "{\"error\":\"fail\"}", HttpStatusCode.BadRequest);
        var service = BuildService(handler);

        var result = await service.CapturarOrdenAsync("ORDER-XYZ");

        Assert.False(result.Exitoso);
    }
}