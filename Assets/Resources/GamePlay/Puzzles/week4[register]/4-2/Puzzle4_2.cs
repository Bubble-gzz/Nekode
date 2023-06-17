using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle4_2 : PuzzleLogic
{
    // Start is called before the first frame update    TMP_Text debugInfo;
    protected override void Awake()
    {
        base.Awake();
        //debugInfo = GameObject.Find("Debug")?.transform.Find("info").GetComponent<TMP_Text>();
    }
    override protected void Start()
    {
        base.Start();
        totalTestCase = 2;

        conditionStatus[0] = true;
        conditionStatus[1] = true;
        conditionStatus[2] = true;
        
        conditions[0] = "Amazing job!";
        conditions[1] = "You are so clever!";
        conditions[2] = "You are our hero!";

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
        
        if (Settings.language == "CH") dialogue.Play("似曾相识的送快递任务。");
        else dialogue.Play("We are studying number sequences.", new Vector2(600, 100));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("该怎么解决呢……");
        else dialogue.Play("Now we have a sequence A, where each term is obtained by doubling the previous term and adding 2.", new Vector2(600, 150));
        while (dialogue.isPlaying) yield return null;

        dialogue.Close(true);

        yield return new WaitForSeconds(0.2f);
        //helpCardPanel = GameUIManager.PopOutPanel(helpCardPanelPrefab).GetComponentInChildren<MyPanel>();
        //while (helpCardPanel.showing) yield return null;
        
        
        if (Settings.language == "CH") SetTarget("将A0的值输出到B0，将A1的值输出到B1。");
        else SetTarget("Output A0 to B0, A1 to B1 respectively");
        GameUIManager.UnFoldUI();
        yield return null;
    }
    public override void GenerateTestCase()
    {
        base.GenerateTestCase();
        if (curTestCase == 1)
        {
            grid.tileTable["A0"][0].UpdateValue(22);
            grid.tileTable["A1"][0].UpdateValue(33);
        }
        else {
            grid.tileTable["A0"][0].UpdateValue(55);
            grid.tileTable["A1"][0].UpdateValue(66); 
        }

        answerTable["B0"] = grid.tileTable["A0"][0].value;
        answerTable["B1"] = grid.tileTable["A1"][0].value;
    }
}
