using System;
using System.Threading;
using System.Threading.Tasks;

public class TimeManager
{
    private float elapsedTime = 0f;
    private float healthMultiplier = 1f;
    private readonly float growthRate;
    private bool isRunning = false;
    private CancellationTokenSource cancellationTokenSource;

    public TimeManager(float growthRatePerSecond = 0.01f)
    {
        this.growthRate = growthRatePerSecond;
    }

    public void Start()
    {
        if (isRunning) return;
        isRunning = true;
        cancellationTokenSource = new CancellationTokenSource();
        RunAsync(cancellationTokenSource.Token);
    }

    public void Stop()
    {
        if (!isRunning) return;
        isRunning = false;
        cancellationTokenSource?.Cancel();
    }

    public float GetHealthMultiplier()
    {
        return healthMultiplier;
    }

    private async void RunAsync(CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(1000, token);
                elapsedTime += 1f;
                healthMultiplier = 1f + (elapsedTime * growthRate);
            }
        }
        catch (TaskCanceledException)
        {
        }
    }
}
