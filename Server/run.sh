#!/bin/bash

killall node
sudo yum -y update
sudo yum -y install nodejs npm --enablerepo=epel
node -v

npm install bind
npm install rwlock
npm install querystring

nohup node /webapps/ngj18/main.js &>/dev/null &
