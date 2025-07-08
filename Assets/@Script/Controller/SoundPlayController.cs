using Cysharp.Threading.Tasks;
using UnityEngine;

public class SoundPlayController : MonoBehaviour
{
    public bool IsBGM = false;
    AudioSource audioSource;
    public void Init()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = IsBGM;
        audioSource.playOnAwake = false;
        if (IsBGM)
            DontDestroyOnLoad(gameObject);
    }
    public async void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();

        if (IsBGM)
            return;

        await WaitTilAudioEnd();
        Destroy(gameObject);
    }
    async UniTask WaitTilAudioEnd()
    {
        while (audioSource.isPlaying)
            await UniTask.Yield();
    }
}
