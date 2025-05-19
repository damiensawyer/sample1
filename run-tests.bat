@echo off
echo Running tests in v1...
cd v1
dotnet test
cd ..

echo.
echo Running tests in v2...
cd v2
dotnet test
cd ..

echo.
echo Running tests in v3...
cd v3
dotnet test
cd ..

echo.
echo All tests completed