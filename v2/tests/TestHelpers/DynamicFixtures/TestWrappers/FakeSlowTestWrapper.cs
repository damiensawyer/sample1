// (c) D. Sawyer <damiensawyer@gmail.com> 2025

namespace TestHelpers.DynamicFixtures.TestWrappers;

/// <summary>
///   Just a fake demo wrapper showing that we can do async work on startup.
///   In reality, you might use this to configure a database or setup another integration type test
/// </summary>
/// <param name="service"></param>
/// <param name="delayMilliseconds"></param>
/// <typeparam name="TService"></typeparam>
public class FakeSlowTestWrapper<TService>(TService service, int delayMilliseconds = 100) : AutoSetupTestWrapper<TService>(service)
{
  public override Task Setup { get; } =  Task.Delay(delayMilliseconds);
}
