// (c) D. Sawyer <damiensawyer@gmail.com> 2025

using System.Collections;
using TestHelpers.DynamicFixtures.TestWrappers;
using Xunit.Abstractions;

namespace TestHelpers.Helpers;

/// <summary>
///     To do:
///     Tear Down
///     Pass in logger --> not sure if you can do this due to XUnit limitations
/// </summary>
/// <typeparam name="TService"></typeparam>
[DamienToDo("Tear Down Handler")]
[DamienToDo("Pass in logger")]
public abstract class FixtureLevelDataProviderBase<TService> : IEnumerable<object[]>
{
  private readonly List<object[]> _data;

  protected FixtureLevelDataProviderBase(List<ITestServiceWrapper<TService>> serviceWrappers)
  {
    _data = [];
    foreach (var wrapper in serviceWrappers)
    {
      wrapper.Setup.Wait(); // I suspect that we have to block here due to XUnit
      _data.Add([wrapper]);
    }
  }


  public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
