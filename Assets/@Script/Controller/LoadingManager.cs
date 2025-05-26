using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public Slider loadingBar;
    public Text loadingText;
    public float fakeDuration = 3.0f;

    private void Start()
    {
        Screen.SetResolution(540, 960, false);
        loadingBar.value = 0;
        loadingText.text = "Loading... 0%";
        StartCoroutine(LoadSceneAsync("StartScene"));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        float timer = 0f;

        while (!op.isDone)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / fakeDuration);
            loadingBar.value = progress;
            loadingText.text = $"Loading... {Mathf.RoundToInt(progress * 100)}%";

            if (progress >= 1f)
            {
                yield return new WaitForSeconds(0.5f);
                op.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
