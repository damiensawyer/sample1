// (c) D.Sawyer <damiensawyer@gmail.com> 2025

namespace TestHelpers.DynamicFixtures.TestWrappers;

public class TestWrapper<TService>(TService service) : ITestServiceWrapper<TService>
{
  public Guid Id { get; } = Guid.NewGuid();
  public TService Service { get; } = service;
  public Task Setup { get; } = Task.CompletedTask;
  public Task Teardown { get; } = Task.CompletedTask;
}
