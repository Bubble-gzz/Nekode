using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle5_6 : PuzzleLogic
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
        
        conditions[0] = "Congratulations!";
        conditions[1] = "You complete the challenge!";
        conditions[2] = "You are our hero!";

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
        
        if (Settings.language == "CH") dialogue.Play("最近我们在办喵喵好声音");
        else dialogue.Play("Recently we are planning to hold a singing contest.", new Vector2(600, 100));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("你可以帮我们设计一个投票系统吗");
        else dialogue.Play("Could you please design a voting system for us?", new Vector2(600, 100));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("简单来说就是找出三个数中出现次数最多的那个数。");
        else dialogue.Play("In simple terms, find the number that appears most frequently among the three numbers.", new Vector2(600, 100));
        while (dialogue.isPlaying) yield return null;

        dialogue.Close(true);

        yield return new WaitForSeconds(0.2f);
        //helpCardPanel = GameUIManager.PopOutPanel(helpCardPanelPrefab).GetComponentInChildren<MyPanel>();
        //while (helpCardPanel.showing) yield return null;
        
        
        if (Settings.language == "CH") SetTarget("将三个数中出现次数最多的数输出到M，如果不存在，输出0.");
        else SetTarget("Output the number with the highest frequency among the three numbers to M. If it doesn't exist, output 0.");
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
        int x = Random.Range(1, 100);
        int y = Random.Range(1, 100);
        int z = Random.Range(1, 100);
        while (x == y) y = Random.Range(1, 100);
        while (z == x || z == y) z =Random.Range(1, 100);
        if (curTestCase == 1)
        {
            grid.tileTable["A"][0].UpdateValue(x);
            grid.tileTable["B"][0].UpdateValue(x);
            grid.tileTable["C"][0].UpdateValue(y);
            answerTable["M"] = x;
        }
        else if (curTestCase == 2)
        {
            grid.tileTable["A"][0].UpdateValue(x);
            grid.tileTable["B"][0].UpdateValue(y);
            grid.tileTable["C"][0].UpdateValue(x);  
            answerTable["M"] = x;
        }
        else if (curTestCase == 3)
        {
            grid.tileTable["A"][0].UpdateValue(y);
            grid.tileTable["B"][0].UpdateValue(x);
            grid.tileTable["C"][0].UpdateValue(x);   
            answerTable["M"] = x;
        }
        else if (curTestCase == 4)
        {
            grid.tileTable["A"][0].UpdateValue(x);
            grid.tileTable["B"][0].UpdateValue(x);
            grid.tileTable["C"][0].UpdateValue(x);
            answerTable["M"] = x;               
        }
        else if (curTestCase == 5)
        {
            grid.tileTable["A"][0].UpdateValue(x);
            grid.tileTable["B"][0].UpdateValue(y);
            grid.tileTable["C"][0].UpdateValue(z);  
            answerTable["M"] = 0;              
        }
    }
}
