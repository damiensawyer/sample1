// (c) D.Sawyer <damiensawyer@gmail.com> 2025

namespace TestHelpers.DynamicFixtures.TestWrappers;

/// <summary>
/// This is so that we don't have to call .setupasync() in each test.
/// possibly a bit hacky... could just make people call it (and a teardown method)
/// </summary>
/// <param name="service"></param>
/// <typeparam name="TService"></typeparam>
public abstract class AutoSetupTestWrapper<TService>(TService service) : ITestServiceWrapper<TService>
{
  private readonly SemaphoreSlim _setupLock = new(1, 1);
  private bool _setupRun;
  public TService Service => GetServiceAsync().GetAwaiter().GetResult();
  public abstract Task Setup { get; }
  public Guid Id { get; } = Guid.NewGuid();

  private async Task<TService> GetServiceAsync()
  {
    await EnsureSetupAsync();
    return service;
  }

  private async Task EnsureSetupAsync()
  {
    if (this._setupRun) return;
    await this._setupLock.WaitAsync();
    try
    {
      if (!this._setupRun)
      {
        await this.Setup;
        this._setupRun = true;
      }
    }
    finally
    {
      this._setupLock.Release();
    }
  }
}
