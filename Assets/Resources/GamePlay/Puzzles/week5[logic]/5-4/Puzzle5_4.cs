using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Puzzle5_4 : PuzzleLogic
{
    TMP_Text debugInfo;
    int logicTilesCount;
    bool logicTileFlag;
    protected override void Awake()
    {
        base.Awake();
        debugInfo = GameObject.Find("Debug")?.transform.Find("info").GetComponent<TMP_Text>();
        GameMessage.OnPassLogicTile.AddListener(OnPassLogicTile);
    }
    override protected void Start()
    {
        base.Start();
        totalTestCase = 8;

        conditionStatus[0] = true;
        conditionStatus[1] = true;
        conditionStatus[2] = false;
        
        conditions[0] = "Implement the demultiplexer successfully.";
        conditions[1] = "Great job!";
        conditions[2] = "Pass GEQ Tiles no more than 4 times in any single test case.";

    }
    
    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
        debugInfo.text = "pass logic tiles:" + logicTilesCount;
    }
    protected override void OnTestStart()
    {
        logicTilesCount = 0;
        logicTileFlag = true;
        base.OnTestStart();
    }
    void OnPassLogicTile()
    {
        logicTilesCount++;
    }
    public override void CheckAnswers()
    {
        if (logicTilesCount > 4) logicTileFlag = false;
        base.CheckAnswers();
    }
    override protected IEnumerator GameProcess()
    {
        yield return base.GameProcess();
        DrBubble.instance.transform.position = new Vector3(27f, 29.2f, 0);
        yield return new WaitForSeconds(1f);
        dialogue.SetTailLL();
        dialogue.Open();
        
        if (Settings.language == "CH") dialogue.Play("这次的目标是做一个多路输出选择器。");
        else dialogue.Play("This time we want you to make a demultiplexer.", new Vector2(600, 160));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("通俗一点说，数字是多少，就点亮对应编号的灯！");
        else dialogue.Play("To put it simple, just turn on the corresponding light that matches the number!", new Vector2(580, 200));
        while (dialogue.isPlaying) yield return null;

        dialogue.Close(true);

        yield return new WaitForSeconds(0.2f);
        //helpCardPanel = GameUIManager.PopOutPanel(helpCardPanelPrefab).GetComponentInChildren<MyPanel>();
        //while (helpCardPanel.showing) yield return null;
        
        
        if (Settings.language == "CH") SetTarget("仅仅将F[A]设置为1");
        else SetTarget("Only set F[A] to 1");
        GameUIManager.UnFoldUI();
        yield return null;
    }
    protected override IEnumerator CheckCondition2()
    {
        conditionStatus[2] = logicTileFlag;
        return base.CheckCondition2();
    }
    public override void GenerateTestCase()
    {
        logicTilesCount = 0;
        base.GenerateTestCase();
        //Debug.Log("Generating Test Case ... ... ");
        //grid.tileTable["A0"][0].UpdateValue(1);
       //grid.tileTable["A1"][0].UpdateValue(2);
        //grid.tileTable["A2"][0].UpdateValue(3);
        grid.tileTable["A"][0].UpdateValue(curTestCase-1);

        
        for(int i = 0; i < 8; i++)
            if (i != curTestCase - 1)
                answerTable["F"+i.ToString()] = 0;
            else
                answerTable["F"+i.ToString()] = 1;
    }
}
