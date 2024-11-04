using System;
using System.Collections.Generic;

namespace SRED_API.Models.Entities;

public partial class Aula
{
    public int IdAula { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Equipo> Equipo { get; set; } = new List<Equipo>();
}
