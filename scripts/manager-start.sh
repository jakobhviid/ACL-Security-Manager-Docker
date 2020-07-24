#!/bin/bash

"$ACL_MANAGER_HOME"/bin/kafka-security-manager -Djava.security.auth.login.config="$CONF_FILES"/jaas.conf -Djava.security.krb5.conf="$CONF_FILES"/krb5.conf
