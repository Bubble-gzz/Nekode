using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    static SceneSwitcher instance;
    CanvasGroup image;
    void Awake()
    {
        if (instance == null)
        {
            image = GetComponentInChildren<CanvasGroup>();
            image.alpha = 0;
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
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
    static public void SwitchTo(string sceneName)
    {
        if (sceneName == "ExitGame") {
            Application.Quit();
            Debug.Log("quit game");
        }
        else {
            if (instance) instance.StartCoroutine(instance._SwitchTo(sceneName));
            else SceneManager.LoadScene(sceneName);
        }
    }
    IEnumerator _SwitchTo(string sceneName)
    {
        yield return StartCoroutine(Fade(0, 1));
        yield return SceneManager.LoadSceneAsync(sceneName);
        FadeIn();
    }
}
