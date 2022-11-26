using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorAnimatorInfo : AnimationInfo
{
    Color targetColor;
    bool animated;
    public ChangeColorAnimatorInfo(GameObject _gameObject, Color _targetColor, bool _animated = true)
    {
        this.gameObject = _gameObject;
        this.targetColor = _targetColor;
        this.animated = _animated;
    }
    public override void Invoke()
    {
        ChangeColorAnimator animator = gameObject.GetComponent<ChangeColorAnimator>();
        animator.info = this;
        if (animator == null) {
            Debug.Log("Animator does not exist!\n");
            return;
        }
        animator.targetColor = targetColor;
        animator.animated = animated;
        animator.Invoke();
    }
}
