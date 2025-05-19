// (c) D. Sawyer <damiensawyer@gmail.com> 2025

using DepthChart.Interfaces;
using DepthChart.Models;
using DepthChart.Tests.Tests.DepthChartTests.Tests.Helpers;
using TestHelpers.DynamicFixtures.TestWrappers;
using TestHelpers.Helpers;
using Xunit.Abstractions;

namespace DepthChart.Tests.Tests.DepthChartTests.Tests;

public class ConcurrentDepthChartTests(ITestOutputHelper outputHelper) : TestBase(outputHelper)
{
  [Theory]
  [MemberData(nameof(ThreadSafeDepthChartServices.GetThreadSafeDepthChartServices), MemberType = typeof(ThreadSafeDepthChartServices))]
  public async Task ConcurrentAddAndRemove_ShouldMaintainConsistency(ITestServiceWrapper<IDepthChartService> depthChart)
  {
    // Arrange
    const int threadCount = 100;
    const int operationsPerThread = 100;
    var tasks = new List<Task>();
    var players = Enumerable.Range(1, threadCount * operationsPerThread).Select(i => new Player(i, $"Player{i}", i % 2 == 0 ? "QB" : "WR")).ToArray();
    var positions = new[]
    {
      "QB",
      "WR",
      "RB",
      "TE"
    };
    var random = new Random(42); // Fixed seed for reproducibility

    // Act
    for (var t = 0; t < threadCount; t++)
    {
      var threadId = t;
      tasks.Add(Task.Run(() =>
      {
        for (var i = 0; i < operationsPerThread; i++)
        {
          var playerIndex = threadId * operationsPerThread + i;
          var player = players[playerIndex];
          var position = positions[random.Next(positions.Length)];

          // Randomly add or remove
          if (random.Next(100) < 70) // 70% chance to add
          {
            var depth = random.Next(10);
            depthChart.Service.AddPlayerToDepthChart(player, position, depth);
          }
          else // 30% chance to remove
            depthChart.Service.RemovePlayerFromDepthChart(player, position);
        }
      }));
    }

    await Task.WhenAll(tasks);

    // Assert
    var result = depthChart.Service.GetFullDepthChart();

    // Verify no position has duplicate players
    foreach (var position in result)
    {
      var playerIds = position.Value.Select(p => p.PlayerId).ToList();
      Assert.Equal(playerIds.Count, playerIds.Distinct().Count());
    }
  }
}
