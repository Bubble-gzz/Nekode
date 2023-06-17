using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle5_3 : PuzzleLogic
{
    //TMP_Text debugInfo;
    protected override void Awake()
    {
        base.Awake();
        //debugInfo = GameObject.Find("Debug")?.transform.Find("info").GetComponent<TMP_Text>();
    }
    override protected void Start()
    {
        base.Start();
        totalTestCase = 5;
        StartCoroutine(GameProcess());
        conditionStatus[0] = true;
        conditionStatus[1] = true;
        conditionStatus[2] = true;
        
        conditions[0] = "Implement the filter successfully.";
        conditions[1] = "Good job";
        conditions[2] = "Have a nice day";

    }
    
    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
        //debugInfo.text = "step:" + Global.stepCount;
    }

    override protected IEnumerator GameProcess()
    {
        yield return base.GameProcess();

        dialogue.Open();
        
        if (Settings.language == "CH") dialogue.Play("请实现一个过滤器，筛掉那些不在[10,20]之间的数。");
        else dialogue.Play("Please implement a comparator.", new Vector2(600, 100));
        while (dialogue.isPlaying) yield return null;

        dialogue.Close(true);

        yield return new WaitForSeconds(0.2f);
        //helpCardPanel = GameUIManager.PopOutPanel(helpCardPanelPrefab).GetComponentInChildren<MyPanel>();
        //while (helpCardPanel.showing) yield return null;
        
        
        if (Settings.language == "CH") SetTarget("如果A在闭区间[10,20]内，则将A输出到B，否则将0输出到B。");
        else SetTarget("If A is within the closed interval [10, 20], output A to B; otherwise, output 0 to B.");
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
        if (curTestCase == 1)
        {
            grid.tileTable["A"][0].UpdateValue(Random.Range(11, 20));
        }
        else if (curTestCase == 2)
        {
            grid.tileTable["A"][0].UpdateValue(Random.Range(-100, 10));
        }
        else if (curTestCase == 3)
        {
            grid.tileTable["A"][0].UpdateValue(10);
        }
        else if (curTestCase == 4)
        {
            grid.tileTable["A"][0].UpdateValue(Random.Range(21, 100));
        }
        else if (curTestCase == 5)
        {
            grid.tileTable["A"][0].UpdateValue(20);
        }
        int a = grid.tileTable["A"][0].value;
        if (a >= 10 && a <= 20) answerTable["B"] = a;
        else answerTable["B"] = 0;
    }
}
