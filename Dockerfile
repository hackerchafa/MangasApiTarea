# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /src

# Copiar el archivo .csproj y restaurar dependencias
COPY ["MangaApi/MangaApi.csproj", "MangasRepo/"]
RUN dotnet restore "MangasRepo/MangaApi.csproj"

# Copiar todo el código
COPY . .

# Publicar la aplicación
WORKDIR "/src/MangasRepo"
RUN dotnet publish "MangaApi.csproj" -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MangasRepo.dll"]
