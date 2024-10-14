using System;
using System.Collections.Generic;

namespace SRED_API.Models.Entities;

public partial class Reporte
{
    public int IdReporte { get; set; }

    public string NoControlAl { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public sbyte Estado { get; set; }

    public DateOnly FechaCreacion { get; set; }

    public string Folio { get; set; } = null!;

    public int EquipoIdEquipo { get; set; }

    public virtual Equipo EquipoIdEquipoNavigation { get; set; } = null!;
}
