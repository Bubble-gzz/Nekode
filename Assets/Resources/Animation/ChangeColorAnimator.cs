using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ChangeColorAnimator : MyAnimation
{
    public Color targetColor;
    public bool animated;
    public ChangeColorAnimatorInfo info;
    SpriteRenderer sprite;
    protected override void Awake()
    {
        base.Awake();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    protected override IEnumerator Animate()
    {
        Color targetColor = this.targetColor;
        bool animated = this.animated;
        ChangeColorAnimatorInfo info = this.info;
        int myOrder = animationOrder.NewOrder();
        info.completed = true;

        float current = 0, speed = 5f;
        Color startColor = sprite.color, endColor = targetColor;
        while (1 - current > speed * Time.deltaTime)
        {
            current += speed * Time.deltaTime;
            sprite.color = Color.Lerp(startColor, endColor, current);
            if (!animationOrder.isLatest(myOrder)) {
                yield break;
            }
            yield return null;
        }
    }
}
