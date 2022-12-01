using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1_3 : PuzzleLogic
{
    override protected void Start()
    {
        base.Start();
        Global.gameMessage.SetNewMessage("转呀转");
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
}
