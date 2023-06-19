using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Threading.Tasks;

public class MyButtonImage : MonoBehaviour
{
    // Start is called before the first frame update
    Image image;
    CanvasGroup canvasGroup;
    public Color NormalColor, HoverColor, ClickedColor;
    public float tweenInterval = 0.07f;
    public Vector3 size_on, size_off;
    public bool shakeNotice = false;
    public float timeBeforeShake = 3f, shakeTimeUnit = 1, shakeInterval = 1;
    public float shakeAngle = 30;
    public bool show, hover, active;
    public bool oneshot = false; // clicked once
    Sequence animationSequence;
    void Awake()
    {
        image = GetComponentInChildren<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
        show = true;
        active = true;
    }
    void Start()
    {
        if (image != null) image.color = NormalColor;
    }

    void OnEnable()
    {
        if (shakeNotice) StartCoroutine(Shake());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Shake()
    {
        Debug.Log("Shake");
        float countDown = timeBeforeShake;
        while (true)
        {
            if (!show || hover) countDown = timeBeforeShake;
            if (countDown > 0) countDown -= Time.deltaTime;
            if (countDown < 0)
            {
                StopTween();
                transform.DOScale(new Vector3(1,1,1), 0.1f);
                animationSequence = DOTween.Sequence();
                animationSequence.Append(transform.DORotate(new Vector3(0, 0, shakeAngle), shakeTimeUnit).SetEase(Ease.InOutQuart));
                animationSequence.Append(transform.DORotate(new Vector3(0, 0, -shakeAngle * 0.7f), shakeTimeUnit).SetEase(Ease.InOutQuart));
                animationSequence.Append(transform.DORotate(new Vector3(0, 0, shakeAngle * 0.3f), shakeTimeUnit).SetEase(Ease.InOutQuart));
                animationSequence.Append(transform.DORotate(new Vector3(0, 0, -shakeAngle * 0.1f), shakeTimeUnit).SetEase(Ease.InOutQuart));
                animationSequence.Append(transform.DORotate(new Vector3(0, 0, 0), shakeTimeUnit).SetEase(Ease.InOutQuart));
                countDown = shakeInterval;
            }
            yield return null;
        }
    }
    void StopTween()
    {
        animationSequence?.Kill();
        image?.DOKill();
        canvasGroup?.DOKill();
        transform?.DOKill();
    }
    public void OnMouseEnter()
    {
        if (!show || !active) return;
        hover = true;
        StopTween();
        transform.DORotate(new Vector3(0, 0, 0), 0.2f);
        animationSequence = DOTween.Sequence();
        animationSequence.Append(transform.DOScale(size_on * 1.1f, tweenInterval));
        animationSequence.Append(transform.DOScale(size_on * 0.95f, tweenInterval));
        animationSequence.Append(transform.DOScale(size_on, tweenInterval));
        if (image != null)
            animationSequence.Insert(0, image.DOColor(HoverColor, tweenInterval * 3));
    }
    public void OnMouseExit()
    {
        //if (!show || !active) return;
        hover = false;
        StopTween();
        transform.DORotate(new Vector3(0, 0, 0), 0.2f);
        animationSequence = DOTween.Sequence();
        animationSequence.Append(transform.DOScale(size_off * 0.9f, tweenInterval));
        animationSequence.Append(transform.DOScale(size_off * 1.05f, tweenInterval));
        animationSequence.Append(transform.DOScale(size_off, tweenInterval));
        if (image != null) animationSequence.Insert(0, image.DOColor(NormalColor, tweenInterval * 3));
    }
    public void OnClicked()
    {
        if (!show || !active) return;
        StopTween();
        transform.DORotate(new Vector3(0, 0, 0), 0.2f);
        if (image != null) image.color = ClickedColor;
        animationSequence = DOTween.Sequence();
        animationSequence.Append(transform.DOScale(size_on * 0.6f, tweenInterval * 0.5f));
        animationSequence.Append(transform.DOScale(size_on * 1.05f, tweenInterval));
        animationSequence.Append(transform.DOScale(size_on, tweenInterval));
       // animationSequence.Insert(0, image.DOColor(ClickedColor, tweenInterval * 0.5f));
        
        if (image != null) {
            if (!oneshot) animationSequence.Insert(0, image.DOColor(HoverColor, tweenInterval*2));
            else animationSequence.Insert(0, image.DOColor(NormalColor, tweenInterval*2));
        }
    }
    public void Disappear()
    {
        hover = false;
        StopTween();
        transform.DORotate(new Vector3(0, 0, 0), 0.2f);
        animationSequence = DOTween.Sequence();
        show = false;
        if (canvasGroup)
        {
            canvasGroup.DOFade(0, tweenInterval);
        }
        else {
            if (image != null) image.DOFade(0, tweenInterval);
        }
        transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), tweenInterval);  
    }
    public void Appear()
    {
        StopTween();
        transform.DORotate(new Vector3(0, 0, 0), 0.2f);
        animationSequence = DOTween.Sequence();
        show = true;
        if (canvasGroup)
        {
            canvasGroup.DOFade(1, tweenInterval * 3);
        }
        else
        {
            if (image != null)
            {
                Color color = NormalColor;
                color.a = 0;
                image.color = color;
                image.DOFade(NormalColor.a, tweenInterval * 3);
            }
        }

        animationSequence.Append(transform.DOScale(new Vector3(0,0,0), 0.01f));
        animationSequence.Append(transform.DOScale(new Vector3(1.2f,1.2f,1.2f), tweenInterval));
        animationSequence.Append(transform.DOScale(new Vector3(0.9f,0.9f,0.9f), tweenInterval));
        animationSequence.Append(transform.DOScale(new Vector3(1f,1f,1f), tweenInterval));
    }
    public void SetActive(bool flag)
    {
        active = flag;
    }
}
