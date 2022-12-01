using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1_4 : PuzzleLogic
{
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        Global.gameMessage.SetNewMessage("小猫可以从蓝色方块中读取信息");
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
