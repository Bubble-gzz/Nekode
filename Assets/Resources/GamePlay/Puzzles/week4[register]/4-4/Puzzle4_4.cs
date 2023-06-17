using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle4_4 : PuzzleLogic
{
    // Start is called before the first frame update
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

        dialogue.Open();
        
        if (Settings.language == "CH") dialogue.Play("就像加法方块那样，寄存器也是可以设置初始值的。");
        else dialogue.Play("Registers can be assigned with initial value just like ADD Tiles do.", new Vector2(600, 100));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("利用寄存器的特性，你可以设置一些有用的常量。");
        else dialogue.Play("You can set some useful constants using this feature.", new Vector2(600, 150));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("这次的目标是做出一个奇怪的加法器。");
        else dialogue.Play("This time, let's make a strange adder.", new Vector2(750, 150));
        while (dialogue.isPlaying) yield return null;

        dialogue.Close(true);

        yield return new WaitForSeconds(0.2f);
        //helpCardPanel = GameUIManager.PopOutPanel(helpCardPanelPrefab).GetComponentInChildren<MyPanel>();
        //while (helpCardPanel.showing) yield return null;
        
        
        if (Settings.language == "CH") SetTarget("完成计算：S0=A0+B0,S1=A1+B1");
        else SetTarget("Complete the calculation: S0=A0+B0,S1=A1+B1");
        GameUIManager.UnFoldUI();
        yield return null;
    }
    public override void GenerateTestCase()
    {
        base.GenerateTestCase();
        if (curTestCase == 1)
        {
            grid.tileTable["A0"][0].UpdateValue(5);
            grid.tileTable["B0"][0].UpdateValue(7);
            grid.tileTable["A1"][0].UpdateValue(8);
            grid.tileTable["B1"][0].UpdateValue(10);
        }
        else if (curTestCase == 2) {
            grid.tileTable["A0"][0].UpdateValue(11);
            grid.tileTable["B0"][0].UpdateValue(22);
            grid.tileTable["A1"][0].UpdateValue(100);
            grid.tileTable["B1"][0].UpdateValue(200);
        }
        else {
            grid.tileTable["A0"][0].UpdateValue(5);
            grid.tileTable["B0"][0].UpdateValue(-5);
            grid.tileTable["A1"][0].UpdateValue(99);
            grid.tileTable["B1"][0].UpdateValue(1);
        }

        answerTable["S0"] = grid.tileTable["A0"][0].value + grid.tileTable["B0"][0].value;
        answerTable["S1"] = grid.tileTable["A1"][0].value + grid.tileTable["B1"][0].value;
    }
}
