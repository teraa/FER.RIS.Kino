# Kino

## `docker-compose.yml`
```yml
services:
  db:
    image: postgres
    container_name: kino-db
    environment:
      - POSTGRES_USER=postgres
      - POSTGRES_PASSWORD=example
  
  server:
    image: "ghcr.io/teraa/kino:master"
    container_name: kino-server
    depends_on:
      - "db"
    ports:
      - "5000:80"
    environment:
      - Db__ConnectionString=Host=db;Port=5432;Database=kino;Username=postgres;Password=example;Include Error Detail=true
      - ASPNETCORE_ENVIRONMENT=Development
```

## Update
```sh
docker-compose stop
docker-compose pull server
docker-compose up -d
```

## URLs
- http://localhost:5000/swagger
- http://localhost:5000/redoc
