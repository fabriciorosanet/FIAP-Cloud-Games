# Estágio 1: Build (Construção) - Usamos o SDK completo do .NET
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

# Copia todos os arquivos .csproj para restaurar as dependências
COPY ["FCG.Api/FCG.Api.csproj", "FCG.Api/"]
COPY ["FCG.Application/FCG.Application.csproj", "FCG.Application/"]
COPY ["FCG.Domain/FCG.Domain.csproj", "FCG.Domain/"]
COPY ["FCG.Infrastructure/FCG.Infrastructure.csproj", "FCG.Infrastructure/"]

# Restaura as dependências
RUN dotnet restore "FCG.Api/FCG.Api.csproj"

# Copia todo o resto do código fonte
COPY . .

# Publica a aplicação
WORKDIR "/source/FCG.Api"
RUN dotnet publish -c Release -o /app/publish

# Estágio 2: Final - Usamos apenas o runtime do ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FCG.Api.dll"]