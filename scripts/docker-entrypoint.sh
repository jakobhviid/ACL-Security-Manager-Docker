#!/bin/bash

# Exit if any command has a non-zero exit status (Exists if a command returns an exception, like it's a programming language)
# Prevents errors in a pipeline from being hidden. So if any command fails, that return code will be used as the return code of the whole pipeline
set -eo pipefail

check-environment.sh

configure-acl-manager.sh

"$ACL_MANAGER_HOME"/bin/kafka-security-manager -Djava.security.auth.login.config="$CONF_FILES"/jaas.conf

# TODO: Start API dotnet

