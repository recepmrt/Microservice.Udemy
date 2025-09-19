FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FirstMicroservice.Categories.WebAPI/FirstMicroservice.Categories.WebAPI.csproj", "FirstMicroservice.Categories.WebAPI/"]
RUN dotnet restore "FirstMicroservice.Categories.WebAPI/FirstMicroservice.Categories.WebAPI.csproj"
COPY . .
WORKDIR "/src/FirstMicroservice.Categories.WebAPI"
RUN dotnet build "./FirstMicroservice.Categories.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FirstMicroservice.Categories.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FirstMicroservice.Categories.WebAPI.dll"]
