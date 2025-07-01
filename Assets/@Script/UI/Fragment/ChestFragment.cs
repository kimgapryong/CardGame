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

    public void Init()
    {
        BindObject(typeof(Objects));
    }

    public void SetInfo(ChestData chestData)
    {
        Manager.Resource.LoadAsync<Sprite>(chestData.SpritePath, (sprite) =>
        {
            GetObject((int)Objects.Image).GetComponent<Image>().sprite = sprite;
        });

        GetObject((int)Objects.NameText).GetComponent<Text>().text = chestData.ChestName;

        Debug.Log(GetObject((int)Objects.PriceText));
        GetObject((int)Objects.PriceText).GetComponent<Text>().text = $"{chestData.Price}";
    }

}
