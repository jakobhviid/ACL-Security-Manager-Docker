#!/bin/bash

if [ -z "$AUTHORIZER_ZOOKEEPER_CONNECT" ]; then
    echo -e "\e[1;32mERROR - Missing 'AUTHORIZER_ZOOKEEPER_CONNECT' \e[0m"
    exit 1
fi

if [[ -z "${ACL_KERBEROS_PUBLIC_URL}" ]]; then
    echo -e "\e[1;32mERROR - Missing 'ACL_KERBEROS_PUBLIC_URL' environment variable. \e[0m"
    exit 1
fi

if [[ -z "${ACL_KERBEROS_REALM}" ]]; then
    echo -e "\e[1;32mERROR - Missing 'ACL_KERBEROS_REALM' environment variable. This is required to use kerberos API for zookeeper keytab \e[0m"
    exit 1
fi
