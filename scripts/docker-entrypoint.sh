#!/bin/bash

# Exit if any command has a non-zero exit status (Exists if a command returns an exception, like it's a programming language)
# Prevents errors in a pipeline from being hidden. So if any command fails, that return code will be used as the return code of the whole pipeline
set -eo pipefail

check-environment.sh

configure-acl-manager.sh

/usr/bin/supervisord

# export SOURCE_CLASS="com.github.simplesteph.ksm.source.FileSourceAcl"
# export SOURCE_FILE_FILENAME="$ACLS_PATH"
# export KSM_READONLY="false"

# "$ACL_MANAGER_HOME"/bin/kafka-security-manager -Djava.security.auth.login.config="$CONF_FILES"/jaas.conf -Djava.security.krb5.conf="$CONF_FILES"/krb5.conf

# PORT=${ACL_API_PORT:-9000}
# # run the server
# dotnet "$API_HOME"/API.dll --urls=http://0.0.0.0:$PORT
