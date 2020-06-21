function set_principal_in_jaas_file() {
    if [ "$#" -ne 2 ]; then
        echo -e "\e[1;32mset_principal_in_jaas_file not used correctly! Provide two parameters! First is jaas file path. Second is the new principal value\e[0m"
    else
        # Escaping argument 2 for special characters
        sed -i -r -E "/principal=/ s/=.*/=\"$(echo $2 | sed -e 's#\([]*^+.$[/]\)#\\\1#g')\";/" $1
    fi
}