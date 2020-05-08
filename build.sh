#! /bin/bash

cd nbuild
dotnet build -c Release
cd ..
nbuild/bin/Release/netcoreapp3.0/nBuild --delete nbuild.config
nbuild/bin/Release/netcoreapp3.0/nBuild --load nbuild.config

dotnet build -c Release