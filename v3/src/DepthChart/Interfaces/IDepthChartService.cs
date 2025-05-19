// (c) D. Sawyer <damiensawyer@gmail.com> 2025

using DepthChart.Models;

namespace DepthChart.Interfaces;

public interface IDepthChartService
{
  void AddPlayerToDepthChart(Player player, string position, int? positionDepth = null);
  List<Player> GetPlayersUnderPlayerInDepthChart(Player player, string position);
  Dictionary<string, List<Player>> GetFullDepthChart();
  void RemovePlayerFromDepthChart(Player player, string position);
}