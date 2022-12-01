using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzel1_2 : PuzzleLogic
{
    override protected void Start()
    {
        base.Start();
        Global.gameMessage.SetNewMessage("路不是直线怎么办呢");
        StartCoroutine(Hint1());
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
        if (Global.currentNeko != null)
        {
            Neko neko = Global.currentNeko;
            if (neko.atDestination) PuzzleComplete();
        }
    }
    IEnumerator Hint1()
    {
        yield return new WaitForSeconds(1f);
        
    }
}
