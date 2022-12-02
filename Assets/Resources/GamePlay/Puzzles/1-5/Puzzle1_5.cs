using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1_5 : PuzzleLogic
{
    override protected void Start()
    {
        base.Start();
        Global.gameMessage.SetNewMessage("请在到达终点前保持下面的条件");
    }

    // Update is called once per frame
    override protected void Update()
    {
        Neko neko = Global.currentNeko;
        base.Update();
        if (neko != null)
        {
            if (neko.atDestination && conditions[0].satisfied) PuzzleComplete();
            if (neko.value == 1)
                conditions[0].SetState(true);
            else conditions[0].SetState(false);
        }
        
    }
}
