#!/bin/bash

if [ -z "$AUTHORIZER_ZOOKEEPER_CONNECT" ]; then
    echo -e "\e[1;32mERROR - Missing 'AUTHORIZER_ZOOKEEPER_CONNECT' \e[0m"
    exit 1
fi

if [ -z "$ACL_KERBEROS_ZOOKEEPER_PRINCIPAL" ]; then
    echo -e "\e[1;32mERROR - Missing 'ACL_KERBEROS_ZOOKEEPER_PRINCIPAL' \e[0m"
    exit 1
fi

KEYTAB_PATH="$CONF_FILES/zkclient.keytab"
if [[ -f "$KEYTAB_PATH" ]]; then
    echo "INFO - Keytab file has been provided"
else
    echo -e "\e[1;32mERROR - Missing ZOOKEEPER KEYTAB FILE '"$CONF_FILES/zkclient.keytab"' \e[0m"
    exit 1
fi
