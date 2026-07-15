
# FIAP Cloud Games - Users API

## Sobre

Microsserviço responsável pelo gerenciamento de usuários da plataforma.

---

# Responsabilidades

- Cadastro de usuários;
- Autenticação;
- Geração de token JWT;
- Controle de autorização.

---

# Tecnologias

- .NET 9
- ASP.NET Core
- Entity Framework Core
- SQL Server
- JWT
- Docker

---

# Arquitetura

O projeto utiliza Clean Architecture dividido em:

- API
- Application
- Domain
- Infrastructure
- Tests

---

# Banco de Dados

Utiliza SQL Server para persistência dos usuários.

---

# Comunicação

Publica eventos:

### UserCreatedEvent

Evento enviado após o cadastro de um usuário.

Consumidores:
- Notifications API

---

# Executando

```bash
docker compose up -d
