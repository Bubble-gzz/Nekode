using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class IndexCircle : MonoBehaviour
{
    // Start is called before the first frame update
    public float tweenDuration = 0.07f;
    Image image;
    bool state;
    public Color color_on = new Color(1,1,1,1), color_off = new Color(0,0,0,0.5f);
    public Vector3 size_on = new Vector3(1.3f,1.3f,1.3f), size_off = new Vector3(1,1,1);
    Sequence animationSequence;
    void Awake()
    {
        image = GetComponent<Image>();
        state = false;
        image.DOColor(color_off, tweenDuration);
        transform.DOScale(size_off, tweenDuration);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetState(bool flag)
    {
        if (state == flag) return;
        state = flag;
        if (flag) {
            animationSequence = DOTween.Sequence();
            image.DOColor(color_on, tweenDuration);
            animationSequence.Append(transform.DOScale(size_on * 1.2f, tweenDuration));
            animationSequence.Append(transform.DOScale(size_on * 0.95f, tweenDuration));
            animationSequence.Append(transform.DOScale(size_on, tweenDuration));
        }
        else {
            image?.DOKill();
            animationSequence?.Kill();
            image.DOColor(color_off, tweenDuration);
            transform.DOScale(size_off, tweenDuration);
        }
    }
}
