FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Audit/Audit.csproj", "Audit/"]
RUN dotnet restore "Audit/Audit.csproj"
COPY . .
WORKDIR "/src/Audit"
RUN dotnet build "Audit.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Audit.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Audit.dll"]