#!/bin/bash

killall node
sudo yum -y update
sudo yum -y install nodejs npm --enablerepo=epel

sudo npm cache clean -f
sudo npm install -g n
sudo n stable
node -v

node /webapps/ngj18/main.js
