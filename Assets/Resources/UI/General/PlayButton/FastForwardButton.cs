using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastForwardButton : MyButton
{
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        onClick.AddListener(FastForward);
    }
    void FastForward()
    {
        if (Global.nekoPlaySpeed < 32) Global.nekoPlaySpeed *= 2;
    }
}
