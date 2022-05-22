FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /source

COPY Kino/Kino.csproj .
RUN dotnet restore

COPY Kino/ .
RUN dotnet publish --no-restore -c Release -o bin/publish


FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

COPY --from=build-env /source/bin/publish .
ENTRYPOINT ["dotnet", "Kino.dll"]
