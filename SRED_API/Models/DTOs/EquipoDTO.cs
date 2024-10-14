namespace SRED_API.Models.DTOs
{
	public class EquipoDTO
	{
        public int Id { get; set; }
		public string Numero { get; set; } = null!;
        public int TipoId { get; set; }
        public int AulaId { get; set; }

    }
}
