namespace Sion.BLL.ViewModels
{
    /// <summary>
    /// Resultado de capturar un pago en PayPal.
    /// Lo usa el handler de Donaciones para saber si registrar en BD.
    /// </summary>
    public class PayPalCaptureResult
    {
        public bool    Exitoso       { get; set; }
        public string  OrderId       { get; set; } = string.Empty;
        public decimal Monto         { get; set; }
        public string  Moneda        { get; set; } = string.Empty;
        public string? NombreDonante { get; set; }
        public string? EmailDonante  { get; set; }
        public string  Estado        { get; set; } = string.Empty;
        public string? ErrorMensaje  { get; set; }
    }
}
