using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    CanvasGroup image;
    void Awake()
    {
        image = GetComponentInChildren<CanvasGroup>();
        image.alpha = 0;
    }
    void Start()
    {
        FadeIn();
    }
    void FadeIn()
    {
        StartCoroutine(Fade(1, 0));
    }
    void FadeOut()
    {
        StartCoroutine(Fade(0, 1));
    }
    IEnumerator Fade(float s, float t)
    {
        float progress = 0, speed = 5;
        while (progress + speed * Time.deltaTime < 1)
        {
            progress += speed * Time.deltaTime;
            image.alpha = Mathf.Lerp(s, t, progress);
            yield return null;
        }
        image.alpha = Mathf.Lerp(s, t, 1);
    }
    public void SwitchTo(string sceneName)
    {
        if (sceneName == "ExitGame") Application.Quit();
        StartCoroutine(_SwitchTo(sceneName));
    }
    IEnumerator _SwitchTo(string sceneName)
    {
        yield return StartCoroutine(Fade(0, 1));
        SceneManager.LoadScene(sceneName);
    }
}
