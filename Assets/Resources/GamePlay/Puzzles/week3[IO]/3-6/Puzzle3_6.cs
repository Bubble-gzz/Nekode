using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle3_6 : PuzzleLogic
{
   protected override void Awake()
    {
        base.Awake();
        //debugInfo = GameObject.Find("Debug")?.transform.Find("info").GetComponent<TMP_Text>();
    }
    override protected void Start()
    {
        base.Start();
        totalTestCase = 3;

        conditionStatus[0] = true;
        conditionStatus[1] = true;
        conditionStatus[2] = true;
        
        conditions[0] = "Complete the challenge";
        conditions[1] = "Use no more than 3 ADD Tiles";
        conditions[2] = "Use no more than 2 I/O-Switch Tiles";

        //GameMessage.ToolReturnedToSlot.AddListener(ToolReturnedToSlot);
    }
    
    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
        //debugInfo.text = "step:" + Global.stepCount + "  arrow:" + RemainingArrows();
    }

    override protected IEnumerator GameProcess()
    {
        yield return base.GameProcess();

        dialogue.Open();
        
        if (Settings.language == "CH") dialogue.Play("今天我们来做十六倍放大器！");
        else dialogue.Play("Today let's make a 16x amplifier!", new Vector2(600, 100));
        while (dialogue.isPlaying) yield return null;

        dialogue.Close(true);

        yield return new WaitForSeconds(0.2f);
        //helpCardPanel = GameUIManager.PopOutPanel(helpCardPanelPrefab).GetComponentInChildren<MyPanel>();
        //while (helpCardPanel.showing) yield return null;
        
        
        if (Settings.language == "CH") SetTarget("B=A*16");
        else SetTarget("B=A*16");
        GameUIManager.UnFoldUI();
        yield return null;
    }
    protected override IEnumerator CheckCondition1()
    {
        conditionStatus[1] = (RemainingTiles(MyTile.Type.ADD) >= 1);
        return base.CheckCondition1();
    }
    protected override IEnumerator CheckCondition2()
    {
        conditionStatus[2] = (2 - RemainingTiles(MyTile.Type.ISwitch)) + (2 - RemainingTiles(MyTile.Type.OSwitch)) >= 2;
        return base.CheckCondition2();
    }
    public override void GenerateTestCase()
    {
        base.GenerateTestCase();
        if (curTestCase == 1)
        {
            grid.tileTable["A"][0].UpdateValue(1);
        }
        else if (curTestCase == 2)
        {
            grid.tileTable["A"][0].UpdateValue(-10);      
        }
        else if (curTestCase == 3)
        {
            grid.tileTable["A"][0].UpdateValue(16);
        }
        //Debug.Log("Generating Test Case ... ... ");
        //grid.tileTable["A0"][0].UpdateValue(1);
       //grid.tileTable["A1"][0].UpdateValue(2);
        //grid.tileTable["A2"][0].UpdateValue(3);

        answerTable["B"] = grid.tileTable["A"][0].value * 16;

    }
}
