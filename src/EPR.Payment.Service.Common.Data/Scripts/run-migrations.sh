#!/bin/bash

if [[ -s $1 ]]; then
  /opt/mssql-tools/bin/sqlcmd -S $SERVER,$PORT -U $USER -P $PASSWORD -d $DATABASE -i "$1" -I
else
  echo The file "$1" is empty. No update has been triggered.
fi
