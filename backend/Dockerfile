# Use the official .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:9.0
WORKDIR /app

# Copy everything and restore as distinct layers
COPY . ./
RUN dotnet restore

# Publish the application
RUN dotnet publish -c Release -o out

# Install Docker CLI and dependencies for DIND
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
        ca-certificates \
        curl \
        gnupg \
        lsb-release && \
    curl -fsSL https://download.docker.com/linux/debian/gpg | gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg && \
    echo "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/debian $(lsb_release -cs) stable" > /etc/apt/sources.list.d/docker.list && \
    apt-get update && \
    apt-get install -y --no-install-recommends docker-ce-cli && \
    rm -rf /var/lib/apt/lists/*

# Set Docker host to use the Docker socket (for DIND)
ENV DOCKER_HOST=unix:///var/run/docker.sock

# Expose port
EXPOSE 5062

# Run the application
CMD ["dotnet", "out/Backend.dll"]
