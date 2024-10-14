using FluentValidation;
using FluentValidation.Results;
using SRED_API.Models.DTOs;
using System.Linq.Expressions;

namespace SRED_API.Models.Validators
{
    public static class TipoValidator
    {
        public static ValidationResult Validate(TipoDTO tipoDTO)
        {
            var validator = new InlineValidator<TipoDTO>();
            validator.RuleFor(x => x.Nombre).NotEmpty().WithMessage("Necesita el nombre del tipo de equipo");
            
            return validator.Validate(tipoDTO);
        }
    }
}
