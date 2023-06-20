using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RatingStar : MonoBehaviour
{
    // Start is called before the first frame update
    Transform star, notch;
    AudioSource sfx_star;
    void Awake()
    {
        star = transform.Find("Star");
        notch = transform.Find("Notch");
        sfx_star = GetComponent<AudioSource>();
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
        if (sfx_star)
        {
            sfx_star.volume = AudioManager.sfxVolume;
            sfx_star.Play();
        }
        var sequence = DOTween.Sequence();
        star.transform.localScale = Vector3.zero;
        star.GetComponent<Image>().DOFade(1, 0.1f);
        sequence.Append( star.transform.DOScale(new Vector3(1.3f, 0.7f, 1), 0.1f) );
        sequence.Append( star.transform.DOScale(new Vector3(0.8f, 1.2f, 1), 0.1f) );
        sequence.Append( star.transform.DOScale(new Vector3(0.9f, 1.1f, 1), 0.1f) );    
        sequence.Append( star.transform.DOScale(new Vector3(1.05f, 0.95f, 1), 0.1f) );    
        sequence.Append( star.transform.DOScale(new Vector3(1, 1, 1), 0.1f) );    
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
