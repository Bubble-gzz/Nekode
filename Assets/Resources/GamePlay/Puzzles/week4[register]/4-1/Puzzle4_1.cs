using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle4_1 : PuzzleLogic
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

        dialogue.Open();
        
        if (Settings.language == "CH") dialogue.Play("你可能已经注意到，目前为止保存数据仍然很困难。");
        else dialogue.Play("You may have noticed that storing data is still challenging at present.", new Vector2(600, 100));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("比如我想让你将A设为22，B设为33。");
        else dialogue.Play("Say, I would like you to set A to 22 and B to 33.", new Vector2(600, 150));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("你将不得不考虑数据丢失的问题。");
        else dialogue.Play("You will have to face the problem of data loss.", new Vector2(750, 150));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("所以我们向你介绍一个新的工具——");
        else dialogue.Play("So let me introduce a new tool for you--", new Vector2(700, 150));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("别管直线定律了吧。");
        else dialogue.Play("just forget about the Law of Straight Line.", new Vector2(700, 150));
        while (dialogue.isPlaying) yield return null;

        dialogue.Close(true);

        yield return new WaitForSeconds(0.2f);
        //helpCardPanel = GameUIManager.PopOutPanel(helpCardPanelPrefab).GetComponentInChildren<MyPanel>();
        //while (helpCardPanel.showing) yield return null;
        
        
        if (Settings.language == "CH") SetTarget("将A设置为22, B设置为33");
        else SetTarget("Set A to 22 and B to 33 respectively.");
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

        answerTable["A"] = 22;
        answerTable["B"] = 33;
    }
}
