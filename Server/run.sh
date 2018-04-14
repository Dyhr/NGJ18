#!/bin/bash

killall node
sudo yum -y update
sudo yum -y install nodejs npm --enablerepo=epel
node -v

#npm install bind
#npm install rwlock
#npm install querystring

nohup node /webapps/ngj18/main.js &>/tmp/serverlog.log &

until pids=$(pidof node)
do
    sleep 1
done
