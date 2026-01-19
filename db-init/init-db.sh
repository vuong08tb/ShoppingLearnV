#!/bin/bash
set -e

SA_PASSWORD="${SA_PASSWORD:-yourStrong(!)Password}"

echo "Waiting for SQL Server to be available..."
retry=0
until /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -Q "SELECT 1" >/dev/null 2>&1; do
    retry=$((retry+1))
    if [ $retry -gt 60 ]; then
        echo "SQL Server did not become available in time." >&2
        exit 1
    fi
    sleep 1
done

echo "Checking if database 'Shopping' exists..."
if ! /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -Q "IF DB_ID('Shopping') IS NOT NULL SELECT 1 AS DbExists" | grep -q "DbExists"; then
  echo "Database 'Shopping' does not exist. Creating database..."
  /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -Q "CREATE DATABASE [Shopping]"
  echo "Importing schema and data from /tmp/shopping.sql..."
    /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -i /tmp/shopping.sql
    echo "Import finished."
else
    echo "Database 'Shopping' already exists. Skipping creation."
fi