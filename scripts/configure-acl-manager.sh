#!/bin/bash

. helpers.sh

zookeeper_keytab_location="$CONF_FILES/zkclient.keytab"
# If they haven't provided their own keytabs in volumes, it is tested if they have provided the necessary environment variables to download the keytab from an API
if [[ -z "${ACL_ZOOKEEPER_KERBEROS_PRINCIPAL}" ]]; then
    if [[ -z "${ACL_KERBEROS_API_URL}" ]]; then
        echo -e "\e[1;32mERROR - One of either 'ACL_ZOOKEEPER_KERBEROS_PRINCIPAL' or 'ACL_KERBEROS_API_URL' must be supplied!! \e[0m"
        exit 1
    else # the user wants to use a kerberos api to get keytabs

        # Test for all the required environment variables
        if [[ -z "${ACL_KERBEROS_API_ZOOKEEPER_USERNAME}" ]]; then
            echo -e "\e[1;32mERROR - Missing 'ACL_KERBEROS_API_ZOOKEEPER_USERNAME' environment variable. This is required to use kerberos API for zookeeper keytab \e[0m"
            exit 1
        fi
        if [[ -z "${ACL_KERBEROS_API_ZOOKEEPER_PASSWORD}" ]]; then
            echo -e "\e[1;32mERROR - Missing 'ACL_KERBEROS_API_ZOOKEEPER_PASSWORD' environment variable. This is required to use kerberos API for zookeeper keytab \e[0m"
            exit 1
        fi

        export ACL_ZOOKEEPER_KERBEROS_PRINCIPAL="$ACL_KERBEROS_API_ZOOKEEPER_USERNAME"@"$ACL_KERBEROS_REALM"
        # response will be 'FAIL' if it can't connect or if the url returned an error
        response=$(curl --fail -X GET -H "Content-Type: application/json" -d "{\"username\":\""$ACL_KERBEROS_API_ZOOKEEPER_USERNAME"\", \"password\":\""$ACL_KERBEROS_API_ZOOKEEPER_PASSWORD"\"}" "$ACL_KERBEROS_API_URL" -o "$zookeeper_keytab_location" && echo "INFO - Using the keytab from the API and a principal name of '"$ACL_KERBEROS_API_ZOOKEEPER_USERNAME"'@'"$ACL_KERBEROS_REALM"'" || echo "FAIL")
        if [ "$response" == "FAIL" ]; then
            echo -e "\e[1;32mERROR - Kerberos API did not succeed when fetching zookeeper keytab. See curl error above for further details \e[0m"
            exit 1
        fi
    fi
else # The user has supplied their own principals

    # test if a keytab has been provided and if it's in the expected directory
    if ! [[ -f "${zookeeper_keytab_location}" ]]; then
        echo -e "\e[1;32mERROR - Missing zookeeper kerberos keytab file '"$zookeeper_keytab_location"'. This is required to enable kerberos when 'ACL_ZOOKEEPER_KERBEROS_PRINCIPAL' is supplied. Provide it with a docker volume or docker mount \e[0m"
        exit 1
    else
        echo "INFO - Using the supplied keytab and the principal name from environment variable 'ACL_ZOOKEEPER_KERBEROS_PRINCIPAL' "
    fi
fi
# Setting the principal which will either be from the environment variable or the export if the kerberos API is to be used
set_principal_in_jaas_file "$CONF_FILES"/jaas.conf "$ACL_ZOOKEEPER_KERBEROS_PRINCIPAL"



if [[ -f "$ACLS_PATH" ]]; then
    echo "INFO - acls.csv file has been provided"
else
    echo "INFO - acls.csv file has not been provided, creating a new one"
    touch "$ACLS_PATH"
fi
