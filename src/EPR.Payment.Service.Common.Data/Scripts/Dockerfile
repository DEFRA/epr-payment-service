FROM ubuntu:22.04

ENV DEBIAN_FRONTEND=noninteractive

RUN apt-get update \
    && apt-get install -y --no-install-recommends ca-certificates curl gnupg \
    && install -d /etc/apt/keyrings \
    && curl -fsSL https://packages.microsoft.com/keys/microsoft.asc \
        | gpg --dearmor -o /etc/apt/keyrings/microsoft.gpg \
    && echo "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/microsoft.gpg] https://packages.microsoft.com/ubuntu/22.04/prod jammy main" \
        > /etc/apt/sources.list.d/microsoft-prod.list \
    && apt-get update \
    && ACCEPT_EULA=Y apt-get install -y --no-install-recommends mssql-tools18 \
    && apt-get purge -y --auto-remove curl gnupg \
    && rm -rf /var/lib/apt/lists/*

# Create a non-root user and define a directory it can use
RUN useradd -m -d /home/nonroot -s /bin/bash nonroot

# Change to the non-root user
USER nonroot

# Create the directory fornon-root user and set it as the working directory
RUN mkdir /home/nonroot/sql
WORKDIR /home/nonroot/sql

# Copy the migration files and script and set their owners to non-root
# so migration script can be executable
COPY --chown=nonroot migrations.sql .
COPY --chown=nonroot run-migrations.sh .

RUN chmod +x run-migrations.sh
CMD ["/bin/bash", "/home/nonroot/sql/run-migrations.sh", "/home/nonroot/sql/migrations.sql"]
