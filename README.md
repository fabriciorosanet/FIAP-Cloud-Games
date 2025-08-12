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

🔄 Pagamentos
```bash
dotnet ef migrations add Initial --project src/FCG.Pagamentos.Infrastructure --context PagamentosDbContext
dotnet ef database update --project src/FCG.Pagamentos.Infrastructure --context PagamentosDbContext
```
🔄 Usuarios
```bash
dotnet ef migrations add Initial --project src/FCG.Usuarios.Infrastructure --context UsuariosDbContext
dotnet ef database update --project src/FCG.Usuarios.Infrastructure --context UsuariosDbContext
```
🔄 Jogos
```bash
dotnet ef migrations add Initial --project src/FCG.Jogos.Infrastructure --context JogosDbContext
dotnet ef database update --project src/FCG.Jogos.Infrastructure --context JogosDbC
```

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

## 🔍 Monitoramento com New Relic

O projeto utiliza **New Relic** para monitoramento de desempenho e rastreamento de erros. Certifique-se de configurar a chave de licença no ambiente antes de iniciar a aplicação. Para mais informações, consulte a [documentação oficial do New Relic](https://docs.newrelic.com/).

---

## 🚀 Deploy com Azure DevOps e WebApp

O pipeline de deploy utiliza o **Azure DevOps** para automação, com publicação em um **Azure WebApp**. Certifique-se de configurar as variáveis de ambiente no pipeline, como a conexão com o banco de dados e as credenciais do Azure.

---
## 🐳 Imagem Docker no Azure Container Registry (ACR)

A aplicação é empacotada em uma imagem Docker e publicada no **Azure Container Registry (ACR)**. Para configurar o ACR, siga os passos abaixo:

1. Crie um ACR no portal do Azure.
2. Configure as credenciais no pipeline do Azure DevOps.
3. A imagem será automaticamente enviada para o repositório após o build.

---

## 📜 Azure Pipelines (CI/CD)

O arquivo `azure-pipelines.yml` está configurado com um pipeline multi-stage. A regra é:

- **CI**: Executado em PRs e commits em qualquer branch, exceto `main`.
- **CD**: Executado apenas em commits na branch `main` (após merge).


### 📂 Estrutura do Projeto

* `FCG.Api`: Camada de API e endpoints
* `FCG.Application`: Regras de negócio e serviços
* `FCG.Domain`: Entidades e interfaces
* `FCG.Infrastructure`: EF Core + Repositórios
* `Configurations`: Serilog, Swagger, Correlation ID
* `Tests`: Testes unitários e de integração
* `Migrations`: Scripts de migração do EF Core
* `Dockerfile`: Configuração para containerização

---
