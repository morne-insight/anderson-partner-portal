# -------------------------------
# BUILD STAGE
# -------------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy everything (API + domain + infra + client)
COPY . .

# Restore & publish ONLY the API project
RUN dotnet restore Anderson/AndersonAPI/AndersonAPI/AndersonAPI.Api/AndersonAPI.Api.csproj

RUN dotnet publish Anderson/AndersonAPI/AndersonAPI/AndersonAPI.Api/AndersonAPI.Api.csproj \
    -c Release \
    -o /app/publish

# -------------------------------
# RUNTIME STAGE
# -------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Install Chromium dependencies for Puppeteer
RUN apt-get update && apt-get install -y --no-install-recommends \
    libatk1.0-0 \
    libatk-bridge2.0-0 \
    libcups2 \
    libdrm2 \
    libxkbcommon0 \
    libxcomposite1 \
    libxdamage1 \
    libxfixes3 \
    libxrandr2 \
    libgbm1 \
    libnss3 \
    libasound2 \
    libx11-xcb1 \
    libxshmfence1 \
    libgtk-3-0 \
    fonts-liberation \
    ca-certificates \
    && rm -rf /var/lib/apt/lists/*

# Copy published output from build stage
COPY --from=build /app/publish .

# Azure App Service container port
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "AndersonAPI.Api.dll"]
