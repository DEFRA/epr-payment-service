#!/bin/bash

set -euo pipefail

if [[ -s $1 ]]; then
  /opt/mssql-tools18/bin/sqlcmd \
    -S "$SERVER,$PORT" \
    -U "$USER" \
    -P "$PASSWORD" \
    -d "$DATABASE" \
    -i "$1" \
    -I \
    -b \
    -r 1 \
    -C
else
  echo The file "$1" is empty. No update has been triggered.
fi
