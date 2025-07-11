using System;
using System.Linq;
using System.Net.Sockets;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class WarningText_Pop : UI_Popup
{
    enum Text
    {
        Warning,
    }
    public void Warning(string message)
    {
        BindText(typeof(Text));
        GetText((int)Text.Warning).text = message;
        FadeOut().Forget();
    }
    async UniTaskVoid FadeOut()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        Destroy(gameObject);
    }
}
