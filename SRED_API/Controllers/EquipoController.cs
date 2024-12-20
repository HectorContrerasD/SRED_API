﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SRED_API.Models.DTOs;
using SRED_API.Models.Entities;
using SRED_API.Models.Validators;
using SRED_API.Repositories;

namespace SRED_API.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class EquipoController : ControllerBase
	{
		private readonly EquipoRepository _repository;
		
        public EquipoController(EquipoRepository Repository)
        {
            _repository = Repository;
		
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
		public async Task<IActionResult> Agregar(EquipoDTO equipoDTO)
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
				await _repository.Insert(equipo);
				return Ok("Equipo agregado correctamente");
			}
			else
			{
				return BadRequest(results.Errors.Select(x => x.ErrorMessage));
			}
		}
        [Authorize(Roles = "Admin,Invitado")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var equipos = await _repository.GetEquipos();
            return equipos != null ? Ok(equipos) : NotFound("No se encontraron equipos");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var equipo = await _repository.GetEquipo(id);
            return equipo != null ? Ok(equipo) : NotFound("No se encontró el equipo");
        }
        [Authorize(Roles = "Invitado")]
        [HttpGet("/api/equipo/poraula")]
		public async Task<IActionResult> GetEquiposPorAula(int idaula)
		{
			var aulaConEquipos = await _repository.GetEquiposByAulaId(idaula);
			return aulaConEquipos != null ? Ok(aulaConEquipos) : NotFound("No se encontraron equipos para el aula");
		}
        [Authorize(Roles = "Admin")]
        [HttpPut]
		public async Task<IActionResult> Editar(EquipoDTO equipoDTO)
		{
			if (equipoDTO == null) { return BadRequest("No estas mandando un dto"); }
			if (equipoDTO.Id !=0)
			{
				var results = EquipoValidator.Validate(equipoDTO);
				if (results.IsValid)
				{
					var equipo = await _repository.Get(equipoDTO.Id);
					if (equipo != null)
					{
						equipo.NumeroIdentificacion = equipoDTO.Numero;
						equipo.AulaIdAula = equipoDTO.AulaId;
						equipo.TipoEquipoIdTipoEquipo= equipoDTO.TipoId;
						await _repository.Update(equipo);
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
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var equipo = await _repository.Get(id);
			if (equipo != null)
			{
				equipo.Estado = 0;
				await _repository.Update(equipo);
				return Ok();
			}
			else
			{
				return NotFound();	
			}
		}
    }
}
