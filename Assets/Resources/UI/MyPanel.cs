using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MyPanel : MonoBehaviour
{
    // Start is called before the first frame update
    public enum Style{
        Fade,
        Fold,
        Passby,
        Pop
    }
    public Style style;
    public float tweenTimeUnit;
    CanvasGroup canvasGroup;
    public GameObject content;
    Sequence animationSequence;
    [HideInInspector]
    public bool showing;
    public bool appearOnStart;
    public Vector2 posWhenHide, posWhenShow;
    RectTransform rect;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        showing = false;
        rect = GetComponent<RectTransform>();
    }
    void Start()
    {
        if (appearOnStart) Appear();
        else {
            canvasGroup.alpha = 0;
            if (style == Style.Fold)
            {
                rect.anchoredPosition = posWhenHide;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Appear();
        }
    }
    public void Appear()
    {
        if (showing) {
            Debug.Log("repetitive calling of Appear()");
            return;
        }
        showing = true;
        animationSequence?.Kill();
        canvasGroup.DOKill();
        transform.DOKill();
        content?.SetActive(true);
        animationSequence = DOTween.Sequence();
        if (style == Style.Pop) {
            canvasGroup.alpha = 0;
            transform.localScale = Vector3.zero;
            canvasGroup.DOFade(1, tweenTimeUnit * 0.5f);
            animationSequence.Append(transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), tweenTimeUnit * 0.2f).SetEase(Ease.OutQuad));
            animationSequence.Append(transform.DOScale(new Vector3(0.95f, 0.95f, 0.95f), tweenTimeUnit * 0.1f));
            animationSequence.Append(transform.DOScale(new Vector3(1f, 1f, 1f), tweenTimeUnit * 0.05f));
        }
        else if (style == Style.Fold) {
            canvasGroup.alpha = 1;
            rect.anchoredPosition = posWhenHide;
            animationSequence.Append(rect.DOAnchorPos(posWhenShow, 0.3f).SetEase(Ease.OutCubic));
        }
    }
    public void Disappear(bool destroy = false)
    {
        if (!showing) {
            Debug.Log("repetitive calling of Disappear()");
            return;
        }
        showing = false;
        animationSequence?.Kill();
        animationSequence = DOTween.Sequence();
        if (style == Style.Pop) {
            canvasGroup.DOFade(0, tweenTimeUnit * 0.5f);
            animationSequence.Append(transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), tweenTimeUnit * 0.1f));
            animationSequence.Append(transform.DOScale(new Vector3(0f, 0f, 0f), tweenTimeUnit * 0.2f).SetEase(Ease.OutQuad));
        }
        else if (style == Style.Fold)
        {
            animationSequence.Append(rect.DOAnchorPos(posWhenHide, 0.3f).SetEase(Ease.OutCubic));
        }
        animationSequence.OnComplete(() => {
            content?.SetActive(false);
            canvasGroup.alpha = 0;
            if (destroy) Destroy(gameObject);
        });
    }
    public void SetStyle(Style newStyle)
    {
        style = newStyle;
    }
}
