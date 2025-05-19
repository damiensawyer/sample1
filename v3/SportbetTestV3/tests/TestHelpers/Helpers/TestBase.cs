// (c) D. Sawyer <damiensawyer@gmail.com> 2025

using Xunit.Abstractions;

namespace TestHelpers.Helpers;

public class TestBase(ITestOutputHelper output)
{
  protected readonly DualOutputHelper Output = new(output);
}
