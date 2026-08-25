# 📦 StockFlow - Sistema de Gerenciamento de Estoque

**StockFlow** é um sistema completo de gerenciamento de estoque desenvolvido com tecnologias modernas. Permite o controle eficiente de produtos, movimentações (entradas e saídas), relatórios analíticos e rastreamento de responsáveis por cada operação.

## ✅ Status do Projeto

- ✅ **Backend**: API .NET funcionando completamente
- ✅ **Frontend**: Interface Vue.js responsiva
- ✅ **Banco de dados**: PostgreSQL configurado e otimizado
- ✅ **CORS**: Configurado e testado
- ✅ **Autenticação**: Sistema completo de usuários
- ✅ **Swagger UI**: Documentação interativa disponível

---

## 🚀 Como Executar o Projeto

### ⚡ Início Rápido

1. **Backend**:
    
    ```bash
    cd StockFlow-Backend/StockFlow
    dotnet run
    ```
    
    📍 Disponível em: **http://localhost:5244**  
    📚 Swagger: **http://localhost:5244**
    
2. **Frontend**:
    
    ```bash
    cd StockFlow-Frontend
    npm run dev
    ```
    
    📍 Disponível em: **http://localhost:5173**
    

### 📋 Pré-requisitos

- **.NET SDK 10.0** ou superior
- **Node.js 18+** com npm
- **PostgreSQL** instalado e configurado
- **Porta 5244** livre para backend
- **Porta 5173** livre para frontend

---

## 🔧 Configuração do Banco de Dados

### Schema do Banco

O sistema criará automaticamente as seguintes tabelas:

- **Users**: Usuários do sistema
- **Products**: Catálogo de produtos
- **Movements**: Histórico de movimentações


---

## 🛠️ Tecnologias Utilizadas

### Backend (.NET 10)

- **ASP.NET Core** - Framework web
- **Entity Framework Core** - ORM
- **PostgreSQL** com Npgsql
- **Swagger/OpenAPI** - Documentação
- **CORS** configurado para desenvolvimento

### Frontend (Vue.js 3)

- **Vue.js 3.4** - Framework principal
- **TypeScript** - Tipagem estática
- **Vite** - Build tool otimizado
- **Vue Router** - Roteamento SPA
- **Pinia** - Gerenciamento de estado
- **Axios** - Cliente HTTP com interceptors

---

## ✨ Funcionalidades Implementadas

### 📊 Dashboard Analítico

- Estatísticas em tempo real
- Contadores: Total produtos, estoque atual, estoque baixo
- Navegação rápida entre módulos

### 📦 Gestão de Produtos

- ✅ **CRUD completo**: Criar, listar, editar, deletar
- ✅ **Validação SKU único**: Impede produtos duplicados
- ✅ **Filtros avançados**: Nome, categoria, status de estoque
- ✅ **Alertas visuais**: Badges para status de estoque
- ✅ **Controle de estoque mínimo**: Sistema de alertas

### 🔄 Movimentações de Estoque

- ✅ **Entrada de produtos**: Adicionar ao estoque
- ✅ **Saída de produtos**: Validação de quantidade disponível
- ✅ **Histórico completo**: Todas as transações registradas
- ✅ **Rastreamento de usuário**: Quem fez cada movimentação
- ✅ **Atualização automática**: Estoque calculado em tempo real

### 📈 Relatórios Visuais

- ✅ **Gráficos de pizza**: Canvas nativo para performance
- ✅ **Análise de movimentações**: Produtos mais/menos movimentados
- ✅ **Análise de estoque**: Distribuição atual do inventário
- ✅ **Design responsivo**: Adaptável a todos os dispositivos

### 🔐 Sistema de Autenticação

- ✅ **Login/Registro**: Sistema completo de usuários
- ✅ **Criptografia de senhas**: Hash seguro
- ✅ **Proteção de rotas**: Middleware de autenticação
- ✅ **Persistência de sessão**: Storage local

---

## 🏗️ Arquitetura do Sistema

### Estrutura Backend

```
StockFlow-Backend/
├── Controllers/           # Endpoints da API
│   ├── ProductController  # CRUD de produtos
│   ├── MovementController # Movimentações
│   └── UserController    # Autenticação
├── Models/               # Entidades do domínio
├── DatabaseContext/      # Configuração EF Core
├── Requests/            # DTOs de entrada
├── Migrations/          # Versionamento do BD
└── Program.cs           # Configuração da API
```

### Estrutura Frontend

```
StockFlow-Frontend/
├── views/               # Páginas da aplicação
│   ├── DashboardView    # Painel principal
│   ├── ProductsView     # Gestão de produtos
│   ├── MovementView     # Movimentações
│   └── ReportsView      # Relatórios
├── stores/              # Estado global (Pinia)
├── api/                 # Configuração Axios
└── router/              # Roteamento SPA
```

---

## 📡 API Endpoints

### 🔐 Autenticação

```http
POST /api/user/register  # Criar conta
POST /api/user/login     # Fazer login
```

### 📦 Produtos

```http
GET    /api/product           # Listar todos
GET    /api/product/{id}      # Buscar por ID
GET    /api/product/sku/{sku} # Buscar por SKU
GET    /api/product/low-stock # Produtos com estoque baixo
POST   /api/product           # Criar produto
PUT    /api/product/{id}      # Atualizar produto
DELETE /api/product/{id}      # Deletar produto
```

### 🔄 Movimentações

```http
GET  /api/movement                  # Todas as movimentações
GET  /api/movement/product/{id}     # Por produto
POST /api/movement                  # Nova movimentação
```

---

## 📊 Métricas do Projeto

- **Endpoints**: 12 endpoints funcionais
- **Tabelas**: 3 entidades principais
- **Componentes Vue**: 8 componentes
- **Cobertura**: Frontend e Backend completos


---

## 🔒 Segurança Implementada

- ✅ **Senhas criptografadas**: Hash seguro no backend
- ✅ **Validação de entrada**: Sanitização de dados
- ✅ **CORS configurado**: Políticas de segurança
- ✅ **SQL Injection**: Proteção via EF Core
- ✅ **XSS Protection**: Escapamento automático Vue.js
- ✅ **Validação de tipos**: TypeScript no frontend

---

## 📝 Notas de Desenvolvimento

### Configurações Importantes

- **Backend Port**: 5244 (configurado no Program.cs)
- **Frontend Port**: 5173 (padrão Vite)
- **Database**: PostgreSQL porta 5432
- **CORS**: Permite apenas localhost:5173


---

## 👨‍💻 Sobre o Desenvolvimento

Este projeto foi desenvolvido como um sistema completo de gerenciamento de estoque, utilizando as melhores práticas de desenvolvimento:

- **Clean Architecture**: Separação de responsabilidades
- **RESTful API**: Endpoints bem estruturados
- **Responsive Design**: Interface adaptável
- **Error Handling**: Tratamento robusto de erros
- **Code Quality**: Código limpo e documentado


---

## 🚀 Tecnologias Utilizadas

### Backend

- **.NET 10.0** - Framework principal
- **ASP.NET Core** - API RESTful
- **Entity Framework Core** - ORM para acesso ao banco de dados
- **PostgreSQL** - Banco de dados relacional
- **Npgsql** - Provider PostgreSQL para .NET
- **Swagger/OpenAPI** - Documentação interativa da API

### Frontend

- **Vue.js 3.4** - Framework JavaScript progressivo
- **TypeScript** - Tipagem estática
- **Vite** - Build tool e dev server
- **Vue Router** - Gerenciamento de rotas
- **Pinia** - Gerenciamento de estado
- **Axios** - Cliente HTTP

---

## ✨ Funcionalidades

### 📊 Dashboard

- Visão geral do estoque com estatísticas em tempo real
- Cards informativos: Total de produtos, itens em estoque, estoque baixo, movimentações
- Ações rápidas para navegação

### 📦 Gerenciamento de Produtos

- **Listagem completa** de produtos com informações detalhadas
- **Filtros avançados**: Busca por nome, SKU ou categoria
- **Filtros de estoque**: Todos, Estoque Baixo, Estoque OK
- **Status visual**: Badges coloridos (Sem Estoque, Estoque Baixo, Normal)
- **Validação de SKU único**: Não permite produtos duplicados
- **Edição e exclusão** de produtos
- **Controle de estoque mínimo**: Alertas automáticos

### 🔄 Movimentações de Estoque

- **Adicionar novos produtos** diretamente na página de movimentações
- **Registrar entradas**: Produtos existentes (seleção por dropdown)
- **Registrar saídas**: Validação de quantidade disponível
- **Histórico completo**: Todas as movimentações com data, responsável, produto, tipo, quantidade e motivo
- **Rastreamento de usuário**: Registro automático de quem realizou cada movimentação
- **Atualização automática**: Estoque atualizado em tempo real

### 📈 Relatórios e Análises

- **Gráfico de Movimentação de Saída**: Visualização em pizza dos produtos com mais e menos saídas
- **Gráfico de Quantidade em Estoque**: Visualização em pizza dos produtos com maior e menor quantidade atual
- **Legendas interativas**: Detalhamento de cada item com valores exatos
- **Design responsivo**: Gráficos adaptáveis a diferentes tamanhos de tela

### 👤 Sistema de Autenticação

- **Login** com validação de credenciais
- **Registro** de novos usuários
- **Proteção de rotas**: Acesso apenas para usuários autenticados
- **Persistência de sessão**: Usuário mantido logado
- **Logout** seguro

### 🎨 Interface Moderna

- Design limpo e intuitivo
- Animações suaves e transições
- Responsivo para desktop e mobile
- Feedback visual para todas as ações
- Mensagens de sucesso e erro

---

## 🏗️ Arquitetura do Projeto

### Backend Structure

```
StockFlow-Backend/
├── StockFlow/
│   ├── Controllers/          # Endpoints da API
│   │   ├── MovementController.cs
│   │   ├── ProductController.cs
│   │   └── UserController.cs
│   ├── Models/              # Modelos de dados
│   │   ├── Movement.cs
│   │   ├── Product.cs
│   │   └── User.cs
│   ├── DatabaseContext/     # Contexto do banco de dados
│   │   ├── DataContext.cs
│   │   └── DesignTimeDbContextFactory.cs
│   ├── Requests/            # DTOs de requisição
│   │   ├── CreateMovementRequest.cs
│   │   ├── CreateProductRequest.cs
│   │   ├── LoginRequest.cs
│   │   └── UpdateProductRequest.cs
│   ├── Mappings/            # Configurações do EF Core
│   │   ├── MovementMap.cs
│   │   ├── ProductMap.cs
│   │   └── UserMap.cs
│   ├── Migrations/          # Migrations do banco de dados
│   ├── Encrypt/             # Criptografia de senhas
│   │   └── PasswordEncryptor.cs
│   ├── Program.cs           # Ponto de entrada da aplicação
│   └── appsettings.json     # Configurações
```

### Frontend Structure

```
StockFlow-Frontend/
├── src/
│   ├── views/               # Páginas da aplicação
│   │   ├── DashboardView.vue
│   │   ├── ProductsView.vue
│   │   ├── MovementView.vue
│   │   ├── ReportsView.vue
│   │   ├── LoginView.vue
│   │   └── RegisterView.vue
│   ├── components/          # Componentes reutilizáveis
│   │   └── Navbar.vue
│   ├── stores/              # Gerenciamento de estado (Pinia)
│   │   ├── auth.ts
│   │   └── products.ts
│   ├── router/              # Configuração de rotas
│   │   └── index.ts
│   ├── api/                 # Configuração do Axios
│   │   └── axios.ts
│   ├── App.vue              # Componente raiz
│   ├── main.ts              # Ponto de entrada
│   └── style.css            # Estilos globais
```

---

## 🗄️ Modelo de Dados

### Tabela: Users

|Campo|Tipo|Descrição|
|---|---|---|
|Id|int|Identificador único|
|Username|string|Nome de usuário|
|Password|string|Senha criptografada|
|Role|string|Papel do usuário|

### Tabela: Products

|Campo|Tipo|Descrição|
|---|---|---|
|Id|int|Identificador único|
|Name|string|Nome do produto|
|SKU|string|Código único (índice único)|
|Category|string|Categoria do produto|
|Price|decimal|Preço do produto|
|QuantityInStock|int|Quantidade em estoque|
|MinimumStock|int|Estoque mínimo alerta|

### Tabela: Movements

|Campo|Tipo|Descrição|
|---|---|---|
|Id|int|Identificador único|
|ProductId|int|ID do produto|
|Quantity|int|Quantidade movimentada|
|Type|string|Tipo (Entrada/Saída)|
|Reason|string|Motivo da movimentação|
|MovementDate|DateTime|Data e hora da movimentação|
|UserId|int|ID do usuário responsável|
|UserName|string|Nome do usuário responsável|

---

## 🔧 Instalação e Configuração

### Pré-requisitos

- **.NET SDK 10.0** ou superior
- **Node.js +**  npm
- **PostgreSQL** instalado e rodando
- **Git** para clonar o repositório

### 1. Clone o repositório

```bash
git clone https://github.com/seu-usuario/stockflow.git
cd stockflow
```

### 2. Configuração do Backend

#### 2.1. Configurar string de conexão

Edite o arquivo `StockFlow-Backend/StockFlow/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=StockFlow;Username=seu_usuario;Password=sua_senha"
  }
}
```

#### 2.2. Restaurar dependências e aplicar migrations

```bash
cd StockFlow-Backend
dotnet restore
dotnet ef database update --project StockFlow
```

#### 2.3. Executar o backend

```bash
dotnet run --project StockFlow/StockFlow.csproj
```

O backend estará disponível em: **http://localhost:5244**

Swagger UI: **http://localhost:5244/swagger**

### 3. Configuração do Frontend

#### 3.1. Instalar dependências

```bash
cd StockFlow-Frontend
npm install
```

#### 3.2. Executar o frontend

```bash
npm run dev
```

O frontend estará disponível em: **http://localhost:5173**

---

## 📡 Endpoints da API

### Autenticação e Usuários

- `POST /api/user/register` - Registrar novo usuário
- `POST /api/user/login` - Fazer login
- `GET /api/user` - Listar todos os usuários

### Produtos

- `GET /api/product` - Listar todos os produtos
- `GET /api/product/{id}` - Buscar produto por ID
- `GET /api/product/sku/{sku}` - Buscar produto por SKU
- `GET /api/product/low-stock` - Listar produtos com estoque baixo
- `POST /api/product` - Criar novo produto
- `PUT /api/product/{id}` - Atualizar produto
- `DELETE /api/product/{id}` - Deletar produto

### Movimentações

- `GET /api/movement` - Listar todas as movimentações
- `GET /api/movement/product/{productId}` - Movimentações por produto
- `POST /api/movement` - Registrar movimentação (entrada/saída)

---

## 🔐 Segurança

- **Senhas criptografadas**: Utilizando hash seguro
- **Validação de dados**: No backend e frontend
- **Proteção de rotas**: Apenas usuários autenticados
- **CORS configurado**: Políticas de segurança
- **Validação de SKU único**: Índice único no banco de dados

---

## 🎯 Funcionalidades Especiais

### ✅ Validações Implementadas

- SKU único (não permite duplicatas)
- Quantidade disponível em saídas
- Usuário autenticado para movimentações
- Dados obrigatórios em todos os formulários

### 📊 Relatórios Visuais

- Gráficos de pizza desenhados nativamente com Canvas
- Cores diferenciadas para cada categoria
- Legendas interativas
- Responsivo e adaptável

### 🔄 Atualizações em Tempo Real

- Estoque atualizado automaticamente
- Histórico de movimentações sempre atualizado
- Dashboard com dados em tempo real


---

## 📝 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](https://claude.ai/chat/LICENSE) para mais detalhes.

---

## 🐈‍⬛ Link do Git

https://github.com/LucasFrotteLafin/StockFlow.git
