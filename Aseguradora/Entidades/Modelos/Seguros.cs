namespace Entidades.Modelos
{
    public class Seguros
    {
        public int SeguroId { get; set; }
        public string? NombreSeguro { get; set; }
        public string? CodigoSeguro { get; set; }
        public decimal SumaAsegurada { get; set; }
        public decimal Prima { get; set; }
    }
}
