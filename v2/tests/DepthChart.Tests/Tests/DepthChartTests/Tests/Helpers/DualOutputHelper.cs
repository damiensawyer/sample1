// (c) D. Sawyer <damiensawyer@gmail.com> 2025

using Xunit.Abstractions;

namespace DepthChart.Tests.Tests.DepthChartTests.Tests.Helpers;

/// <summary>
///     Helper class so that XUnit will write to both the IDE (Rider) test output window and to the CLI when run from the
///     console and in CICD
/// </summary>
public class DualOutputHelper(ITestOutputHelper testOutput) : ITestOutputHelper
{
  public void WriteLine(string message)
  {
    Console.WriteLine(message);
    testOutput.WriteLine(message);
  }

  public void WriteLine(string format, params object[] args) => testOutput.WriteLine(format, args);
}
