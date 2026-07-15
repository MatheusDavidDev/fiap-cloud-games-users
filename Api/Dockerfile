FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore FCG.Users.sln

RUN dotnet publish src/FCG.Users.Api/FCG.Users.Api.csproj \
    -c Release \
    -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "FCG.Users.Api.dll"]