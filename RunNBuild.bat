@echo off
color 0a
title Step: Compile nBuild
:compileNBuild
cd nBuild
dotnet build -c Release
cd ..
title Step: Generate Project and Solution
:nbuild
nbuild\bin\Release\netcoreapp3.0\nBuild.exe nbuild.config
:compile
title Step: Compile zSim
dotnet build -c Release
echo.
echo.
echo.
echo nBuild/Compile task has finished. Please check the log for any errors before closing the console window
pause