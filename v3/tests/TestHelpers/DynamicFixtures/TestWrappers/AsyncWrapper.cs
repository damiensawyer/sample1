// (c) D.Sawyer <damiensawyer@gmail.com> 2025

namespace TestHelpers.DynamicFixtures.TestWrappers;

/// <summary>
/// Used to wrap services which need async startup called
/// </summary>
public class AsyncWrapper<TService>(TService service) : ITestServiceWrapper<TService>
{
  public Guid Id { get; }
  public TService Service { get; } = service;
  public Task Setup { get; } = Task.CompletedTask;
}


