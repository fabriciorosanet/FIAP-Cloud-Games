## 🚀 Como Iniciar o Projeto FCG

### ✅ Pré-requisitos

* .NET 9 SDK instalado
* SQL Server LocalDB ou instância acessível
* EF Core CLI instalado globalmente:

  ```bash
  dotnet tool install --global dotnet-ef
  ```

---

### 📦 Restauração de Pacotes

Após clonar o repositório, navegue até a raiz do projeto e execute:

```bash
dotnet restore
```

---

### 🛠️ Aplicando Migrations e Criando o Banco

Para aplicar as migrations e criar o banco de dados, acesse a pasta raiz do projeto FCG.Infrastructure:

```bash
dotnet ef migrations add CreateUsuarios --project .\FCG.Infrastructure.csproj --startup-project ..\FCG.Api\FCG.Api.csproj
```

> 🟨 **Atenção**:
> Caso você enfrente erro relacionado à localização de arquivo (`Unable to retrieve project metadata` ou `MSB1009`), **substitua os caminhos pelos absolutos**, incluindo o arquivo `.csproj`. Exemplo:

```bash
dotnet ef database update --project "G:\\FIAP\\FCG\\src\\FCG.Infrastructure\\FCG.Infrastructure.csproj" --startup-project "G:\\FIAP\\FCG\\src\\FCG.Api\\FCG.Api.csproj"
```

---

### 🧱 Gerando Migrations

Para gerar uma nova migration do Entity Framework, , acesse a pasta raiz do projeto FCG.Infrastructure:

```bash
dotnet ef migrations add NomeDaMigration --project .\FCG.Infrastructure.csproj --startup-project ..\FCG.Api\FCG.Api.csproj
```

> ❗ Se houver erro de caminho, use os caminhos absolutos da mesma forma mostrada acima.

---

### 🔄 Executando a Aplicação

Com o banco atualizado, você pode rodar a API com:

```bash
dotnet run --project FCG.Api
```

A API será iniciada em `https://localhost:5018` (ou conforme configurado).

---

### 🐞 Logs e Correlation ID

* O projeto utiliza **Serilog com arquivos por data**, logando automaticamente **somente erros**.
* Cada requisição possui um **Correlation ID** adicionado aos logs, facilitando rastreabilidade.
* Logs são salvos na pasta `/logs/YYYY-MM-DD/` no servidor local.

---

### 📂 Estrutura do Projeto

* `FCG.Api`: Camada de API e endpoints
* `FCG.Application`: Regras de negócio e serviços
* `FCG.Domain`: Entidades e interfaces
* `FCG.Infrastructure`: EF Core + Repositórios
* `Configurations`: Serilog, Swagger, Correlation ID

---
