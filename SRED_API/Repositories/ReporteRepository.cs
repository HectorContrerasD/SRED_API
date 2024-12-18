using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SRED_API.Models.DTOs;
using SRED_API.Models.Entities;

namespace SRED_API.Repositories
{
    public class ReporteRepository(WebsitosSredContext context) : Repository<Reporte>(context)
    {
        private readonly WebsitosSredContext Context = context;
        public async Task<List<ReporteDatosDTO>> GetReportes()
        {
            var reportes= await Context.Reporte.Include(x=>x.EquipoIdEquipoNavigation).OrderBy(x=>x.Folio).Select(x => new ReporteDatosDTO
            {
                Id = x.IdReporte,
                Aula =x.EquipoIdEquipoNavigation.AulaIdAulaNavigation.Nombre,
                Descripcion =x.Descripcion,
                FechaCreacion = x.FechaCreacion,
                Equipo = $"{x.EquipoIdEquipoNavigation.TipoEquipoIdTipoEquipoNavigation.Nombre} {x.EquipoIdEquipoNavigation.NumeroIdentificacion}",
                Folio = x.Folio,
                Estado =x.Estado,
                NoControlTrabajo = x.NoControlAl,
            }).ToListAsync();
            return reportes;
        }
        public async Task<ReporteDatosDTO> GetReporteById(int id)
        {
            var reporte = await Context.Reporte.Include(x=>x.EquipoIdEquipoNavigation).Where(x=>x.IdReporte == id).Select(x=> new ReporteDatosDTO
            {
                Id = x.IdReporte,
                Descripcion = x.Descripcion,
                Equipo = x.EquipoIdEquipoNavigation.TipoEquipoIdTipoEquipoNavigation.Nombre,
                Estado = x.Estado,
                Aula = x.EquipoIdEquipoNavigation.AulaIdAulaNavigation.Nombre,
                FechaCreacion =x.FechaCreacion,
                Folio = x.Folio,
                NumeroEquipo = x.EquipoIdEquipoNavigation.NumeroIdentificacion,
                NoControlTrabajo = x.NoControlAl
            }).FirstOrDefaultAsync();
            return reporte;
        }
        public async Task<List<ReporteDatosDTO>>GetReporteByUsuario(string numcontrol)
        {
            var reportes = await Context.Reporte.Include(x => x.EquipoIdEquipoNavigation).OrderBy(x => x.Folio).Where(x => x.NoControlAl == numcontrol).Select(x=>new ReporteDatosDTO
            {
                Id = x.IdReporte,
                Aula = x.EquipoIdEquipoNavigation.AulaIdAulaNavigation.Nombre,
                Descripcion = x.Descripcion,
                FechaCreacion = x.FechaCreacion,
                Equipo = $"{x.EquipoIdEquipoNavigation.TipoEquipoIdTipoEquipoNavigation.Nombre} {x.EquipoIdEquipoNavigation.NumeroIdentificacion}",
                Folio = x.Folio,
                Estado = x.Estado,
                NoControlTrabajo = x.NoControlAl,
            } ).ToListAsync();
            return reportes;
        }
        public async Task<List<ReporteDatosDTO>> GetReportesXFecha(DateOnly fecha)
        {
            var reportes = await Context.Reporte.Include(x => x.EquipoIdEquipoNavigation).OrderBy(x => x.Folio).Where(x=>x.FechaCreacion == fecha).Select(x => new ReporteDatosDTO
            {
                Id = x.IdReporte,
                Aula = x.EquipoIdEquipoNavigation.AulaIdAulaNavigation.Nombre,
                Descripcion = x.Descripcion,
                Equipo = $"{x.EquipoIdEquipoNavigation.TipoEquipoIdTipoEquipoNavigation.Nombre} {x.EquipoIdEquipoNavigation.NumeroIdentificacion}",
                Folio = x.Folio,
                FechaCreacion = x.FechaCreacion,
                Estado = x.Estado,
                NoControlTrabajo = x.NoControlAl
            }).ToListAsync();
            return reportes;
        }
        public async Task<List<ReporteDatosDTO>> GetReportesXAtender()
        {
            var reportes = await Context.Reporte.Include(x => x.EquipoIdEquipoNavigation).OrderByDescending(x => x.Folio).Where(x=>x.Estado == 0).Select(x => new ReporteDatosDTO
            {
                Id = x.IdReporte,
                Aula = x.EquipoIdEquipoNavigation.AulaIdAulaNavigation.Nombre,
                Descripcion = x.Descripcion,
                Equipo = $"{x.EquipoIdEquipoNavigation.TipoEquipoIdTipoEquipoNavigation.Nombre} {x.EquipoIdEquipoNavigation.NumeroIdentificacion}",
                Folio = x.Folio,
                FechaCreacion=x.FechaCreacion,
                Estado = x.Estado,
                NoControlTrabajo    =x.NoControlAl
            }).ToListAsync();
            return reportes;
        }
        public async Task<List<ReporteDatosDTO>> GetReportesAtendidos()
        {
            var reportes = await Context.Reporte.Include(x => x.EquipoIdEquipoNavigation).OrderBy(x => x.Folio).Where(x => x.Estado == 1).Select(x => new ReporteDatosDTO
            {
                Id = x.IdReporte,
                Aula = x.EquipoIdEquipoNavigation.AulaIdAulaNavigation.Nombre,
                Descripcion = x.Descripcion,
                Equipo = $"{x.EquipoIdEquipoNavigation.TipoEquipoIdTipoEquipoNavigation.Nombre} {x.EquipoIdEquipoNavigation.NumeroIdentificacion}",
                Folio = x.Folio,
                FechaCreacion = x.FechaCreacion,
                Estado = x.Estado,
                NoControlTrabajo=x.NoControlAl
            }).ToListAsync();
            return reportes;
        }
        public async Task<List<ReporteDatosDTO>> GetReportesRecientes()
        {
            var reportes = await Context.Reporte.Include(x => x.EquipoIdEquipoNavigation).OrderByDescending(x => x.Folio).ThenBy(x=>x.FechaCreacion).Select(x => new ReporteDatosDTO
            {
                Id = x.IdReporte,
                Aula = x.EquipoIdEquipoNavigation.AulaIdAulaNavigation.Nombre,
                Descripcion = x.Descripcion,
                Equipo = $"{x.EquipoIdEquipoNavigation.TipoEquipoIdTipoEquipoNavigation.Nombre} {x.EquipoIdEquipoNavigation.NumeroIdentificacion}",
                Folio = x.Folio,
                FechaCreacion = x.FechaCreacion,
                Estado = x.Estado,
                NoControlTrabajo = x.NoControlAl
            }).ToListAsync();
            return reportes;
        }
        public async Task<List<ReporteDatosDTO>> GetReportesAntiguos()
        {
            var reportes = await Context.Reporte.Include(x => x.EquipoIdEquipoNavigation).OrderBy(x => x.Folio).ThenBy(x => x.FechaCreacion).Select(x => new ReporteDatosDTO
            {
                Id = x.IdReporte,
                Aula = x.EquipoIdEquipoNavigation.AulaIdAulaNavigation.Nombre,
                Descripcion = x.Descripcion,
                Equipo = $"{x.EquipoIdEquipoNavigation.TipoEquipoIdTipoEquipoNavigation.Nombre} {x.EquipoIdEquipoNavigation.NumeroIdentificacion}",
                Folio = x.Folio,
                FechaCreacion=x.FechaCreacion,
                Estado = x.Estado,
                NoControlTrabajo=x.NoControlAl
            }).ToListAsync();
            return reportes;
        }
        public async Task<int> Count()
        {
            return await Context.Reporte.CountAsync();
        }
    }
}
