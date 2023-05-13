using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TurnPageIcon : MonoBehaviour
{
    // Start is called before the first frame update
    Image image;
    public Color NormalColor, HoverColor, ClickedColor;
    public float tweenInterval = 0.07f;
    bool show;
    void Awake()
    {
        image = GetComponentInChildren<Image>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public async void PopOut()
    {
        if (!show) return;
        image.DOColor(HoverColor, tweenInterval * 3);
        await transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), tweenInterval).AsyncWaitForCompletion();
        await transform.DOScale(new Vector3(1.05f, 1.05f, 1.05f), tweenInterval).AsyncWaitForCompletion();
        await transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), tweenInterval).AsyncWaitForCompletion();
    }
    public async void PopBack()
    {
        if (!show) return;
        image.DOColor(NormalColor, tweenInterval * 3);
        await transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), tweenInterval).AsyncWaitForCompletion();
        await transform.DOScale(new Vector3(1.05f, 1.05f, 1.05f), tweenInterval).AsyncWaitForCompletion();
        await transform.DOScale(new Vector3(1f, 1f, 1f), tweenInterval).AsyncWaitForCompletion();
    }
    public async void Clicked()
    {
        if (!show) return;
        image.DOColor(ClickedColor, 0.1f);
        await transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), tweenInterval * 0.5f).AsyncWaitForCompletion();
        image.DOColor(HoverColor, tweenInterval*2);
        await transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), tweenInterval).AsyncWaitForCompletion();
        transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), tweenInterval);   
    }
    public void Disappear()
    {
        show = false;
        image.color = ClickedColor;
        image.DOFade(0, tweenInterval);
        transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), tweenInterval);  
    }
    public async void Appear()
    {
        show = true;
        image.color = NormalColor;
        image.DOFade(0, 0.001f);
        image.DOFade(NormalColor.a, tweenInterval * 3);
        await transform.DOScale(new Vector3(0,0,0), 0.01f).AsyncWaitForCompletion();
        await transform.DOScale(new Vector3(1.2f,1.2f,1.2f), tweenInterval).AsyncWaitForCompletion();
        await transform.DOScale(new Vector3(0.9f,0.9f,0.9f), tweenInterval).AsyncWaitForCompletion();
        await transform.DOScale(new Vector3(1f,1f,1f), tweenInterval).AsyncWaitForCompletion();
    }
}
