// (c) D.Sawyer <damiensawyer@gmail.com> 2025

namespace TestHelpers.DynamicFixtures.TestWrappers;

public interface ITestServiceWrapper<out TService>
{
  Guid Id { get; }

  public TService Service { get; }

  Task Setup { get; }
}
