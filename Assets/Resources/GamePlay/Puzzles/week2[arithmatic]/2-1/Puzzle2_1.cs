using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Puzzle2_1 : PuzzleLogic
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
        conditionStatus[2] = true;
        
        conditions[0] = "Learn to use ADD TILE";
        conditions[1] = "Good job";
        conditions[2] = "Find something new";

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
        DrBubble.instance.transform.position = new Vector3(30.7f, 31.5f, 0);
        yield return new WaitForSeconds(1f);
        dialogue.SetTailLM();
        dialogue.Open();
        
        if (Settings.language == "CH") dialogue.Play("恭喜你学完了基本的知识。");
        else dialogue.Play("Congratulations on covering the basics.", new Vector2(760, 120));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("下面让我们做一些比拷贝更有意义的事情。");
        else dialogue.Play("Now let's do something more meaningful than simply copying.", new Vector2(750, 160));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("看到输入端口了吗？");
        else dialogue.Play("Look at the input port,", new Vector2(520, 120));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("请你将它的值加1后放到输出端口。");
        else dialogue.Play("Try to add 1 to its value and place the result in the output port.", new Vector2(750, 160));
        while (dialogue.isPlaying) yield return null;
        
        if (Settings.language == "CH") dialogue.Play("这需要用到新的工具——");
        else dialogue.Play("The job will involve a new tool--", new Vector2(650, 120));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("加法方块!");
        else dialogue.Play("ADD TILE!", new Vector2(300, 120));
        while (dialogue.isPlaying) yield return null;

        dialogue.Close(true);

        yield return new WaitForSeconds(0.2f);
        helpCardPanel = GameUIManager.PopOutPanel(helpCardPanelPrefab).GetComponentInChildren<MyPanel>();
        yield return new WaitForSeconds(0.5f);
        while (helpCardPanel.showing) yield return null;
        
        if (Settings.language == "CH") SetTarget("将A的值加一后输出到B");
        else SetTarget("Add the value of A by 1 and output it to B.");
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
        //grid.tileTable["A"][0].UpdateValue(curTestCase);

        answerTable["B"] = grid.tileTable["A"][0].value + 1;
    }
}
