using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownButton : MyButton
{
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        onClick.AddListener(SlowDown);
    }

    // Update is called once per frame
    void SlowDown()
    {
        if (Global.nekoPlaySpeed > 1) Global.nekoPlaySpeed /= 2;
    }
}
