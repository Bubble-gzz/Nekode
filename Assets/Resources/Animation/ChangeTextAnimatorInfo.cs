using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTextAnimatorInfo : AnimationInfo
{
    string newText;
    public ChangeTextAnimatorInfo(GameObject _gameObject, string _newText)
    {
        this.gameObject = _gameObject;
        this.newText = _newText;
    }
    public override void Invoke()
    {
        ChangeTextAnimator animator = gameObject.GetComponent<ChangeTextAnimator>();
        animator.info = this;
        if (animator == null) {
            Debug.Log("Animator does not exist!\n");
            return;
        }
        animator.newText = newText;
        animator.Invoke();
    }
}
