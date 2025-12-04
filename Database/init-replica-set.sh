#!/bin/bash

#######################################################################
#        This script initialises a MongoDB replica set                #
#                       DO NOT MODIFY                                 #
#######################################################################

echo "Waiting for MongoDB to start instance..."

until mongosh --quiet --host $HOST --port $PORT --eval "db.runCommand({ ping: 1 }).ok" | grep 1 &>/dev/null; do
  sleep 1
done

echo "MongoDB has started successfully"

echo "Initiating MongoDB replica set..."

mongosh -u $MONGO_INITDB_ROOT_USERNAME -p $MONGO_INITDB_ROOT_PASSWORD --host $HOST --port $PORT --eval "
  rs.initiate({
    _id: '${REPLICA_SET_NAME}',
    members: [
      {
        _id: 0,
        host: '${HOST}:${PORT}'
      }
    ]
  })
"