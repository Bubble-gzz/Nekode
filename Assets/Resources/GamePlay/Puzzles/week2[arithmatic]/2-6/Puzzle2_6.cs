using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Puzzle2_6 : PuzzleLogic
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
        
        if (Settings.language == "CH") dialogue.Play("我们在研究数列。");
        else dialogue.Play("We are studying number sequences.", new Vector2(600, 100));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("现在有一个数列A，每一项都是前一项的两倍加2。");
        else dialogue.Play("Now we have a sequence A, where each term is obtained by doubling the previous term and adding 2.", new Vector2(600, 150));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("已知A0=1，也就是小猫现在所想的数。");
        else dialogue.Play("Given that A0=1, which is the number the kitten is currently thinking of", new Vector2(750, 150));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("我们想请你帮忙推导出A1~A6.");
        else dialogue.Play("We would like to ask for your help in deducing A1 to A6.", new Vector2(700, 150));
        while (dialogue.isPlaying) yield return null;

        dialogue.Close(true);

        yield return new WaitForSeconds(0.2f);
        //helpCardPanel = GameUIManager.PopOutPanel(helpCardPanelPrefab).GetComponentInChildren<MyPanel>();
        //while (helpCardPanel.showing) yield return null;
        
        
        if (Settings.language == "CH") SetTarget("求出A1~A6,其中A[i]=A[i-1]*2+2,已知A[0]=1");
        else SetTarget("Fill in A1~A6, where A[i]=A[i-1]*2+2, given that A[0]=1");
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

        answerTable["A1"] = 1 * 2 + 2;
        answerTable["A2"] = answerTable["A1"] * 2 + 2;
        answerTable["A3"] = answerTable["A2"] * 2 + 2;
        answerTable["A4"] = answerTable["A3"] * 2 + 2;
        answerTable["A5"] = answerTable["A4"] * 2 + 2;
        answerTable["A6"] = answerTable["A5"] * 2 + 2;
    }
}
