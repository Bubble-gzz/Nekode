using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle5_1 : PuzzleLogic
{
    // Start is called before the first frame update    // Start is called before the first frame update    TMP_Text debugInfo;
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
        DrBubble.instance.transform.position = new Vector3(30.7f, 31.6f, 0);
        yield return new WaitForSeconds(1f);
        dialogue.SetTailLM();
        dialogue.Open();
        
        if (Settings.language == "CH") dialogue.Play("目前为止不管输入是什么,小猫的路线总是固定的。");
        else dialogue.Play("So far our kitten's route is always the same regardless of the input.", new Vector2(760, 160));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("为了支持更加灵活的逻辑，我们决定引入逻辑方块。");
        else dialogue.Play("So we decide to introduce LOGIC TILES to add some flexibility.", new Vector2(760, 160));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("利用GEQ方块实现一个正负数分类器吧。");
        else dialogue.Play("Let's create a positive/negative number classifier with GEQ Tile.", new Vector2(760, 160));
        while (dialogue.isPlaying) yield return null;

        dialogue.Close(true);

        yield return new WaitForSeconds(0.2f);
        //helpCardPanel = GameUIManager.PopOutPanel(helpCardPanelPrefab).GetComponentInChildren<MyPanel>();
        //while (helpCardPanel.showing) yield return null;
        
        
        if (Settings.language == "CH") SetTarget("如果A>=0，则将1输出到B，否则将0输出到B");
        else SetTarget("If A is greater than or equal to 0, output 1 to B; otherwise, output 0 to B.");
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
            grid.tileTable["A"][0].UpdateValue(Random.Range(1, 100));
        }
        else if (curTestCase == 2)
        {
            grid.tileTable["A"][0].UpdateValue(0);            
        }
        else if (curTestCase == 3)
        {
            grid.tileTable["A"][0].UpdateValue(Random.Range(-99, 0));            
        }

        if (grid.tileTable["A"][0].value >= 0) answerTable["B"] = 1;
        else answerTable["B"] = 0;
    }
}
