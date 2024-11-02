FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release

WORKDIR /src
COPY ["src/CustomerApi/CustomerApi.csproj", "src/CustomerApi/"]
#COPY ["src/CustomerEntities/CustomerEntities.csproj", "src/CustomerEntities/"]

RUN dotnet restore "src/CustomerApi/CustomerApi.csproj"
COPY . .

#RUN dotnet tool install --global dotnet-ef 
#RUN /root/.dotnet/tools/dotnet-ef database update --project "/src/src/CustomerEntities/CustomerEntities.csproj"

WORKDIR "/src/src/CustomerApi"
RUN dotnet build "CustomerApi.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "CustomerApi.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CustomerApi.dll"]
