using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopAnimator : MyAnimation
{
    Vector3 originScale;
    Canvas canvas;
    public PopAnimatorInfo info;
    SpriteRenderer sprite;
    public bool widthOnly = false;
    public float duration;
    delegate float EaseFunc(float x);
    public enum Type{
        Appear,
        Emphasize,
        Disappear,
        PopOut,
        PopOut_TileText,
        PopBack,
        LinearOut,
        LinearBack,
        LinearDisappear
    }
    public Type type;
    protected override void Awake()
    {
        base.Awake();
        originScale = transform.localScale;
        sprite = GetComponentInChildren<SpriteRenderer>();
        canvas = GetComponentInChildren<Canvas>();
    }
    private IEnumerator ChangeSize(float start, float end, float sec, int myOrder, EaseFunc easeFunc)
    {
        float size, timer = 0;
        if (widthOnly) originScale.x = transform.localScale.x;
        if (start < 0) start = transform.localScale.x / originScale.x; // continue with current size
        size = start;
        //Debug.Log("localScale = " + transform.localScale);
        if (!widthOnly) transform.localScale = originScale * size;
        else transform.localScale = new Vector2(originScale.x, originScale.y * size);
        while (timer + Time.deltaTime < sec)
        {
            timer += Time.deltaTime;
            size = start + (end - start) * easeFunc(timer / sec);
            if (!widthOnly) transform.localScale = originScale * size;
            else transform.localScale = new Vector2(originScale.x, originScale.y * size);
            if (!animationOrder.isLatest(myOrder)) yield break;
            yield return null;
        }
        size = end;
        if (!widthOnly) transform.localScale = originScale * size;   
        else transform.localScale = new Vector2(originScale.x, originScale.y * size);       
    }
    protected override IEnumerator Animate()
    {
        Type type = this.type;
        PopAnimatorInfo info = this.info;
        bool block = this.block;
        int myOrder = animationOrder.NewOrder();
        if (!block) info.completed = true;
        float duration = this.duration;
        
        if (type == Type.Appear)
        {
            if (sprite != null) sprite.enabled = true;
            if (canvas != null) canvas.enabled = true;
            yield return ChangeSize(0, 1.5f, duration, myOrder, Tween.EaseInOut);
            yield return ChangeSize(1.5f, 0.8f, duration, myOrder, Tween.EaseInOut);
            yield return ChangeSize(0.8f, 1.0f, duration, myOrder, Tween.EaseInOut);
        }
        else if (type == Type.Emphasize)
        {
            yield return ChangeSize(1.0f, 1.5f, duration, myOrder, Tween.EaseInOut);
            yield return ChangeSize(1.5f, 0.8f, duration, myOrder, Tween.EaseInOut);
            yield return ChangeSize(0.8f, 1.0f, duration, myOrder, Tween.EaseInOut);
        }
        else if (type == Type.Disappear)
        {
            yield return ChangeSize(1.0f, 1.15f, duration, myOrder, Tween.EaseInOut);
            yield return ChangeSize(1.15f, 0.0f, duration, myOrder, Tween.EaseInOut);
        }
        else if (type == Type.PopOut)
        {
            yield return ChangeSize(-1, 1.3f, duration, myOrder, Tween.EaseInOut);
            yield return ChangeSize(1.3f, 1.05f, duration, myOrder, Tween.EaseInOut);
            yield return ChangeSize(1.05f, 1.15f, duration, myOrder, Tween.EaseInOut);
        }
        else if (type == Type.PopOut_TileText)
        {
            yield return ChangeSize(-1, 1.8f, duration, myOrder, Tween.EaseInOut);
            yield return ChangeSize(1.8f, 1.3f, duration, myOrder, Tween.EaseInOut);
            yield return ChangeSize(1.3f, 1.5f, duration, myOrder, Tween.EaseInOut);
        }
        else if (type == Type.PopBack)
        {
            yield return ChangeSize(-1, 0.9f, duration, myOrder, Tween.EaseInOut);
            yield return ChangeSize(0.9f, 1.15f, duration, myOrder, Tween.EaseInOut);
            yield return ChangeSize(1.15f, 1.0f, duration, myOrder, Tween.EaseInOut);
        }
        else if (type == Type.LinearOut)
        {
            yield return ChangeSize(-1, 1.15f, duration, myOrder, Tween.Linear);
        }
        else if (type == Type.LinearBack)
        {
            yield return ChangeSize(-1, 1.0f, duration, myOrder, Tween.Linear);
        }
        else if (type == Type.LinearDisappear)
        {
            yield return ChangeSize(-1, 0f, duration, myOrder, Tween.Linear);
        }
        if (block) info.completed = true;
    }
}
