// (c) D.Sawyer <damiensawyer@gmail.com> 2025

using DepthChart.Interfaces;
using DepthChart.Models;
using DepthChart.Services;
using TestHelpers.DynamicFixtures.TestWrappers;

namespace DepthChart.Tests.Tests.DepthChartTests.Tests.Helpers;

public static class ThreadSafeDepthChartServices
{
  public static IEnumerable<object[]> GetThreadSafeDepthChartServices()
  {
    yield return [new AsyncWrapper<IDepthChartService>(new ConcurrentDepthChartService(NflPositions.ValidPositions))];
    yield return [new FakeSlowTestWrapper<IDepthChartService>(new ConcurrentDepthChartService(NflPositions.ValidPositions))]; // just showing a test wrapper which needs to do setup work
    //yield return [new WillAlwaysFailDepthChart()];
  }
}

