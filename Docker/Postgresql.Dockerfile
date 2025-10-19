# Postgresql.Dockerfile - basic Postgres server
# Usage: build with `docker build -t my-postgres -f Postgresql.Dockerfile .`
# Run with: `docker run -p 5432:5432 -v pgdata:/var/lib/postgresql/data --name my-postgres my-postgres`

FROM postgres:15-alpine

# Replace these defaults with secure values for production
ENV POSTGRES_USER=product_user
ENV POSTGRES_PASSWORD=change_me
ENV POSTGRES_DB=productdb

# Persist database files
VOLUME ["/var/lib/postgresql/data"]

# Expose the default Postgres port
EXPOSE 5432

# Optional: place SQL/.sh init scripts in ./initdb and uncomment the next line
# COPY ./initdb /docker-entrypoint-initdb.d/

# Use the official entrypoint; no additional CMD needed