using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

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
            UnityEngine.Debug.Log(value);
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
        return (int)Mathf.Pow(2, (int)(elapsedTime / 120f));
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

                
                float baseGrowth = growthRate; // 0.007f 등
                float exponentFactor = Mathf.Exp(elapsedTime / 300f); // 5분마다 e배 증가
                float increase = baseGrowth * exponentFactor;
                healthMultiplier += increase;

                
                if (moneyTimer >= 10f)
                {
                    moneyCycleCount++;
                    Money += 20 * moneyCycleCount;
                    moneyTimer = 0f;
                }
            }
        }
        catch (TaskCanceledException)
        {
            // 정상 취소 시 무시
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
