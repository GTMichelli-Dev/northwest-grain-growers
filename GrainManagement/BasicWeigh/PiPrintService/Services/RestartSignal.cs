namespace WebPrintService.Services;

public class RestartSignal
{
    private readonly ManualResetEventSlim _signal = new(false);

    public void TriggerRestart() => _signal.Set();
    public bool WaitForRestart(TimeSpan timeout) => _signal.Wait(timeout);
    public void Reset() => _signal.Reset();
}
