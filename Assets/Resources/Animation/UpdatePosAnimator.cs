using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePosAnimator : MyAnimation
{
    public Vector3 target;
    public bool animated;
    public bool local;
    public UpdatePosAnimatorInfo info;
    public bool linear;
    public float duration;
    
    protected override IEnumerator Animate()
    {
        Vector3 target = this.target;
        bool animated = this.animated;
        bool block = this.block;
        bool local = this.local;
        bool linear = this.linear;
        float duration = this.duration;
        UpdatePosAnimatorInfo info = this.info;
        int myOrder = animationOrder.NewOrder();
        //Debug.Log("updatePos.block: " + block);
        if (!block) info.completed = true;
        Debug.Log("block:" + block);
        if (animated)
        {
            if (linear)
            {
                Vector3 delta;
                if (!local) delta = target - transform.position;
                else delta = target - transform.localPosition;
                float dist = delta.magnitude, progress = 0;
                float speed = 1 / duration;
                
                while (progress + speed * Time.deltaTime < 1)
                {
                    progress += speed * Time.deltaTime;
                    if (!local) transform.position += (Vector3)delta.normalized * dist * speed * Time.deltaTime;
                    else transform.localPosition += (Vector3)delta.normalized * dist * speed * Time.deltaTime;
                    yield return null;
                    if (!animationOrder.isLatest(myOrder)) {
                        yield break;
                    }
                }
            }
            else
            {
                while (true)
                {
                    Vector3 delta;
                    if (!local) delta = target - transform.position;
                    else delta = target - transform.localPosition;
                    float dist = delta.magnitude;
                    if (dist < 0.01f) break;
                    float speed = Mathf.Max(0.5f, dist * 10);
                    if (!local) transform.position += (Vector3)delta.normalized * speed * Time.deltaTime;
                    else transform.localPosition += (Vector3)delta.normalized * speed * Time.deltaTime;
                    yield return null;
                    if (!animationOrder.isLatest(myOrder)) {
                        yield break;
                    }
                }
            }
        }
        if (!local) transform.position = target;
        else transform.localPosition = target;
        if (block) info.completed = true;
    }
}
