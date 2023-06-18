using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Puzzle5_2 : PuzzleLogic
{
    //TMP_Text debugInfo;
    bool stepWithinRestriction;
    protected override void Awake()
    {
        base.Awake();
        //debugInfo = GameObject.Find("Debug")?.transform.Find("info").GetComponent<TMP_Text>();
    }
    override protected void Start()
    {
        base.Start();
        totalTestCase = 5;

        conditionStatus[0] = true;
        conditionStatus[1] = false;
        conditionStatus[2] = false;
        
        conditions[0] = "Implement the comparator successfully.";
        conditions[1] = "Finish each test case within 20 steps";
        conditions[2] = "Do not use SUB Tile";

    }
    
    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
        //debugInfo.text = "step:" + Global.stepCount;
    }
    protected override void OnTestStart()
    {
        stepWithinRestriction = true;
        base.OnTestStart();
    }
    public override void CheckAnswers()
    {
        if (Global.stepCount > 20) stepWithinRestriction = false;
        base.CheckAnswers();
    }

    override protected IEnumerator GameProcess()
    {
        yield return base.GameProcess();
        DrBubble.instance.transform.position = new Vector3(31.3f, 28.5f, 0);
        yield return new WaitForSeconds(1f);
        dialogue.SetTailLM();
        dialogue.Open();
        
        if (Settings.language == "CH") dialogue.Play("请实现一个比较器。");
        else dialogue.Play("Please implement a comparator.", new Vector2(650, 120));
        while (dialogue.isPlaying) yield return null;

        dialogue.Close(true);

        yield return new WaitForSeconds(0.2f);
        //helpCardPanel = GameUIManager.PopOutPanel(helpCardPanelPrefab).GetComponentInChildren<MyPanel>();
        //while (helpCardPanel.showing) yield return null;
        
        
        if (Settings.language == "CH") SetTarget("如果A=B，将1输出到F，否则将0输出到F。");
        else SetTarget("If A equals B, output 1 to F; otherwise, output 0 to F.");
        GameUIManager.UnFoldUI();
        yield return null;
    }
    protected override IEnumerator CheckCondition1()
    {
        conditionStatus[1] = stepWithinRestriction;
        return base.CheckCondition1();
    }
    protected override IEnumerator CheckCondition2()
    {
        conditionStatus[2] = (RemainingTiles(MyTile.Type.SUB) >= 1);
        return base.CheckCondition2();
    }
    public override void GenerateTestCase()
    {
        base.GenerateTestCase();
        //Debug.Log("Generating Test Case ... ... ");
        //grid.tileTable["A0"][0].UpdateValue(1);
       //grid.tileTable["A1"][0].UpdateValue(2);
        //grid.tileTable["A2"][0].UpdateValue(3);
        if (curTestCase == 1)
        {
            int x = Random.Range(1, 100);
            grid.tileTable["A"][0].UpdateValue(x);
            grid.tileTable["B"][0].UpdateValue(x);
        }
        else if (curTestCase == 2)
        {
            int x = Random.Range(-99, 0);
            grid.tileTable["A"][0].UpdateValue(x);
            grid.tileTable["B"][0].UpdateValue(x);    
        }
        else if (curTestCase == 3)
        {
            grid.tileTable["A"][0].UpdateValue(Random.Range(-99, 0));
            grid.tileTable["B"][0].UpdateValue(Random.Range(1, 100));          
        }
        else if (curTestCase == 4)
        {
            int x = Random.Range(1, 100);
            int y = Random.Range(1, 100);
            while (x == y) y = Random.Range(1, 100);
            grid.tileTable["A"][0].UpdateValue(x);
            grid.tileTable["B"][0].UpdateValue(y);
        }
        else if (curTestCase == 5)
        {
            grid.tileTable["A"][0].UpdateValue(0);
            grid.tileTable["B"][0].UpdateValue(Random.Range(1, 100));
        }


        if (grid.tileTable["A"][0].value == grid.tileTable["B"][0].value) answerTable["F"] = 1;
        else answerTable["F"] = 0;
    }
}
