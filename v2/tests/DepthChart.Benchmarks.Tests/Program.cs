// (c) D.Sawyer <damiensawyer@gmail.com> 2025

using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using DepthChart.Interfaces;
using DepthChart.Models;
using DepthChart.Services;

namespace DepthChart.Benchmarks.Tests;

public class Program
{
  public static void Main(string[] args)
  {
    var config = new ChartDepthConfig();
//    _ = BenchmarkRunner.Run<DamoBenchmarks>(config, args);

    BenchmarkRunner.Run<DepthChartServiceBenchmark>(config, args);

  }
}


public class ConcurrentDepthChartServiceNFL() : ConcurrentDepthChartService(NflPositions.ValidPositions)
{
}

public class SingleThreadedDepthChartNFL() : SingleThreadedDepthChart(NflPositions.ValidPositions)
{
}

public class SingleThreadedDepthChartServiceNFL() : SingleThreadedDepthChart(NflPositions.ValidPositions)
{
}
