#!/bin/bash

set -e # exit if any command has a non-zero exit status

run_migrator_cmd="dotnet ./migrator/JoBoard.AuthService.Migrator.dll --migrate"
echo "Run Migrator: $run_migrator_cmd" >&1
exec $run_migrator_cmd