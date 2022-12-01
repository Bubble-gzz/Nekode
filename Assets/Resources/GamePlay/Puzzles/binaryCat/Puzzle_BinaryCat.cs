using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_BinaryCat : PuzzleLogic
{
    override protected void Start()
    {
        base.Start();
        Global.gameMessage.SetNewMessage("用2进制表示X中的数");
    }

    // Update is called once per frame
    override protected void Update()
    {
        Neko neko = Global.currentNeko;
        base.Update();
        if (neko != null)
        {
            int x = Global.grid.tileTable["X"].value;
            if (neko.atDestination && conditions[0].satisfied && conditions[1].satisfied && conditions[2].satisfied) PuzzleComplete();
            if (x % 2 == Global.grid.tileTable["C"].value)
                conditions[2].SetState(true);
            else conditions[2].SetState(false);
            if ((x/2) % 2 == Global.grid.tileTable["B"].value)
                conditions[1].SetState(true);
            else conditions[1].SetState(false);
            if ((x/4) % 2 == Global.grid.tileTable["A"].value)
                conditions[0].SetState(true);
            else conditions[0].SetState(false);
        }
        
    }
}
