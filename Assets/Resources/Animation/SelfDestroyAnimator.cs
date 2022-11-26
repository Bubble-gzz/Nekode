using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class SelfDestroyAnimator : MyAnimation
{
    public bool animated;
    public bool widthOnly;
    public SelfDestroyAnimatorInfo info;

    protected override IEnumerator Animate()
    {
        bool animated = this.animated;
        bool widthOnly = this.widthOnly;
        bool block = this.block;
        SelfDestroyAnimatorInfo info = this.info;
        if (!block) info.completed = true;
        PopAnimator[] popAnimators;
        if (gameObject.GetComponent<PopAnimator>())
        {
            popAnimators = new PopAnimator[1]{gameObject.GetComponent<PopAnimator>()};
        }
        else popAnimators = gameObject.GetComponentsInChildren<PopAnimator>();
        foreach(PopAnimator popAnimator in popAnimators)
        {
            PopAnimatorInfo popAnimatorInfo = new PopAnimatorInfo(popAnimator.gameObject, PopAnimator.Type.Disappear);
            popAnimatorInfo.block = block;
            objectAnimationBuffer.Add(popAnimatorInfo);
        }
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        if (block) info.completed = true;
        yield break;
    }
}
