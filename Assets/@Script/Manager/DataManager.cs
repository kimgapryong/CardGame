using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlacementData;

public interface ILoader<Key, Item>
{
    Dictionary<Key, Item> MakeDic();
    bool Validate();
}
public class DataManager 
{
    public Dictionary<int, HeroData> HeroDatas { get; private set; }
    public Dictionary<int, Skills> SkillDatas { get; private set; }
    public Dictionary<int, AnimationData> AnimDatas { get; private set; }
    public Dictionary<int, MonsterData> MonDatas { get; private set; }
    public Dictionary<int, ChestData> ChestDatas { get; private set; }
    public Dictionary<Define.HeroRating, UpgradeData> UpgradeDatas { get; private set; }
    public Dictionary<int, HeroUpgradeData> HeroUpgradeDatas { get; private set; }
    public Dictionary<Define.HeroRating, PriceData> PriceDatas { get; private set; }
    public Dictionary<Define.HeroRating, PlacementData> PlacementData { get; private set; }


    public void Init(Action onComplete = null)
    {
        var loadActions = new List<Action<Action>>()
    {
        (cb) => LoadJson<HeroLoader, int, HeroData>("HeroData.json", (loader) => { HeroDatas = loader.MakeDic(); cb(); }),
        (cb) => LoadJson<SkillLoader, int, Skills>("SkillData.json", (loader) => { SkillDatas = loader.MakeDic(); cb(); }),
        (cb) => LoadJson<AnimationLoader, int, AnimationData>("AnimData.json", (loader) => { AnimDatas = loader.MakeDic(); cb(); }),
        (cb) => LoadJson<MonsterLoader, int, MonsterData>("MonData.json", (loader) => { MonDatas = loader.MakeDic(); cb(); }),
        (cb) => LoadJson<UpgradeDataLoader, Define.HeroRating, UpgradeData>("UpgradeData.json", (loader) => { UpgradeDatas = loader.MakeDic(); cb(); }),
        (cb) => LoadJson<HeroUpgradeDataLoader, int, HeroUpgradeData>("HeroUpgradeData.json", (loader) => { HeroUpgradeDatas = loader.MakeDic(); cb(); }),
        (cb) => LoadJson<PriceDataLoader, Define.HeroRating, PriceData>("PriceData.json", (loader) => { PriceDatas = loader.MakeDic(); cb(); }),
        (cb) => LoadJson<ChestDataLoader, int, ChestData>("ChestData.json", (loader) => { ChestDatas = loader.MakeDic(); cb(); }),
        (cb) => LoadJson<PlacementDataLoader, Define.HeroRating, PlacementData>("PlacementData.json", (loader) => { PlacementData = loader.MakeDic(); cb(); })
    };

        int count = 0;
        int total = loadActions.Count;

        foreach (var load in loadActions)
        {
            load(() =>
            {
                count++;
                if (count == total)
                    onComplete?.Invoke();
            });
        }
    }
    void LoadJson<Loader, Key, Value>(string key, Action<Loader> callback) where Loader : ILoader<Key, Value>
    {
        Manager.Resource.LoadAsync<TextAsset>(key, (textAsset) =>
        {
            //Loader loader = JsonConvert.DeserializeObject<Loader>(textAsset.text);
            Loader loader = JsonUtility.FromJson<Loader>(textAsset.text);
            callback?.Invoke(loader);
        });
    }
    public bool Loaded()
    {
        if (HeroDatas == null)
            return false;
        if (SkillDatas == null)
            return false;
        if (AnimDatas == null)
            return false;
        if (MonDatas == null)
            return false;

            

        return true;
    }
   
}
