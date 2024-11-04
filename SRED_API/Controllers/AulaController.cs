﻿using FluentValidation.Results;
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
		private readonly EquipoRepository _equipoRepository;
		public AulaController(AulaRepository Repository, EquipoRepository equipoRepository)
		{
			_repository = Repository;
			_equipoRepository = equipoRepository;
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
		[HttpDelete]
		public ActionResult Delete(int id)
		{
			var aula = _repository.Get(id);
			if (aula != null)
			{
				var equiposxAula = _equipoRepository.GetAll().Where(x => x.AulaIdAulaNavigation.IdAula == id);
                foreach (var item in equiposxAula)
                {
					_equipoRepository.Delete(item);
                }
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