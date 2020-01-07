# Working image built from guide:
# https://medium.com/front-end-weekly/net-core-web-api-with-docker-compose-postgresql-and-ef-core-21f47351224f
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
LABEL maintainer="e@dwelsh.uk"

WORKDIR /app
COPY ./src/GoodNews/GoodNews.csproj ./
RUN dotnet restore GoodNews.csproj

COPY ./src/GoodNews ./
RUN dotnet publish GoodNews.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "GoodNews.dll"]