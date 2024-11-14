using System;
using System.Collections.Generic;

namespace SRED_API.Models.Entities;

public partial class Tipoequipo
{
    public int IdTipoEquipo { get; set; }

    public string Nombre { get; set; } = null!;

    public sbyte? Estado { get; set; }

    public virtual ICollection<Equipo> Equipo { get; set; } = new List<Equipo>();
}
