// (c) D. Sawyer <damiensawyer@gmail.com> 2025

using DepthChart.Interfaces;
using DepthChart.Models;
using DepthChart.Services;
using TestHelpers.DynamicFixtures.TestWrappers;

namespace DepthChart.Tests.Tests.DepthChartTests.Tests.Helpers;

public static class DepthChartServices
{
  public static IEnumerable<object[]> GetDepthChartServices()
  {
    yield return [new TestWrapper<IDepthChartService> (new SingleThreadedDepthChart(NflPositions.ValidPositions))];
    yield return [new TestWrapper<IDepthChartService>(new ConcurrentDepthChartService(NflPositions.ValidPositions))];
  }
}
