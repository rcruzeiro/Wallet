FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 8080

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Wallet.sln ./
COPY Wallet.Adapter/Wallet.Adapter.csproj Wallet.Adapter/
COPY Wallet.API/Wallet.API.csproj Wallet.API/
COPY Wallet.DI/Wallet.DI.csproj Wallet.DI/
COPY Wallet.DTO/Wallet.DTO.csproj Wallet.DTO/
COPY Wallet.Services.Entity/Wallet.Services.Entity.csproj Wallet.Services.Entity/
COPY Wallet.Entities/Wallet.Entities.csproj Wallet.Entities/
COPY Wallet.Objects/Wallet.Objects.csproj Wallet.Objects/
COPY Wallet.Repository/Wallet.Repository.csproj Wallet.Repository/
COPY Wallet.Services.Task/Wallet.Services.Task.csproj Wallet.Services.Task/
COPY Wallet.Repository.MySQL/Wallet.Repository.MySQL.csproj Wallet.Repository.MySQL/
COPY Wallet.API.Messages/Wallet.API.Messages.csproj Wallet.API.Messages/
COPY docker-compose.dcproj ./
RUN dotnet restore Wallet.sln -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/Wallet.API
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Wallet.API.dll"]
