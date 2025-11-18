#!/bin/bash

# Backup Barbara para OneDrive

BACKUP_DIR="$HOME/OneDrive/Backups/Barbara"
DATE=$(date +%Y%m%d_%H%M%S)

echo "🔄 Iniciando backup Barbara - $DATE"

mkdir -p "$BACKUP_DIR/$DATE"

# Backup SQL Server
echo "📦 Backup SQL Server..."
docker exec barbara-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P ${SQL_PASSWORD:-YourStrong!Passw0rd} -Q "BACKUP DATABASE [Barbara] TO DISK = N'/tmp/barbara.bak'"
docker cp barbara-sqlserver:/tmp/barbara.bak "$BACKUP_DIR/$DATE/barbara.bak"

# Backup MongoDB
echo "📦 Backup MongoDB..."
docker exec barbara-mongodb mongodump --archive=/tmp/mongodb_backup.gz --gzip
docker cp barbara-mongodb:/tmp/mongodb_backup.gz "$BACKUP_DIR/$DATE/mongodb.gz"

# Backup Redis
echo "📦 Backup Redis..."
docker exec barbara-redis redis-cli --pass ${REDIS_PASSWORD:-changeme} SAVE
docker cp barbara-redis:/data/dump.rdb "$BACKUP_DIR/$DATE/redis.rdb"

# Backup MinIO
echo "📦 Backup MinIO..."
docker exec barbara-minio tar czf /tmp/minio_backup.tar.gz /data
docker cp barbara-minio:/tmp/minio_backup.tar.gz "$BACKUP_DIR/$DATE/minio.tar.gz"

# Backup configs
echo "📦 Backup configs..."
cp -r monitoring "$BACKUP_DIR/$DATE/"
cp docker-compose.avila-full.yml "$BACKUP_DIR/$DATE/"
cp .env.production "$BACKUP_DIR/$DATE/.env.production.backup"

# Limpar backups antigos (30 dias)
find "$BACKUP_DIR" -type d -mtime +30 -exec rm -rf {} + 2>/dev/null

echo "✅ Backup concluído em: $BACKUP_DIR/$DATE"
echo "📊 Tamanho: $(du -sh $BACKUP_DIR/$DATE | cut -f1)"
