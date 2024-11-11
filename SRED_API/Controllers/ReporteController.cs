using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SRED_API.Models.DTOs;
using SRED_API.Models.Entities;
using SRED_API.Models.Validators;
using SRED_API.Repositories;

namespace SRED_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReporteController : ControllerBase
    {
        private readonly ReporteRepository _repository;
        public ReporteController(ReporteRepository reporteRepository)
        {
            _repository = reporteRepository;
        }
        public async Task<IActionResult> Agregar(ReporteDTO dto)
        {
            if (dto== null) { return BadRequest("No se está recibiendo un dto"); }
            var results= ReporteValidator.Validate(dto);
            if (results.IsValid)
            {
                var reporte = new Reporte
                {
                    NoControlAl = dto.NoControlTrabajo,
                    EquipoIdEquipo = dto.EquipoId,
                    Descripcion = dto.Descripcion
                };
                await _repository.Insert(reporte);
            }
            else
            {

            }
            return Ok();
        }
    }
}
