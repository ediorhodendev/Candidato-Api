Projeto CRUD de Candidatos
Este é um projeto de exemplo que demonstra como criar uma aplicação de CRUD de candidatos usando Angular 13 e .NET 6, com funcionalidades de criação, atualização, exclusão e listagem de candidatos. Além disso, a aplicação inclui validações de CPF e email e foi construída com o uso da biblioteca Po-UI para a interface do usuário.

Estrutura da Solution
A Solution consiste nos seguintes projetos:

CandidatoApp: O projeto Angular que contém a interface do usuário da aplicação.
CrudCandidatosApi: O projeto .NET que contém a API da aplicação.
CandidatoService: Camada de serviço que lida com a lógica de negócios.
CandidatoRepository: Camada de acesso a dados que lida com o banco de dados e implementações concretas.
Candidato.Tests: Projetos de testes unitários para testar a aplicação.
Funcionalidades
Cadastro de um novo candidato.
Atualização de um candidato existente.
Exclusão de um candidato.
Listagem de todos os candidatos.
Validação de CPF e email.
Mensagens de erro detalhadas para validações de campos.
Requisitos
Angular 13 ou superior.
.NET 6 ou superior.
Entity Framework Core.
Docker (para o banco de dados SQL Server).
Swagger/OpenAPI para documentação da API.
Testes unitários para garantir a qualidade do código.
Testes de integração.
Configuração
Certifique-se de configurar a string de conexão com o banco de dados SQL Server no arquivo appsettings.json do projeto CrudCandidatosApi.
