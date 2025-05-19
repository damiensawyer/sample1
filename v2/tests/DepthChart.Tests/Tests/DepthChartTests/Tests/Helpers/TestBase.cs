// (c) D. Sawyer <damiensawyer@gmail.com> 2025

using Xunit.Abstractions;

namespace DepthChart.Tests.Tests.DepthChartTests.Tests.Helpers;

public class TestBase(ITestOutputHelper output)
{
  protected readonly DualOutputHelper Output = new(output);
}
