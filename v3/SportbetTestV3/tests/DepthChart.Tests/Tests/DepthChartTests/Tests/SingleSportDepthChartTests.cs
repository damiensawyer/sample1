// (c) D. Sawyer <damiensawyer@gmail.com> 2025

using System.Diagnostics;
using DepthChart.Extensions;
using DepthChart.Interfaces;
using DepthChart.Models;
using DepthChart.Tests.Tests.DepthChartTests.Tests.Helpers;
using TestHelpers.DynamicFixtures.TestWrappers;

namespace DepthChart.Tests.Tests.DepthChartTests.Tests;

public class SingleSportDepthChartTests
{
  private readonly Player _alice = new(2, "Alice", "QB");
  private readonly Player _bob = new(1, "Bob", "WR");
  private readonly Player _charlie = new(3, "Charlie", "RB");
  private readonly Player _david = new(4, "David", "QB");

  [Theory]
  [MemberData(nameof(DepthChartServices.GetDepthChartServices), MemberType = typeof(DepthChartServices))]
  public void AddPlayer_ShouldAddPlayers(ITestServiceWrapper<IDepthChartService> _depthChart)
  {
    // Arrange
    _depthChart.Service.AddPlayerToDepthChart(this._bob, "WR", 0);
    _depthChart.Service.AddPlayerToDepthChart(this._alice, "WR", 1);
    _depthChart.Service.AddPlayerToDepthChart(this._charlie, "WR", 2);
    _depthChart.Service.AddPlayerToDepthChart(this._david, "WR");

    // Act
    var depthChart = _depthChart.Service.GetFullDepthChart();
    var playerList = depthChart.Single();

    // Assert
    Assert.Equal("WR", playerList.Key);
    Assert.True(playerList.Value.Count == 4);
    Assert.Equal(this._bob.PlayerId, playerList.Value[0].PlayerId);
    Assert.Equal(this._alice.PlayerId, playerList.Value[1].PlayerId);
    Assert.Equal(this._charlie.PlayerId, playerList.Value[2].PlayerId);
    Assert.Equal(this._david.PlayerId, playerList.Value[3].PlayerId);
  }

  [Theory]
  [MemberData(nameof(DepthChartServices.GetDepthChartServices), MemberType = typeof(DepthChartServices))]
  public void RemovingAllPlayersFromGivenPositionRemovesPositionFromDepthChart(ITestServiceWrapper<IDepthChartService> _depthChart)
  {
    // Arrange
    _depthChart.Service.AddPlayerToDepthChart(this._bob, "WR", 0);
    _depthChart.Service.AddPlayerToDepthChart(this._alice, "WR", 1);
    _depthChart.Service.AddPlayerToDepthChart(this._charlie, "WR", 2);

    _depthChart.Service.RemovePlayerFromDepthChart(this._bob, "WR");
    _depthChart.Service.RemovePlayerFromDepthChart(this._alice, "WR");
    _depthChart.Service.RemovePlayerFromDepthChart(this._charlie, "WR");

    // Act
    var depthChart = _depthChart.Service.GetFullDepthChart();

    // Assert
    Assert.Empty(depthChart);
  }

  [Theory]
  [MemberData(nameof(DepthChartServices.GetDepthChartServices), MemberType = typeof(DepthChartServices))]
  public void AllowCaseInsensitiveLookupOnPosition(ITestServiceWrapper<IDepthChartService> _depthChart)
  {

    // Arrange
    _depthChart.Service.AddPlayerToDepthChart(this._bob, "WR");

    // Act
    var result = _depthChart.Service.GetFullDepthChart();

    // Assert
    Assert.Single(result);
    Assert.Single(result["wr"]);
    Assert.Equal(this._bob.PlayerId, result["wr"][0].PlayerId);
  }

  [Theory]
  [MemberData(nameof(DepthChartServices.GetDepthChartServices), MemberType = typeof(DepthChartServices))]
  public void KeyValuePairFormatting(ITestServiceWrapper<IDepthChartService> _depthChart)
  {
    // Arrange
    _depthChart.Service.AddPlayerToDepthChart(this._bob, "WR", 0);
    _depthChart.Service.AddPlayerToDepthChart(this._alice, "WR", 1);
    _depthChart.Service.AddPlayerToDepthChart(this._charlie, "WR", 2);

    // Act
    var depthChart = _depthChart.Service.GetFullDepthChart();
    var playerList = depthChart.Single();

    // Assert
    Assert.Equal("WR: [1, 2, 3]", playerList.ToFormattedString());
  }

  [Theory]
  [MemberData(nameof(DepthChartServices.GetDepthChartServices), MemberType = typeof(DepthChartServices))]
  public void AddPlayer_Twice_ShouldMoveThem(ITestServiceWrapper<IDepthChartService> _depthChart)
  {
    // Arrange

    _depthChart.Service.AddPlayerToDepthChart(this._bob, "WR", 0);
    _depthChart.Service.AddPlayerToDepthChart(this._alice, "WR", 1);
    _depthChart.Service.AddPlayerToDepthChart(this._charlie, "WR", 2);
    _depthChart.Service.AddPlayerToDepthChart(this._bob, "WR", 3);

    // Act
    var depthChart = _depthChart.Service.GetFullDepthChart();
    var playerList = depthChart.Single();

    // Assert
    Assert.Equal("WR", playerList.Key);
    Assert.True(playerList.Value.Count == 3);
    Assert.Equal(this._alice.PlayerId, playerList.Value[0].PlayerId);
    Assert.Equal(this._charlie.PlayerId, playerList.Value[1].PlayerId);
    Assert.Equal(this._bob.PlayerId, playerList.Value[2].PlayerId);
  }

  [Theory]
  [MemberData(nameof(DepthChartServices.GetDepthChartServices), MemberType = typeof(DepthChartServices))]
  public void Adding_Bad_Position_ShouldThrowException(ITestServiceWrapper<IDepthChartService> _depthChart) =>
    Assert.Throws<ArgumentException>(() => _depthChart.Service.AddPlayerToDepthChart(this._bob, "bad", 0));

  [Theory]
  [MemberData(nameof(DepthChartServices.GetDepthChartServices), MemberType = typeof(DepthChartServices))]
  public void AddPlayer_ToEndOfChart_ShouldAppend(ITestServiceWrapper<IDepthChartService> _depthChart)
  {
    // Arrange
    _depthChart.Service.AddPlayerToDepthChart(this._bob, "WR");
    _depthChart.Service.AddPlayerToDepthChart(this._alice, "WR");

    // Act
    var depthChart = _depthChart.Service.GetFullDepthChart();
    var playerList = depthChart.Single();

    // Assert
    Assert.Equal("WR", playerList.Key);
    Assert.Equal(2, playerList.Value.Count);
    Assert.Equal(this._bob.PlayerId, playerList.Value[0].PlayerId);
    Assert.Equal(this._alice.PlayerId, playerList.Value[1].PlayerId);
  }

  [Theory]
  [MemberData(nameof(DepthChartServices.GetDepthChartServices), MemberType = typeof(DepthChartServices))]
  public void AddPlayer_ToMultiplePositions_ShouldWork(ITestServiceWrapper<IDepthChartService> _depthChart)
  {
    // Arrange
    _depthChart.Service.AddPlayerToDepthChart(this._bob, "WR");
    _depthChart.Service.AddPlayerToDepthChart(this._bob, "KR");

    // Act
    var depthChart = _depthChart.Service.GetFullDepthChart();

    // Assert
    Assert.Equal(2, depthChart.Count);
    Assert.Single(depthChart["WR"]);
    Assert.Single(depthChart["KR"]);
  }

  [Theory]
  [MemberData(nameof(DepthChartServices.GetDepthChartServices), MemberType = typeof(DepthChartServices))]
  public void AddPlayer_TwiceToSamePosition_ShouldUpdatePosition(ITestServiceWrapper<IDepthChartService> _depthChart)
  {
    // Arrange
    _depthChart.Service.AddPlayerToDepthChart(this._bob, "WR", 0);
    _depthChart.Service.AddPlayerToDepthChart(this._alice, "WR", 1);
    _depthChart.Service.AddPlayerToDepthChart(this._bob, "WR", 1);

    // Act
    var depthChart = _depthChart.Service.GetFullDepthChart();
    var playerList = depthChart.Single();

    // Assert
    Assert.Equal("WR", playerList.Key);
    Assert.Equal(2, playerList.Value.Count);
    Assert.Equal(this._alice.PlayerId, playerList.Value[0].PlayerId);
    Assert.Equal(this._bob.PlayerId, playerList.Value[1].PlayerId);
  }

  [Theory]
  [MemberData(nameof(DepthChartServices.GetDepthChartServices), MemberType = typeof(DepthChartServices))]
  public void RemovePlayer_ShouldRemoveCorrectly(ITestServiceWrapper<IDepthChartService> _depthChart)
  {
    // Arrange
    _depthChart.Service.AddPlayerToDepthChart(this._bob, "WR", 0);
    _depthChart.Service.AddPlayerToDepthChart(this._alice, "WR", 1);
    _depthChart.Service.RemovePlayerFromDepthChart(this._bob, "WR");

    // Act
    var depthChart = _depthChart.Service.GetFullDepthChart();
    var playerList = depthChart.Single();

    // Assert
    Assert.Equal("WR", playerList.Key);
    Assert.Single(playerList.Value);
    Assert.Equal(this._alice.PlayerId, playerList.Value[0].PlayerId);
  }

  [Theory]
  [MemberData(nameof(DepthChartServices.GetDepthChartServices), MemberType = typeof(DepthChartServices))]
  public void MultiplePlayerRemovalIsIdempotent(ITestServiceWrapper<IDepthChartService> _depthChart)
  {
    // Arrange
    _depthChart.Service.AddPlayerToDepthChart(this._bob, "WR", 0);
    _depthChart.Service.RemovePlayerFromDepthChart(this._bob, "WR");
    _depthChart.Service.RemovePlayerFromDepthChart(this._bob, "WR");

    // Act
    var depthChart = _depthChart.Service.GetFullDepthChart();

    // Assert
    Assert.Empty(depthChart);
  }

  [Theory]
  [MemberData(nameof(DepthChartServices.GetDepthChartServices), MemberType = typeof(DepthChartServices))]
  public void Removal_OfNonExistingPlayer_ShouldNotThrowException(ITestServiceWrapper<IDepthChartService> _depthChart) =>
    _depthChart.Service.RemovePlayerFromDepthChart(this._bob, "WR");

  [Theory]
  [MemberData(nameof(DepthChartServices.GetDepthChartServices), MemberType = typeof(DepthChartServices))]
  public void GetPlayersUnderPlayer_ShouldReturnCorrectPlayers(ITestServiceWrapper<IDepthChartService> _depthChart)
  {
    // Arrange
    _depthChart.Service.AddPlayerToDepthChart(this._bob, "WR", 0);
    _depthChart.Service.AddPlayerToDepthChart(this._alice, "WR", 0);
    _depthChart.Service.AddPlayerToDepthChart(this._charlie, "WR", 2);

    // Act
    var playersUnder = _depthChart.Service.GetPlayersUnderPlayerInDepthChart(this._alice, "WR");

    // Assert
    Assert.Equal(2, playersUnder.Count);
    Assert.Equal(this._bob.PlayerId, playersUnder[0].PlayerId);
    Assert.Equal(this._charlie.PlayerId, playersUnder[1].PlayerId);
  }

  [Theory]
  [MemberData(nameof(DepthChartServices.GetDepthChartServices), MemberType = typeof(DepthChartServices))]
  public void GetPlayersUnderPlayer_LastPlayer_ShouldReturnEmpty(ITestServiceWrapper<IDepthChartService> _depthChart)
  {
    // Arrange
    _depthChart.Service.AddPlayerToDepthChart(this._bob, "WR", 0);
    _depthChart.Service.AddPlayerToDepthChart(this._alice, "WR", 1);
    _depthChart.Service.AddPlayerToDepthChart(this._charlie, "WR", 2);

    // Act
    var playersUnder = _depthChart.Service.GetPlayersUnderPlayerInDepthChart(this._charlie, "WR");

    // Assert
    Assert.Empty(playersUnder);
  }

  [Theory]
  [MemberData(nameof(DepthChartServices.GetDepthChartServices), MemberType = typeof(DepthChartServices))]
  public void GetPlayersUnderPlayer_PlayerNotInChart_ShouldReturnEmpty(ITestServiceWrapper<IDepthChartService> _depthChart)
  {
    // Arrange

    // Act
    var playersUnder = _depthChart.Service.GetPlayersUnderPlayerInDepthChart(this._bob, "WR");

    // Assert
    Assert.Empty(playersUnder);
  }

  [Theory]
  [MemberData(nameof(DepthChartServices.GetDepthChartServices), MemberType = typeof(DepthChartServices))]
  public void AddPlayer_ShouldPushMultiplePlayersDown(ITestServiceWrapper<IDepthChartService> _depthChart)
  {
    // Arrange
    var dave = new Player(4, "Dave", "WR");

    _depthChart.Service.AddPlayerToDepthChart(this._bob, "WR", 0); // Bob at 0
    _depthChart.Service.AddPlayerToDepthChart(this._charlie, "WR", 1); // Charlie at 1
    _depthChart.Service.AddPlayerToDepthChart(dave, "WR", 2); // Dave at 2
    _depthChart.Service.AddPlayerToDepthChart(this._alice, "WR", 1); // Insert at position 1

    // Act
    var depthChart = _depthChart.Service.GetFullDepthChart();
    var playerList = depthChart.Single();

    // Assert
    Assert.Equal("WR", playerList.Key);
    Assert.Equal(4, playerList.Value.Count);
    Assert.Equal(this._bob.PlayerId, playerList.Value[0].PlayerId);
    Assert.Equal(this._alice.PlayerId, playerList.Value[1].PlayerId);
    Assert.Equal(this._charlie.PlayerId, playerList.Value[2].PlayerId);
    Assert.Equal(dave.PlayerId, playerList.Value[3].PlayerId);
  }

  [Theory]
  [MemberData(nameof(DepthChartServices.GetDepthChartServices), MemberType = typeof(DepthChartServices))]
  public void AddPlayer_ShouldPushDownCorrespondingPlayers(ITestServiceWrapper<IDepthChartService> _depthChart)
  {
    // Arrange
    _depthChart.Service.AddPlayerToDepthChart(this._charlie, "WR", 0); // Charlie is starter
    _depthChart.Service.AddPlayerToDepthChart(this._david, "WR", 1); // David is backup
    _depthChart.Service.AddPlayerToDepthChart(this._alice, "WR", 0); // Alice becomes starter, bumps Charlie to backup

    // Act
    var depthChart = _depthChart.Service.GetFullDepthChart();
    var playerList = depthChart.Single();

    // Assert
    Assert.Equal("WR", playerList.Key);
    Assert.Equal(3, playerList.Value.Count);
    Assert.Equal(this._alice.PlayerId, playerList.Value[0].PlayerId);
    Assert.Equal(this._charlie.PlayerId, playerList.Value[1].PlayerId);
    Assert.Equal(this._david.PlayerId, playerList.Value[2].PlayerId);
  }

  [Theory]
  [MemberData(nameof(DepthChartServices.GetDepthChartServices), MemberType = typeof(DepthChartServices))]
  public void GetFullDepthChart_EmptyDepthChart_ShouldReturnEmptyDictionary(ITestServiceWrapper<IDepthChartService> _depthChart)
  {
    // Arrange

    // Act
    var depthChart = _depthChart.Service.GetFullDepthChart();

    // Assert
    Assert.NotNull(depthChart);
    Assert.Empty(depthChart);
  }

  [Theory]
  [MemberData(nameof(DepthChartServices.GetDepthChartServices), MemberType = typeof(DepthChartServices))]
  public void AddPlayer_ShouldPutOutlyingPositionsToBottom(ITestServiceWrapper<IDepthChartService> _depthChart)
  {
    // Arrange
    _depthChart.Service.AddPlayerToDepthChart(this._charlie, "WR", 0); // Charlie is starter
    _depthChart.Service.AddPlayerToDepthChart(this._david, "WR", 1); // David is backup
    _depthChart.Service.AddPlayerToDepthChart(this._alice, "WR", 0); // Alice becomes starter, bumps Charlie to backup
    _depthChart.Service.AddPlayerToDepthChart(this._bob, "WR", 1000); // If we add someone beyond the number in the chart, assume that they're at the end

    // Act
    var depthChart = _depthChart.Service.GetFullDepthChart();
    var playerList = depthChart.Single();

    // Assert
    Assert.Equal("WR", playerList.Key);
    Assert.Equal(4, playerList.Value.Count);
    Assert.Equal(this._alice.PlayerId, playerList.Value[0].PlayerId);
    Assert.Equal(this._charlie.PlayerId, playerList.Value[1].PlayerId);
    Assert.Equal(this._david.PlayerId, playerList.Value[2].PlayerId);
    Assert.Equal(this._bob.PlayerId, playerList.Value[3].PlayerId);
  }

  [Theory]
  [MemberData(nameof(DepthChartServices.GetDepthChartServices), MemberType = typeof(DepthChartServices))]
  public void AddPlayer_ShouldPushExistingPlayerDown(ITestServiceWrapper<IDepthChartService> _depthChart)
  {
    // Arrange
    _depthChart.Service.AddPlayerToDepthChart(this._bob, "WR", 0);
    _depthChart.Service.AddPlayerToDepthChart(this._alice, "WR", 0);

    // Act
    var depthChart = _depthChart.Service.GetFullDepthChart();
    var playerList = depthChart.Single();

    // Assert
    Assert.Equal("WR", playerList.Key);
    Assert.Equal(2, playerList.Value.Count);
    Assert.Equal(this._alice.PlayerId, playerList.Value[0].PlayerId);
    Assert.Equal(this._bob.PlayerId, playerList.Value[1].PlayerId);
  }

  [Theory]
  [MemberData(nameof(DepthChartServices.GetDepthChartServices), MemberType = typeof(DepthChartServices))]
  public void AddPlayer_LargeDepthChart_ShouldPerformEfficiently(ITestServiceWrapper<IDepthChartService> _depthChart)
  {
    // Arrange
    const int playerCount = 10000;
    var stopwatch = new Stopwatch();

    // Act
    stopwatch.Start();

    for (var i = 0; i < playerCount; i++)
    {
      var player = new Player(i, $"Player{i}", "WR");
      _depthChart.Service.AddPlayerToDepthChart(player, "WR");
    }

    // Get the full depth chart
    var depthChart = _depthChart.Service.GetFullDepthChart();
    stopwatch.Stop();

    // Assert
    Assert.Single(depthChart.Keys);
    Assert.Equal(playerCount, depthChart["WR"].Count);
    Assert.Equal(0, depthChart["WR"][0].PlayerId);
    Assert.Equal(playerCount - 1, depthChart["WR"][playerCount - 1].PlayerId);

    // Performance assertion - adjust the threshold based on your requirements
    // This ensures the operation completes within a reasonable time frame
    Assert.True(stopwatch.ElapsedMilliseconds < 10000, $"Large depth chart operation took too long: {stopwatch.ElapsedMilliseconds}ms");
  }
}
