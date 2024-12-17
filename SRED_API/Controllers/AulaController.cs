using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using SRED_API.Helpers;
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
		private readonly EquipoRepository _equipoRepository;
		public AulaController(AulaRepository Repository, EquipoRepository equipoRepository)
		{
			_repository = Repository;
			_equipoRepository = equipoRepository;
		}
		[Authorize(Roles ="Admin")]
		[HttpPost]
		public async Task<IActionResult> Agregar(AulaDTO aulaDTO)
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
					Nombre = aulaDTO.Nombre.ToUpper(),
		
				};
                await _repository.Insert(aula);
				return Ok("Aula agregada correctamente");
			}
			else
			{
				return BadRequest(results.Errors.Select(x => x.ErrorMessage));
			}

		}
		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
            var aulas = await _repository.GetAll().Where(x => x.Estado == 1).Select(x=> new AulaDTO
			{
				Id = x.IdAula,
				Nombre = x.Nombre
			}).ToListAsync();
			return aulas != null ? Ok(aulas) : NotFound("No se encontraron aulas");
		}
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			var aula = await _repository.GetAula(id);
			
			return aula != null ? Ok(aula) : NotFound("No se encontró el aula");
		}
        [Authorize(Roles = "Invitado")]
        [HttpGet("/aulas/conequipos")]
		public async Task<IActionResult> GetAulasConEquipos()
		{
			var aulas = await _repository.GetAulasConEquipos();
			return aulas != null ? Ok(aulas) : NotFound("No se encontraron aulas que tengan equipos");
		}
        [Authorize(Roles = "Admin")]
        [HttpPut]
		public async Task<IActionResult> Editar(AulaDTO aulaDTO)
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
					var aula =  await _repository.Get(aulaDTO.Id);
					//var aula = _repository.Get(aulaDTO.Id);
					if (aula == null)
					{
						return NotFound("El aula no existe");
					}
					aula.Nombre = aulaDTO.Nombre.ToUpper();

					await _repository.Update(aula);
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
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var aula = await _repository.Get(id);
			if (aula != null)
			{
				var equiposxAula =  _equipoRepository.GetAll().Where(x => x.AulaIdAulaNavigation.IdAula == id && x.Estado==1).ToList();
				if (equiposxAula != null)
				{
					foreach (var item in equiposxAula)
					{
						item.Estado = 0;
						await _equipoRepository.Update(item);
					}
				}
				aula.Estado = 0;
				await _repository.Update(aula);
				return Ok("Aula eliminada correctamente");
            }
			else
			{
				return NotFound();
			}
		}

    }
}
