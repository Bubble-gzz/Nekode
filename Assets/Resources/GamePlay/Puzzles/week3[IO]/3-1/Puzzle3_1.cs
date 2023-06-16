using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Puzzle3_1 : PuzzleLogic
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
        totalTestCase = 1;
        StartCoroutine(GameProcess());
        conditionStatus[0] = true;
        conditionStatus[1] = true;
        conditionStatus[2] = true;
        
        conditions[0] = "Complete the tutorial";
        conditions[1] = "Learn a new tool";
        conditions[2] = "Have a nice day";

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
        
        if (Settings.language == "CH") dialogue.Play("每当出现一条直线，就代表会有新鲜事物。");
        else dialogue.Play("Whenever a straight line appears, something new will be introduced.", new Vector2(600, 100));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("我称之为直线定律。");
        else dialogue.Play("I called it Law of Straight Line.", new Vector2(600, 150));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("没错，这次我们又为你准备了新道具。");
        else dialogue.Play("And of course we've got another new stuff for you this time.", new Vector2(750, 150));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("看到这三个透明的加法方块了吗？");
        else dialogue.Play("Look at these 3 transparent add tiles,", new Vector2(700, 150));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("透明意味着你没有权限编辑。");
        else dialogue.Play("Transparency means that you don't have permission to edit them.", new Vector2(700, 150));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("而我希望你将小猫的数字输出到端口A！");
        else dialogue.Play("But I want you to output the kitten's number to port A!", new Vector2(700, 150));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("为了完成这个不可能的任务，你需要了解一下IO的概念。");
        else dialogue.Play("To accomplish this impossible task, you need to learn about I/O.", new Vector2(700, 150));
        while (dialogue.isPlaying) yield return null;

        dialogue.Close(true);

        yield return new WaitForSeconds(0.2f);
        //helpCardPanel = GameUIManager.PopOutPanel(helpCardPanelPrefab).GetComponentInChildren<MyPanel>();
        //while (helpCardPanel.showing) yield return null;
        
        
        if (Settings.language == "CH") SetTarget("将小猫的数字输出到端口A");
        else SetTarget("Output kitten's number to port A.");
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

        answerTable["A"] = 31;
    }
}
