using FluentValidation;
using FluentValidation.Results;
using SRED_API.Models.DTOs;

namespace SRED_API.Models.Validators
{
    public static class AulaValidator
    {
        public static ValidationResult Validate(AulaDTO aula)
        {
            var validator = new InlineValidator<AulaDTO>();
            validator.RuleFor(x => x.Nombre).NotEmpty().WithMessage("Necesita agregar un nombre del aula");
            validator.RuleFor(x => x.Tipo).NotEmpty().WithMessage("Necesita agregar el tipo del aula");

            
            return validator.Validate(aula);
        }
    }
}
