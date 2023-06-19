using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class MyCondition : MonoBehaviour
{
    // Start is called before the first frame update
    Transform mark;
    TMP_Text condition;
    CanvasGroup canvasGroup;
    RectTransform rect;
    Vector2 pos0;
    void Awake()
    {
        condition = GetComponentInChildren<TMP_Text>();//.text;
        mark = transform.Find("checkbox/mark");
        canvasGroup = GetComponentInChildren<CanvasGroup>();
        canvasGroup.alpha = 0;
        rect = GetComponent<RectTransform>();
        pos0 = rect.anchoredPosition;
        rect.anchoredPosition = pos0 + new Vector2(100, 0);
        
    }
    void Start()
    {
        Color color = mark.GetComponent<Image>().color;
        color.a = 0;
        mark.GetComponent<Image>().color = color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CheckBox()
    {
        var sequence = DOTween.Sequence();
        mark.GetComponent<Image>().DOFade(1, 0.2f);
        sequence.Append( mark.transform.DOScale(Vector3.one * 1.3f, 0.3f) );
        sequence.Append( mark.transform.DOScale(Vector3.one * 0.9f, 0.1f) );
        sequence.Append( mark.transform.DOScale(Vector3.one * 1f, 0.1f) );        
    }
    public void UnCheckBox()
    {
        var sequence = DOTween.Sequence();
        mark.GetComponent<Image>().DOFade(0, 0.2f);
        sequence.Append( mark.transform.DOScale(Vector3.one * 1.2f, 0.1f) );
        sequence.Append( mark.transform.DOScale(Vector3.one * 0.9f, 0.2f) );
    }
    public void SetCondition(string newCondition)
    {
        condition.text = newCondition;
        canvasGroup.DOFade(1, 0.2f);
        rect.DOAnchorPos(pos0, 0.2f);
    }
}
