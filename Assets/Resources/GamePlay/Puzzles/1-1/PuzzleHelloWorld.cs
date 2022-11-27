using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleHelloWorld : PuzzleLogic
{
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        Global.gameMessage.SetNewMessage("怎么才能让小猫到达终点呢");
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
