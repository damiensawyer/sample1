// (c) D.Sawyer <damiensawyer@gmail.com> 2025

using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;

namespace DepthChart.Benchmarks.Tests;

public class ChartDepthConfig : ManualConfig
{
  public ChartDepthConfig()
  {
    this.ArtifactsPath = "benchmark_results";
    AddLogger(ConsoleLogger.Default);
    AddExporter(HtmlExporter.Default);
    AddColumnProvider(DefaultColumnProviders.Instance);

    AddDiagnoser(MemoryDiagnoser.Default); // Memory usage
    AddDiagnoser(new DisassemblyDiagnoser(new DisassemblyDiagnoserConfig())); // Disassembly, for understanding the generated assembly code

    // .... just do a short run while setting up.... 
    var job = Job.ShortRun.WithWarmupCount(1).WithIterationCount(1);
    AddJob(job);
  }
}
