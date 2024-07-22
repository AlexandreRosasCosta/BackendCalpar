# Projeto BackendCalpar

## Descrição

O projeto BackendCalpar é uma aplicação ASP.NET Core que consome uma API externa, armazena os dados em um banco de dados SQLite e expõe operações CRUD (Create, Read, Update, Delete) através de endpoints HTTP e HTTPS. A aplicação utiliza autenticação JWT (JSON Web Token) para proteger os endpoints e também gera documentação da API usando Swagger.

## Funcionalidades

- Consumo de API externa para buscar e armazenar dados.
- Operações CRUD (Create, Read, Update, Delete) para o modelo User.
- Autenticação e autorização utilizando JWT.
- Documentação da API gerada automaticamente com Swagger.
- Suporte a CORS para permitir requisições de qualquer origem.

## Requisitos

- [.NET 8 SDK]
- Banco de dados SQLite
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.AspNetCore.Authentication.JwtBearer
- Microsoft.EntityFrameworkCore.Sqlite
- Newtonsoft.Json
- Swashbuckle.AspNetCore
- Swashbuckle.AspNetCore.Annotations
- Swashbuckle.AspNetCore.SwaggerGen
- Swashbuckle.Core
- System.IdentityModel.Tokens.Jwt
- Swashbuckle.AspNetCore.Swagger
- Swashbuckle.AspNetCore.SwaggerUI

## Configuração do Projeto

### Clonando o Repositório
- git clone https://github.com/AlexandreRosasCosta/BackendCalpar.git
- cd BackendCalpar

### Configuração do Banco de Dados
- A string de conexão para o banco de dados SQLite está definida no arquivo appsettings.Development.json. Certifique-se de que o arquivo app.db é acessível e tem as permissões necessárias. 

## Executando a Aplicação

### Usando .NET CLI
- dotnet ef migrations add InitialCreate
- dotnet ef database update
- dotnet run

## Documentação da API
A documentação da API é gerada automaticamente e pode ser acessada no Swagger UI (/).

## Endpoints da API
### Autenticação
GET /api/User/auth/get-token: Autenticação de usuário e obtenção do token JWT.
### Usuários
- GET /api/User: Obtém todos os usuários. (Requer autenticação)
- GET /api/User/{id}: Obtém um usuário específico pelo ID. (Requer autenticação)
- GET /api/health: Verifica a saúde da API.
- POST /api/User: Cria um novo usuário. (Requer autenticação)
- POST /api/User/fetch-users: Busca e armazena usuários da API externa. (Requer autenticação)
- PUT /api/User/{id}: Atualiza um usuário existente pelo ID. (Requer autenticação)
- DELETE /api/User/{id}: Exclui um usuário específico pelo ID. (Requer autenticação)

## Autenticação e Autorização
A aplicação utiliza JWT (JSON Web Token) para autenticação e autorização. Os tokens são gerados ao autenticar o usuário através do endpoint /api/User/auth/get-token e devem ser enviados no cabeçalho das requisições protegidas.

### Exemplo de Requisição para Obter Token:

GET /auth/get-token

### Exemplo de Header de Autorização
Authorization: Bearer {seu-token-jwt}