# Base image cho runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Image để build app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ShoeStoreMvc/ShoeStoreMvc.csproj", "ShoeStoreMvc/"]
RUN dotnet restore "./ShoeStoreMvc/ShoeStoreMvc.csproj"
COPY . .
WORKDIR "/src/ShoeStoreMvc"
RUN dotnet build "./ShoeStoreMvc.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish app
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ShoeStoreMvc.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShoeStoreMvc.dll"]
