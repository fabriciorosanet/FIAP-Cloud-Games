using CorrelationId;
using CorrelationId.Abstractions;
using CorrelationId.DependencyInjection;
using FCG.Api.Configurations;
using FCG.Api.Configurations.Serilog;
using FCG.Api.Configurations.Swagger;
using FCG.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructure(builder.Configuration);
LoggingConfiguration.ConfigureLogging(builder);
builder.Services.AddDefaultCorrelationId(options =>
{
	options.AddToLoggingScope = true;
	options.UpdateTraceIdentifier = true;
});

builder.Services.AddEndpointsApiExplorer();                                    
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerServices();
builder.Services.AddApplicationServices();

// Adicionar serviços ao container
// builder.Services.AddOpenApi();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	db.Database.Migrate();
	DataSeeder.Seed(db);
}

// Configurar o pipeline de requisição HTTP
if (app.Environment.IsDevelopment())
{
	app.UseSwaggerSetup();
}

app.UseHttpsRedirection();
app.UseCorrelationId(); // Certifique-se de que o middleware de CorrelationId é chamado aqui
app.EndpointsMap(); 

app.Run();
