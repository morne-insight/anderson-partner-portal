# -------------------------------
# BUILD STAGE
# -------------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0.11 AS build
WORKDIR /src

# Copy everything (API + domain + infra + client)
COPY . .

# Restore & publish ONLY the API project
RUN dotnet restore AndersonAPI/AndersonAPI/AndersonAPI.Api/AndersonAPI.Api.csproj

RUN dotnet publish AndersonAPI/AndersonAPI/AndersonAPI.Api/AndersonAPI.Api.csproj \
    -c Release \
    -o /app/publish \
    -p:GenerateOpenApiClient=false

# -------------------------------
# RUNTIME STAGE
# -------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0.11 AS runtime
WORKDIR /app

# Install Chromium dependencies for Puppeteer + SSH server
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
    openssh-server \
    && rm -rf /var/lib/apt/lists/*

# --- SSH configuration (Azure App Service custom container convention) ---
# Create sshd runtime dir
RUN mkdir -p /var/run/sshd \
    # Set root password expected for App Service SSH flows
    && echo "root:Docker!" | chpasswd

# Copy sshd config + init script
COPY sshd_config /etc/ssh/sshd_config
COPY init_container.sh /app/init_container.sh
RUN chmod +x /app/init_container.sh \
    && chmod 600 /etc/ssh/sshd_config

# Copy published output from build stage
COPY --from=build /app/publish .

# Azure App Service container port
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
EXPOSE 2222

ENTRYPOINT ["/app/init_container.sh"]
