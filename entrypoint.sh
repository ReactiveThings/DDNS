#!/bin/bash

env >> /etc/environment

echo "$@"
exec "$@"
