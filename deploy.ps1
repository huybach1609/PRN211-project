#!/usr/bin/env pwsh

$USERNAME = "weback1609"
$HOSTNAME = "weback1609server"
$SERVER_PATH = "/mnt/sda1/personal/project"
$CONTAINER_NAME_CLIENT = "project-todoapp-client"
$CONTAINER_NAME_SERVER = "project-todoapp-server"

Write-Host "🏗️  Building Docker images..." -ForegroundColor Blue

# Build client image
Write-Host "📦 Building client image..." -ForegroundColor Yellow
docker build -t "$USERNAME/todoapp-client:latest" -f ./todoapp.client/Dockerfile ./todoapp.client 2>&1 | ForEach-Object { Write-Host $_ }

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Client build failed!" -ForegroundColor Red
    exit 1
}

# Build server image
Write-Host "📦 Building server image..." -ForegroundColor Yellow
docker build -t "$USERNAME/todoapp-server:latest" -f ./todoapp.server/Dockerfile ./todoapp.server 2>&1 | ForEach-Object { Write-Host $_ }

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Server build failed!" -ForegroundColor Red
    exit 1
}

Write-Host "✅ Builds successful!" -ForegroundColor Green

Write-Host "📤 Pushing to Docker Hub..." -ForegroundColor Blue

# Push client image
Write-Host "📤 Pushing client image..." -ForegroundColor Yellow
docker push "$USERNAME/todoapp-client:latest" 2>&1 | ForEach-Object { Write-Host $_ }

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Client push failed!" -ForegroundColor Red
    exit 1
}

# Push server image
Write-Host "📤 Pushing server image..." -ForegroundColor Yellow
docker push "$USERNAME/todoapp-server:latest" 2>&1 | ForEach-Object { Write-Host $_ }

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Server push failed!" -ForegroundColor Red
    exit 1
}

Write-Host "✅ Push successful!" -ForegroundColor Green

Write-Host "🚀 Deploying to server..." -ForegroundColor Blue

$deployCmd = "cd $SERVER_PATH && " +
             "echo '📥 Pulling latest images...' && " +
             "docker pull $USERNAME/todoapp-client:latest && " +
             "docker pull $USERNAME/todoapp-server:latest && " +
             "echo '🌐 Checking network...' && " +
             "(docker network inspect docker_app-network >/dev/null 2>&1 || docker network create docker_app-network) && " +
             "echo '🛑 Stopping containers...' && " +
             "(docker compose stop $CONTAINER_NAME_CLIENT || echo 'Client container not running') && " +
             "(docker compose stop $CONTAINER_NAME_SERVER || echo 'Server container not running') && " +
             "echo '🗑️  Removing containers...' && " +
             "(docker compose rm -f $CONTAINER_NAME_CLIENT || echo 'No client container to remove') && " +
             "(docker compose rm -f $CONTAINER_NAME_SERVER || echo 'No server container to remove') && " +
             "echo '🚀 Starting new containers...' && " +
             "docker compose up -d && " +
             "echo '⏳ Waiting for startup and health checks...' && " +
             "sleep 15 && " +
             "echo '📊 Container status:' && " +
             "docker ps --filter name=todoapp --format 'table {{.Names}}\t{{.Status}}\t{{.Ports}}\t{{.Networks}}' && " +
             "echo '🏥 Health check status:' && " +
             "docker inspect --format='{{.Name}}: {{.State.Health.Status}}' $CONTAINER_NAME_SERVER 2>/dev/null || echo 'Health check not available' && " +
             "echo '📋 Recent client logs:' && " +
             "docker logs --tail 10 --timestamps $CONTAINER_NAME_CLIENT && " +
             "echo '📋 Recent server logs:' && " +
             "docker logs --tail 10 --timestamps $CONTAINER_NAME_SERVER"

ssh $HOSTNAME $deployCmd

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Deploy complete!" -ForegroundColor Green
    Write-Host "🌐 Application should be available at: https://todoapp.weback1609.io.vn" -ForegroundColor Cyan
    Write-Host "🏥 Health check endpoint: https://todoapp.weback1609.io.vn/api/health" -ForegroundColor Cyan
    Write-Host "📊 Monitoring: Check container health status and logs above" -ForegroundColor Yellow
} else {
    Write-Host "❌ Deploy failed!" -ForegroundColor Red
    Write-Host "🔍 Checking error details..." -ForegroundColor Yellow
    ssh $HOSTNAME "echo 'Client logs:' && docker logs --tail 15 $CONTAINER_NAME_CLIENT 2>&1 || echo 'No client container logs available'; echo 'Server logs:' && docker logs --tail 15 $CONTAINER_NAME_SERVER 2>&1 || echo 'No server container logs available'; echo 'Network info:' && docker network ls | grep app-network"
    exit 1
}
