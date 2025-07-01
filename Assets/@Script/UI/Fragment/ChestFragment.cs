using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ChestFragment : UI_Base
{
    public enum Objects
    {
        Image,
        NameText,
        PriceText,
    }
    ChestData _chestData;

    public override bool Init()
    {
        if(base.Init() == false)
            return false;

        Debug.Log(_chestData);
        BindObject(typeof(Objects));

        

        Manager.Resource.LoadAsync<Sprite>(_chestData.Sprite, (sprite) =>
        {
            GetObject((int)Objects.Image).GetComponent<Image>().sprite = sprite;
            GetObject((int)Objects.NameText).GetComponent<Text>().text = _chestData.ChestName;

            Debug.Log(GetObject((int)Objects.PriceText));
            GetObject((int)Objects.PriceText).GetComponent<Text>().text = $"{_chestData.Price}";
        });

        
        return true;
    }

    public void SetInfo(ChestData chestData)
    {
        _chestData = chestData;
    }

}
