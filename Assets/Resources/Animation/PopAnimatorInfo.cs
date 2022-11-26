using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopAnimatorInfo : AnimationInfo
{
    PopAnimator.Type type;
    float duration;
    public PopAnimatorInfo(GameObject _gameObject, PopAnimator.Type _type = PopAnimator.Type.Appear, float _duration = 0.15f)
    {
        this.gameObject = _gameObject;
        this.type = _type;
        this.duration = _duration;
    }
    public override void Invoke()
    {
        //Debug.Log("Invoke gameObject : " + gameObject);
        PopAnimator animator = gameObject.GetComponent<PopAnimator>();
        //Debug.Log("debugCount : " + Global.debugCount);
        animator.info = this;
        if (animator == null) {
            Debug.Log("Animator does not exist!\n");
            return;
        }
        animator.type = type;
        animator.block = this.block;
        animator.duration = this.duration;
        animator.Invoke();
    }
}
