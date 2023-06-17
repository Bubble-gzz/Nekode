using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle4_6 : PuzzleLogic
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
        
        if (Settings.language == "CH") dialogue.Play("我们暂时没有足够的寄存器。");
        else dialogue.Play("We are short of registers currently.", new Vector2(600, 100));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("你可以用其它方法实现上次的交换器吗？");
        else dialogue.Play("Could you implement a swapper like before in another way?", new Vector2(600, 150));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("好消息是，我们这儿还有一些减法方块。");
        else dialogue.Play("Good news is that we have some SUB Tiles for you.", new Vector2(750, 150));
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
