using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Puzzle3_2 : PuzzleLogic
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
        totalTestCase = 3;

        conditionStatus[0] = true;
        conditionStatus[1] = true;
        conditionStatus[2] = true;
        
        conditions[0] = "ADDER!";
        conditions[1] = "ADDER!";
        conditions[2] = "ADDER!";

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
        DrBubble.instance.transform.position = new Vector3(29.6f, 31.6f, 0);
        yield return new WaitForSeconds(1f);
        dialogue.SetTailLL();
        dialogue.Open();
        
        if (Settings.language == "CH") dialogue.Play("我此刻很激动。");
        else dialogue.Play("I'm exciting right now.", new Vector2(460, 120));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("因为我们终于要有一个加法器了。");
        else dialogue.Play("Because we are finally going to have an ADDER.", new Vector2(700, 160));
        while (dialogue.isPlaying) yield return null;

        dialogue.Close(true);

        yield return new WaitForSeconds(0.2f);
        //helpCardPanel = GameUIManager.PopOutPanel(helpCardPanelPrefab).GetComponentInChildren<MyPanel>();
        //while (helpCardPanel.showing) yield return null;
        
        
        if (Settings.language == "CH") SetTarget("将A+B的结果输出到S。");
        else SetTarget("Output the sum of A and B to port S.");
        GameUIManager.UnFoldUI();
        yield return null;
    }
    public override void GenerateTestCase()
    {
        base.GenerateTestCase();
        if (curTestCase == 1)
        {
            grid.tileTable["A"][0].UpdateValue(2);
            grid.tileTable["B"][0].UpdateValue(3);  
        }
        else if (curTestCase == 2)
        {
            grid.tileTable["A"][0].UpdateValue(11);
            grid.tileTable["B"][0].UpdateValue(12);              
        }
        else if (curTestCase == 3)
        {
            grid.tileTable["A"][0].UpdateValue(-5);
            grid.tileTable["B"][0].UpdateValue(100);  
        }
        //Debug.Log("Generating Test Case ... ... ");
        //grid.tileTable["A0"][0].UpdateValue(1);
       //grid.tileTable["A1"][0].UpdateValue(2);
        //grid.tileTable["A2"][0].UpdateValue(3);

        answerTable["S"] = grid.tileTable["A"][0].value + grid.tileTable["B"][0].value;
    }
}
