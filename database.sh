#!/bin/bash

#######################################################################
# This script allows the usage for a MongoDB database for the         #
# project in this repository (only for development)                   #
#######################################################################

proof_success() {
    if [ $? -ne 0 ]; then
        echo $1
        exit 1
    fi
}

option1="start"
option2="access"
option3="stop"

# Access necessary secrets for the database
secrets=$(dotnet user-secrets list --json | sed '1d;$d')
user=$(echo $secrets | python3 -c "import sys, json; print(json.loads(sys.stdin.read())['AuthDatabase:Username'])")
proof_success "ERROR: Fix the provided error"
password=$(echo $secrets | python3 -c "import sys, json; print(json.loads(sys.stdin.read())['AuthDatabase:Password'])")
proof_success "ERROR: Fix the provided error"

# Proof if docker is installed on current machine
docker &> /dev/null
proof_success "ERROR: docker is missing. Please install docker before trying again."

# Proof if arguments are available
if [ "$1" == "" ]; then
    echo "ERROR: Missing argument."
    echo "Options:"
    echo -e "\e[1m$option1\e[0m -> Will start a docker container with the mongodb image"
    echo -e "\e[1m$option2\e[0m -> Get access to the current runnning container"
    echo -e "\e[1m$option3\e[0m -> Stop the current running container and remove it"
    exit 1
fi

# Starting container
if [ "$1" == "start" ]; then
    output=$(docker run --name mongodb -p 27017:27017 -d -e MONGO_INITDB_ROOT_USERNAME=$user -e MONGO_INITDB_ROOT_PASSWORD=$password mongodb/mongodb-community-server:latest 2>&1)
    code=$?
    if [ $code -eq 125 ]; then
        echo "ERROR: Container is already running on on localhost:27017"
        exit 1
    fi
    if [ $code -ne 0 ]; then
        echo "ERROR:"
        echo $output
        exit 1
    fi
    echo "Container is running and listening on localhost:27017"
    exit 0
fi

# Stopping container
if [ "$1" == "stop" ]; then
    docker stop mongodb
    docker rm mongodb
    if [ $? -eq 0 ]; then
        echo "mongodb container is stopped and removed"
        exit 0
    fi
    exit 1
fi

# Accessing the container
if [ "$1" == "access" ]; then
    container_id=$(docker ps -aqf "name=mongodb")
    if [ "$container_id" == "" ]; then
        echo "ERROR: Missing running container"
        exit 1
    fi
    echo "To get access to the database use the following command:"
    echo "mongosh --username [username] --password [password]"
    docker exec -it $container_id /bin/bash
    exit 0
fi

echo "ERROR: Invalid argument"


