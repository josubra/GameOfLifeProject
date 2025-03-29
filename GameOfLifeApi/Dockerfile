FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GameOfLifeApi/GameOfLifeApi.csproj", "GameOfLifeApi/"]
RUN dotnet restore "./GameOfLifeApi/GameOfLifeApi.csproj"
COPY . .
WORKDIR "/src/GameOfLifeApi"
RUN dotnet build "./GameOfLifeApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GameOfLifeApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameOfLifeApi.dll"]