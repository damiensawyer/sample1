#!/bin/bash
cd v1
dotnet test SportsbetTestV1.sln
cd ..

cd v2
dotnet test SportsbetTestV2.sln
echo Running benchmarks in v2...
cd tests/DepthChart.Benchmarks.Tests
dotnet run --configuration RELEASE
cd ../../..

echo.
echo Running tests in v3...
cd v3
dotnet test SportsbetTestV3.sln
echo Running benchmarks in v3...
cd tests/DepthChart.Benchmarks.Tests
dotnet run --configuration RELEASE
cd ../../..

echo All tests completed
