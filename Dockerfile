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

RUN apt-get update && \
    apt-get install -y wget curl && \
    apt-get install -y --no-install-recommends openjdk-11-jre-headless

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

COPY ./configuration/jaas.conf ${CONF_FILES}/jaas.conf

CMD [ "docker-entrypoint.sh" ]