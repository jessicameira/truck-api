Requisitos para do projeto

.NET versão 8 -> https://dotnet.microsoft.com/pt-br/download/dotnet/8.0
SQLSERVER -> https://www.microsoft.com/en-us/sql-server/sql-server-downloads#trysql
Node v22.18.0 -> https://nodejs.org/pt/download
Angular npm install -g @angular/cli

Para este projeto foi utilizado o EntityFramework: 
dotnet tool list --global: dotnet-ef 9.0.8

Para instalar
dotnet tool install --global dotnet-ef

Caso esteja utilizando o wsl será necessário adicionar no path do .zshrc ou .bashrc
export PATH="$PATH:$HOME/.dotnet/tools"

Clonar o repositório 

renomear o arquivo .env.sample em src\backend\TruckControl.Api para .env

Na raíz do projeto executar o comando:
npm install
npm run install:all

EXECUTANDO OS TESTES:
Para executar os testes, é necessário entrar na pasta do backend:
´cd src\backend´
executar:

Para ver a cobertura dos testes será necessário que esteja instalado o reportgenerator:
dotnet tool install -g dotnet-reportgenerator-globaltool

E então executar o comando roda os testes e o coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura

Esse comando cria um xml, assim podemos criar o html para acompanhar no navegador, a estrutura usada é:
reportgenerator
-reports:"Path\To\TestProject\TestResults\{guid}\coverage.cobertura.xml"
-targetdir:"coveragereport"
-reporttypes:Html

Para esse projeto será:
reportgenerator -reports:"TruckControl.Api.Tests\coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html
reportgenerator -reports:"TruckControl.Application.Tests\coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html

EXECUTANDO O PROJETO LOCAL:
Para executar a aplicação na raíz do projeto primeiro execute as migrations e então suba o projeto:
dotnet ef database update --project TruckControl.Infrastructure --startup-project TruckControl.Api
npm run dev