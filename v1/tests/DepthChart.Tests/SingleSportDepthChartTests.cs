using System.Diagnostics;
using DepthChart.Enums;
using DepthChart.Extensions;
using DepthChart.Helpers;
using DepthChart.Models;

namespace DepthChart.Tests
{
    public class SingleSportDepthChartTests
    {
        private readonly Player _alice = new(2, "Alice", "QB");
        private readonly Player _bob = new(1, "Bob", "WR");
        private readonly Player _charlie = new(3, "Charlie", "RB");
        private readonly Player _david = new(4, "David", "QB");

        [Fact]
        public void AddPlayer_ShouldAddPlayers()
        {
            // Arrange
            var _depthChart = DepthChartFactory.CreateDepthChart(SportType.NFL);
            _depthChart.AddPlayerToDepthChart(this._bob, "WR", 0);
            _depthChart.AddPlayerToDepthChart(this._alice, "WR", 1);
            _depthChart.AddPlayerToDepthChart(this._charlie, "WR", 2);
            _depthChart.AddPlayerToDepthChart(this._david, "WR");

            // Act
            var depthChart = _depthChart.GetFullDepthChart();
            var playerList = depthChart.Single();

            // Assert
            Assert.Equal("WR", playerList.Key);
            Assert.True(playerList.Value.Count == 4);
            Assert.Equal(this._bob.PlayerId, playerList.Value[0].PlayerId);
            Assert.Equal(this._alice.PlayerId, playerList.Value[1].PlayerId);
            Assert.Equal(this._charlie.PlayerId, playerList.Value[2].PlayerId);
            Assert.Equal(this._david.PlayerId, playerList.Value[3].PlayerId);
        }

        [Fact]
        public void RemovingAllPlayersFromGivenPositionRemovesPositionFromDepthChart()
        {
            // Arrange
            var _depthChart = DepthChartFactory.CreateDepthChart(SportType.NFL);
            _depthChart.AddPlayerToDepthChart(this._bob, "WR", 0);
            _depthChart.AddPlayerToDepthChart(this._alice, "WR", 1);
            _depthChart.AddPlayerToDepthChart(this._charlie, "WR", 2);

            _depthChart.RemovePlayerFromDepthChart(this._bob, "WR");
            _depthChart.RemovePlayerFromDepthChart(this._alice, "WR");
            _depthChart.RemovePlayerFromDepthChart(this._charlie, "WR");

            // Act
            var depthChart = _depthChart.GetFullDepthChart();

            // Assert
            Assert.Empty(depthChart);
        }


        [Fact]
        public void KeyValuePairFormatting()
        {
            // Arrange
            var _depthChart = DepthChartFactory.CreateDepthChart(SportType.NFL);
            _depthChart.AddPlayerToDepthChart(this._bob, "WR", 0);
            _depthChart.AddPlayerToDepthChart(this._alice, "WR", 1);
            _depthChart.AddPlayerToDepthChart(this._charlie, "WR", 2);

            // Act
            var depthChart = _depthChart.GetFullDepthChart();
            var playerList = depthChart.Single();

            // Assert
            Assert.Equal("WR: [1, 2, 3]", playerList.ToFormattedString());
        }

        [Fact]
        public void AddPlayer_Twice_ShouldMoveThem()
        {
            // Arrange
            var _depthChart = DepthChartFactory.CreateDepthChart(SportType.NFL);
            _depthChart.AddPlayerToDepthChart(this._bob, "WR", 0);
            _depthChart.AddPlayerToDepthChart(this._alice, "WR", 1);
            _depthChart.AddPlayerToDepthChart(this._charlie, "WR", 2);
            _depthChart.AddPlayerToDepthChart(this._bob, "WR", 3);

            // Act
            var depthChart = _depthChart.GetFullDepthChart();
            var playerList = depthChart.Single();

            // Assert
            Assert.Equal("WR", playerList.Key);
            Assert.True(playerList.Value.Count == 3);
            Assert.Equal(this._alice.PlayerId, playerList.Value[0].PlayerId);
            Assert.Equal(this._charlie.PlayerId, playerList.Value[1].PlayerId);
            Assert.Equal(this._bob.PlayerId, playerList.Value[2].PlayerId);
        }

        [Fact]
        public void Adding_Bad_Position_ShouldThrowException()
        {
            var _depthChart = DepthChartFactory.CreateDepthChart(SportType.NFL);
            Assert.Throws<ArgumentException>(() => _depthChart.AddPlayerToDepthChart(this._bob, "bad", 0));
        }

        [Fact]
        public void AddPlayer_ToEndOfChart_ShouldAppend()
        {
            // Arrange
            var _depthChart = DepthChartFactory.CreateDepthChart(SportType.NFL);
            _depthChart.AddPlayerToDepthChart(this._bob, "WR");
            _depthChart.AddPlayerToDepthChart(this._alice, "WR");

            // Act
            var depthChart = _depthChart.GetFullDepthChart();
            var playerList = depthChart.Single();

            // Assert
            Assert.Equal("WR", playerList.Key);
            Assert.Equal(2, playerList.Value.Count);
            Assert.Equal(this._bob.PlayerId, playerList.Value[0].PlayerId);
            Assert.Equal(this._alice.PlayerId, playerList.Value[1].PlayerId);
        }

        [Fact]
        public void AddPlayer_ToMultiplePositions_ShouldWork()
        {
            // Arrange
            var _depthChart = DepthChartFactory.CreateDepthChart(SportType.NFL);
            _depthChart.AddPlayerToDepthChart(this._bob, "WR");
            _depthChart.AddPlayerToDepthChart(this._bob, "KR");

            // Act
            var depthChart = _depthChart.GetFullDepthChart();

            // Assert
            Assert.Equal(2, depthChart.Count);
            Assert.Single(depthChart["WR"]);
            Assert.Single(depthChart["KR"]);
        }

        [Fact]
        public void AddPlayer_TwiceToSamePosition_ShouldUpdatePosition()
        {
            // Arrange
            var _depthChart = DepthChartFactory.CreateDepthChart(SportType.NFL);
            _depthChart.AddPlayerToDepthChart(this._bob, "WR", 0);
            _depthChart.AddPlayerToDepthChart(this._alice, "WR", 1);
            _depthChart.AddPlayerToDepthChart(this._bob, "WR", 1);

            // Act
            var depthChart = _depthChart.GetFullDepthChart();
            var playerList = depthChart.Single();

            // Assert
            Assert.Equal("WR", playerList.Key);
            Assert.Equal(2, playerList.Value.Count);
            Assert.Equal(this._alice.PlayerId, playerList.Value[0].PlayerId);
            Assert.Equal(this._bob.PlayerId, playerList.Value[1].PlayerId);
        }

        [Fact]
        public void RemovePlayer_ShouldRemoveCorrectly()
        {
            // Arrange
            var _depthChart = DepthChartFactory.CreateDepthChart(SportType.NFL);
            _depthChart.AddPlayerToDepthChart(this._bob, "WR", 0);
            _depthChart.AddPlayerToDepthChart(this._alice, "WR", 1);
            _depthChart.RemovePlayerFromDepthChart(this._bob, "WR");

            // Act
            var depthChart = _depthChart.GetFullDepthChart();
            var playerList = depthChart.Single();

            // Assert
            Assert.Equal("WR", playerList.Key);
            Assert.Single(playerList.Value);
            Assert.Equal(this._alice.PlayerId, playerList.Value[0].PlayerId);
        }
        
        [Fact]
        public void MultiplePlayerRemovalIsIdempotent()
        {
            // Arrange
            var _depthChart = DepthChartFactory.CreateDepthChart(SportType.NFL);
            _depthChart.AddPlayerToDepthChart(this._bob, "WR", 0);
            _depthChart.RemovePlayerFromDepthChart(this._bob, "WR");
            _depthChart.RemovePlayerFromDepthChart(this._bob, "WR");

            // Act
            var depthChart = _depthChart.GetFullDepthChart();

            // Assert
            Assert.Empty(depthChart);
        }

        [Fact]
        public void Removal_OfNonExistingPlayer_ShouldNotThrowException()
        {
            var _depthChart = DepthChartFactory.CreateDepthChart(SportType.NFL);
            _depthChart.RemovePlayerFromDepthChart(this._bob, "WR");
        }

        [Fact]
        public void GetPlayersUnderPlayer_ShouldReturnCorrectPlayers()
        {
            // Arrange
            var _depthChart = DepthChartFactory.CreateDepthChart(SportType.NFL);
            _depthChart.AddPlayerToDepthChart(this._bob, "WR", 0);
            _depthChart.AddPlayerToDepthChart(this._alice, "WR", 0);
            _depthChart.AddPlayerToDepthChart(this._charlie, "WR", 2);

            // Act
            var playersUnder = _depthChart.GetPlayersUnderPlayerInDepthChart(this._alice, "WR");

            // Assert
            Assert.Equal(2, playersUnder.Count);
            Assert.Equal(this._bob.PlayerId, playersUnder[0].PlayerId);
            Assert.Equal(this._charlie.PlayerId, playersUnder[1].PlayerId);
        }

        [Fact]
        public void GetPlayersUnderPlayer_LastPlayer_ShouldReturnEmpty()
        {
            // Arrange
            var _depthChart = DepthChartFactory.CreateDepthChart(SportType.NFL);
            _depthChart.AddPlayerToDepthChart(this._bob, "WR", 0);
            _depthChart.AddPlayerToDepthChart(this._alice, "WR", 1);
            _depthChart.AddPlayerToDepthChart(this._charlie, "WR", 2);

            // Act
            var playersUnder = _depthChart.GetPlayersUnderPlayerInDepthChart(this._charlie, "WR");

            // Assert
            Assert.Empty(playersUnder);
        }

        [Fact]
        public void GetPlayersUnderPlayer_PlayerNotInChart_ShouldReturnEmpty()
        {
            // Arrange
            var _depthChart = DepthChartFactory.CreateDepthChart(SportType.NFL);

            // Act
            var playersUnder = _depthChart.GetPlayersUnderPlayerInDepthChart(this._bob, "WR");

            // Assert
            Assert.Empty(playersUnder);
        }

        [Fact]
        public void AddPlayer_ShouldPushMultiplePlayersDown()
        {
            // Arrange
            var _depthChart = DepthChartFactory.CreateDepthChart(SportType.NFL);
            var dave = new Player(4, "Dave", "WR");

            _depthChart.AddPlayerToDepthChart(this._bob, "WR", 0); // Bob at 0
            _depthChart.AddPlayerToDepthChart(this._charlie, "WR", 1); // Charlie at 1
            _depthChart.AddPlayerToDepthChart(dave, "WR", 2); // Dave at 2
            _depthChart.AddPlayerToDepthChart(this._alice, "WR", 1); // Insert at position 1

            // Act
            var depthChart = _depthChart.GetFullDepthChart();
            var playerList = depthChart.Single();

            // Assert
            Assert.Equal("WR", playerList.Key);
            Assert.Equal(4, playerList.Value.Count);
            Assert.Equal(this._bob.PlayerId, playerList.Value[0].PlayerId);
            Assert.Equal(this._alice.PlayerId, playerList.Value[1].PlayerId);
            Assert.Equal(this._charlie.PlayerId, playerList.Value[2].PlayerId);
            Assert.Equal(dave.PlayerId, playerList.Value[3].PlayerId);
        }

        [Fact]
        public void AddPlayer_ShouldPushDownCorrespondingPlayers()
        {
            // Arrange
            var _depthChart = DepthChartFactory.CreateDepthChart(SportType.NFL);
            _depthChart.AddPlayerToDepthChart(this._charlie, "WR", 0); // Charlie is starter
            _depthChart.AddPlayerToDepthChart(this._david, "WR", 1); // David is backup
            _depthChart.AddPlayerToDepthChart(this._alice, "WR", 0); // Alice becomes starter, bumps Charlie to backup

            // Act
            var depthChart = _depthChart.GetFullDepthChart();
            var playerList = depthChart.Single();

            // Assert
            Assert.Equal("WR", playerList.Key);
            Assert.Equal(3, playerList.Value.Count);
            Assert.Equal(this._alice.PlayerId, playerList.Value[0].PlayerId);
            Assert.Equal(this._charlie.PlayerId, playerList.Value[1].PlayerId);
            Assert.Equal(this._david.PlayerId, playerList.Value[2].PlayerId);
        }

        [Fact]
        public void GetFullDepthChart_EmptyDepthChart_ShouldReturnEmptyDictionary()
        {
            // Arrange
            var _depthChart = DepthChartFactory.CreateDepthChart(SportType.NFL);

            // Act
            var depthChart = _depthChart.GetFullDepthChart();

            // Assert
            Assert.NotNull(depthChart);
            Assert.Empty(depthChart);
        }

        [Fact]
        public void AddPlayer_ShouldPutOutlyingPositionsToBottom()
        {
            // Arrange
            var _depthChart = DepthChartFactory.CreateDepthChart(SportType.NFL);
            _depthChart.AddPlayerToDepthChart(this._charlie, "WR", 0); // Charlie is starter
            _depthChart.AddPlayerToDepthChart(this._david, "WR", 1); // David is backup
            _depthChart.AddPlayerToDepthChart(this._alice, "WR", 0); // Alice becomes starter, bumps Charlie to backup
            _depthChart.AddPlayerToDepthChart(this._bob, "WR", 1000); // If we add someone beyond the number in the chart, assume that they're at the end 

            // Act
            var depthChart = _depthChart.GetFullDepthChart();
            var playerList = depthChart.Single();

            // Assert
            Assert.Equal("WR", playerList.Key);
            Assert.Equal(4, playerList.Value.Count);
            Assert.Equal(this._alice.PlayerId, playerList.Value[0].PlayerId);
            Assert.Equal(this._charlie.PlayerId, playerList.Value[1].PlayerId);
            Assert.Equal(this._david.PlayerId, playerList.Value[2].PlayerId);
            Assert.Equal(this._bob.PlayerId, playerList.Value[3].PlayerId);
        }


        [Fact]
        public void AddPlayer_ShouldPushExistingPlayerDown()
        {
            // Arrange
            var _depthChart = DepthChartFactory.CreateDepthChart(SportType.NFL);
            _depthChart.AddPlayerToDepthChart(this._bob, "WR", 0);
            _depthChart.AddPlayerToDepthChart(this._alice, "WR", 0);

            // Act
            var depthChart = _depthChart.GetFullDepthChart();
            var playerList = depthChart.Single();

            // Assert
            Assert.Equal("WR", playerList.Key);
            Assert.Equal(2, playerList.Value.Count);
            Assert.Equal(this._alice.PlayerId, playerList.Value[0].PlayerId);
            Assert.Equal(this._bob.PlayerId, playerList.Value[1].PlayerId);
        }

        [Fact]
        public void AddPlayer_LargeDepthChart_ShouldPerformEfficiently()
        {
            // Arrange
            var _depthChart = DepthChartFactory.CreateDepthChart(SportType.NFL);
            const int playerCount = 20000;
            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();

            for (var i = 0; i < playerCount; i++)
            {
                var player = new Player(i, $"Player{i}", "WR");
                _depthChart.AddPlayerToDepthChart(player, "WR");
            }

            // Get the full depth chart
            var depthChart = _depthChart.GetFullDepthChart();
            stopwatch.Stop();

            // Assert
            Assert.Single(depthChart.Keys);
            Assert.Equal(playerCount, depthChart["WR"].Count);
            Assert.Equal(0, depthChart["WR"][0].PlayerId);
            Assert.Equal(playerCount - 1, depthChart["WR"][playerCount - 1].PlayerId);

            // Performance assertion - adjust the threshold based on your requirements
            // This ensures the operation completes within a reasonable time frame
            Assert.True(stopwatch.ElapsedMilliseconds < 5000, $"Large depth chart operation took too long: {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}