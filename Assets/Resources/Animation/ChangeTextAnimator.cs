using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeTextAnimator : MyAnimation
{
    public string newText;
    public bool animated;
    public ChangeTextAnimatorInfo info;
    [SerializeField]
    protected TMP_Text text;

    protected override void Awake()
    {
        base.Awake();
        if (text == null) text = GetComponentInChildren<TMP_Text>();
    }
    protected override IEnumerator Animate()
    {
        string newText = this.newText;
        bool animated = this.animated;
        ChangeTextAnimatorInfo info = this.info;
        info.completed = true;
        text.text = newText;
        yield break;
    }
}
