// (c) D. Sawyer <damiensawyer@gmail.com> 2025

using DepthChart.Interfaces;

namespace DepthChart.Models;

public class NflPositions : ISportPositions
{
  public static IReadOnlySet<string> ValidPositions => new HashSet<string>
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