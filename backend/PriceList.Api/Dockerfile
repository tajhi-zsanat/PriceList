# --- Build stage ---
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# copy csproj files first for proper restore caching
COPY backend/PriceList.Core/PriceList.Core.csproj backend/PriceList.Core/
COPY backend/PriceList.Infrastructure/PriceList.Infrastructure.csproj backend/PriceList.Infrastructure/
COPY backend/PriceList.Api/PriceList.Api.csproj backend/PriceList.Api/

RUN dotnet restore backend/PriceList.Api/PriceList.Api.csproj

# now copy the rest of source
COPY backend/ ./backend/

# build & publish
WORKDIR /src/backend/PriceList.Api
RUN dotnet publish PriceList.Api.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# --- Runtime stage ---
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# ASP.NET listens on 8080 inside the container
ENV ASPNETCORE_URLS=http://0.0.0.0:8080

# optional: reduce noisy globalization warnings
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# copy build output
COPY --from=build /app/publish ./

# expose HTTP
EXPOSE 8080

# Ensure uploads folder exists (we'll mount a volume here)
RUN mkdir -p /app/wwwroot/uploads

ENTRYPOINT ["dotnet", "PriceList.Api.dll"]
