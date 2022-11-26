using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationInfo
{
    public GameObject gameObject;
    public bool completed = false;
    public bool block = false;
    public virtual void Invoke()
    {

    } 
}
