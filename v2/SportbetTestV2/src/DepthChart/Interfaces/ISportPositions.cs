// (c) D. Sawyer <damiensawyer@gmail.com> 2025

namespace DepthChart.Interfaces;

public interface ISportPositions
{
  static abstract IReadOnlySet<string> ValidPositions { get; }
}