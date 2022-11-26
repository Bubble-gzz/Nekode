using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAnimation : MonoBehaviour
{
    protected AnimationOrder animationOrder;
    protected AnimationBuffer objectAnimationBuffer;
    public bool block = false;
    protected virtual void Awake()
    {
        animationOrder = new AnimationOrder(20);
        objectAnimationBuffer = GetComponent<AnimationBuffer>();
    }
    public void Invoke() {
        StartCoroutine(Animate());
    }   
    virtual protected IEnumerator Animate()
    {
        yield return null;
    }
}
