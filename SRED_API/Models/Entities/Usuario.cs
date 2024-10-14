using System;
using System.Collections.Generic;

namespace SRED_API.Models.Entities;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string NumeroControl { get; set; } = null!;

    public sbyte Rol { get; set; }
}
