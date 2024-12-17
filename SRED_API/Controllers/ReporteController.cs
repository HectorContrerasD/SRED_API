using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SRED_API.Models.DTOs;
using SRED_API.Models.Entities;
using SRED_API.Models.Validators;
using SRED_API.Repositories;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace SRED_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReporteController : ControllerBase
    {
        private readonly ReporteRepository _repository;
        public ReporteController(ReporteRepository reporteRepository)
        {
            _repository = reporteRepository;
        }
        [Authorize(Roles = "Encargado")]
        [HttpGet("reportes")] //fromquery = /reportes?fecha=2024-11-10
        public async Task<IActionResult> GetReportes([FromQuery] DateOnly? fecha = null)
        {
            var reportes = await _repository.GetReportes();
            if (fecha.HasValue)
            {
                reportes = reportes.Where(x => x.FechaCreacion == fecha).OrderBy(x=>x.Folio).ToList();
            }
            return reportes.Any() ? Ok(reportes) : NotFound();
        }
        [Authorize(Roles = "Encargado")]
        [HttpGet("reportesxatender")]
        public async Task<IActionResult> GetReportesXAtender([FromQuery] DateOnly? fecha = null)
        {
            var reportes = await _repository.GetReportesXAtender();
            if (fecha.HasValue)
            {
                reportes = reportes.Where(x => x.FechaCreacion == fecha).OrderBy(x=>x.Folio).ToList();
            }
            return reportes.Any() ? Ok(reportes) : NotFound();
        }
        [Authorize(Roles = "Encargado")]
        [HttpGet("reportesatendidos")]
        public async Task<IActionResult> GetReportesAtendidos(DateOnly? fecha = null)
        {
            var reportes = await _repository.GetReportesAtendidos();
            if (fecha.HasValue)
            {
                reportes = reportes.Where(x => x.FechaCreacion == fecha).OrderBy(x => x.Folio).ToList();
            }
            return reportes.Any() ? Ok(reportes) : NotFound();
        }
        [Authorize(Roles = "Encargado")]
        [HttpGet("reportesxmasantiguos")]
        public async Task<IActionResult> GetReportesXAntiguedad()
        {
            var reportes = await _repository.GetReportesAntiguos();
            return reportes.Any() ? Ok(reportes) : NotFound();
        }
        [Authorize(Roles = "Encargado")]
        [HttpGet("reportesxmasrecientes")]
        public async Task<IActionResult> GetReportesxMasRecientes1()
        {
            var reportes = await _repository.GetReportesRecientes();
            return reportes.Any() ? Ok(reportes) : NotFound();
        }
        [Authorize(Roles = "Encargado,Invitado")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReporteById(int id)
        {
            var reporte = await _repository.GetReporteById(id);
            if(reporte==null) return NotFound();
            return Ok(reporte);
            
        }
        [Authorize(Roles = "Invitado")]
        [HttpGet("pornumerocontrol/{numctrl}")]
        public async Task<IActionResult> GetReporteByNumControl(string numctrl)
        {
            var reportes = await _repository.GetReporteByUsuario(numctrl);
            return reportes.Any() ? Ok(reportes) : NotFound();
           
        }
        [Authorize(Roles = "Invitado")]
        [HttpPost]
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
                reporte.Estado = 0;
                reporte.FechaCreacion = DateOnly.FromDateTime(DateTime.Now);
                int count = await _repository.Count();
                
                reporte.Folio = $"DTICS{(count + 1):D5}";
                await _repository.Insert(reporte);
                return Ok("Reporte agregado correctamente");
            }
            else
            {
                return BadRequest(results.Errors.Select(x => x.ErrorMessage));
            }
        }
        [Authorize(Roles = "Encargado")]
        [HttpPut("{id}")]
        public async Task<IActionResult> ModificarEstado(int id)
        {
            var reporte = await _repository.Get(id);
            if (reporte == null) { return NotFound(); } 
            reporte.Estado= 1;
            await _repository.Update(reporte);
            string emailAdd = $"{reporte.NoControlAl}@rcarbonifera.tecnm.mx";
            try
            {
                var mailMess = new MailMessage();
                mailMess.From = new MailAddress("201G0239@rcarbonifera.tecnm.mx");
                mailMess.To.Add(emailAdd);
                mailMess.Subject = "Reporte atendido";
                mailMess.Body = $"Estimado usuario, el reporte con el número de folio {reporte.Folio} ha sido atendido, que tenga un buen día";
                using (var smtpClient = new SmtpClient("Smtp.outlook.office365.com", 587))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new System.Net.NetworkCredential("201G0239@rcarbonifera.tecnm.mx", "Ingreso2020");
                    smtpClient.EnableSsl = true;
                    await smtpClient.SendMailAsync(mailMess);
                }

            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Error al mandar el mensaje: {ex.Message}");
            }
            return Ok("Estado Modificado");
        }
    }
}
