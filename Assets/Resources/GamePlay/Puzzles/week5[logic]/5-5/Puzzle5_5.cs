using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle5_5 : PuzzleLogic
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
        totalTestCase = 3;
        StartCoroutine(GameProcess());
        conditionStatus[0] = true;
        conditionStatus[1] = true;
        conditionStatus[2] = true;
        
        conditions[0] = "Implement the comparator successfully.";
        conditions[1] = "You are so clever";
        conditions[2] = "You are a quick learner";

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
        
        if (Settings.language == "CH") dialogue.Play("求出两个数中较小的那一个");
        else dialogue.Play("Could you always find the smaller number?", new Vector2(600, 100));
        while (dialogue.isPlaying) yield return null;

        dialogue.Close(true);

        yield return new WaitForSeconds(0.2f);
        //helpCardPanel = GameUIManager.PopOutPanel(helpCardPanelPrefab).GetComponentInChildren<MyPanel>();
        //while (helpCardPanel.showing) yield return null;
        
        
        if (Settings.language == "CH") SetTarget("将A和B中的较小值输出到MIN。");
        else SetTarget("Output the smaller value between A and B to MIN.");
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
            int x = Random.Range(1, 100);
            int y = Random.Range(1, 100);
            while (x == y) y = Random.Range(1, 100);
            grid.tileTable["A"][0].UpdateValue(Mathf.Min(x, y));
            grid.tileTable["B"][0].UpdateValue(Mathf.Max(x, y));
        }
        else if (curTestCase == 2)
        {
            int x = Random.Range(1, 100);
            int y = Random.Range(1, 100);
            while (x == y) y = Random.Range(1, 100);
            grid.tileTable["A"][0].UpdateValue(Mathf.Max(x, y));
            grid.tileTable["B"][0].UpdateValue(Mathf.Min(x, y));    
        }
        else if (curTestCase == 3)
        {
            int x = Random.Range(1, 100);
            grid.tileTable["A"][0].UpdateValue(x);
            grid.tileTable["B"][0].UpdateValue(x);          
        }


        answerTable["MIN"] = Mathf.Min(grid.tileTable["A"][0].value, grid.tileTable["B"][0].value);
    }
}
