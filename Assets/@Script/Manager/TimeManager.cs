using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TimeManager
{
    private float elapsedTime = 0f;
    private float healthMultiplier = 1f;
    private readonly float growthRate;
    private bool isRunning = false;
    private CancellationTokenSource cancellationTokenSource;

    public Action<float> moneyAction;
    public Action<float, float> hpAction;
    public Action dieAction;

    private float _money;
    public float Money
    {
        get { return _money; }
        set
        {
            
            _money = value;
            moneyAction?.Invoke(value);
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
            hpAction?.Invoke(value, Max);
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
        UnityEngine.Debug.Log("게임 시작");
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

    
    public int GetSpawnMultiplier()
    {
        return (int)Mathf.Pow(3, (int)(elapsedTime / 60f));
    }

    private async void RunAsync(CancellationToken token)
    {
        float moneyTimer = 0f;
        int moneyCycleCount = 0;

        try
        {
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(1000, token);

                elapsedTime += 1f;
                moneyTimer += 1f;

                
                float baseGrowth = growthRate; // ex: 0.007f
                float growthStep = (float)Math.Pow(3, (int)(elapsedTime / 60f)); // 1분마다 3배
                float increase = baseGrowth * growthStep;
                healthMultiplier += increase;

                
                if (moneyTimer >= 10f)
                {
                    moneyCycleCount++;
                    Money += 10 * moneyCycleCount;
                    moneyTimer = 0f;
                }
            }
        }
        catch (TaskCanceledException)
        {
            // 무시
        }
    }

    public void ResetAll()
    {
        Stop();
        moneyAction = null;
        hpAction = null;
        dieAction = null;
        elapsedTime = 0f;
        healthMultiplier = 1f;
        _money = 0f;
        curHp = 0f;
    }
}
