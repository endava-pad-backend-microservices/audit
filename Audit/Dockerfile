FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["*.csproj", "./"]
RUN dotnet restore 
COPY . ./
RUN dotnet publish "Audit.csproj" -o /app
FROM base AS final
WORKDIR /app
COPY --from=build /app .

ENV COUCHDB_URL=pad-b-audit-database \
 EUREKA_URL=pad-b-registry \
 SERVER_URL=pad-b-audit \
 COUCHDB_USER=admin \
 COUCHDB_PASSWORD=123456 \
 COUCHDB_PORT=5986 \
 DOCKERIZE_VERSION=v0.6.0 \
 ASPNETCORE_URLS=http://+:8087

 CMD wget https://github.com/jwilder/dockerize/releases/download/$DOCKERIZE_VERSION/dockerize-alpine-linux-amd64-$DOCKERIZE_VERSION.tar.gz \
&& tar -C /usr/local/bin -xzvf dockerize-alpine-linux-amd64-$DOCKERIZE_VERSION.tar.gz \ 
&& rm dockerize-alpine-linux-amd64-$DOCKERIZE_VERSION.tar.gz \
&& dockerize -wait tcp://$EUREKA_URL:8761 -timeout 60s yarn start \
&& dockerize -wait tcp://$COUCHDB_URL:5986 -timeout 60s yarn start
COPY ./default.ini /opt/couchdb/etc/
COPY ./default.ini /opt/couchdb/etc/local.d/
ENTRYPOINT ["dotnet", "Audit.dll"]
EXPOSE 8087