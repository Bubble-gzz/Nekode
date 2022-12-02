using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1_10 : PuzzleLogic
{
    override protected void Start()
    {
        base.Start();
        Global.gameMessage.SetNewMessage("请保持原样");
    }

    // Update is called once per frame
    override protected void Update()
    {
        Neko neko = Global.currentNeko;
        base.Update();
        if (neko != null)
        {
            if (neko.atDestination && conditions[0].satisfied) PuzzleComplete();
            if (Global.grid.tileTable.ContainsKey("A") && Global.grid.tileTable["A"].value == 0) conditions[0].SetState(false);
            else conditions[0].SetState(true);
            if (Global.grid.tileTable.ContainsKey("B") && Global.grid.tileTable["B"].value == 1) conditions[1].SetState(false);
            else conditions[1].SetState(true);
            if (Global.grid.tileTable.ContainsKey("C") && Global.grid.tileTable["C"].value == 0) conditions[2].SetState(false);
            else conditions[2].SetState(true);
            if (Global.grid.tileTable.ContainsKey("D") && Global.grid.tileTable["D"].value == 1) conditions[3].SetState(false);
            else conditions[3].SetState(true);
            if (Global.grid.tileTable.ContainsKey("E") && Global.grid.tileTable["E"].value == 1) conditions[4].SetState(false);
            else conditions[4].SetState(true);
        }
        
    }
}
