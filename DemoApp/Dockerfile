#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DemoApp/DemoApp.csproj", "DemoApp/"]
COPY ["Services/Catalogue/Catalogue.csproj", "Services/Catalogue/"]
COPY ["Services/Inventory/Inventory.csproj", "Services/Inventory/"]
RUN dotnet restore "./DemoApp/./DemoApp.csproj"
COPY . .
WORKDIR "/src/DemoApp"
RUN dotnet build "./DemoApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DemoApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DemoApp.dll"]