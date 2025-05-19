// (c) D. Sawyer <damiensawyer@gmail.com> 2025

using DepthChart.Extensions;
using DepthChart.Interfaces;
using DepthChart.Models;
using DepthChart.Tests.Tests.DepthChartTests.Tests.Helpers;
using DepthChart.Tests.Tests.DepthChartTests.Tests.ServiceSets;
using Xunit.Abstractions;

namespace DepthChart.Tests.Tests.DepthChartTests.Tests;

public class MultiPositionDepthChartTests(ITestOutputHelper outputHelper) : TestBase(outputHelper)
{
  private readonly Player _alice = new(2, "Alice", "QB");
  private readonly Player _bob = new(1, "Bob", "WR");
  private readonly Player _charlie = new(3, "Charlie", "RB");
  private readonly Player _david = new(4, "David", "QB");
  private readonly Player _eve = new(5, "Eve", "TE");

  [Theory]
  [MemberData(nameof(DepthChartServices.GetDepthChartServices), MemberType = typeof(DepthChartServices))]
  public void GetFullDepthChart_WithMultiplePositions_ShouldReturnAllPositions(IDepthChartService depthChart)
  {
    // Arrange
    depthChart.AddPlayerToDepthChart(this._bob, "WR");
    depthChart.AddPlayerToDepthChart(this._alice, "QB");
    depthChart.AddPlayerToDepthChart(this._charlie, "RB");

    // Act
    var result = depthChart.GetFullDepthChart();

    // Assert
    Assert.Equal(3, result.Count);
    Assert.Single(result["WR"]);
    Assert.Single(result["QB"]);
    Assert.Single(result["RB"]);
    Assert.Equal(this._bob.PlayerId, result["WR"][0].PlayerId);
    Assert.Equal(this._alice.PlayerId, result["QB"][0].PlayerId);
    Assert.Equal(this._charlie.PlayerId, result["RB"][0].PlayerId);
  }

  [Theory]
  [MemberData(nameof(DepthChartServices.GetDepthChartServices), MemberType = typeof(DepthChartServices))]
  public void RemovePlayerFromDepthChart_WithMultiplePositions_ShouldOnlyRemoveFromSpecifiedPosition(IDepthChartService depthChart)
  {
    // Arrange
    depthChart.AddPlayerToDepthChart(this._bob, "WR");
    depthChart.AddPlayerToDepthChart(this._bob, "KR");
    depthChart.AddPlayerToDepthChart(this._alice, "QB");

    // Act
    depthChart.RemovePlayerFromDepthChart(this._bob, "WR");
    var result = depthChart.GetFullDepthChart();

    // Assert
    Assert.Equal(2, result.Count);
    Assert.DoesNotContain("WR", result.Keys);
    Assert.Single(result["KR"]);
    Assert.Single(result["QB"]);
    Assert.Equal(this._bob.PlayerId, result["KR"][0].PlayerId);
  }

  [Theory]
  [MemberData(nameof(DepthChartServices.GetDepthChartServices), MemberType = typeof(DepthChartServices))]
  public void ToFormattedString_WithMultiplePositions_ShouldFormatCorrectly(IDepthChartService depthChart)
  {
    // Arrange
    depthChart.AddPlayerToDepthChart(this._bob, "WR");
    depthChart.AddPlayerToDepthChart(this._alice, "QB");
    depthChart.AddPlayerToDepthChart(this._charlie, "WR", 1);

    // Act
    var result = depthChart.GetFullDepthChart();
    var formatted = string.Join(", ", result.Select(kv => kv.ToFormattedString()));

    // Assert
    Assert.Contains("WR: [1, 3]", formatted);
    Assert.Contains("QB: [2]", formatted);
  }

  [Theory]
  [MemberData(nameof(DepthChartServices.GetDepthChartServices), MemberType = typeof(DepthChartServices))]
  public void GetPlayersUnderPlayerInDepthChart_WithMultiplePositions_ShouldOnlyConsiderSpecifiedPosition(IDepthChartService depthChart)
  {
    // Arrange
    depthChart.AddPlayerToDepthChart(this._bob, "WR", 0);
    depthChart.AddPlayerToDepthChart(this._charlie, "WR", 1);
    depthChart.AddPlayerToDepthChart(this._alice, "QB", 0);
    depthChart.AddPlayerToDepthChart(this._david, "QB", 1);

    // Act
    var playersUnderBob = depthChart.GetPlayersUnderPlayerInDepthChart(this._bob, "WR");
    var playersUnderAlice = depthChart.GetPlayersUnderPlayerInDepthChart(this._alice, "QB");

    // Assert
    Assert.Single(playersUnderBob);
    Assert.Equal(this._charlie.PlayerId, playersUnderBob[0].PlayerId);
    Assert.Single(playersUnderAlice);
    Assert.Equal(this._david.PlayerId, playersUnderAlice[0].PlayerId);
  }

  [Theory]
  [MemberData(nameof(DepthChartServices.GetDepthChartServices), MemberType = typeof(DepthChartServices))]
  public void AddPlayerToDepthChart_PlayerInMultiplePositions_ShouldMaintainAllPositions(IDepthChartService depthChart)
  {
    // Arrange

    // Add Bob to multiple positions
    depthChart.AddPlayerToDepthChart(this._bob, "WR", 0);
    depthChart.AddPlayerToDepthChart(this._bob, "KR", 0);

    // Update Bob's position in one chart
    depthChart.AddPlayerToDepthChart(this._alice, "WR", 0); // Push Bob down in WR

    // Act
    var result = depthChart.GetFullDepthChart();

    // Assert
    Assert.Equal(2, result.Count);
    Assert.Equal(2, result["WR"].Count);
    Assert.Single(result["KR"]);

    // Bob should be pushed down in WR but still first in KR
    Assert.Equal(this._alice.PlayerId, result["WR"][0].PlayerId);
    Assert.Equal(this._bob.PlayerId, result["WR"][1].PlayerId);
    Assert.Equal(this._bob.PlayerId, result["KR"][0].PlayerId);
  }

  [Theory]
  [MemberData(nameof(DepthChartServices.GetDepthChartServices), MemberType = typeof(DepthChartServices))]
  public void AddPlayersToMultiplePositions_ThenRemoveAll_ShouldLeaveEmptyChart(IDepthChartService depthChart)
  {
    // Arrange

    // Add players to various positions
    depthChart.AddPlayerToDepthChart(this._bob, "WR");
    depthChart.AddPlayerToDepthChart(this._alice, "QB");
    depthChart.AddPlayerToDepthChart(this._charlie, "RB");
    depthChart.AddPlayerToDepthChart(this._david, "TE");
    depthChart.AddPlayerToDepthChart(this._eve, "KR");

    // Act - remove all players
    depthChart.RemovePlayerFromDepthChart(this._bob, "WR");
    depthChart.RemovePlayerFromDepthChart(this._alice, "QB");
    depthChart.RemovePlayerFromDepthChart(this._charlie, "RB");
    depthChart.RemovePlayerFromDepthChart(this._david, "TE");
    depthChart.RemovePlayerFromDepthChart(this._eve, "KR");

    var result = depthChart.GetFullDepthChart();

    // Assert
    Assert.Empty(result);
  }

  [Theory]
  [MemberData(nameof(DepthChartServices.GetDepthChartServices), MemberType = typeof(DepthChartServices))]
  public void GetPlayersUnderPlayer_PlayerNotInSpecifiedPosition_ShouldReturnEmpty(IDepthChartService depthChart)
  {
    // Arrange
    depthChart.AddPlayerToDepthChart(this._bob, "WR");
    depthChart.AddPlayerToDepthChart(this._alice, "QB");
    depthChart.AddPlayerToDepthChart(this._charlie, "QB", 1);

    // Act - Bob is in WR, not QB
    var playersUnder = depthChart.GetPlayersUnderPlayerInDepthChart(this._bob, "QB");

    // Assert
    Assert.Empty(playersUnder);
  }

  [Theory]
  [MemberData(nameof(DepthChartServices.GetDepthChartServices), MemberType = typeof(DepthChartServices))]
  public void UpdatePlayerPositionDepth_AcrossMultiplePositions_ShouldWorkIndependently(IDepthChartService depthChart)
  {
    // Arrange

    // Add multiple players to multiple positions
    depthChart.AddPlayerToDepthChart(this._alice, "QB", 0);
    depthChart.AddPlayerToDepthChart(this._david, "QB", 1);
    depthChart.AddPlayerToDepthChart(this._bob, "WR", 0);
    depthChart.AddPlayerToDepthChart(this._charlie, "WR", 1);
    depthChart.AddPlayerToDepthChart(this._eve, "TE", 0);

    // Act - update player positions independently
    depthChart.AddPlayerToDepthChart(this._alice, "WR", 2); // Add Alice to end of WR list
    depthChart.AddPlayerToDepthChart(this._bob, "KR", 0); // Add Bob to KR
    depthChart.AddPlayerToDepthChart(this._david, "QB", 0); // Move David to QB starter

    var result = depthChart.GetFullDepthChart();

    // Assert
    // Check QB has been reordered
    Assert.Equal(2, result["QB"].Count);
    Assert.Equal(this._david.PlayerId, result["QB"][0].PlayerId);
    Assert.Equal(this._alice.PlayerId, result["QB"][1].PlayerId);

    // Check WR has Alice at the end
    Assert.Equal(3, result["WR"].Count);
    Assert.Equal(this._bob.PlayerId, result["WR"][0].PlayerId);
    Assert.Equal(this._charlie.PlayerId, result["WR"][1].PlayerId);
    Assert.Equal(this._alice.PlayerId, result["WR"][2].PlayerId);

    // Check Bob is in KR
    Assert.Single(result["KR"]);
    Assert.Equal(this._bob.PlayerId, result["KR"][0].PlayerId);

    // Check TE is unchanged
    Assert.Single(result["TE"]);
    Assert.Equal(this._eve.PlayerId, result["TE"][0].PlayerId);
  }
}
