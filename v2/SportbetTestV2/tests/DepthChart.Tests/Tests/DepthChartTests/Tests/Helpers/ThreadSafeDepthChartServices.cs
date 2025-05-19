// (c) D. Sawyer <damiensawyer@gmail.com> 2025

using DepthChart.Models;
using DepthChart.Services;

namespace DepthChart.Tests.Tests.DepthChartTests.Tests.Helpers;

public static class ThreadSafeDepthChartServices
{
  public static IEnumerable<object[]> GetThreadSafeDepthChartServices()
  {
    yield return [new ConcurrentDepthChartService(NflPositions.ValidPositions)];
    // Including either of these should cause the test to fail.
    // yield return [new SingleThreadedDepthChart(NflPositions.ValidPositions)];
    // yield return [new WillAlwaysFailDepthChart()];
  }
}
