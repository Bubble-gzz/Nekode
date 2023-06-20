using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayButton : MyButton
{
    // Start is called before the first frame update
    static PlayButton Instance;
    AudioSource sfx;
    [SerializeField]
    Sprite[] textures = new Sprite[2];
    enum Icon{
        Pause,
        Play
    }
    Image icon;
    public Transform bubble;
    bool bubbleIsShowing;
    override protected void Awake()
    {
        Instance = this;
        base.Awake();
        Global.gameState = Global.GameState.Editing;
        icon = GetComponent<Image>();
        sfx = GetComponent<AudioSource>();
        bubble = transform.Find("Bubble");
        bubble.gameObject.SetActive(false);
        bubbleIsShowing = false;
    }
    override protected void Start()
    {
        GetComponent<Image>().sprite = textures[(int)Icon.Play];
        onClick.AddListener(OnClick);
        Global.onGameStateChanged.AddListener(OnGameStateChanged);
        //rect = GetComponent<RectTransform>();
        // if (Global.currentPuzzleName == "你好世界")
        // {
        //     rect.anchoredPosition = new Vector2(Screen.width * 0.46f, Screen.height * 0.2f);
        // }
    }

    void OnGameStateChanged()
    {
        if (Global.gameState == Global.GameState.Playing)
        {
            icon.sprite = textures[(int)Icon.Pause];
        }
        else
        {
            icon.sprite = textures[(int)Icon.Play];
        }
    }

    void OnClick()
    {
        if (bubbleIsShowing)
        {
            bubbleIsShowing = false;
            Sequence seq = DOTween.Sequence();
            seq.Append(bubble.DOScale(new Vector2(1.05f, 1.05f), 0.05f));
            seq.Append(bubble.DOScale(new Vector2(0f, 0f), 0.1f));
            bubble.GetComponent<CanvasGroup>().DOFade(0, 0.1f);
            seq.onComplete = (()=>{bubble.gameObject.SetActive(false);});
        }
        if (Global.gameState == Global.GameState.Editing)
        {
            Global.grid.MapBackUp();
            GameUIManager.FoldEditUI();
            GameMessage.OnPlay.Invoke();
            if (sfx)
            {
                sfx.volume = AudioManager.sfxVolume;
                sfx.Play();
            }
        }
        if (Global.gameState == Global.GameState.Playing)
        {
            Global.SetGameState(Global.GameState.Paused);
        }
        else {
            Global.SetGameState(Global.GameState.Playing);
        }
    }
    void PopBubble()
    {
        bubbleIsShowing = true;
        bubble.gameObject.SetActive(true);
        bubble.GetComponent<CanvasGroup>().alpha = 0;
        Sequence seq = DOTween.Sequence();
        seq.Append(bubble.DOScale(new Vector2(1.2f, 0.8f), 0.1f));
        seq.Append(bubble.DOScale(new Vector2(0.9f, 1.1f), 0.1f));
        seq.Append(bubble.DOScale(new Vector2(1.05f, 0.95f), 0.1f));
        seq.Append(bubble.DOScale(new Vector2(1.0f, 1.0f), 0.1f));

        bubble.GetComponent<CanvasGroup>().DOFade(1, 0.2f);
        
        //seq.Append(bubble.DOScale());
    }
    static public void Hint()
    {
        if (Instance.isActiveAndEnabled && Global.gameState == Global.GameState.Editing)
            Instance.PopBubble();
    }
    static public void StopPlaying()
    {
        if (Global.gameState != Global.GameState.Playing) return;
        Global.SetGameState(Global.GameState.Paused);
    }
}
