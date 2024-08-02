FROM mcr.microsoft.com/mssql-tools

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
