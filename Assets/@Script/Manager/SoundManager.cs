using UnityEngine;

public class SoundManager
{
    SoundPlayController BGMPlayer = null;
    public void PlaySFX(string key)
    {
        Manager.Resource.Instantiate("soundPlayer", null, (go) =>
        {
            SoundPlayController soundPlayer = go.GetComponent<SoundPlayController>();
            soundPlayer.Init();

            Manager.Resource.LoadAsync<AudioClip>(key, (clip) =>
            {
                soundPlayer.PlaySound(clip);
            });
        });
    }

    public void PlayBGM(string key)
    {
        if (BGMPlayer == null)
        {
            Manager.Resource.Instantiate("soundPlayer", null, (go) =>
            {
                BGMPlayer = go.GetOrAddComponent<SoundPlayController>();
                BGMPlayer.IsBGM = true;
                BGMPlayer.Init();

                Manager.Resource.LoadAsync<AudioClip>(key, (clip) =>
                {
                    Debug.Log(key);
                    BGMPlayer.PlaySound(clip);
                });
            });
            return;
        }
        Manager.Resource.LoadAsync<AudioClip>(key, (clip) =>
        {
            Debug.Log(key);
            BGMPlayer.PlaySound(clip);
        });
    }
}
