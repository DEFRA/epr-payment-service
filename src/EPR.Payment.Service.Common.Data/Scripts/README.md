## Local test of migration Docker container ##

### Prepare Azure SQL Edge Database

1. Start container using azure-sql-edge:latest image. 
   ```shell
   docker run -e 'ACCEPT_EULA=Y' -e 'MSSQL_SA_PASSWORD=Password1!' -p 1433:1433 \
      --name azuresqledge -d mcr.microsoft.com/azure-sql-edge:latest
   ```
2. Find IP address of *azuresqledge* container.
   ```shell
   docker inspect azuresqledge | jq '.[].NetworkSettings.Networks.bridge.IPAddress'
   ```
   or
   ```shell
   docker inspect azuresqledge | grep IPAddress
   ```
3. Start mssql-tools interactive terminal. Connect to the database using sqlcmd using the IPAddress (here it is 172.17.0.2).
   ```shell
   docker run --rm -it mcr.microsoft.com/mssql-tools:latest /opt/mssql-tools/bin/sqlcmd -S 172.17.0.2 -U sa -P Password1!
   ```
4. Verify it works.
   ```sql
   select @@version;
   go
   ```
   The output should be similar to:
   > Microsoft Azure SQL Edge Developer (RTM) - 15.0.2000.1574 (ARM64) \
   Jan 25 2023 10:36:08 \
   Copyright (C) 2019 Microsoft Corporation \
   Linux (Ubuntu 18.04.6 LTS aarch64) <ARM64> 
5. Create `[Accounts]` database.
   ```sql
   create database [Accounts];
   go
   ```
### Run database migration ###

1. Build docker image.
   ```shell
   docker build -t database-migrations .
   ```
2. Start container.
   ```shell
   docker run --rm \
      -e SERVER=172.17.0.2 \
      -e PORT=1433 \
      -e USER=sa \
      -e PASSWORD=Password1! \
      -e DATABASE=Accounts \
      database-migrations
   ```
