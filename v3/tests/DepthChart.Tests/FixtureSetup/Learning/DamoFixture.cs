// (c) D. Sawyer <damiensawyer@gmail.com> 2025

using Xunit.Abstractions;

namespace DepthChart.Tests.FixtureSetup.Learning;

public class DamoFixture : IAsyncDisposable
{
  public DamoFixture() => Console.WriteLine($"\"-------------CONSTRUCTING {nameof(DamoFixture)}");

  public async ValueTask DisposeAsync()
  {
    // If there's someway that we can get access to the constructor we could pass in ITestOutputHelper
    await Task.CompletedTask;
    Console.WriteLine($"-------------DISPOSING {nameof(DamoFixture)}");
    Console.WriteLine($"CLEANING up {nameof(DamoFixture)} async in AUTO DISPOSE ASYNC");
  }

  public async Task DamoSetupAsync(ITestOutputHelper testOutputHelper)
  {
    await Task.CompletedTask;
    testOutputHelper.WriteLine($"Setting up {nameof(DamoFixture)} async");
  }

  public async Task DamoTearDownAsync(ITestOutputHelper testOutputHelper)
  {
    await Task.CompletedTask;
    testOutputHelper.WriteLine($"CLEANING up {nameof(DamoFixture)} async in MANUAL DISPOSE ASYNC");
  }
}