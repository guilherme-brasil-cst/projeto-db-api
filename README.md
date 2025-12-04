Integrantes: 
```
Guilherme Brasil - 22309720
Gabriel Rodrigues - 22310104

```
# Projeto: API + DB + GUI em C#

## Objetivo

API para gerenciar Categories e Products utilizando ASP.NET Core Web
API, Entity Framework Core e SQLite.\
### Recomendação do uso do Postman para testes.

## Tecnologias

-   .NET 9\
-   ASP.NET Core Web API\
-   SQLite\
-   Entity Framework Core\
-   Visual Studio Code\
-   Postman

------------------------------------------------------------------------

# 1. Instalação dos Pré-requisitos

## 1.1 Instalar .NET 9 SDK

Verificar instalação:

    dotnet --version

## 1.2 Instalar VS Code e extensões

-   C#\
-   C# Dev Kit\
-   SQLite Explorer (opcional)

## 1.3 Instalar EF CLI

    dotnet tool install --global dotnet-ef

Verificar:

    dotnet tool list -g

## 1.4 Instalar Postman

------------------------------------------------------------------------

# 2. Abrir o Projeto no VS Code

1.  Extrair o ZIP.\
2.  Abrir a pasta do projeto no VS Code.\
3.  Abrir terminal integrado pelo menu **Terminal \> New Terminal**.

------------------------------------------------------------------------

# 3. Configuração do Banco SQLite

Arquivo `appsettings.json`:

``` json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=app.db"
  }
}
```

O banco `app.db` será criado na pasta principal do projeto.

------------------------------------------------------------------------

# 4. Recriando o Banco do Zero

### Criar migration inicial:

    dotnet ef migrations add InitialCreate

### Criar o banco:

    dotnet ef database update

### Caso já exista:

-   Deletar `app.db`
-   Deletar pasta `Migrations`
-   Rodar novamente:

```{=html}
<!-- -->
```
    dotnet ef migrations add InitialCreate
    dotnet ef database update

------------------------------------------------------------------------

# 5. Rodar o Projeto

    dotnet run

A saída mostrará a porta em uso (ex: `http://localhost:5270`).\
A API deve ser testada **somente via Postman**.

------------------------------------------------------------------------

# 6. Ajustar Porta da Aplicação

Editar `Properties/launchSettings.json`:

``` json
"applicationUrl": "http://localhost:5270"
```

------------------------------------------------------------------------

# 7. Manual Completo para uso no Postman

## 7.1 Criar Collection

Criar uma nova Collection e adicionar as requisições.

------------------------------------------------------------------------

# 7.2 Endpoints Disponíveis

## /api/categories

### GET

    GET /api/categories

### POST

    POST /api/categories
    Body:
    {
      "name": "Armas"
    }

### PUT

    PUT /api/categories/1
    Body:
    {
      "name": "Armas Pesadas"
    }

### DELETE

    DELETE /api/categories/1

------------------------------------------------------------------------

## /api/products

### GET

    GET /api/products

### POST

    POST /api/products
    Body:
    {
      "name": "Espada Longa",
      "price": 120,
      "categoryId": 1
    }

### PUT

    PUT /api/products/1
    Body:
    {
      "name": "Espada Curta",
      "price": 90,
      "categoryId": 1
    }

### DELETE

    DELETE /api/products/1

------------------------------------------------------------------------

# 8. Estrutura das Pastas

-   **Models/** → Classes Category e Product\
-   **Controllers/** → Controllers da API\
-   **Data/** → DataContext (EF Core)\
-   **Migrations/** → Arquivos de migração\
-   **Program.cs** → Configuração principal\
-   **appsettings.json** → Connection string\
-   **launchSettings.json** → Configuração de execução

------------------------------------------------------------------------

# 9. Relacionamento das Tabelas

-   Uma Category possui muitos Products (1:N).

### Modelo simples:

Categoria (1) ------ (N) Produtos

### Exemplo de retorno JSON:

``` json
{
  "id": 1,
  "name": "Armas",
  "products": [
    {
      "id": 1,
      "name": "Espada Longa",
      "price": 120,
      "categoryId": 1
    }
  ]
}
```

------------------------------------------------------------------------

# 10. Possíveis Erros e Soluções

  -----------------------------------------------------------------------
  Erro               Causa                    Solução
  ------------------ ------------------------ ---------------------------
  404 Not Found      Rota incorreta           Verificar a URL no Postman

  500 Internal Error Categoria não encontrada Verificar IDs enviados

  Cannot open        Banco não criado         Rodar migrations
  database                                    
  -----------------------------------------------------------------------

------------------------------------------------------------------------

# 11. Execução Limpa do Zero

    rm app.db
    rm -r Migrations
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    dotnet run

------------------------------------------------------------------------

# 12. Resumo dos passos

1.  Instalar todos os requisitos\
2.  Abrir o projeto no VS Code\
3.  Criar migrations\
4.  Criar banco\
5.  Executar com `dotnet run`\
6.  Testar tudo via Postman\
7.  Não usar Swagger

------------------------------------------------------------------------
