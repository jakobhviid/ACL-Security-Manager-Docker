#!/bin/bash

# Load helper functions for configuring kafka server.properties
. helpers.sh

set_principal_in_jaas_file "$CONF_FILES"/jaas.conf "$ACL_KERBEROS_ZOOKEEPER_PRINCIPAL"

if [[ -f "$ACLS_PATH" ]]; then
    echo "INFO - acls.csv file has been provided"
else
    echo "INFO - acls.csv file has not been provided, creating a new one"
    touch "$ACLS_PATH"
fi
