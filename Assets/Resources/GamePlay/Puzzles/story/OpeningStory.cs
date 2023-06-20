using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class OpeningStory : MonoBehaviour
{
    // Start is called before the first frame update
    public MyDialogueBox dialogue;
    public TMP_Text text_middle;
    public RectTransform triangle;

    Vector3 triangleOrigin;
    public RectTransform upperEyelid, lowerEyelid;
    public Image sceneOfWakeUp;
    public MyPanel skipButtonPanel;
    bool canSkip;
    void Awake()
    {
    }
    void Start()
    {
        StartCoroutine(Story());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FinishPlaying()
    {
        PlayerPrefs.SetInt("FirstPlay", 1);
        SceneSwitcher.SwitchTo("Menu");
    }
    IEnumerator Story()
    {
        canSkip = PlayerPrefs.GetInt("FirstPlay", 0) != 0;
        if (canSkip) {
            skipButtonPanel.GetComponentInChildren<TMP_Text>().color = new Color(1,1,1,1);
            skipButtonPanel.Appear();
        }
        else skipButtonPanel.Disappear();

        text_middle.color = new Color(1,1,1,1);
        text_middle.GetComponent<CanvasGroup>().alpha = 0;
        triangle.GetComponent<CanvasGroup>().alpha = 0;
        sceneOfWakeUp.color = new Color(1,1,1,1);
        yield return new WaitForSeconds(0.5f);
        //SceneSwitcher.SwitchTo("Menu");

        text_middle.text = "AD 3023";
        yield return text_middle.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
        yield return new WaitForSeconds(2f);
        yield return text_middle.GetComponent<CanvasGroup>().DOFade(0, 0.5f);
        yield return new WaitForSeconds(1f);

        triangleOrigin = triangle.anchoredPosition = new Vector2(0, -100);
        triangle.GetComponent<Image>().color = new Color(1,1,1,1);
        
        yield return BeforeWakeUp();
        AudioManager.PlayMusicByName("Opening");
        yield return new WaitForSeconds(3.4f);

        yield return BlinkEyes(30, 0.2f);
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 2; i++)
        {
            yield return BlinkEyes(40f, 0.1f);
        }
        yield return new WaitForSeconds(0.5f);
        yield return BlinkEyes(80, 0.2f);
        yield return BlinkEyes(100, 0.2f);
        yield return new WaitForSeconds(0.3f);
        yield return OpenEyes(300, 0.7f);
        yield return OpenEyes(100, 0.2f);
        yield return OpenEyes(540, 0.1f);

        text_middle.color = new Color(0,0,0,1);
        triangle.GetComponent<Image>().color = new Color(0,0,0,1);
        if (canSkip) skipButtonPanel.GetComponentInChildren<TMP_Text>().color = new Color(0,0,0,1);
        yield return PopDialogue("Meow Meow Meow~", 0.2f);
        yield return PopDialogue("Meow? Meow.", 0.2f);
        yield return PopDialogue("Meow! Meow! Meow~", 0.2f);
        
        yield return FadeDialogue("I found myself surrounded by cats.", 0.2f);
        yield return FadeDialogue("They picked me up in the forest.", 0.2f);
        yield return FadeDialogue("They gave me food and water.", 0.2f);
        yield return FadeDialogue("They shared shelter with me.", 0.2f);
        yield return FadeDialogue("They saved my life.", 0.2f);

        yield return sceneOfWakeUp.DOFade(0, 1f).WaitForCompletion();

        yield return FadeDialogue("After quite a few months, I learned some Meow Language.", 0.2f);
        yield return FadeDialogue("And I finally realized that I had travelled to the future of future,", 0.2f);
        yield return FadeDialogue("where human beings had become history from a long, long time ago.", 0.2f);
        yield return FadeDialogue("It had been almost impossible to rewind the time.", 0.2f);
        yield return FadeDialogue("So I settled down. Surprisingly comfortable.", 0.2f);
        yield return FadeDialogue("I had free food, free water, free bed and--", 0.2f);
        yield return PopDialogue("of course free cat cafe.", 0.2f);
        yield return FadeDialogue("Acutally the blue planet itself had become a natural cat cafe.", 0.2f);
        yield return FadeDialogue("I had nothing to complain about except that I was too boring.", 0.2f);
        yield return PopDialogue("I must work.", 0.2f);
        yield return PopDialogue("Right, I must work to find the meaning of my life.", 0.2f);


        yield return FadeDialogue("Finally I met Dr.Bubble, working as his assistant.", 0.2f);
        yield return FadeDialogue("Nekode -- Neko's code (neko = cat),", 0.2f);
        yield return FadeDialogue("is one of his strange projects.", 0.2f);
        yield return FadeDialogue("I like the idea anyway.", 0.2f);
        yield return FadeDialogue("So I even archived the project as an interactive diary.", 0.2f);

        yield return FadeDialogue("This is where this game comes from, my dear friend.", 0.2f);
        yield return FadeDialogue("I don't know what tense should I use when you are reading these words,", 0.2f);
        yield return FadeDialogue("But I hope this interactive diary could bring you some fun.", 0.2f);
        yield return FadeDialogue("Have a good time, whether you are from past, present, or future.", 0.2f);

        FinishPlaying();
    }
    IEnumerator OpenEyes(float width, float duration)
    {
        upperEyelid.DOAnchorPos(new Vector2(0, width), duration);
        lowerEyelid.DOAnchorPos(new Vector2(0, -width), duration);
        yield return new WaitForSeconds(duration);
    }
    IEnumerator CloseEyes(float duration)
    {
        upperEyelid.DOAnchorPos(new Vector2(0, 0), duration);
        lowerEyelid.DOAnchorPos(new Vector2(0, -0), duration);
        yield return new WaitForSeconds(duration);
    }
    IEnumerator BlinkEyes(float width, float duration)
    {
        yield return OpenEyes(width, duration);
        yield return CloseEyes(duration);
    } 
    IEnumerator BeforeWakeUp()
    {
        yield return PopDialogue("No, it's not AD 3023 now!!", 0.2f);
        yield return PopDialogue("It's actually ......", 0.2f);
        yield return PopDialogue("I don't know.", 0.2f);
        yield return PopDialogue("It was supposed to be AD 3033 when I arrive.", 0.2f);
        yield return PopDialogue("But the time machine seems to have encountered some problems.", 0.2f);
        yield return PopDialogue("So I don't know where I am now.", 0.2f);
        yield return PopDialogue("Or to be more precise, when I am now......", 0.2f);
    }
    IEnumerator PopDialogue(string content, float waitsec, bool waitForInput = true)
    {
        text_middle.text = content;
        text_middle.GetComponent<CanvasGroup>().alpha = 0;
        text_middle.GetComponent<CanvasGroup>().DOFade(1, 0.05f);
        yield return text_middle.transform.DOScale(Vector3.one * 1.05f, 0.07f).WaitForCompletion();
        yield return text_middle.transform.DOScale(Vector3.one * 0.96f, 0.07f).WaitForCompletion();
        yield return text_middle.transform.DOScale(Vector3.one * 1.03f, 0.07f).WaitForCompletion();
        yield return text_middle.transform.DOScale(Vector3.one, 0.07f).WaitForCompletion();
        yield return new WaitForSeconds(waitsec);
        if (waitForInput)
        {
            yield return WaitForInput();
        }
        yield return text_middle.GetComponent<CanvasGroup>().DOFade(0, 0.2f).WaitForCompletion();
    }

    IEnumerator FadeDialogue(string content, float waitsec, bool waitForInput = true)
    {
        text_middle.text = content;
        text_middle.GetComponent<CanvasGroup>().alpha = 0;
        text_middle.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
        yield return new WaitForSeconds(waitsec);
        if (waitForInput)
        {
            yield return WaitForInput();
        }
        yield return text_middle.GetComponent<CanvasGroup>().DOFade(0, 0.2f).WaitForCompletion();
    }


    IEnumerator WaitForInput()
    {
        triangle.anchoredPosition = triangleOrigin + new Vector3(0, 5, 0);
        triangle.DOAnchorPos(triangleOrigin - new Vector3(0, 10, 0), 0.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        triangle.GetComponent<CanvasGroup>().DOFade(1, 0.1f);
        while (!Input.anyKeyDown) yield return null;
        triangle.GetComponent<CanvasGroup>().DOFade(0, 0.05f);
        triangle.transform.DOKill();
    }
}
