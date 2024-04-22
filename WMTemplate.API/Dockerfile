FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WMTemplate.API/WMTemplate.API.csproj", "WMTemplate.API/"]
COPY ["WMTemplate.Application/WMTemplate.Application.csproj", "WMTemplate.Application/"]
COPY ["WMTemplate.Domain/WMTemplate.Domain.csproj", "WMTemplate.Domain/"]
COPY ["WMTemplate.Infrastructure/WMTemplate.Infrastructure.csproj", "WMTemplate.Infrastructure/"]
RUN dotnet restore "WMTemplate.API/WMTemplate.API.csproj"
COPY . .
WORKDIR "/src/WMTemplate.API"
RUN dotnet build "WMTemplate.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "WMTemplate.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /src/Novle.API/https/aspnetapp.pfx /https/
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WMTemplate.API.dll"]
