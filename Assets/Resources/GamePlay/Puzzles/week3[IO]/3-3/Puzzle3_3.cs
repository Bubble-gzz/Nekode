using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Puzzle3_3 : PuzzleLogic
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
        totalTestCase = 2;

        conditionStatus[0] = true;
        conditionStatus[1] = true;
        conditionStatus[2] = true;
        
        conditions[0] = "ADDER! ADDER! ADDER! ADDER!";
        conditions[1] = "ADDER! ADDER! ADDER! ADDER!";
        conditions[2] = "ADDER! ADDER! ADDER! ADDER!";

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
        DrBubble.instance.transform.position = new Vector3(28.3f, 28.3f, 0);
        yield return new WaitForSeconds(1f);
        dialogue.SetTailLL();
        dialogue.Open();
        
        if (Settings.language == "CH") dialogue.Play("我们打算量产加法器");
        else dialogue.Play("We plan to mass-produce adders.", new Vector2(660, 120));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("而量产意味着成本降低。");
        else dialogue.Play("And mass-production means lower cost.", new Vector2(750, 120));
        while (dialogue.isPlaying) yield return null;

        dialogue.Close(true);

        yield return new WaitForSeconds(0.2f);
        //helpCardPanel = GameUIManager.PopOutPanel(helpCardPanelPrefab).GetComponentInChildren<MyPanel>();
        //while (helpCardPanel.showing) yield return null;
        
        
        if (Settings.language == "CH") SetTarget("S0=A0+B0,S1=A1+B1,S2=A2+B2,S3=A3+B3");
        else SetTarget("S0=A0+B0,S1=A1+B1,S2=A2+B2,S3=A3+B3");
        GameUIManager.UnFoldUI();
        yield return null;
    }
    public override void GenerateTestCase()
    {
        base.GenerateTestCase();
        if (curTestCase == 1)
        {
            grid.tileTable["A0"][0].UpdateValue(1);
            grid.tileTable["B0"][0].UpdateValue(2);  
            grid.tileTable["A1"][0].UpdateValue(3);
            grid.tileTable["B1"][0].UpdateValue(4);  
            grid.tileTable["A2"][0].UpdateValue(5);
            grid.tileTable["B2"][0].UpdateValue(6);  
            grid.tileTable["A3"][0].UpdateValue(7);
            grid.tileTable["B3"][0].UpdateValue(8);  
        }
        else if (curTestCase == 2)
        {
            grid.tileTable["A0"][0].UpdateValue(5);
            grid.tileTable["B0"][0].UpdateValue(13);  
            grid.tileTable["A1"][0].UpdateValue(3);
            grid.tileTable["B1"][0].UpdateValue(6);  
            grid.tileTable["A2"][0].UpdateValue(100);
            grid.tileTable["B2"][0].UpdateValue(200);  
            grid.tileTable["A3"][0].UpdateValue(99);
            grid.tileTable["B3"][0].UpdateValue(99);          
        }
        //Debug.Log("Generating Test Case ... ... ");
        //grid.tileTable["A0"][0].UpdateValue(1);
       //grid.tileTable["A1"][0].UpdateValue(2);
        //grid.tileTable["A2"][0].UpdateValue(3);

        answerTable["S0"] = grid.tileTable["A0"][0].value + grid.tileTable["B0"][0].value;
        answerTable["S1"] = grid.tileTable["A1"][0].value + grid.tileTable["B1"][0].value;
        answerTable["S2"] = grid.tileTable["A2"][0].value + grid.tileTable["B2"][0].value;
        answerTable["S3"] = grid.tileTable["A3"][0].value + grid.tileTable["B3"][0].value;

    }
}
