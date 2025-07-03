# 📚 StudyPlanner - Sistema de Planejamento de Estudos

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

O **StudyPlanner** é uma aplicação de linha de comando (CLI) para ajudar estudantes a organizar e monitorar suas sessões de estudo, matérias e metas acadêmicas.

---

## ✨ Funcionalidades

- **Cadastro de matérias** com nome e cor  
- **Registro de sessões de estudo** com data/hora de início e fim  
- **Definição de metas de estudo** por matéria com prazo  
- **Estatísticas de estudo**:
  - Horas estudadas por matéria (últimos 7 dias, 30 dias ou período completo)
  - Sequência de dias consecutivos com estudo (streak)
- **Acompanhamento de metas** com status:
  - ✅ Em dia (On Track)
  - ⚠️ Aviso (Warning)
  - 🟢 Concluído (Done)
  - ❌ Expirado (Expired)

---

## ⚙️ Tecnologias Utilizadas

- [.NET 9.0](https://dotnet.microsoft.com/)
- Entity Framework Core (SQLite)
- xUnit (testes unitários)
- Moq (mocking em testes)

---

## 🚀 Como Executar

### Pré-requisitos

- [.NET SDK 9.0](https://dotnet.microsoft.com/download/dotnet/9.0)

---

### Passos

# Clone o repositório

```
git clone https://github.com/seu-usuario/StudyPlanner.git
cd StudyPlanner
```

# Execute o aplicativo
```
dotnet run
```

---

# 🗂️ Estrutura de Arquivos

```
StudyPlanner/
├── Models/             # Modelos de dados
│   ├── Goal.cs
│   ├── StudySession.cs
│   └── Subject.cs
├── Services/           # Serviços de aplicação
│   ├── GoalService.cs
│   ├── SessionService.cs
│   ├── StatisticsService.cs
│   └── SubjectService.cs
├── Tests/              # Testes unitários
│   ├── Helpers/
│   │   └── MockHelpers.cs
│   └── ServicesTests/
│       ├── GoalServiceTests.cs
│       ├── SessionServiceTests.cs
│       ├── StatisticsServiceTests.cs
│       └── SubjectServiceTests.cs
├── PlannerContext.cs   # Contexto do banco de dados
├── Program.cs          # Ponto de entrada
└── StudyPlanner.csproj # Configuração do projeto
```

---

# 📊 Banco de Dados
SQLite (arquivo studyplanner.db)
Migrações automáticas na primeira execução
