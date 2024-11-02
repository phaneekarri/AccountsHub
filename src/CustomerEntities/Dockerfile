FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/CustomerEntities/CustomerEntities.csproj", "src/CustomerEntities/"]
RUN dotnet restore "src/CustomerEntities/CustomerEntities.csproj"
COPY . .


WORKDIR /src/src/CustomerEntities
RUN dotnet build "CustomerEntities.csproj" -c Release -o /app/build
RUN dotnet publish "CustomerEntities.csproj" -c Release -o /app/publish


# Copy the build output from the build stage
COPY --from=build /app .

# Copy the wait-for-db.sh script from the solutions directory
COPY ../wait-for-it.sh /app/wait-for-it.sh

# Ensure the script is executable
RUN chmod +x /app/wait-for-it.sh

WORKDIR /src/src/CustomerEntities
# Install EF Core tools globally
RUN dotnet tool install --global dotnet-ef

# Ensure global tools are on PATH
ENV PATH="${PATH}:/root/.dotnet/tools"

#RUN dotnet tool install --global dotnet-ef 
#RUN /root/.dotnet/tools/dotnet-ef database update --project "/src/src/CustomerEntities/CustomerEntities.csproj"

#ENTRYPOINT dotnet-ef database update --project CustomerEntities.csproj
# Entry point to run migrations
ENTRYPOINT ["/bin/bash", "-c", "/app/wait-for-it.sh && dotnet ef database update --project CustomerEntities"]