using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;


public static class GetDataList
{
    public static List<T> GetList<T>(string path)
    {
        List<T> dataList = new List<T>();

        Manager.Resource.LoadAsync<TextAsset>(path, (dataLoader) =>
        {
            if (dataLoader == null)
                return;
            dataList = JsonConvert.DeserializeObject<List<T>>(dataLoader.text);
        });
        return dataList;
    }

    public static List<HeroData> GetHeroList()
    {
        return GetList<HeroData>("HeroData.json");
    }
    public static List<Skills> GetSkillList()
    {
        return GetList<Skills>("SkillData.json");
    }
    public static List<AnimationData> GetAnimList()
    {
        return GetList<AnimationData>("AnimData.json");
    }
}
