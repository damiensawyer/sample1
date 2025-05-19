// (c) D. Sawyer <damiensawyer@gmail.com> 2025

using DepthChart.Interfaces;
using DepthChart.Models;
using System.Collections.Concurrent;

namespace DepthChart.Services;

public class ConcurrentDepthChartService(IReadOnlySet<string> sportPositions) : IDepthChartService
{
  private readonly ConcurrentDictionary<string, List<Player>> _depthCharts = new(StringComparer.OrdinalIgnoreCase);
  private readonly HashSet<string> _sportPositions = new(sportPositions ?? throw new ArgumentNullException(nameof(sportPositions)), StringComparer.OrdinalIgnoreCase);
  private readonly object _syncLock = new();

  public void AddPlayerToDepthChart(Player player, string position, int? positionDepth = null)
  {
    if (!_sportPositions.Contains(position)) throw new ArgumentException($"Invalid position for this sport: {position}");

    _depthCharts.AddOrUpdate(position,
      // Add new list if key doesn't exist
      _ =>
      {
        var list = new List<Player>();
        AddPlayerToList(list, player, positionDepth);
        return list;
      },
      // Update existing list
      (_, existingList) =>
      {
        lock (_syncLock)
        {
          var listCopy = new List<Player>(existingList);
          AddPlayerToList(listCopy, player, positionDepth);
          return listCopy;
        }
      });
  }

  public List<Player> GetPlayersUnderPlayerInDepthChart(Player player, string position)
  {
    if (!_depthCharts.TryGetValue(position, out var chart)) return [];

    lock (_syncLock)
    {
      var playerIndex = chart.FindIndex(p => p.PlayerId == player.PlayerId);
      if (playerIndex < 0 || playerIndex >= chart.Count - 1) return [];

      return [.. chart.Skip(playerIndex + 1)];
    }
  }

  public Dictionary<string, List<Player>> GetFullDepthChart()
  {
    var result = new Dictionary<string, List<Player>>(StringComparer.OrdinalIgnoreCase);

    foreach (var position in _depthCharts)
      lock (_syncLock)
        result[position.Key] = [.. position.Value];

    return result;
  }

  public void RemovePlayerFromDepthChart(Player player, string position)
  {
    if (_depthCharts.TryGetValue(position, out var chart))
      lock (_syncLock)
      {
        var index = chart.FindIndex(p => p.PlayerId == player.PlayerId);
        if (index >= 0)
        {
          var newList = new List<Player>(chart);
          newList.RemoveAt(index);

          if (newList.Count == 0)
            _depthCharts.TryRemove(position, out _);
          else
            _depthCharts[position] = newList;
        }
      }
  }

  private static void AddPlayerToList(List<Player> list, Player player, int? positionDepth)
  {
    // Remove player if already in this position's chart
    var existingIndex = list.FindIndex(p => p.PlayerId == player.PlayerId);
    if (existingIndex >= 0) list.RemoveAt(existingIndex);

    // Add at specified position or end
    if (positionDepth.HasValue)
    {
      var insertAt = Math.Min(positionDepth.Value, list.Count);
      list.Insert(insertAt, player);
    }
    else
      list.Add(player);
  }
}
