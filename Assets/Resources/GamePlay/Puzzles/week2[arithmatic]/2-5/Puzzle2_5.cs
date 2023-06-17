using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Puzzle2_5 : PuzzleLogic
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

        conditionStatus[0] = true;
        conditionStatus[1] = true;
        conditionStatus[2] = false;
        
        conditions[0] = "Calculate correctly";
        conditions[1] = "Good job";
        conditions[2] = "Use no more than 2 add tiles";

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
        
        if (Settings.language == "CH") dialogue.Play("在猫咪的日常生活中，前天，昨天，今天，后天，明天是很重要的日子。");
        else dialogue.Play("In the daily life of a cat, the day before yesterday, yesterday, today, the day after tomorrow, and tomorrow are important days.", new Vector2(850, 200));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("如果抽象成数字，就是要知道一个数附近的5个数。");
        else dialogue.Play("In abstract form, it is about knowing the five numbers surrounding a given number.", new Vector2(800, 170));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("我们想请你设计出这样的日历表。");
        else dialogue.Play("Could you please help us design such a lookup table?", new Vector2(800, 170));
        while (dialogue.isPlaying) yield return null;

        dialogue.Close(true);

        yield return new WaitForSeconds(0.2f);
        //helpCardPanel = GameUIManager.PopOutPanel(helpCardPanelPrefab).GetComponentInChildren<MyPanel>();
        //while (helpCardPanel.showing) yield return null;
        
        
        if (Settings.language == "CH") SetTarget("求出A-2,A-1,A,A+1,A+2,分别输出到L2,L1,M,R1,R2中");
        else SetTarget("Output A-2,A-1,A,A+1,A+2 to L2,L1,M,R1,R2 respectively.");
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

        answerTable["L2"] = grid.tileTable["A"][0].value - 2;
        answerTable["L1"] = grid.tileTable["A"][0].value - 1;
        answerTable["M"] = grid.tileTable["A"][0].value;
        answerTable["R1"] = grid.tileTable["A"][0].value + 1;
        answerTable["R2"] = grid.tileTable["A"][0].value + 2;
    }
}
