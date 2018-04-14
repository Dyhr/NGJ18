#!/bin/bash

killall node
sudo yum -y update
sudo yum -y install nodejs npm --enablerepo=epel
node -v

node /webapps/ngj18/main.js &
disown
