# 📦 StockFlow - Sistema de Gerenciamento de Estoque

![StockFlow](https://img.shields.io/badge/StockFlow-v1.0-blue)
![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)
![Vue.js](https://img.shields.io/badge/Vue.js-3.4-4FC08D)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-Latest-336791)
![License](https://img.shields.io/badge/license-MIT-green)

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

### Configuração Atual (appsettings.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=5432;Database=StockFlow;User Id=postgres;Password=240505;"
  }
}
```

### Schema do Banco
O sistema criará automaticamente as seguintes tabelas:

- **Users**: Usuários do sistema
- **Products**: Catálogo de produtos
- **Movements**: Histórico de movimentações

### ⚠️ Problema Resolvido
**Issue**: Erro de CORS e coluna SupplierId  
**Solução**: 
- CORS configurado corretamente
- Coluna SupplierId removida do banco
- Validações de SKU único implementadas

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

## 🐛 Problemas Resolvidos

### ❌ Erro de CORS
**Problema**: `Access to XMLHttpRequest has been blocked by CORS policy`  
**Causa**: Configuração incorreta de CORS no backend  
**Solução**: 
- Configuração específica para `http://localhost:5173`
- Headers apropriados: `Access-Control-Allow-Origin`
- `withCredentials: true` no axios

### ❌ Erro 500 ao criar produtos
**Problema**: `SupplierId column cannot be null`  
**Causa**: Coluna órfã no banco de dados  
**Solução**: 
```sql
ALTER TABLE "Products" DROP COLUMN IF EXISTS "SupplierId";
```

### ❌ Problemas de validação
**Problema**: SKU duplicados e dados inválidos  
**Solução**: 
- Validação de SKU único no backend
- Tratamento de erros com try/catch
- Feedback visual no frontend

---

## 🔧 Comandos de Desenvolvimento

### Backend (.NET)
```bash
# Executar em desenvolvimento
dotnet run --project StockFlow

# Executar com hot reload
dotnet watch run --project StockFlow

# Aplicar migrations
dotnet ef database update --project StockFlow

# Criar nova migration
dotnet ef migrations add NomeMigration --project StockFlow
```

### Frontend (Vue/Vite)
```bash
# Desenvolvimento com hot reload
npm run dev

# Build para produção
npm run build

# Preview do build
npm run preview

# Limpar cache do npm
npm run clean
```

### Banco de Dados (PostgreSQL)
```bash
# Conectar ao banco
psql -h localhost -U postgres -d StockFlow

# Backup do banco
pg_dump -h localhost -U postgres StockFlow > backup.sql

# Restaurar backup
psql -h localhost -U postgres StockFlow < backup.sql
```

---

## 📊 Métricas do Projeto

- **Linhas de código**: ~2.500+ linhas
- **Endpoints**: 12 endpoints funcionais
- **Tabelas**: 3 entidades principais
- **Componentes Vue**: 8 componentes
- **Cobertura**: Frontend e Backend completos

---

## 🚀 Próximas Funcionalidades

### 🔮 Roadmap v2.0
- [ ] **Relatórios em PDF**: Exportação de dados
- [ ] **Sistema de Roles**: Admin, Usuário, Visualizador
- [ ] **Notificações**: Email para estoque baixo
- [ ] **Dashboard avançado**: Mais gráficos e KPIs
- [ ] **Multi-depósito**: Controle por localização
- [ ] **API de integração**: Webhooks e REST avançado
- [ ] **App mobile**: React Native ou Flutter
- [ ] **Código de barras**: Scanner integrado

### 🎯 Melhorias Técnicas
- [ ] **Testes unitários**: Backend e Frontend
- [ ] **CI/CD Pipeline**: GitHub Actions
- [ ] **Docker**: Containerização completa
- [ ] **Logs estruturados**: Serilog integration
- [ ] **Cache Redis**: Performance otimizada
- [ ] **Monitoring**: Application Insights

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

### Troubleshooting Comum
1. **Backend não inicia**: Verificar se PostgreSQL está rodando
2. **Erro de conexão**: Confirmar credenciais no appsettings.json  
3. **CORS Error**: Verificar se frontend está em localhost:5173
4. **Erro 500**: Verificar logs do backend no console

---

## 👨‍💻 Sobre o Desenvolvimento

Este projeto foi desenvolvido como um sistema completo de gerenciamento de estoque, utilizando as melhores práticas de desenvolvimento:

- **Clean Architecture**: Separação de responsabilidades
- **RESTful API**: Endpoints bem estruturados  
- **Responsive Design**: Interface adaptável
- **Error Handling**: Tratamento robusto de erros
- **Code Quality**: Código limpo e documentado

---

## 📧 Suporte e Contato

Para dúvidas, problemas ou sugestões:

- 🐛 **Issues**: Use o GitHub Issues para reportar bugs
- 💡 **Feature Requests**: Sugira melhorias via Issues
- 📧 **Email**: Contato direto para questões específicas
- 📖 **Documentação**: Consulte este README e o Swagger UI

---

## 🙏 Agradecimentos

Desenvolvido com ❤️ utilizando:
- **.NET Team** - Pela excelente framework
- **Vue.js Community** - Pelo framework incrível  
- **PostgreSQL** - Pela confiabilidade do banco
- **Vite Team** - Pela ferramenta de build moderna
- **Open Source Community** - Por todas as bibliotecas utilizadas

---

<div align="center">

**⭐ StockFlow - Sistema de Estoque Moderno ⭐**

*Desenvolvido com as melhores tecnologias do mercado*

**[🚀 Demo](#) | [📚 Docs](#) | [🐛 Issues](#) | [💡 Features](#)**

</div>

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
| Campo     | Tipo    | Descrição                |
|-----------|---------|--------------------------|
| Id        | int     | Identificador único      |
| Username  | string  | Nome de usuário          |
| Password  | string  | Senha criptografada      |
| Role      | string  | Papel do usuário         |

### Tabela: Products
| Campo            | Tipo    | Descrição                    |
|------------------|---------|------------------------------|
| Id               | int     | Identificador único          |
| Name             | string  | Nome do produto              |
| SKU              | string  | Código único (índice único)  |
| Category         | string  | Categoria do produto         |
| Price            | decimal | Preço do produto             |
| QuantityInStock  | int     | Quantidade em estoque        |
| MinimumStock     | int     | Estoque mínimo alerta        |

### Tabela: Movements
| Campo         | Tipo     | Descrição                      |
|---------------|----------|--------------------------------|
| Id            | int      | Identificador único            |
| ProductId     | int      | ID do produto                  |
| Quantity      | int      | Quantidade movimentada         |
| Type          | string   | Tipo (Entrada/Saída)           |
| Reason        | string   | Motivo da movimentação         |
| MovementDate  | DateTime | Data e hora da movimentação    |
| UserId        | int      | ID do usuário responsável      |
| UserName      | string   | Nome do usuário responsável    |

---

## 🔧 Instalação e Configuração

### Pré-requisitos
- **.NET SDK 10.0** ou superior
- **Node.js 18+** e npm
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

## 🚀 Próximos Passos e Melhorias Futuras

- [ ] Implementar roles e permissões
- [ ] Adicionar exportação de relatórios (PDF/Excel)
- [ ] Notificações por email para estoque baixo
- [ ] Dashboard com mais gráficos e métricas
- [ ] Sistema de backup automático
- [ ] App mobile (React Native)
- [ ] Integração com código de barras
- [ ] Multi-empresa/Multi-depósito

---

## 👨‍💻 Desenvolvimento

### Comandos Úteis

**Backend:**
```bash
# Criar nova migration
dotnet ef migrations add NomeDaMigration --project StockFlow

# Aplicar migrations
dotnet ef database update --project StockFlow

# Reverter migration
dotnet ef database update NomeMigrationAnterior --project StockFlow

# Remover última migration
dotnet ef migrations remove --project StockFlow
```

**Frontend:**
```bash
# Desenvolvimento
npm run dev

# Build de produção
npm run build

# Preview do build
npm run preview
```

---

## 📝 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

## 👥 Contribuindo

Contribuições são bem-vindas! Sinta-se à vontade para abrir issues e pull requests.

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'Adiciona MinhaFeature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abra um Pull Request

---

## 📧 Contato

Para dúvidas, sugestões ou feedback, entre em contato através de:
- Email: seu-email@exemplo.com
- GitHub Issues: [StockFlow Issues](https://github.com/seu-usuario/stockflow/issues)

---

## 🙏 Agradecimentos

Desenvolvido com ❤️ usando as melhores tecnologias do mercado.

**Stack:**
- .NET Team pela excelente framework
- Vue.js Team pelo framework incrível
- PostgreSQL pela confiabilidade
- Toda a comunidade open source

---

<div align="center">
  <strong>⭐ Se este projeto foi útil, considere dar uma estrela no GitHub! ⭐</strong>
</div>
