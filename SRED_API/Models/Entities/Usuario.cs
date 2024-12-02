using System;
using System.Collections.Generic;

namespace SRED_API.Models.Entities;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public sbyte Rol { get; set; }

    public string Contraseña { get; set; } = null!;
}
