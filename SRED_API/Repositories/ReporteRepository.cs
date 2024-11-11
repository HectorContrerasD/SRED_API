﻿using Microsoft.EntityFrameworkCore;
using SRED_API.Models.DTOs;
using SRED_API.Models.Entities;

namespace SRED_API.Repositories
{
    public class ReporteRepository(WebsitosSredContext context) : Repository<Reporte>(context)
    {
        private readonly WebsitosSredContext Context = context;
        public async Task<List<ReporteDatosDTO>> GetReportes()
        {
            var reportes= await Context.Reporte.Include(x=>x.EquipoIdEquipoNavigation).Select(x => new ReporteDatosDTO
            {
                Id = x.IdReporte,
                Aula =x.EquipoIdEquipoNavigation.AulaIdAulaNavigation.Nombre,
                Descripcion =x.Descripcion,
                
                Equipo = $"{x.EquipoIdEquipoNavigation.TipoEquipoIdTipoEquipoNavigation.Nombre} {x.EquipoIdEquipoNavigation.NumeroIdentificacion}",
                Folio = x.Folio
            }).ToListAsync();
            return reportes;
        }
        public async Task<List<ReporteDatosDTO>> GetReportesXFecha(DateOnly fecha)
        {
            var reportes = await Context.Reporte.Include(x => x.EquipoIdEquipoNavigation).Where(x=>x.FechaCreacion == fecha).Select(x => new ReporteDatosDTO
            {
                Id = x.IdReporte,
                Aula = x.EquipoIdEquipoNavigation.AulaIdAulaNavigation.Nombre,
                Descripcion = x.Descripcion,
                Equipo = $"{x.EquipoIdEquipoNavigation.TipoEquipoIdTipoEquipoNavigation.Nombre} {x.EquipoIdEquipoNavigation.NumeroIdentificacion}",
                Folio = x.Folio
            }).ToListAsync();
            return reportes;
        }
        public async Task<List<ReporteDatosDTO>> GetReportesXAtender(DateOnly fecha)
        {
            var reportes = await Context.Reporte.Include(x => x.EquipoIdEquipoNavigation).Where(x => x.FechaCreacion == fecha).Select(x => new ReporteDatosDTO
            {
                Id = x.IdReporte,
                Aula = x.EquipoIdEquipoNavigation.AulaIdAulaNavigation.Nombre,
                Descripcion = x.Descripcion,
                Equipo = $"{x.EquipoIdEquipoNavigation.TipoEquipoIdTipoEquipoNavigation.Nombre} {x.EquipoIdEquipoNavigation.NumeroIdentificacion}",
                Folio = x.Folio
            }).ToListAsync();
            return reportes;
        }
    }
}
