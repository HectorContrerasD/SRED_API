﻿using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRED_API.Helpers;
using SRED_API.Models.DTOs;
using SRED_API.Models.Entities;
using SRED_API.Models.Validators;
using SRED_API.Repositories;

namespace SRED_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TipoController : ControllerBase
	{
		private readonly TipoRepository _repository;
		private readonly EquipoRepository _equipoRepository;
        public TipoController(TipoRepository Repository, EquipoRepository equipoRepository)
        {
				_repository = Repository;
			_equipoRepository = equipoRepository;
        }
		[HttpPost]
		public async Task<IActionResult> Agregar(TipoDTO tipoDTO)
		{
			if (tipoDTO == null) { return BadRequest("No estas enviando un dto"); }
			var results = TipoValidator.Validate(tipoDTO);
			if (results.IsValid) 
			{
				var tipo = new Tipoequipo
				{
					Nombre = tipoDTO.Nombre,
					//Icono = tipoDTO.Icono
				};
				await _repository.Insert(tipo);
				if (string.IsNullOrEmpty(tipoDTO.Icono))
				{
					System.IO.File.Copy("wwwroot/images/Default.jpg", $"wwwroot/images/{tipo.IdTipoEquipo}.jpg");
				}
				else
				{
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", $"{tipo.IdTipoEquipo}.jpg");
					var bytes = Convert.FromBase64String(tipoDTO.Icono);
					System.IO.File.WriteAllBytes(path, bytes);
				}
              
                return Ok("Tipo de equipo agregado correctamente");
			}else
			{
				return BadRequest(results.Errors.Select(x => x.ErrorMessage));
			}
		}
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var tipos = await  _repository.GetAll().Select(x=> new TipoDTO
			{
				Id = x.IdTipoEquipo,
				Nombre = x.Nombre,
				Icono = ImageToBase64Helper.ConvertBase64($"wwwroot/images/{x.IdTipoEquipo}.jpg")
			}).ToListAsync();
			return Ok(tipos);
		
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			var tipo = await _repository.GetTipo(id);
			if (tipo == null)
			{
				return NotFound();
			}
			var tipoDTO = new TipoDTO
			{
				Icono = ImageToBase64Helper.ConvertBase64($"wwwroot/images/{tipo.IdTipoEquipo}.jpg"),
				Id = tipo.IdTipoEquipo,
				Nombre = tipo.Nombre
			};
			return Ok(tipoDTO);
		}
		[HttpPut]
		public async Task<IActionResult> Editar(TipoDTO tipoDTO)
		{
			if (tipoDTO == null)
			{
				return BadRequest("No estas mandando un dto");
			}
			if (tipoDTO.Id != 0)
			{
				var result = TipoValidator.Validate(tipoDTO);
				if (result.IsValid)
				{
					var tipo = await _repository.Get(tipoDTO.Id);
					if (tipo == null)
					{
						return NotFound("No se encontró al tipo de equipo");
					}
					tipo.Nombre = tipoDTO.Nombre;
					//tipo.Icono = tipoDTO.Icono;
					await _repository.Update(tipo);
					return Ok("Tipo de equipo editado correctamente");
				}
				else
				{
					return BadRequest(result.Errors.Select(x => x.ErrorMessage));
				}
			}
			else
			{
				return BadRequest();
			}
		}
		[HttpPut("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var tipo = await _repository.Get(id);
			if (tipo == null)
			{
				return NotFound();

			}
			var equiposxTipo =  _equipoRepository.GetAll().Where(x => x.TipoEquipoIdTipoEquipo == id);
			if (equiposxTipo!=null)
			{
				foreach (var item in equiposxTipo)
				{
					await _equipoRepository.Delete(item);
				}
			}
			await _repository.Delete(tipo);
			return Ok();
        }
		
       
    }
}
