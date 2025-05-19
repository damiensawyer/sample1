// (c) D.Sawyer <damiensawyer@gmail.com> 2025

using DepthChart.Models;
using DepthChart.Services;

namespace DepthChart.Tests.Tests.DepthChartTests.Tests.ServiceSets;

public static class DepthChartServices
{
  public static IEnumerable<object[]> GetDepthChartServices()
  {
    yield return [new SingleThreadedDepthChart(NflPositions.ValidPositions)];
    yield return [new ConcurrentDepthChartService(NflPositions.ValidPositions)];
    //yield return [new WillAlwaysFailDepthChart()];
  }
}

