
# TRUCK API

Projeto de CRUD básico para caminhões, contemplando back, front e testes unitários e autenticação com jwt.


## Requisitos do Ambiente

Para rodar esse projeto, você precisará dos seguintes itens:

.NET versão 8 https://dotnet.microsoft.com/pt-br/download/dotnet/8.0

SQLSERVER https://www.microsoft.com/en-us/sql-server/sql-server-downloads#trysql

Node v22.18.0 -> https://nodejs.org/pt/download

Angular npm install -g @angular/cli
## Instalação de dependencias

Para este projeto foi utilizado o EntityFramework version 9.0.8: 
```bash
  dotnet tool list --global: dotnet-ef 
```

Para instalar
```bash
  dotnet tool install --global dotnet-ef
```

Caso esteja utilizando o wsl será necessário adicionar no path do .zshrc ou .bashrc:
```bash
  export PATH="$PATH:$HOME/.dotnet/tools"
```

    
## Rodando localmente

Clone o projeto

```bash
  git clone git@github.com:jessicameira/truck-api.git
```
ou

```bash
  git clone https://github.com/jessicameira/truck-api.git
```

Entre no diretório do projeto

```bash
  cd truck-api
```
renomeie o arquivo .env.sample em src\backend\TruckControl.Api para .env
Altere para a sua conexão de banco

Instale as dependências

```bash
  npm install
  npm run install:all
```

Rode as migrations antes de iniciar:
```bash
  dotnet ef database update --project TruckControl.Infrastructure --startup-project TruckControl.Api
```

Inicie o servidor

```bash
  npm run dev
```
Para acessar o front a url é: http://localhost:4200

Para rodar apenas o back:
```bash
  npm run dev:backend
```
## Rodando os testes

Para executar os testes, é necessário entrar na pasta do backend:

```bash
  cd src\backend
  dotnet test
```
Para ver a cobertura dos testes será necessário que esteja instalado o reportgenerator:
```bash
  dotnet tool install -g dotnet-reportgenerator-globaltool
```

E então executar o comando roda os testes e o coverage
```bash
  dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
```

Esse comando cria um xml, assim podemos criar o html para acompanhar no navegador, a estrutura usada é:
```bash
  reportgenerator -reports:"TruckControl.Api.Tests\coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html
  reportgenerator -reports:"TruckControl.Application.Tests\coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html
```

## Documentação da API

a url padrão do swagger para este projeto é: http://localhost:5286/docs/index.html



