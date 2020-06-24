FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

COPY ./API/API.csproj /build/API.csproj

RUN dotnet restore ./build/API.csproj

COPY ./API/ /build/

WORKDIR /build/

RUN dotnet publish ./API.csproj -c ${BUILDCONFIG} -o out /p:Version=${VERSION}

FROM ubuntu:20.04

LABEL MAINTAINER="Oliver Marco van Komen"
ENV DEBIAN_FRONTEND=noninteractive 

RUN apt-get update && \
    apt-get install -y wget curl && \
    apt-get install -y --no-install-recommends openjdk-11-jre-headless && \
    apt-get install -y krb5-user && \
    apt-get install -y supervisor

# Installing aspnetcore runtime. This is in a seperate block as it is subject to change pretty often
RUN wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    apt-get update && \
    apt-get install -y apt-transport-https && \
    apt-get update && \
    apt-get install -y dotnet-sdk-3.1

ENV ACL_MANAGER_HOME=/opt/manager
ENV CONF_FILES=/conf
ENV ACLS_PATH=${CONF_FILES}/acls.csv
ENV API_HOME=/opt/api
ENV ASPNETCORE_ENVIRONMENT=Production

COPY ./acl-manager-code ${ACL_MANAGER_HOME}

# Copy scripts
COPY ./scripts /tmp/
RUN chmod +x /tmp/*.sh && \
    mv /tmp/* /usr/bin && \
    rm -rf /tmp/*

COPY --from=build /build/out ${API_HOME}

RUN mkdir ${CONF_FILES}

COPY ./configuration/ ${CONF_FILES}/

CMD [ "docker-entrypoint.sh" ]