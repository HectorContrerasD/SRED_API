using FluentValidation;
using FluentValidation.Results;
using SRED_API.Models.DTOs;

namespace SRED_API.Models.Validators
{
    public static class EquipoValidator
    {
        public static ValidationResult Validate(EquipoDTO equipoDTO)
        {
            var validator = new InlineValidator<EquipoDTO>();
            validator.RuleFor(x => x.Numero).NotEmpty().WithMessage("Necesita el número del equipo");
            validator.RuleFor(x => x.TipoId).NotEmpty().WithMessage("Necesita el tipo de equipo");
            validator.RuleFor(x => x.AulaId).NotEmpty().WithMessage("Necesita el Aula en la que se encuentra el equipo");
            return validator.Validate(equipoDTO);
        }
    }
}
