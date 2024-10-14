using System;
using System.Collections.Generic;

namespace SRED_API.Models.Entities;

public partial class Equipo
{
    public int IdEquipo { get; set; }

    public string NumeroIdentificacion { get; set; } = null!;

    public int TipoEquipoIdTipoEquipo { get; set; }

    public int AulaIdAula { get; set; }

    public virtual Aula AulaIdAulaNavigation { get; set; } = null!;

    public virtual ICollection<Reporte> Reporte { get; set; } = new List<Reporte>();

    public virtual Tipoequipo TipoEquipoIdTipoEquipoNavigation { get; set; } = null!;
}
