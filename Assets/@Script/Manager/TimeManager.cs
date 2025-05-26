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

    public Action<float> moneyAction;
    public Action<float, float> hpAction;
    private float _money;
    public float Money
    {
        get { return _money; }
        set
        {
            _money = value;
            moneyAction.Invoke(value);
        }
    }
    public float Max { get; set; }
    private float curHp;
    public float CurHp
    {
        get { return curHp; }
        set
        {
            curHp = value;
            hpAction.Invoke(value, Max);
        }
    }
    public void SetHp(float hp)
    {
        Max = hp;
        curHp = hp;
    }
    public TimeManager(float growthRatePerSecond = 0.007f)
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
        float moneyTimer = 0f;

        try
        {
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(1000, token);

                elapsedTime += 1f;
                moneyTimer += 1f;
                healthMultiplier = 1f + (elapsedTime * growthRate);

                if (moneyTimer >= 10f)
                {
                    Money += 50;
                    moneyTimer = 0f;
                }
            }
        }
        catch (TaskCanceledException)
        {
            // 중단 시 아무것도 안 함
        }
    }
}
