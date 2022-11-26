using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePosAnimatorInfo : AnimationInfo
{
    Vector3 target;
    bool animated;
    bool local = false;
    bool linear;
    float duration;
    public UpdatePosAnimatorInfo(GameObject _gameObject, Vector3 _target, bool _linear = true, float _duration = 1, bool _block = false, bool _animated = true, bool _local = false)
    {
        this.gameObject = _gameObject;
        this.target = _target;
        this.animated = _animated;
        this.local = _local;
        this.linear = _linear;
        this.duration = _duration;
        this.block = _block;
    }
    public override void Invoke()
    {
        UpdatePosAnimator animator = gameObject.GetComponent<UpdatePosAnimator>();
        animator.info = this;
        if (animator == null) {
            Debug.Log("Animator does not exist!\n");
            return;
        }
        animator.target = target;
        animator.animated = animated;
        animator.block = block;
        animator.local = local;
        animator.linear = linear;
        animator.duration = duration;
        animator.block = block;
        animator.Invoke();
    }
}
