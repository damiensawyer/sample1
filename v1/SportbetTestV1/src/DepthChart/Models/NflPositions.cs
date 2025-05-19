using DepthChart.Interfaces;

namespace DepthChart.Models;

public class NflPositions : ISportPositions
{
    public IReadOnlySet<string> ValidPositions { get; } = new HashSet<string>
    {
        "QB",
        "WR",
        "RB",
        "TE",
        "K",
        "P",
        "KR",
        "PR"
    };
}