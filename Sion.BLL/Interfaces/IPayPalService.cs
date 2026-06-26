using Sion.BLL.ViewModels;

namespace Sion.BLL.Interfaces
{
    public interface IPayPalService
    {
        /// <summary>Crea una orden en PayPal y retorna el Order ID para el JS SDK.</summary>
        Task<string> CrearOrdenAsync(decimal monto, string moneda = "USD");

        /// <summary>Captura el pago de una orden aprobada por el usuario.</summary>
        Task<PayPalCaptureResult> CapturarOrdenAsync(string orderId);
    }
}
