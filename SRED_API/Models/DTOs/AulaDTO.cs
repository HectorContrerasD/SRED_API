﻿namespace SRED_API.Models.DTOs
{
	public class AulaDTO
	{
        public int Id { get; set; }
		public string Nombre { get; set; } = null!;
    }
	public class AulaConEquiposDTO: AulaDTO
	{
		public List<EquipoDatosDto> Equipos { get; set; }= new();
	}
	
}

