#!/bin/bash

dotnet restore
dotnet build --no-restore
dotnet test --no-build --verbosity normal
