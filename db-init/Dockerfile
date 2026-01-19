FROM mcr.microsoft.com/mssql/server:2019-latest
USER root

# Install prerequisites and mssql-tools so sqlcmd is available for initialization
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl apt-transport-https ca-certificates gnupg \
    && curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - \
    && curl https://packages.microsoft.com/config/ubuntu/20.04/prod.list > /etc/apt/sources.list.d/mssql-release.list \
    && apt-get update \
    && ACCEPT_EULA=Y apt-get install -y --no-install-recommends msodbcsql17 mssql-tools unixodbc-dev \
	&& rm -rf /var/lib/apt/lists/*

COPY db-init/init-db.sh /usr/src/init-db.sh
COPY ShoppingLearn/dump/shopping.sql /tmp/shopping.sql
RUN chmod +x /usr/src/init-db.sh

CMD /bin/bash /usr/src/init-db.sh & /opt/mssql/bin/sqlservr