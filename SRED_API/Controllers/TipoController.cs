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
					Icono = tipoDTO.Icono
				};
				await _repository.Insert(tipo);
				return Ok("Tipo de equipo agregado correctamente");
			}else
			{
				return BadRequest(results.Errors.Select(x => x.ErrorMessage));
			}
		}
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var tipos = await _repository.GetTipos();
			return tipos != null ? Ok(tipos) : NotFound("No se encontraron tipos");
		}
		[HttpGet("id")]
		public async Task<IActionResult> Get(int id)
		{
			var tipo = await _repository.GetTipo(id);
			return tipo != null ? Ok(tipo) : NotFound("No se encontró el tipo");
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
					tipo.Icono = tipoDTO.Icono;
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
		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
		{
			var tipo = await _repository.Get(id);
			if (tipo == null)
			{
				return NotFound();

			}
			var equiposxTipo =  _equipoRepository.GetAll().Where(x => x.TipoEquipoIdTipoEquipo == id);
            foreach (var item in equiposxTipo)
            {
                await _equipoRepository.Delete(item);
            }
			await _repository.Delete(tipo);
			return Ok();
        }
	}
}
