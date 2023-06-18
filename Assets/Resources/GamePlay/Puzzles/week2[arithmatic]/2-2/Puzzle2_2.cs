using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Puzzle2_2 : PuzzleLogic
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
        totalTestCase = 3;

        conditionStatus[0] = true;
        conditionStatus[1] = true;
        conditionStatus[2] = false;
        
        conditions[0] = "Calculate correctly";
        conditions[1] = "Just a placeholder";
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
        DrBubble.instance.transform.position = new Vector3(30.7f, 28.5f, 0);
        yield return new WaitForSeconds(1f);
        dialogue.SetTailLM();
        dialogue.Open();
        
        if (Settings.language == "CH") dialogue.Play("第二课，会让你更加熟练地使用加法方块！");
        else dialogue.Play("Lesson 2, try to get more familiar with ADD TILE!", new Vector2(750, 160));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("依然是给输入加上一个数，而这次我们有三对端口。");
        else dialogue.Play("Once again, let's add a number to the input, and this time we have three pairs of ports.", new Vector2(650, 200));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("分别将A0,A1,A2加上5，输出到对应的B0,B1,B2中。");
        else dialogue.Play("Add A0,A1,A2 by 5 and place the results into according output ports.", new Vector2(820, 160));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("对了，可以试试给出更加节约资源的方案哦");
        else dialogue.Play("By the way, we would be happy to see a more economical solution.", new Vector2(800, 160));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("此外，我们首次采用了多组测试数据来保证您的设计的可靠性");
        else dialogue.Play("In addition, we have incorporated multiple test cases for the first time to ensure the reliability of your design.", new Vector2(800, 200));
        while (dialogue.isPlaying) yield return null;

        dialogue.Close(true);

        yield return new WaitForSeconds(0.2f);
        //helpCardPanel = GameUIManager.PopOutPanel(helpCardPanelPrefab).GetComponentInChildren<MyPanel>();
        //while (helpCardPanel.showing) yield return null;
        
        
        if (Settings.language == "CH") SetTarget("将A0,A1,A2的值分别加上5，输出到对应的B0,B1,B2中。");
        else SetTarget("Add the values of A0, A1, A2 by 5 and output them to B0, B1, B2 respectively.");
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
        if (curTestCase == 1)
        {
            grid.tileTable["A0"][0].UpdateValue(1);
            grid.tileTable["A1"][0].UpdateValue(2);
            grid.tileTable["A2"][0].UpdateValue(3);           
        }
        else if (curTestCase == 2)
        {
            grid.tileTable["A0"][0].UpdateValue(2);
            grid.tileTable["A1"][0].UpdateValue(3);
            grid.tileTable["A2"][0].UpdateValue(1);              
        }
        else if (curTestCase == 3)
        {
            grid.tileTable["A0"][0].UpdateValue(3);
            grid.tileTable["A1"][0].UpdateValue(1);
            grid.tileTable["A2"][0].UpdateValue(2);    
        }
        //Debug.Log("Generating Test Case ... ... ");
        //grid.tileTable["A0"][0].UpdateValue(1);
       //grid.tileTable["A1"][0].UpdateValue(2);
        //grid.tileTable["A2"][0].UpdateValue(3);

        answerTable["B0"] = grid.tileTable["A0"][0].value + 5;
        answerTable["B1"] = grid.tileTable["A1"][0].value + 5;
        answerTable["B2"] = grid.tileTable["A2"][0].value + 5;
    }
}
