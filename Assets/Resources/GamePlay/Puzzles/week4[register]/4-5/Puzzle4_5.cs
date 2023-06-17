using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle4_5 : PuzzleLogic
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
        totalTestCase = 3;

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
        
        if (Settings.language == "CH") dialogue.Play("有了寄存器，交换数据变得很方便。");
        else dialogue.Play("It becomes convenient to swap data with the help of register.", new Vector2(600, 100));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("试试交换A和B中的数据");
        else dialogue.Play("Try swapping the data stored in A and B.", new Vector2(600, 150));
        while (dialogue.isPlaying) yield return null;

        dialogue.Close(true);

        yield return new WaitForSeconds(0.2f);
        //helpCardPanel = GameUIManager.PopOutPanel(helpCardPanelPrefab).GetComponentInChildren<MyPanel>();
        //while (helpCardPanel.showing) yield return null;
        
        
        if (Settings.language == "CH") SetTarget("交换A和B中的数据");
        else SetTarget("Swap the data stored in A and B");
        GameUIManager.UnFoldUI();
        yield return null;
    }
    public override void GenerateTestCase()
    {
        base.GenerateTestCase();
        if (curTestCase == 1)
        {
            grid.tileTable["A"][0].UpdateValue(5);
            grid.tileTable["B"][0].UpdateValue(7);
        }
        else if (curTestCase == 2) {
            grid.tileTable["A"][0].UpdateValue(1);
            grid.tileTable["B"][0].UpdateValue(4);
        }
        else {
            grid.tileTable["A"][0].UpdateValue(10);
            grid.tileTable["B"][0].UpdateValue(100);
        }

        answerTable["A"] = grid.tileTable["B"][0].value;
        answerTable["B"] = grid.tileTable["A"][0].value;
    }
}
