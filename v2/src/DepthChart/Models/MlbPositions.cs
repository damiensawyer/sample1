// (c) D. Sawyer <damiensawyer@gmail.com> 2025

using DepthChart.Interfaces;

namespace DepthChart.Models;

public class MlbPositions : ISportPositions
{
  public static IReadOnlySet<string> ValidPositions { get; } = new HashSet<string>
  {
    "SP",
    "RP",
    "C",
    "1B",
    "2B",
    "3B",
    "SS",
    "LF",
    "RF",
    "CF",
    "DH"
  };
}