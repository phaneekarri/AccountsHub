FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/UserAuthEntities/UserAuthEntities.csproj", "src/UserAuthEntities/"]
RUN dotnet restore "src/UserAuthEntities/UserAuthEntities.csproj"
COPY . .
WORKDIR "/src/src/YourEntitiesProject"
RUN dotnet build "UserAuthEntities.csproj" -c Release -o /app/build
RUN dotnet publish "UserAuthEntities.csproj" -c Release -o /app/publish

FROM build AS final
WORKDIR /app/publish
ENTRYPOINT ["dotnet", "ef", "database", "update", "--project", "UserAuthEntities.csproj"]
