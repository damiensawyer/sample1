#!/bin/bash
dotnet tool install -g dotnet-format
# dotnet format
dotnet format style --verbosity diagnostic --severity info

