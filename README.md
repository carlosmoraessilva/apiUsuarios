# ApiUsuarios

API REST de gerenciamento de usuários desenvolvida com **ASP.NET Core 8**, **SQL Server** e **Entity Framework Core**. Inclui autenticação JWT, hash de senhas (HMAC-SHA512) e documentação interativa via Swagger.

## Tecnologias

| Tecnologia | Uso |
|---|---|
| ASP.NET Core 8 | Framework da API |
| Entity Framework Core 8 | ORM e migrations |
| SQL Server | Banco de dados |
| JWT Bearer | Autenticação |
| Swashbuckle (Swagger) | Documentação e testes da API |

## Funcionalidades

- Cadastro (registro) de usuários
- Login com geração de token JWT (válido por 1 dia)
- Listagem de usuários
- Busca de usuário por ID
- Edição de usuário
- Remoção de usuário
- Senhas armazenadas com hash + salt (HMAC-SHA512)
- Endpoints de usuário protegidos por `[Authorize]`

## Estrutura do projeto

```
ApiUsuarios/
├── Controllers/
│   ├── LoginController.cs      # Register e Login (públicos)
│   └── UsuarioController.cs    # CRUD de usuários (protegido)
├── Data/
│   └── AppDbContext.cs
├── Dto/
│   ├── Login/
│   │   └── UsuarioLoginDto.cs
│   └── Usuario/
│       ├── UsuarioCriacaoDto.cs
│       └── UsuarioEdicaoDto.cs
├── Models/
│   ├── ResponseModel.cs
│   └── UsuarioModel.cs
├── Services/
│   ├── Senha/
│   │   ├── ISenhaInterface.cs
│   │   └── SenhaService.cs
│   └── Usuario/
│       ├── IUsuarioInterface.cs
│       └── UsuarioService.cs
├── Migrations/
├── Program.cs
└── appsettings.json
```

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (LocalDB, Express ou instância completa)
- Ferramenta EF Core CLI (opcional, para migrations):

```bash
dotnet tool install --global dotnet-ef
```

## Configuração

Edite `ApiUsuarios/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ApiUsuarios;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "AppSettings": {
    "Token": "sua-chave-secreta-com-no-minimo-64-caracteres-para-hmac-sha512"
  }
}
```

| Chave | Descrição |
|---|---|
| `ConnectionStrings:DefaultConnection` | String de conexão com o SQL Server |
| `AppSettings:Token` | Chave secreta usada para assinar o JWT |

## Como executar

Na raiz da solution:

```bash
# Restaurar pacotes
dotnet restore

# Aplicar migrations (cria o banco e a tabela Usuarios)
dotnet ef database update --project ApiUsuarios

# Rodar a API
dotnet run --project ApiUsuarios
```

A API sobe em:

- HTTP: `http://localhost:5003`
- HTTPS: `https://localhost:7086`
- Swagger (Development): `http://localhost:5003/swagger`

## Endpoints

### Autenticação (`LoginController`) — públicos

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/api/Login/register` | Cadastra um novo usuário |
| `POST` | `/api/Login/login` | Autentica e retorna o token JWT |

#### Body — Register (`UsuarioCriacaoDto`)

```json
{
  "usuario": "joao.silva",
  "nome": "João",
  "sobrenome": "Silva",
  "email": "joao@email.com",
  "senha": "Senha123!",
  "confirmaSenha": "Senha123!"
}
```

#### Body — Login (`UsuarioLoginDto`)

```json
{
  "email": "joao@email.com",
  "senha": "Senha123!"
}
```

### Usuários (`UsuarioController`) — requer JWT

Inclua o header:

```
Authorization: Bearer {seu_token}
```

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/api/Usuario` | Lista todos os usuários |
| `GET` | `/api/Usuario/{id}` | Busca usuário por ID |
| `PUT` | `/api/Usuario` | Edita um usuário |
| `DELETE` | `/api/Usuario/{id}` | Remove um usuário |

#### Body — Edição (`UsuarioEdicaoDto`)

```json
{
  "id": 1,
  "usuario": "joao.silva",
  "nome": "João",
  "sobrenome": "Silva Atualizado",
  "email": "joao@email.com"
}
```

## Modelo de resposta

Todas as operações retornam um `ResponseModel<T>`:

```json
{
  "dados": { },
  "mensagem": "Operação realizada com sucesso.",
  "status": true
}
```

| Campo | Tipo | Descrição |
|---|---|---|
| `dados` | `T` | Payload da operação (usuário, lista, etc.) |
| `mensagem` | `string` | Mensagem descritiva do resultado |
| `status` | `bool` | `true` em sucesso; `false` em erro de negócio |

## Autenticação JWT

1. Cadastre um usuário em `POST /api/Login/register`
2. Faça login em `POST /api/Login/login`
3. Copie o valor de `dados.token` da resposta
4. No Swagger, clique em **Authorize** e informe: `bearer {token}`
5. Use os endpoints de `/api/Usuario`

O token contém as claims `Email` e `Username`, é assinado com HMAC-SHA512 e expira em **1 dia**.

## Modelo de usuário

| Campo | Tipo | Descrição |
|---|---|---|
| `Id` | `int` | Identificador |
| `Usuario` | `string` | Nome de usuário (único) |
| `Nome` | `string` | Nome |
| `Sobrenome` | `string` | Sobrenome |
| `Email` | `string` | E-mail (único) |
| `Token` | `string` | Último JWT gerado no login |
| `DataCriacao` | `DateTime` | Data de criação |
| `DataAlteracao` | `DateTime` | Data da última alteração |
| `SenhaHash` / `SenhaSalt` | `byte[]` | Credenciais (não enviadas no cadastro em texto puro) |

## Licença

Projeto de uso educacional / pessoal.
