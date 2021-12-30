# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln ./source/
COPY ./ ./source/

RUN dotnet restore ./source/Endpoints/Iwentys.Endpoints.Api/Iwentys.Endpoints.Api.csproj

RUN dotnet publish ./source/Endpoints/Iwentys.Endpoints.Api/Iwentys.Endpoints.Api.csproj -c release -o /app --no-restore
# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Iwentys.Endpoints.Api.dll"]