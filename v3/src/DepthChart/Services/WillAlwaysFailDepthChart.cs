// (c) D. Sawyer <damiensawyer@gmail.com> 2025

using DepthChart.Interfaces;
using DepthChart.Models;

namespace DepthChart.Services;

public class WillAlwaysFailDepthChart : IDepthChartService
{
  public void AddPlayerToDepthChart(Player player, string position, int? positionDepth = null) => throw new NotImplementedException();

  public List<Player> GetPlayersUnderPlayerInDepthChart(Player player, string position) => throw new NotImplementedException();

  public Dictionary<string, List<Player>> GetFullDepthChart() => throw new NotImplementedException();

  public void RemovePlayerFromDepthChart(Player player, string position) => throw new NotImplementedException();
}