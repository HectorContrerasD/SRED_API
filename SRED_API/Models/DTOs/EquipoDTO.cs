namespace SRED_API.Models.DTOs
{
	public class EquipoDTO
	{
        public int Id { get; set; }
		public string Numero { get; set; } = null!;
        public int TipoId { get; set; }
        public int AulaId { get; set; }

    }
    public class EquipoDatosDto : EquipoDTO
    {
        public string Nombre { get; set; }
        public string Aula { get; set; }
        public string Tipo { get; set; }
        public string IconoTipo { get; set; }   
    }
}
