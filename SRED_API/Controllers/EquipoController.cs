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
	public class EquipoController : ControllerBase
	{
		private readonly EquipoRepository _repository;
        public EquipoController(EquipoRepository Repository)
        {
            _repository = Repository;
        }
		[HttpPost]
		public ActionResult<EquipoDTO> Agregar(EquipoDTO equipoDTO)
		{
			if (equipoDTO == null) { return BadRequest("No estas mandando un dto"); }
			var results = EquipoValidator.Validate(equipoDTO);
			if (results.IsValid) 
			{
				var equipo = new Equipo
				{
					NumeroIdentificacion = equipoDTO.Numero,
					TipoEquipoIdTipoEquipo = equipoDTO.TipoId,
					AulaIdAula = equipoDTO.AulaId
				};
				_repository.Insert(equipo);
				return Ok("Equipo agregado correctamente");
			}
			else
			{
				return BadRequest(results.Errors.Select(x => x.ErrorMessage));
			}
			
		}
		[HttpPut]
		public ActionResult<EquipoDTO> Editar(EquipoDTO equipoDTO)
		{
			if (equipoDTO == null) { return BadRequest("No estas mandando un dto"); }
			if (equipoDTO.Id !=0)
			{
				var results = EquipoValidator.Validate(equipoDTO);
				if (results.IsValid)
				{
					var equipo = _repository.Get(equipoDTO.Id);
					if (equipo != null)
					{
						equipo.NumeroIdentificacion = equipoDTO.Numero;
						equipo.AulaIdAula = equipoDTO.AulaId;
						equipo.TipoEquipoIdTipoEquipo= equipoDTO.TipoId;
						_repository.Update(equipo);
						return Ok("Equipo editado correctamente");
					}
					else
					{
						return NotFound("El equipo no existe");
					}
				}
				else
				{
					return BadRequest(results.Errors.Select(x => x.ErrorMessage));
				}
			}
			else
			{
				return BadRequest();
			}
		}
		[HttpDelete]
		public ActionResult Delete(int id)
		{
			var equipo = _repository.Get(id);
			if (equipo != null)
			{
				_repository.Delete(id);
				return Ok();
			}
			else
			{

				return NotFound();	
			}
		}
    }
}
