using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle4_3 : PuzzleLogic
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
        totalTestCase = 1;
        StartCoroutine(GameProcess());
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

        //helpCardPanel = GameUIManager.PopOutPanel(helpCardPanelPrefab).GetComponentInChildren<MyPanel>();
        //while (helpCardPanel.showing) yield return null;
        
        
        if (Settings.language == "CH") SetTarget("完成计算：B0=A*3, B1=A*5, B2=A*7");
        else SetTarget("Complete the calculation: B0=A*3, B1=A*5, B2=A*7");
        GameUIManager.UnFoldUI();
        yield return null;
    }
    public override void GenerateTestCase()
    {
        base.GenerateTestCase();
        //Debug.Log("Generating Test Case ... ... ");
        //grid.tileTable["A0"][0].UpdateValue(1);
       //grid.tileTable["A1"][0].UpdateValue(2);
        grid.tileTable["A"][0].UpdateValue(1);

        answerTable["B0"] = grid.tileTable["A"][0].value * 3;
        answerTable["B1"] = grid.tileTable["A"][0].value * 5;
        answerTable["B2"] = grid.tileTable["A"][0].value * 7;
    }
}
