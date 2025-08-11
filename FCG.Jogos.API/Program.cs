using FCG.Jogos.API.Endpoints;
using FCG.Jogos.Application.Jogos.Interfaces;
using FCG.Jogos.Application.Jogos.Services;
using FCG.Jogos.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/jogos-api-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Configuração dos serviços
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do Entity Framework
builder.Services.AddDbContext<JogosDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro dos serviços
builder.Services.AddScoped<IJogoService, JogoService>();
builder.Services.AddScoped<ICompraService, CompraService>();

var app = builder.Build();

// Configuração do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Mapeamento dos endpoints
app.MapJogoEndpoints();
app.MapCompraEndpoints();

// Migração automática do banco de dados
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<JogosDbContext>();
    db.Database.Migrate();
}

app.Run(); 