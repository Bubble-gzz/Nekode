using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TestResultBulb : MonoBehaviour
{
    // Start is called before the first frame update
    public float tweenDuration = 0.07f;
    Image image;
    bool state;
    public Color color_pass, color_fail, color_pending;
    public Vector3 size_on = new Vector3(1.3f,1.3f,1.3f), size_off = new Vector3(1,1,1);
    Sequence animationSequence;
    void Awake()
    {
        image = GetComponent<Image>();
        state = false;
        image.color = color_pending;
        transform.localScale = size_off;
       // image.DOColor(color_pending, tweenDuration);
       // transform.DOScale(size_off, tweenDuration);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PopOut()
    {
        animationSequence?.Kill();
        animationSequence = DOTween.Sequence();
        animationSequence.Append(transform.DOScale(size_on * 1.2f, tweenDuration));
        animationSequence.Append(transform.DOScale(size_on * 0.95f, tweenDuration));
        animationSequence.Append(transform.DOScale(size_on, tweenDuration));
    }
    public void PopBack()
    {
        animationSequence?.Kill();
        animationSequence = DOTween.Sequence();
        animationSequence.Append(transform.DOScale(size_off * 0.8f, tweenDuration));
        animationSequence.Append(transform.DOScale(size_off * 1.05f, tweenDuration));
        animationSequence.Append(transform.DOScale(size_off * 1, tweenDuration));
    }
    public void SetPass()
    {
        image.DOColor(color_pass, tweenDuration);
        PopOut();
    }
    public void SetFail()
    {
        image.DOColor(color_fail, tweenDuration);
        PopOut();
    }
    public void SetPending()
    {
        image.DOColor(color_pending, tweenDuration);
        PopBack();
    }
}
