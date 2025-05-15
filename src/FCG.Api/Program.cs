using CorrelationId;
using CorrelationId.DependencyInjection;
using FCG.Api.Configurations;
using FCG.Api.Configurations.Serilog;
using FCG.Api.Configurations.Swagger;
using FCG.Infrastructure;
using FCG.Application.Authentication;
using FCG.Api.Configurations.Authentication;
using FCG.Application.Authentication.Interface;
using FCG.Application.Authentication.Service;
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
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddScoped<ITokenService, TokenService>();

// Adicionar servi�os ao container
// builder.Services.AddOpenApi();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	db.Database.Migrate();
}

// Configurar o pipeline de requisi��o HTTP
if (app.Environment.IsDevelopment())
{
	app.UseSwaggerSetup();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCorrelationId(); // Certifique-se de que o middleware de CorrelationId � chamado aqui
app.EndpointsMap(); 

app.Run();
