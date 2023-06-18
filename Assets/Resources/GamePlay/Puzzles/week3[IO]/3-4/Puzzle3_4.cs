using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Puzzle3_4 : PuzzleLogic
{

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
        
        conditions[0] = "DOUBLE ! DOUBLE ! ";
        conditions[1] = "DOUBLE ! DOUBLE ! ";
        conditions[2] = "DOUBLE ! DOUBLE ! ";

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
        DrBubble.instance.transform.position = new Vector3(30.8f, 31.6f, 0);
        yield return new WaitForSeconds(1f);
        dialogue.SetTailLM();
        dialogue.Open();
        
        if (Settings.language == "CH") dialogue.Play("今天我们来做双倍放大器！");
        else dialogue.Play("Today let's make a double amplifier!", new Vector2(720, 120));
        while (dialogue.isPlaying) yield return null;

        dialogue.Close(true);

        yield return new WaitForSeconds(0.2f);
        //helpCardPanel = GameUIManager.PopOutPanel(helpCardPanelPrefab).GetComponentInChildren<MyPanel>();
        //while (helpCardPanel.showing) yield return null;
        
        
        if (Settings.language == "CH") SetTarget("B=A*2");
        else SetTarget("B=A*2");
        GameUIManager.UnFoldUI();
        yield return null;
    }
    public override void GenerateTestCase()
    {
        base.GenerateTestCase();
        if (curTestCase == 1)
        {
            grid.tileTable["A"][0].UpdateValue(3);
        }
        else if (curTestCase == 2)
        {
            grid.tileTable["A"][0].UpdateValue(15);      
        }
        else if (curTestCase == 3)
        {
            grid.tileTable["A"][0].UpdateValue(40);
        }
        //Debug.Log("Generating Test Case ... ... ");
        //grid.tileTable["A0"][0].UpdateValue(1);
       //grid.tileTable["A1"][0].UpdateValue(2);
        //grid.tileTable["A2"][0].UpdateValue(3);

        answerTable["B"] = grid.tileTable["A"][0].value * 2;

    }
}
