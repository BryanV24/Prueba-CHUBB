namespace Entidades.DTOs
{
    public class AsignacionSegurosAsegurados
    {
        public int AseguradoId { get; set; }
        public List<int> SeguroId { get; set; } = new List<int>();
    }
}
