#!/bin/bash

#######################################################################
#   This script generates the keyfile for a MongoDB replica set       #
#                       DO NOT MODIFY                                 #
#######################################################################

# Echo MONGO_REPLICA_SET_KEY to the keyfile.

echo $MONGO_REPLICA_SET_KEY > /data/configdb/keyfile

# Change the permissions of the keyfile.

chmod 400 /data/configdb/keyfile

# Change the ownership of the keyfile.

chown mongodb:mongodb /data/configdb/keyfile