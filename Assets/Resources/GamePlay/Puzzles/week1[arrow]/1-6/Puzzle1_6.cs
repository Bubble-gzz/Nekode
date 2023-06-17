using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Puzzle1_6 : PuzzleLogic
{
    TMP_Text debugInfo;
    protected override void Awake()
    {
        base.Awake();
        debugInfo = GameObject.Find("Debug")?.transform.Find("info").GetComponent<TMP_Text>();
    }
    override protected void Start()
    {
        base.Start();
        totalTestCase = 1;
        conditionStatus[0] = true;
        conditionStatus[1] = false;
        conditionStatus[2] = false;
        
        conditions[0] = "Complete the challenge";
        conditions[1] = "Complete the job within 35 steps";
        conditions[2] = "Use no more than " + (25-16).ToString() + " arrows";

        //GameMessage.ToolReturnedToSlot.AddListener(ToolReturnedToSlot);
    }
    
    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
        debugInfo.text = "step:" + Global.stepCount + "  arrow:" + RemainingArrows();
    }

    override protected IEnumerator GameProcess()
    {
        yield return base.GameProcess();

        dialogue.Open();
        
        if (Settings.language == "CH") dialogue.Play("这是我们给你准备的挑战");
        else dialogue.Play("Here is a challenge prepared for you.", new Vector2(650, 150));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("但我相信这难不倒聪明的你。。");
        else dialogue.Play("But I believe that you are clever enough to solve it, aren't you?", new Vector2(650, 150));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("将4个输入端口的值分别拷贝到对应的输出端口。");
        else dialogue.Play("Try to copy the values of 4 input ports to the output ports respectively.", new Vector2(650, 170));
        while (dialogue.isPlaying) yield return null;
        
        dialogue.Close(true);

        yield return new WaitForSeconds(0.2f);
        //helpCardPanel = GameUIManager.PopOutPanel(helpCardPanelPrefab).GetComponentInChildren<MyPanel>();
        //while (helpCardPanel.showing) yield return null;
        
        
        if (Settings.language == "CH") SetTarget("令B0=A0, B1=A1, B2=A2, B3=A3。");
        else SetTarget("Set B0 to A0, B1 to A1, B2 to A2, B3 to A3 respectively.");
        GameUIManager.UnFoldUI();
        yield return null;
    }

    public override void GenerateTestCase()
    {
        base.GenerateTestCase();
        //Debug.Log("Generating Test Case ... ... ");
        //grid.tileTable["A0"][0].UpdateValue(1);
       //grid.tileTable["A1"][0].UpdateValue(2);
        //grid.tileTable["A2"][0].UpdateValue(3);

        answerTable["B0"] = grid.tileTable["A0"][0].value;
        answerTable["B1"] = grid.tileTable["A1"][0].value;
        answerTable["B2"] = grid.tileTable["A2"][0].value;
        answerTable["B3"] = grid.tileTable["A3"][0].value;
    }
    protected override IEnumerator CheckCondition1()
    {
        conditionStatus[1] = (Global.stepCount <= 35);
        return base.CheckCondition1();
    }    
    protected override IEnumerator CheckCondition2()
    {
        conditionStatus[2] = (RemainingArrows() >= 25-16);
        return base.CheckCondition2();
    }
    int RemainingArrows()
    {
        return RemainingTiles(MyTile.Type.Arrow);
    }
}
