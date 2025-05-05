# 🛠️ Sistema de Manutenção Industrial

Sistema de gerenciamento de chamados de manutenção industrial, desenvolvido com **ASP.NET Core 9.0** e **Razor Pages**, ideal para ambientes industriais que necessitam organizar e acompanhar a manutenção de máquinas.

---

## 📋 Funcionalidades

- 🔐 Autenticação e autorização de usuários (Admin / Operador)  
- 🏭 Gestão de máquinas (CRUD)  
- 🛎️ Abertura e acompanhamento de chamados de manutenção  
- 🔄 Atualização de status dos chamados  
- 📤 Exportação de relatórios em Excel  
- 💻 Interface responsiva com **Bootstrap 5**

---

## 🚀 Configuração do Ambiente

### ✅ Pré-requisitos

- [.NET SDK 9.0](https://dotnet.microsoft.com/en-us/download) ou superior
- [Visual Studio Code](https://code.visualstudio.com/)
- [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) **ou** SQLite

### 🧩 Extensões Recomendadas para o VS Code

- C# for Visual Studio Code
- NuGet Package Manager
- SQL Server (mssql)

## ⚙️ Instalação

1. **Clone o repositório**:
   ```bash
   git clone https://github.com/thismacedo/ManutencaoIndustrial.git
   cd ManutencaoIndustrial
   ```

---
## Restaure as dependências:
```bash
dotnet restore
```
## Configure a string de conexão no appsettings.json:
```bash
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ManutencaoIndustrial;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```
## Execute as migrações do banco de dados:
```bash
dotnet ef database update
```
## Inicie o sistema:
```bash
dotnet run
```
---
### 🔑 Primeiro Acesso
Um usuário administrador é criado automaticamente na primeira execução:

- Email: admin@admin.com
- Senha: admin123

### 📦 Pacotes NuGet Utilizados
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- ClosedXML (para exportação em Excel)

---
### 👥 Perfis de Usuário
## 👷 Operador
- Abertura de chamados de manutenção
- Visualização e acompanhamento de status dos chamados

## 👨‍💼 Administrador
- Todas as funcionalidades do Operador
- Gestão de máquinas
- Atualização de status
- Exportação de relatórios
- Cadastro e gestão de usuários

---
### 📄 Licença
Distribuído sob a licença MIT. Veja o arquivo LICENSE para mais detalhes.
