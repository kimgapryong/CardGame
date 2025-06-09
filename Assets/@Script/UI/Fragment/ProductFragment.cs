using UnityEngine;

public class ProductFragment : UI_Base
{
    public enum Image
    {
        ProductImage
    }
    public enum Button
    {
        ProductFragment
    }
    public enum Text
    {
        ProductName,
        Price
    }
    public ProductData ProductData { get; set; }
    ResourceManager resourceManager;
    void Start()
    {
        resourceManager = new ResourceManager();
        BindImage(typeof(Image));
        BindButton(typeof(Button));
        BindText(typeof(Text));

        resourceManager.LoadAsync<Sprite>(ProductData.ProductSpriteKey, (sprite) =>
        {
            GetImage((int)Image.ProductImage).sprite = sprite;
        });
        GetText((int)Text.ProductName).text = ProductData.ProductName;
        GetText((int)Text.Price).text = ProductData.ProductName;

        GetButton((int)Button.ProductFragment).onClick.AddListener(() =>
        {
            switch (ProductData.Type)
            {
                case Define.ProductType.Card:
                    break;
                case Define.ProductType.Goods:
                    break;
            }
        });

    }
}
