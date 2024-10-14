using FluentValidation.Results;
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
	public class AulaController : ControllerBase
	{
		private readonly AulaRepository _repository;
		public AulaController(AulaRepository Repository)
		{
			_repository = Repository;
		}
		[HttpPost]
		public ActionResult<AulaDTO> Agregar(AulaDTO aulaDTO)
		{
			if (aulaDTO == null)
			{
				return BadRequest();
			}
			ValidationResult results = AulaValidator.Validate(aulaDTO);
			if (results.IsValid)
			{
				var aula = new Aula
				{
					Nombre = aulaDTO.Nombre,
					Tipo = aulaDTO.Tipo,
				};
				_repository.Insert(aula);
				return Ok("Aula agregada correctamente");
			}
			else
			{
				return BadRequest(results.Errors.Select(x => x.ErrorMessage));
			}

		}
		[HttpGet]
		public ActionResult<AulaDTO> GetAll()
		{
			var aulas = _repository.GetAulas();
			return aulas != null ? Ok(aulas) : NotFound("No se encontraron aulas");
		}
		[HttpGet("id")]
		public ActionResult<AulaDTO> Get(int id)
		{
			var aula = _repository.GetAula(id);
			return aula != null ? Ok(aula) : NotFound("No se encontró el aula");
		}
		[HttpPut]
		public ActionResult<AulaDTO> Editar(AulaDTO aulaDTO)
		{
			if (aulaDTO == null)
			{
				return BadRequest();
			}
			if (aulaDTO.Id != 0)
			{
				var results = AulaValidator.Validate(aulaDTO);
                if (results.IsValid)
                {   
					var aula = _repository.Get(aulaDTO.Id);
					if (aula == null)
					{
						return NotFound("El aula no existe");
					}
					aula.Nombre = aulaDTO.Nombre;
					aula.Tipo = aulaDTO.Tipo;
					_repository.Update(aula);
					return Ok("Aula editada correctamente");
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

    }
}
