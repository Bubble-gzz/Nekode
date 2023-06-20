using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle10to2 : PuzzleLogic
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
        totalTestCase = 8;

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
        DrBubble.instance.transform.position = new Vector3(30.7f, 28.5f, 0);
        yield return new WaitForSeconds(1f);
        dialogue.SetTailLM();
        dialogue.Open();
        
        if (Settings.language == "CH") dialogue.Play("你知道二进制表示吗？");
        else dialogue.Play("Do you know binary representation?", new Vector2(700, 120));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("它只用0和1来表示数字");
        else dialogue.Play("It represents numbers with just 0 and 1.", new Vector2(780, 120));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("举个例子，在二进制下，你是这样从0数到7的：");
        else dialogue.Play("For example, in binary form, you can count from 0 to 7 like this:", new Vector2(700, 160));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("000, 001, 010, 011, 100, 101, 110, 111.");
        else dialogue.Play("000, 001, 010, 011, 100, 101, 110, 111.", new Vector2(650, 120));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("现在试试将输入的数转化为二进制数位吧！");
        else dialogue.Play("Now try to make a machine converting the input number to binary bits!", new Vector2(800, 160));
        while (dialogue.isPlaying) yield return null;
        
        dialogue.Close(true);

        yield return new WaitForSeconds(0.2f);
        //helpCardPanel = GameUIManager.PopOutPanel(helpCardPanelPrefab).GetComponentInChildren<MyPanel>();
        //while (helpCardPanel.showing) yield return null;
        
        
        if (Settings.language == "CH") SetTarget("Convert x to binary form (b[2]b[1]b[0])2, and save the bits to b2,b1,b0 respectively.");
        else SetTarget("Convert X(0<=X<=7) to binary form (b[2]b[1]b[0])<sub>2</sub>, and\nsave the bits to B2,B1,B0 respectively.");
        GameUIManager.UnFoldUI();
        yield return null;
    }
    public override void GenerateTestCase()
    {
        base.GenerateTestCase();
        
        grid.tileTable["X"][0].UpdateValue(curTestCase);


        answerTable["B2"] = grid.tileTable["X"][0].value >> 2;
        answerTable["B1"] = (grid.tileTable["X"][0].value >> 1) & 1;
        answerTable["B0"] = grid.tileTable["X"][0].value & 1;
    
    }
}
