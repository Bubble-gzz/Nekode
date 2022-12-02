using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1_9 : PuzzleLogic
{
    override protected void Start()
    {
        base.Start();
        Global.gameMessage.SetNewMessage("绿色方块可以接收小猫的数");
    }

    // Update is called once per frame
    override protected void Update()
    {
        Neko neko = Global.currentNeko;
        base.Update();
        if (neko != null)
        {
            if (neko.atDestination && conditions[0].satisfied) PuzzleComplete();
            if (Global.grid.tileTable.ContainsKey("A"))
            {
                if (Global.grid.tileTable["A"].value == 1)
                    conditions[0].SetState(true);
                else conditions[0].SetState(false);
            }
            else conditions[0].SetState(false);
        }
        
    }
}
