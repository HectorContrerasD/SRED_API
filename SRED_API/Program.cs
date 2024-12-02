using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SRED_API.Helpers;
using SRED_API.Models.Entities;
using SRED_API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodos", policy =>
    {
        policy.AllowAnyOrigin() // Cambia esto al origen que necesites permitir
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddDbContext<WebsitosSredContext>
	(
		x => x.UseMySql("database=websitos_SRED;user=websitos_sred;password=56kXu91^j;server=65.181.111.21",
		Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.11.8-mariadb"))
	);
builder.Services.AddTransient<AulaRepository>();
builder.Services.AddTransient<EquipoRepository>();
builder.Services.AddTransient<TipoRepository>();
builder.Services.AddTransient<ReporteRepository>();
builder.Services.AddTransient<UsuarioReporisitory>();
builder.Services.AddSingleton<JWTHelper>();
var app = builder.Build();

app.UseCors("PermitirTodos");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();	

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
