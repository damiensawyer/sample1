using DepthChart.Enums;
using DepthChart.Extensions;
using DepthChart.Helpers;
using DepthChart.Models;

namespace DepthChart.Tests
{
    public class MultiPositionDepthChartTests
    {
        private readonly Player _alice = new(2, "Alice", "QB");
        private readonly Player _bob = new(1, "Bob", "WR");
        private readonly Player _charlie = new(3, "Charlie", "RB");
        private readonly Player _david = new(4, "David", "QB");
        private readonly Player _eve = new(5, "Eve", "TE");

        [Fact]
        public void GetFullDepthChart_WithMultiplePositions_ShouldReturnAllPositions()
        {
            // Arrange
            var depthChart = DepthChartFactory.CreateDepthChart(SportType.NFL);
            depthChart.AddPlayerToDepthChart(_bob, "WR");
            depthChart.AddPlayerToDepthChart(_alice, "QB");
            depthChart.AddPlayerToDepthChart(_charlie, "RB");

            // Act
            var result = depthChart.GetFullDepthChart();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Single(result["WR"]);
            Assert.Single(result["QB"]);
            Assert.Single(result["RB"]);
            Assert.Equal(_bob.PlayerId, result["WR"][0].PlayerId);
            Assert.Equal(_alice.PlayerId, result["QB"][0].PlayerId);
            Assert.Equal(_charlie.PlayerId, result["RB"][0].PlayerId);
        }

        [Fact]
        public void RemovePlayerFromDepthChart_WithMultiplePositions_ShouldOnlyRemoveFromSpecifiedPosition()
        {
            // Arrange
            var depthChart = DepthChartFactory.CreateDepthChart(SportType.NFL);
            depthChart.AddPlayerToDepthChart(_bob, "WR");
            depthChart.AddPlayerToDepthChart(_bob, "KR");
            depthChart.AddPlayerToDepthChart(_alice, "QB");

            // Act
            depthChart.RemovePlayerFromDepthChart(_bob, "WR");
            var result = depthChart.GetFullDepthChart();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.DoesNotContain("WR", result.Keys);
            Assert.Single(result["KR"]);
            Assert.Single(result["QB"]);
            Assert.Equal(_bob.PlayerId, result["KR"][0].PlayerId);
        }

        [Fact]
        public void ToFormattedString_WithMultiplePositions_ShouldFormatCorrectly()
        {
            // Arrange
            var depthChart = DepthChartFactory.CreateDepthChart(SportType.NFL);
            depthChart.AddPlayerToDepthChart(_bob, "WR");
            depthChart.AddPlayerToDepthChart(_alice, "QB");
            depthChart.AddPlayerToDepthChart(_charlie, "WR", 1);

            // Act
            var result = depthChart.GetFullDepthChart();
            var formatted = string.Join(", ", result.Select(kv => kv.ToFormattedString()));

            // Assert
            Assert.Contains("WR: [1, 3]", formatted);
            Assert.Contains("QB: [2]", formatted);
        }

        [Fact]
        public void GetPlayersUnderPlayerInDepthChart_WithMultiplePositions_ShouldOnlyConsiderSpecifiedPosition()
        {
            // Arrange
            var depthChart = DepthChartFactory.CreateDepthChart(SportType.NFL);
            depthChart.AddPlayerToDepthChart(_bob, "WR", 0);
            depthChart.AddPlayerToDepthChart(_charlie, "WR", 1);
            depthChart.AddPlayerToDepthChart(_alice, "QB", 0);
            depthChart.AddPlayerToDepthChart(_david, "QB", 1);

            // Act
            var playersUnderBob = depthChart.GetPlayersUnderPlayerInDepthChart(_bob, "WR");
            var playersUnderAlice = depthChart.GetPlayersUnderPlayerInDepthChart(_alice, "QB");

            // Assert
            Assert.Single(playersUnderBob);
            Assert.Equal(_charlie.PlayerId, playersUnderBob[0].PlayerId);
            Assert.Single(playersUnderAlice);
            Assert.Equal(_david.PlayerId, playersUnderAlice[0].PlayerId);
        }

        [Fact]
        public void AddPlayerToDepthChart_PlayerInMultiplePositions_ShouldMaintainAllPositions()
        {
            // Arrange
            var depthChart = DepthChartFactory.CreateDepthChart(SportType.NFL);
            
            // Add Bob to multiple positions
            depthChart.AddPlayerToDepthChart(_bob, "WR", 0);
            depthChart.AddPlayerToDepthChart(_bob, "KR", 0);
            
            // Update Bob's position in one chart
            depthChart.AddPlayerToDepthChart(_alice, "WR", 0); // Push Bob down in WR
            
            // Act
            var result = depthChart.GetFullDepthChart();
            
            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(2, result["WR"].Count);
            Assert.Single(result["KR"]);
            
            // Bob should be pushed down in WR but still first in KR
            Assert.Equal(_alice.PlayerId, result["WR"][0].PlayerId);
            Assert.Equal(_bob.PlayerId, result["WR"][1].PlayerId);
            Assert.Equal(_bob.PlayerId, result["KR"][0].PlayerId);
        }

        [Fact]
        public void AddPlayersToMultiplePositions_ThenRemoveAll_ShouldLeaveEmptyChart()
        {
            // Arrange
            var depthChart = DepthChartFactory.CreateDepthChart(SportType.NFL);
            
            // Add players to various positions
            depthChart.AddPlayerToDepthChart(_bob, "WR");
            depthChart.AddPlayerToDepthChart(_alice, "QB");
            depthChart.AddPlayerToDepthChart(_charlie, "RB");
            depthChart.AddPlayerToDepthChart(_david, "TE");
            depthChart.AddPlayerToDepthChart(_eve, "KR");
            
            // Act - remove all players
            depthChart.RemovePlayerFromDepthChart(_bob, "WR");
            depthChart.RemovePlayerFromDepthChart(_alice, "QB");
            depthChart.RemovePlayerFromDepthChart(_charlie, "RB");
            depthChart.RemovePlayerFromDepthChart(_david, "TE");
            depthChart.RemovePlayerFromDepthChart(_eve, "KR");
            
            var result = depthChart.GetFullDepthChart();
            
            // Assert
            Assert.Empty(result);
        }
        
        [Fact]
        public void GetPlayersUnderPlayer_PlayerNotInSpecifiedPosition_ShouldReturnEmpty()
        {
            // Arrange
            var depthChart = DepthChartFactory.CreateDepthChart(SportType.NFL);
            depthChart.AddPlayerToDepthChart(_bob, "WR");
            depthChart.AddPlayerToDepthChart(_alice, "QB");
            depthChart.AddPlayerToDepthChart(_charlie, "QB", 1);
            
            // Act - Bob is in WR, not QB
            var playersUnder = depthChart.GetPlayersUnderPlayerInDepthChart(_bob, "QB");
            
            // Assert
            Assert.Empty(playersUnder);
        }

        [Fact]
        public void UpdatePlayerPositionDepth_AcrossMultiplePositions_ShouldWorkIndependently()
        {
            // Arrange
            var depthChart = DepthChartFactory.CreateDepthChart(SportType.NFL);
            
            // Add multiple players to multiple positions
            depthChart.AddPlayerToDepthChart(_alice, "QB", 0);
            depthChart.AddPlayerToDepthChart(_david, "QB", 1);
            depthChart.AddPlayerToDepthChart(_bob, "WR", 0);
            depthChart.AddPlayerToDepthChart(_charlie, "WR", 1);
            depthChart.AddPlayerToDepthChart(_eve, "TE", 0);
            
            // Act - update player positions independently
            depthChart.AddPlayerToDepthChart(_alice, "WR", 2); // Add Alice to end of WR list
            depthChart.AddPlayerToDepthChart(_bob, "KR", 0);   // Add Bob to KR
            depthChart.AddPlayerToDepthChart(_david, "QB", 0); // Move David to QB starter
            
            var result = depthChart.GetFullDepthChart();
            
            // Assert
            // Check QB has been reordered
            Assert.Equal(2, result["QB"].Count);
            Assert.Equal(_david.PlayerId, result["QB"][0].PlayerId);
            Assert.Equal(_alice.PlayerId, result["QB"][1].PlayerId);
            
            // Check WR has Alice at the end
            Assert.Equal(3, result["WR"].Count);
            Assert.Equal(_bob.PlayerId, result["WR"][0].PlayerId);
            Assert.Equal(_charlie.PlayerId, result["WR"][1].PlayerId);
            Assert.Equal(_alice.PlayerId, result["WR"][2].PlayerId);
            
            // Check Bob is in KR
            Assert.Single(result["KR"]);
            Assert.Equal(_bob.PlayerId, result["KR"][0].PlayerId);
            
            // Check TE is unchanged
            Assert.Single(result["TE"]);
            Assert.Equal(_eve.PlayerId, result["TE"][0].PlayerId);
        }
    }
}