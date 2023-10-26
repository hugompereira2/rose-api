<h1 align="center"> ROSE-API :rose: </h1>

<div align="center">
  <sub> Made with 💖 by
    <a href="https://github.com/hugompereira2">Hugo Mendonça Pereira</a>
  </sub>
</div>

## Summary (Portugues)

Bem-vindo à Rose-API, uma API de informações climáticas. Esta API permite que você obtenha dados meteorológicos de cidades e aeroportos do Brasil. Através desta API, você pode acessar informações sobre temperatura, umidade, pressão atmosférica, direção do vento e muito mais.

## Summary (English)

Welcome to Rose-API, a weather information API. This API allows you to retrieve weather data for cities and airports in Brazil. With this API, you can access information about temperature, humidity, atmospheric pressure, wind direction, and more.

## 🚀 Tecnologias Utilizadas ##

- ASP.NET Core: Framework de desenvolvimento web
- Docker: Plataforma de contêiner
- Dapper: Micro ORM para acesso a bancos de dados
- Microsoft SQL Server: Banco de dados relacional
- Docker Compose: Orquestrador de contêiner para facilitar a execução

## Executando a API com Docker Compose

Certifique-se de ter o Docker e o Docker Compose instalados na sua máquina.

1. Clone este repositório:
```sh
git clone https://github.com/hugompereira2/rose-api.git
```
2. Navegue até o diretório do projeto:
```sh
cd rose-api
```
3. Altere o arquivo "example.env" para:
```sh
.env
```
4. Execute o Docker Compose para criar e iniciar os contêineres da API e do banco de dados:
```sh
docker-compose up -d
```
5. Aguarde até que o serviço esteja em execução.
6. Execute o projeto:
```sh
dotnet run --project rose-api.csproj
```
7. Acesse a documentação da API
```sh
http://localhost:5162/swagger/index.html
```
