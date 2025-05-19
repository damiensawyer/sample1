// (c) D. Sawyer <damiensawyer@gmail.com> 2025

using DepthChart.Enums;
using DepthChart.Interfaces;
using DepthChart.Models;
using DepthChart.Services;

namespace DepthChart.Helpers;

public static class DepthChartFactory
{
  public static IDepthChartService CreateSingleThreadedDepthChart(SportType sportType) => sportType switch
  {
    SportType.NFL => new SingleThreadedDepthChart(NflPositions.ValidPositions),
    SportType.MLB => new SingleThreadedDepthChart(MlbPositions.ValidPositions),
    _ => throw new ArgumentOutOfRangeException(nameof(sportType))
  };
}