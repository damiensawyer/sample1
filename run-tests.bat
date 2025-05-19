@echo off
echo Running tests in v1...
cd v1
dotnet test
cd ..

echo.
echo Running tests in v2...
cd v2
dotnet test
echo Running benchmarks in v2...
cd tests\DepthChart.Benchmarks.tests
dotnet run --configuration RELEASE
cd ..\..\..

echo.
echo Running tests in v3...
cd v3
dotnet test
echo Running benchmarks in v3...
cd tests\DepthChart.Benchmarks.tests
dotnet run --configuration RELEASE
cd ..\..\..

echo.
echo All tests completed