FROM mysql:8.0.35

EXPOSE 3306
LABEL version="0.1" description="groupOrdering Mysql伺服器"

# 設定mysql
COPY ./my.cnf /etc/my.cnf
# 將所有sql檔移到docker內部自動執行位置
COPY ./sql /docker-entrypoint-initdb.d