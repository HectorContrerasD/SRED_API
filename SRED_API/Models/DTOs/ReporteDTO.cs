using Microsoft.EntityFrameworkCore.Storage;
using SRED_API.Models.Entities;

namespace SRED_API.Models.DTOs
{
    public class ReporteDTO
    {
        public int Id { get; set; }
        public string NoControlTrabajo { get; set; }
        public string Descripcion { get; set; } = null!;
        public int EquipoId { get; set; }


    }
    public class ReporteDatosDTO:ReporteDTO
    {
        public string Folio { get; set; } = null!;
        public string Equipo { get; set; } = null!;
        public string Aula { get; set; } = null!;
        public DateOnly FechaCreacion { get; set; }
        public sbyte Estado { get; set; }

    }
}
