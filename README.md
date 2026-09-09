# StockFlow

Sistema web de gerenciamento de estoque com controle de produtos, movimentações, relatórios e gerenciamento de usuários com fluxo de aprovação de acesso.

---

## Índice

- [Visão Geral](#visão-geral)
- [Stack](#stack)
- [Pré-requisitos](#pré-requisitos)
- [Instalação e Execução](#instalação-e-execução)
- [Configuração](#configuração)
- [Funcionalidades](#funcionalidades)
- [Arquitetura](#arquitetura)
- [Endpoints da API](#endpoints-da-api)
- [Banco de Dados](#banco-de-dados)
- [Autenticação e Permissões](#autenticação-e-permissões)
- [Repositório](#repositório)

---

## Visão Geral

O StockFlow é uma aplicação fullstack composta por uma API RESTful em .NET e uma SPA em Vue 3. O sistema permite o controle completo de estoque de produtos, registro de entradas e saídas, visualização de relatórios analíticos e gerenciamento de usuários com um fluxo de aprovação de acesso controlado pelo administrador.

---

## Stack

### Backend
| Tecnologia | Versão | Uso |
|---|---|---|
| .NET | 10.0 | Framework principal |
| ASP.NET Core | 10.0 | API RESTful |
| Entity Framework Core | 8.x | ORM e migrations |
| PostgreSQL + Npgsql | — | Banco de dados |
| JWT Bearer | — | Autenticação |
| Swagger / OpenAPI | — | Documentação interativa |

### Frontend
| Tecnologia | Versão | Uso |
|---|---|---|
| Vue.js | 3.4 | Framework principal |
| TypeScript | — | Tipagem estática |
| Vite | 5.x | Build tool e dev server |
| Vue Router | 4.x | Roteamento SPA |
| Pinia | — | Gerenciamento de estado |
| Axios | — | Cliente HTTP |

---

## Pré-requisitos

- [.NET SDK 10.0+](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/) com npm
- [PostgreSQL 14+](https://www.postgresql.org/) rodando na porta 5432

---

## Instalação e Execução

### 1. Clone o repositório

```bash
git clone https://github.com/LucasFrotteLafin/StockFlow.git
cd StockFlow
```

### 2. Backend

```bash
cd StockFlow-Backend/StockFlow
dotnet restore
dotnet run
```

A API sobe em `http://localhost:5244`.  
O Swagger UI fica disponível em `http://localhost:5244` (rota raiz).  
As migrations pendentes são aplicadas automaticamente na inicialização.

### 3. Frontend

```bash
cd StockFlow-Frontend
npm install
npm run dev
```

A aplicação fica disponível em `http://localhost:5173`.

---

## Configuração

### Banco de dados

Edite `StockFlow-Backend/StockFlow/appsettings.json` com as credenciais do seu PostgreSQL:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=5432;Database=StockFlow;User Id=postgres;Password=SUA_SENHA;"
  },
  "JwtSettings": {
    "Key": "ChaveSecretaComMinimo32CaracteresAqui",
    "Issuer": "FocusSpaceAPI",
    "Audience": "FocusSpaceClient",
    "DurationMinutes": 60
  }
}
```

> A chave JWT deve ter no mínimo 32 caracteres.

### Credenciais padrão do admin

```
Usuário: admin
Senha:   admin123
```

---

## Funcionalidades

### Login e controle de acesso

- Autenticação via JWT com expiração de 60 minutos
- Sessão armazenada em `sessionStorage` — expirada ao fechar o navegador
- Dois perfis: **Admin** e **User**
- Proteção de rotas no frontend por perfil

### Solicitação de acesso (cadastro)

1. O usuário acessa `/register` e preenche usuário e senha
2. A senha é criptografada automaticamente (SHA-256) antes de ser salva
3. A solicitação fica com status **Pendente** aguardando aprovação
4. Enquanto não aprovada, o login retorna erro — o usuário não existe ainda na tabela `Users`
5. O admin acessa a página **Pessoas** e aprova ou rejeita a solicitação
6. Ao aprovar, o usuário é criado em `Users` com o hash já armazenado e pode fazer login normalmente
7. A aprovação é feita uma única vez e não pode ser revertida pelo sistema

### Página Pessoas (somente Admin)

- Lista de todas as solicitações pendentes com data de criação
- Ações de **Aprovar** e **Rejeitar** com feedback visual (toast)
- Badge na navbar indica a quantidade de solicitações pendentes em tempo real
- Lista de todos os usuários com acesso ativo

### Dashboard

- Contadores em tempo real: total de produtos, quantidade em estoque, produtos com estoque baixo, total de movimentações
- Ações rápidas para navegação entre módulos

### Produtos

- CRUD completo (criar, listar, editar, excluir)
- Validação de SKU único — impede duplicatas
- Filtros por nome, SKU, categoria e status de estoque
- Indicador visual de estoque baixo (quando abaixo do mínimo configurado)
- `QuantityInStock` é calculado automaticamente pelas movimentações, não editado diretamente

### Movimentações de Estoque

- Registro de **Entrada** e **Saída** com motivo obrigatório
- Validação de quantidade disponível em saídas
- Estoque do produto atualizado automaticamente ao registrar a movimentação
- Histórico completo: data, responsável, produto, tipo, quantidade e motivo
- Rastreamento do usuário que realizou cada movimentação

### Relatórios

- Gráfico de pizza: produtos com maior volume de saídas
- Gráfico de pizza: distribuição do estoque atual por produto
- Renderizados com Canvas nativo (sem dependência externa)
- Legendas com valores exatos e design responsivo

---

## Arquitetura

```
StockFlow/
├── StockFlow-Backend/
│   └── StockFlow/
│       ├── Controllers/
│       │   ├── UserController.cs          # Login
│       │   ├── UserRequestController.cs   # Solicitações de acesso
│       │   ├── ProductController.cs       # CRUD de produtos
│       │   └── MovementController.cs      # Movimentações
│       ├── DatabaseContext/
│       │   ├── DataContext.cs             # DbContext e configurações EF
│       │   └── DesignTimeDbContextFactory.cs
│       ├── Encrypt/
│       │   └── PasswordEncryptor.cs       # SHA-256
│       ├── Mappings/
│       │   ├── UserMap.cs
│       │   └── ProductMap.cs
│       ├── Migrations/
│       │   ├── InitialCreate              # Tabelas base
│       │   ├── AddUserInfoToMovements     # UserId e UserName em Movements
│       │   └── AddUserRequestTable        # Tabela UserRequests
│       ├── Models/
│       │   ├── User.cs
│       │   ├── UserRequest.cs
│       │   ├── Product.cs
│       │   └── Movement.cs
│       ├── Requests/                      # DTOs de entrada
│       ├── Services/
│       │   └── JwtService.cs
│       ├── appsettings.json
│       └── Program.cs
│
└── StockFlow-Frontend/
    └── src/
        ├── api/
        │   └── axios.ts                   # Instância Axios + interceptors JWT
        ├── components/
        │   └── Navbar.vue
        ├── router/
        │   └── index.ts                   # Rotas + guards de autenticação/role
        ├── stores/
        │   ├── auth.ts                    # Login, logout, sessão
        │   ├── people.ts                  # Solicitações e usuários
        │   └── products.ts                # Estado de produtos
        ├── views/
        │   ├── LoginView.vue
        │   ├── RegisterView.vue
        │   ├── DashboardView.vue
        │   ├── ProductsView.vue
        │   ├── MovementView.vue
        │   ├── ReportsView.vue
        │   └── PeopleView.vue             # Somente Admin
        ├── App.vue
        └── main.ts
```

---

## Endpoints da API

### Autenticação

| Método | Rota | Auth | Descrição |
|---|---|---|---|
| `POST` | `/api/user/login` | Público | Retorna JWT token |

### Solicitações de acesso

| Método | Rota | Auth | Descrição |
|---|---|---|---|
| `POST` | `/api/userrequest/register` | Público | Solicita criação de conta |
| `GET` | `/api/userrequest` | Admin | Lista solicitações pendentes |
| `GET` | `/api/userrequest/approved` | Admin | Lista usuários aprovados |
| `POST` | `/api/userrequest/{id}/approve` | Admin | Aprova solicitação e cria o usuário |
| `POST` | `/api/userrequest/{id}/reject` | Admin | Rejeita solicitação |

### Produtos

| Método | Rota | Auth | Descrição |
|---|---|---|---|
| `GET` | `/api/product` | JWT | Lista todos os produtos |
| `GET` | `/api/product/{id}` | JWT | Busca por ID |
| `GET` | `/api/product/sku/{sku}` | JWT | Busca por SKU |
| `GET` | `/api/product/low-stock` | JWT | Produtos com estoque abaixo do mínimo |
| `POST` | `/api/product` | JWT | Cria produto |
| `PUT` | `/api/product/{id}` | JWT | Atualiza produto |
| `DELETE` | `/api/product/{id}` | JWT | Remove produto |

### Movimentações

| Método | Rota | Auth | Descrição |
|---|---|---|---|
| `GET` | `/api/movement` | JWT | Lista todas as movimentações |
| `GET` | `/api/movement/product/{id}` | JWT | Movimentações de um produto |
| `POST` | `/api/movement` | JWT | Registra entrada ou saída |

---

## Banco de Dados

### Tabela `Users`

| Campo | Tipo | Descrição |
|---|---|---|
| `Id` | `int` | PK, auto-increment |
| `Username` | `varchar(50)` | Único |
| `Password` | `varchar(255)` | Hash SHA-256 em Base64 |
| `Role` | `varchar(20)` | `Admin` ou `User` |

### Tabela `UserRequests`

| Campo | Tipo | Descrição |
|---|---|---|
| `Id` | `int` | PK, auto-increment |
| `Username` | `varchar(100)` | Único |
| `PasswordHash` | `text` | Hash SHA-256 da senha solicitada |
| `Status` | `text` | `Pendente`, `Aprovado` ou `Rejeitado` |
| `RequestDate` | `timestamptz` | Data da solicitação |
| `ApprovedDate` | `timestamptz?` | Data da aprovação (nullable) |

### Tabela `Products`

| Campo | Tipo | Descrição |
|---|---|---|
| `Id` | `int` | PK, auto-increment |
| `Name` | `varchar(100)` | Nome do produto |
| `SKU` | `varchar(50)` | Código único |
| `Category` | `varchar(50)` | Categoria |
| `Price` | `decimal` | Preço unitário |
| `QuantityInStock` | `int` | Quantidade atual |
| `MinimumStock` | `int` | Limite para alerta de estoque baixo |

### Tabela `Movements`

| Campo | Tipo | Descrição |
|---|---|---|
| `Id` | `int` | PK, auto-increment |
| `ProductId` | `int` | FK para Products |
| `Quantity` | `int` | Quantidade movimentada |
| `Type` | `text` | `Entrada` ou `Saída` |
| `Reason` | `text` | Motivo da movimentação |
| `MovementDate` | `timestamptz` | Data e hora |
| `UserId` | `int` | ID do usuário responsável |
| `UserName` | `text` | Nome do usuário (desnormalizado) |

---

## Autenticação e Permissões

O sistema usa **JWT Bearer** com as seguintes regras:

| Recurso | User | Admin |
|---|---|---|
| Login | ✅ | ✅ |
| Dashboard | ✅ | ✅ |
| Produtos | ✅ | ✅ |
| Movimentações | ✅ | ✅ |
| Relatórios | ✅ | ✅ |
| Pessoas (aprovação) | ❌ | ✅ |

O token expira em **60 minutos**. A sessão é armazenada em `sessionStorage` e limpa ao fechar o navegador.

A criptografia de senhas usa **SHA-256** — o hash é gerado no backend antes de qualquer persistência, tanto no cadastro (`UserRequests.PasswordHash`) quanto na criação do usuário aprovado (`Users.Password`).

---

## Repositório

https://github.com/LucasFrotteLafin/StockFlow.git
