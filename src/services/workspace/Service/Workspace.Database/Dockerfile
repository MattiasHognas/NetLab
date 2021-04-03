FROM mcr.microsoft.com/mssql/server:latest

USER root
RUN mkdir -p /usr/src/app
WORKDIR /usr/src/app

COPY . /usr/src/app

RUN chmod +x /usr/src/app/run-initialization.sh

ENV SA_PASSWORD zu4VPR!E8tR
ENV ACCEPT_EULA Y
ENV MSSQL_PID Express

EXPOSE 5201:1433

CMD /bin/bash ./entrypoint.sh
