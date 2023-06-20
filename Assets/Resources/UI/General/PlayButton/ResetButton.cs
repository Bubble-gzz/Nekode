using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResetButton : MyButton
{
    // Start is called before the first frame update

    static ResetButton Instance;
    static public List<Coroutine> coroutinesToBeKilledOnReset = new List<Coroutine>();
    Image icon;
    public Transform bubble;
    bool bubbleIsShowing;
    protected override void Awake()
    {
        Instance = this;
        base.Awake();
        icon = GetComponent<Image>();
        bubble = transform.Find("Bubble");
        bubble.gameObject.SetActive(false);
        bubbleIsShowing = false;
    }
    override protected void Start()
    {
        icon.enabled = false;
        icon.material.color = new Color(1,1,1,1);
        onClick.AddListener(OnClick);
        Global.onGameStateChanged.AddListener(OnGameStateChanged);
    }

    void OnGameStateChanged()
    {
        if (Global.gameState == Global.GameState.Editing)
        {
            icon.enabled = false;
        }
        else {
            icon.enabled = true;
        }
    }
    public void OnClick()
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

        coroutinesToBeKilledOnReset.RemoveAll(item => item == null);
        foreach(var coroutine in coroutinesToBeKilledOnReset.ToArray()) {
            coroutinesToBeKilledOnReset.Remove(coroutine);
            StopCoroutine(coroutine);
        }
        Global.SetGameState(Global.GameState.Editing);
        GameUIManager.UnFoldEditUI();
        GameMessage.OnResetGridState.Invoke();
        GameMessage.OnReset.Invoke();
        if (Global.gameMode == Global.GameMode.Test)
            PuzzleLogic.curTestCase = 1;
    }
    void PopBubble()
    {
        if (bubbleIsShowing) return;
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
        if (Instance == null) return;
        if (Instance.isActiveAndEnabled)
            Instance.PopBubble();
    }
}
