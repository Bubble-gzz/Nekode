using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RatingStar : MonoBehaviour
{
    // Start is called before the first frame update
    Transform star, notch;
    void Awake()
    {
        star = transform.Find("Star");
        notch = transform.Find("Notch");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Pass()
    {
        var sequence = DOTween.Sequence();
        star.transform.localScale = Vector3.zero;
        star.GetComponent<Image>().DOFade(1, 0.1f);
        sequence.Append( star.transform.DOScale(Vector3.one * 1.3f, 0.1f) );
        sequence.Append( star.transform.DOScale(Vector3.one * 0.9f, 0.1f) );
        sequence.Append( star.transform.DOScale(Vector3.one * 1f, 0.1f) );    
    }
    public void Fail()
    {
        var sequence = DOTween.Sequence();
        notch.transform.localScale = Vector3.zero;
        notch.GetComponent<Image>().DOFade(1, 0.1f);
        sequence.Append( notch.transform.DOScale(Vector3.one * 1.3f, 0.1f) );
        sequence.Append( notch.transform.DOScale(Vector3.one * 0.9f, 0.1f) );
        sequence.Append( notch.transform.DOScale(Vector3.one * 1f, 0.1f) );    
    }
}
