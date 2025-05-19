using DepthChart.Enums;
using DepthChart.Models;
using DepthChart.Services;

namespace DepthChart.Helpers;

public static class DepthChartFactory
{
    public static DepthChartService CreateDepthChart(SportType sportType) =>
        sportType switch
        {
            SportType.NFL => new DepthChartService(new NflPositions()),
            SportType.MLB => new DepthChartService(new MlbPositions()),
            _ => throw new ArgumentOutOfRangeException(nameof(sportType))
        };
}