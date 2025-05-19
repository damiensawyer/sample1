namespace DepthChart.Interfaces;

public interface ISportPositions
{
    IReadOnlySet<string> ValidPositions { get; }
}