using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SRED_API.Helpers;
using SRED_API.Models.Entities;
using SRED_API.Repositories;
using System.Text;

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
        policy.AllowAnyOrigin() 
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
builder.Services.AddHttpClient("client", builder=>
{
    builder.BaseAddress = new Uri("https://sie.itesrc.edu.mx/api/");
    builder.DefaultRequestHeaders.Accept.Clear();
    builder.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer
    (
        x =>
        {
            var issuer = builder.Configuration.GetSection("Jwt").GetValue<string>("Issuer");
            var audience = builder.Configuration.GetSection("Jwt").GetValue<string>("Audience");
            var secret = builder.Configuration.GetSection("Jwt").GetValue<string>("Secret");
            x.TokenValidationParameters = new()
            {
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret ?? "")),
                ValidateLifetime = true
            };

        }
    );
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
