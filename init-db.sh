#!/bin/bash
set -e

export PATH="$PATH:/opt/mssql-tools/bin"

echo "Starting database initialization..."

# Install mssql-tools if not present
if ! command -v sqlcmd &> /dev/null; then
    echo "Installing mssql-tools..."
    apt-get update
    apt-get install -y gnupg curl
    curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add -
    curl https://packages.microsoft.com/config/ubuntu/20.04/prod.list | tee /etc/apt/sources.list.d/msprod.list
    apt-get update
    ACCEPT_EULA=Y apt-get install -y mssql-tools unixodbc-dev
    echo "mssql-tools installed."
else
    echo "sqlcmd already available."
fi

echo "Waiting for SQL Server to be ready..."
COUNTER=0
MAX_ATTEMPTS=30
while [ $COUNTER -lt $MAX_ATTEMPTS ]; do
  if sqlcmd -S localhost -U SA -P "${MSSQL_SA_PASSWORD}" -Q "SELECT 1" 2>/dev/null; then
    echo "SQL Server is ready!"
    break
  fi
  COUNTER=$((COUNTER + 1))
  echo "Attempt $COUNTER/$MAX_ATTEMPTS: Waiting for SQL Server..."
  sleep 1
done

if [ $COUNTER -eq $MAX_ATTEMPTS ]; then
  echo "ERROR: SQL Server did not start within $MAX_ATTEMPTS seconds"
  exit 1
fi

echo "Creating/Attaching database and setting up user..."

# Execute T-SQL to create/attach database and setup user
sqlcmd -S localhost -U SA -P "${MSSQL_SA_PASSWORD}" << EOF
-- Create or attach database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'userdb')
BEGIN
    DECLARE @fileExists INT;
    EXEC master.dbo.xp_fileexist '/var/opt/mssql/data/userdb.mdf', @fileExists OUTPUT;
    
    IF @fileExists = 1
    BEGIN
        CREATE DATABASE userdb ON (FILENAME = '/var/opt/mssql/data/userdb.mdf'), (FILENAME = '/var/opt/mssql/data/userdb_log.ldf') FOR ATTACH;
        PRINT 'Database attached from existing files.';
    END
    ELSE
    BEGIN
        CREATE DATABASE userdb;
        PRINT 'New database created.';
    END
END
ELSE
BEGIN
    PRINT 'Database userdb already exists.';
END
GO

-- Create login if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.server_principals WHERE name = 'apiuser')
BEGIN
    CREATE LOGIN apiuser WITH PASSWORD = '${API_USER_PASSWORD}';
    PRINT 'Login created for apiuser.';
END
ELSE
BEGIN
    PRINT 'Login already exists for apiuser.';
END
GO

IF NOT EXISTS (SELECT name FROM sys.server_principals WHERE name = 'migrationuser')
BEGIN
    CREATE LOGIN migrationuser WITH PASSWORD = '${AUTH_DB_MIGRATION_PASSWORD}';
    PRINT 'Login created for migrationuser.';
END
ELSE
BEGIN
    PRINT 'Login already exists for migrationuser.';
END
GO

-- Use the database and create user
USE userdb;
GO

IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'apiuser')
BEGIN
    CREATE USER apiuser FOR LOGIN apiuser;
    PRINT 'User created for apiuser.';
END
ELSE
BEGIN
    PRINT 'User already exists for apiuser.';
END
GO

IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'migrationuser')
BEGIN
    CREATE USER migrationuser FOR LOGIN migrationuser;
    PRINT 'User created for migrationuser.';
END
ELSE
BEGIN
    PRINT 'User already exists for migrationuser.';
END
GO

-- Grant permissions
ALTER ROLE db_datareader ADD MEMBER apiuser;
ALTER ROLE db_datawriter ADD MEMBER apiuser;
ALTER ROLE db_owner ADD MEMBER migrationuser;
PRINT 'Permissions granted to apiuser and migrationuser.';
GO

SELECT 'Database initialization complete!' AS Status;
EOF

echo "Database initialization finished successfully."