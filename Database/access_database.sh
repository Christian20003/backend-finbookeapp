#!/bin/bash

#######################################################################
#     This script starts a shell inside the MongoDB container         #
#                       DO NOT MODIFY                                 #
#######################################################################

container_id=$(docker ps -aqf "name=mongodb-1")
if [ "$container_id" == "" ]; then
    echo "ERROR: Missing running container"
    exit 1
fi

username=$(grep "DATABASE_USERNAME" ./.env | awk -F'"' '{print $2}')
password=$(grep "DATABASE_PASSWORD" ./.env | awk -F'"' '{print $2}')

if [ "$username" == "" ]; then
    echo "ERROR: Missing username in .env file"
    echo "Create the DATABASE_USERNAME environment variable and set a value"
    exit 1
fi

if [ "$password" == "" ]; then
    echo "ERROR: Missing password in .env file"
    echo "Create the DATABASE_PASSWORD environment variable and set a value"
    exit 1
fi

docker exec -it $container_id mongosh -u $username -p $password