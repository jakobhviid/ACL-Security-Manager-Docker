#!/bin/bash

# Exit if any command has a non-zero exit status (Exists if a command returns an exception, like it's a programming language)
# Prevents errors in a pipeline from being hidden. So if any command fails, that return code will be used as the return code of the whole pipeline
set -eo pipefail

if ! [ -z "$ACL_INIT_USERS" ]; then
    PORT=${ACL_API_PORT:-9000}
    echo "INFO - 'ACL_INIT_USERS' has been provided. Creating users. Waiting for API to be ready"
    # Wait until the api has started and listens on the port. The max is 15 seconds
    counter=0
    while [ -z "$(netstat -tln | grep "$PORT")" ]; do
        # 15 seconds have passed
        if [ "$counter" -eq 15 ]; then
            echo -e "\e[1;32mERROR - Creating initial user did not succeed. Server did not start \e[0m"
            exit 1
        else
            echo "Waiting for API to start ..."
            sleep 1
            ((counter++))
        fi
    done
    echo "API has started"

    IFS=',' # Comma seperated string of principals (users)
    read -r -a users <<<"$ACL_INIT_USERS"
    for user in "${users[@]}"; do
        response=$(curl --fail --connect-timeout 5 --retry 5 --retry-delay 5 --retry-max-time 30 --retry-connrefused --max-time 5 -X POST -H "Content-Type: application/json" -d "{\"apiKey\":\""$ACL_API_KEY"\", \"principalName\":\""$user"\"}" "http://127.0.0.1:"$PORT"/new-super-user" || echo "FAIL")
        if [ "$response" == "FAIL" ]; then
            echo -e "\e[1;32mERROR - Creating initial user did not succeed. See curl error above. \e[0m"
            exit 1
        fi
    done
fi
