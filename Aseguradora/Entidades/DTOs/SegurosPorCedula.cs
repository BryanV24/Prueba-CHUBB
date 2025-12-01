namespace Entidades.DTOs
{
    public class SegurosPorCedula
    {
        public string? Cedula { get; set; }
        public string? NombreCompleto { get; set; }
        public string? NombreSeguro { get; set; }
        public string? CodigoSeguro { get; set; }
        public decimal SumaAsegurada { get; set; }
        public decimal Prima { get; set; }
    }
}
