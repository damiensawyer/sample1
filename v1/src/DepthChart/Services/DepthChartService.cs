using DepthChart.Interfaces;
using DepthChart.Models;

namespace DepthChart.Services;

public class DepthChartService(ISportPositions sportPositions) 
{
    private readonly Dictionary<string, List<Player>> _depthCharts = new();
    private readonly ISportPositions _sportPositions = sportPositions ?? throw new ArgumentNullException(nameof(sportPositions));

    public void AddPlayerToDepthChart(Player player, string position, int? positionDepth = null)
    {
        if (!this._sportPositions.ValidPositions.Contains(position))
            throw new ArgumentException($"Invalid position for this sport: {position}");

        if (!this._depthCharts.TryGetValue(position, out var chart))
        {
            chart = [];
            this._depthCharts[position] = chart;
        }

        // Remove player if already in this position's chart
        var existingIndex = chart.FindIndex(p => p.PlayerId == player.PlayerId);
        if (existingIndex >= 0)
            chart.RemoveAt(existingIndex);

        // Add at specified position or end
        if (positionDepth.HasValue)
        {
            var insertAt = Math.Min(positionDepth.Value, chart.Count);
            chart.Insert(insertAt, player);
        }
        else
        {
            chart.Add(player);
        }
    }

    public List<Player> GetPlayersUnderPlayerInDepthChart(Player player, string position)
    {
        if (!this._depthCharts.TryGetValue(position, out var chart))
            return [];

        var playerIndex = chart.FindIndex(p => p.PlayerId == player.PlayerId);
        if (playerIndex < 0 || playerIndex >= chart.Count - 1)
            return [];

        return [..chart.Skip(playerIndex + 1)];
    }

    public Dictionary<string, List<Player>> GetFullDepthChart()
    {
        Dictionary<string, List<Player>> result = new();

        foreach (var position in this._depthCharts)
            result[position.Key] = [..position.Value];

        return result;
    }

    public void RemovePlayerFromDepthChart(Player player, string position)
    {
        if (!this._depthCharts.TryGetValue(position, out var chart))
            return;

        var index = chart.FindIndex(p => p.PlayerId == player.PlayerId);
        if (index < 0) return;
        chart.RemoveAt(index);

        // Remove the Position if this was the last entry
        if (!this._depthCharts[position].Any())
            this._depthCharts.Remove(position);
    }
}