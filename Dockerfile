# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /src

# Copiar el archivo .csproj y restaurar dependencias
COPY ["MangaApi.csproj", "."]
RUN dotnet restore "./MangaApi.csproj"

# Copiar todo el código fuente
COPY . .

# Publicar la aplicación
RUN dotnet publish "MangaApi.csproj" -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MangaApi.dll"]
