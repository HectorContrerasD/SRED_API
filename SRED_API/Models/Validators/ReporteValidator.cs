using FluentValidation;
using FluentValidation.Results;
using SRED_API.Models.DTOs;

namespace SRED_API.Models.Validators
{
    public static class ReporteValidator
    {
        public static ValidationResult Validate(ReporteDTO reporte)
        {
        var validator = new InlineValidator<ReporteDTO>();
            validator.RuleFor(x => x.NoControlTrabajo).NotEmpty()
                .WithMessage("Agregue su número de control o trabajo");
            validator.RuleFor(x => x.Descripcion).NotEmpty()
                .WithMessage("Agregue una descripción del problema");
            validator.RuleFor(x => x.EquipoId).NotEmpty()
                .WithMessage("Agregue el equipo del que desea hacer el reporte");
            return validator.Validate(reporte);
        }
    }
}
