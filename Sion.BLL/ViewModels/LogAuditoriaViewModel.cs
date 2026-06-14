namespace Sion.BLL.ViewModels
{
    public class LogAuditoriaViewModel
    {
        public int Id { get; set; }
        public string Accion { get; set; } = string.Empty;
        public string Entidad { get; set; } = string.Empty;
        public string UsuarioEmail { get; set; } = string.Empty;
        public string Detalle { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
    }
}
