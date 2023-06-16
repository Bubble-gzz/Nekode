using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Puzzle2_3 : PuzzleLogic
{
    TMP_Text debugInfo;
    protected override void Awake()
    {
        base.Awake();
        //debugInfo = GameObject.Find("Debug")?.transform.Find("info").GetComponent<TMP_Text>();
    }
    override protected void Start()
    {
        base.Start();
        totalTestCase = 1;
        StartCoroutine(GameProcess());
        conditionStatus[0] = true;
        conditionStatus[1] = true;
        conditionStatus[2] = false;
        
        conditions[0] = "Count from 1 to 3 successfully";
        conditions[1] = "ACCOMPLISH THE FEAT";
        conditions[2] = "Do the job with JUST ONE add tile";

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
        
        if (Settings.language == "CH") dialogue.Play("有了加法，任何猫咪都能够从1数到3了。");
        else dialogue.Play("Any kitten can count from 1 to 3 now thanks to ADD tiles.", new Vector2(650, 150));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("在这个小小的棋盘中，完成这项创举！");
        else dialogue.Play("Could you accomplish the feat in this tiny grid?", new Vector2(650, 150));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("记住，方块用得越少越好~");
        else dialogue.Play("Remember that the fewer, the better.", new Vector2(650, 170));

        while (dialogue.isPlaying) yield return null;

        dialogue.Close(true);

        yield return new WaitForSeconds(0.2f);
        //helpCardPanel = GameUIManager.PopOutPanel(helpCardPanelPrefab).GetComponentInChildren<MyPanel>();
        //while (helpCardPanel.showing) yield return null;
        
        
        if (Settings.language == "CH") SetTarget("完成下列赋值：A1=1，A2=2，A3=3");
        else SetTarget("Complete the following data assignment: A1=1, A2=2, A3=3");
        GameUIManager.UnFoldUI();
        yield return null;
    }
    protected override IEnumerator CheckCondition2()
    {
        conditionStatus[2] = (RemainingTiles(MyTile.Type.ADD) >= 2);
        return base.CheckCondition2();
    }
    public override void GenerateTestCase()
    {
        base.GenerateTestCase();
        //Debug.Log("Generating Test Case ... ... ");
        //grid.tileTable["A0"][0].UpdateValue(1);
       //grid.tileTable["A1"][0].UpdateValue(2);
        //grid.tileTable["A2"][0].UpdateValue(3);

        answerTable["A1"] = 1;
        answerTable["A2"] = 2;
        answerTable["A3"] = 3;
    }
}
