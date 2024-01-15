#!/bin/sh

apt-get update && apt upgrade -y
apt-get install -y mariadb-server
