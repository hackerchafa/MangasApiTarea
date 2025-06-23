FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /src
COPY ["MangaApi/MangaApi.csproj", "MangaApi/"]
RUN dotnet restore "MangaApi/MangaApi.csproj"
COPY . .
WORKDIR "/src/MangaApi"
RUN dotnet build "MangaApi.csproj" -c Release -o /app/build
RUN dotnet publish "MangaApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MangaApi.dll"]
