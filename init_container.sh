#!/bin/sh
set -e

echo "Starting sshd..."
# Some base images don't have 'service', so call sshd directly
/usr/sbin/sshd || true

echo "Starting app..."
exec dotnet AndersonAPI.Api.dll
