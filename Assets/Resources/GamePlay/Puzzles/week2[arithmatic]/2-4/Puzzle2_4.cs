using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Puzzle2_4 : PuzzleLogic
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
        
        conditions[0] = "Calculate correctly";
        conditions[1] = "Amazing job";
        conditions[2] = "Use no more than 3 add tiles";

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
        
        if (Settings.language == "CH") dialogue.Play("一个优秀的工程师应该能够灵活应对不同的地形。");
        else dialogue.Play("An excellent engineer should be adaptive to different terrains.", new Vector2(650, 150));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("分别将A0,A1,A2加上7,8,9,输出到对应的B0,B1,B2中");
        else dialogue.Play("Add A0,A1,A2 by 7,8,9 respectively and place the results into according output ports.", new Vector2(800, 170));
        while (dialogue.isPlaying) yield return null;

        dialogue.Close(true);

        yield return new WaitForSeconds(0.2f);
        //helpCardPanel = GameUIManager.PopOutPanel(helpCardPanelPrefab).GetComponentInChildren<MyPanel>();
        //while (helpCardPanel.showing) yield return null;
        
        
        if (Settings.language == "CH") SetTarget("分别将A0,A1,A2加上7,8,9,输出到对应的B0,B1,B2中");
        else SetTarget("Add A0,A1,A2 by 7,8,9 respectively and place the results into according output ports.");
        GameUIManager.UnFoldUI();
        yield return null;
    }
    protected override IEnumerator CheckCondition2()
    {
        conditionStatus[2] = (RemainingTiles(MyTile.Type.ADD) >= 3);
        return base.CheckCondition2();
    }
    public override void GenerateTestCase()
    {
        base.GenerateTestCase();
        //Debug.Log("Generating Test Case ... ... ");
        //grid.tileTable["A0"][0].UpdateValue(1);
       //grid.tileTable["A1"][0].UpdateValue(2);
        //grid.tileTable["A2"][0].UpdateValue(3);

        answerTable["B0"] = grid.tileTable["A0"][0].value + 7;
        answerTable["B1"] = grid.tileTable["A1"][0].value + 8;
        answerTable["B2"] = grid.tileTable["A2"][0].value + 9;
    }
}
