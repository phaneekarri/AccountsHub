FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5028
ENV ASPNETCORE_URLS=http://+:5028

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release

WORKDIR /src
COPY ["src/UserAuthApi/UserAuthApi.csproj", "src/UserAuthApi/"]
COPY ["src/UserAuthEntities/UserAuthEntities.csproj", "src/UserAuthEntities/"]
RUN dotnet restore "src/UserAuthApi/UserAuthApi.csproj"
COPY . .

WORKDIR "/src/src/UserAuthApi"
RUN dotnet build "UserAuthApi.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "UserAuthApi.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserAuthApi.dll"]
