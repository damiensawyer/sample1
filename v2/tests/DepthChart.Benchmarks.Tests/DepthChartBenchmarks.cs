// (c) D.Sawyer <damiensawyer@gmail.com> 2025

using BenchmarkDotNet.Attributes;
using DepthChart.Interfaces;
using DepthChart.Models;

namespace DepthChart.Benchmarks.Tests;

[Config(typeof(ChartDepthConfig))]
public class DepthChartServiceBenchmark
{
  private IDepthChartService service;

  [ParamsSource(nameof(ServiceTypes))] public Type ServiceType { get; set; }

  public IEnumerable<Type> ServiceTypes => [typeof(ConcurrentDepthChartServiceNFL), typeof(SingleThreadedDepthChartNFL)];


  [IterationSetup]
  public void IterationSetup()
  {
    this.service = (IDepthChartService)Activator.CreateInstance(this.ServiceType);
  }

  [Benchmark]
  public void AddPlayerBenchmark()
  {
    var player = new Player(1, "Player1", "WR");
    this.service.AddPlayerToDepthChart(player, "WR", 1);
  }

  // ... add more benchmarks as required
}
