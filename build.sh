#!/usr/bin/env bash

if [[ !$1 ]]; then
    CONFIGURATION="Debug"
fi

if [[ $1 ]]; then
    CONFIGURATION=$1
fi

dotnet restore
dotnet build ./ReadLine -c $CONFIGURATION
dotnet build ./ReadLine.Demo -c $CONFIGURATION
dotnet build ./ReadLine.Tests -c $CONFIGURATION
