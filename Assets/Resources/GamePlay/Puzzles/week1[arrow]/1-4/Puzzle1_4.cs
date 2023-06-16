using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Puzzle1_4 : PuzzleLogic
{
    TMP_Text debugInfo;
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
        
        conditions[0] = "Set all 3 output ports to 1.";
        conditions[1] = "Can not come up with other stuff here...";
        conditions[2] = "You made it!";

        //GameMessage.ToolReturnedToSlot.AddListener(ToolReturnedToSlot);
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
        
        if (Settings.language == "CH") dialogue.Play("抱歉，我似乎忘了告诉你一件重要的事……");
        else dialogue.Play("Oops, forgot to tell you something important", new Vector2(650, 150));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") {
            dialogue.Play("其实一个方格的四条边是互相独立的，所以可以同时放置箭头。");
            while (dialogue.isPlaying) yield return null;
        }
        else {
            dialogue.Play("In fact, the four sides of a tile are independent of each other.", new Vector2(650, 170));
            while (dialogue.isPlaying) yield return null;
            dialogue.Play("So at most 4 arrows can be placed in one tile simultaneously.", new Vector2(650, 170));
            while (dialogue.isPlaying) yield return null;
        }

        if (Settings.language == "CH") dialogue.Play("小猫离开方格的方向取决于他面前的那个箭头。");
        else dialogue.Play("The direction the kitten leaves the grid depends on the arrow in front of him.", new Vector2(650, 150));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("抱歉我可能说的不太清楚，你试试就知道啦。");
        else dialogue.Play("Apologies if my explanation wasn't clear enough. Perhaps you'll grasp it better through practical experience.", new Vector2(700, 200));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("像上次那样，试着将3个输出端口置为1吧！");
        else dialogue.Play("Now let's try to set the 3 output ports to 1 just like before.", new Vector2(650, 150));
        while (dialogue.isPlaying) yield return null;
        
        dialogue.Close(true);

        yield return new WaitForSeconds(0.2f);
        //helpCardPanel = GameUIManager.PopOutPanel(helpCardPanelPrefab).GetComponentInChildren<MyPanel>();
        //while (helpCardPanel.showing) yield return null;
        
        
        if (Settings.language == "CH") SetTarget("将三个输出端口的值全都设置为1");
        else SetTarget("Set the values of all 3 output ports to 1");
        GameUIManager.UnFoldUI();
        yield return null;
    }

    public override void GenerateTestCase()
    {
        base.GenerateTestCase();
        //Debug.Log("Generating Test Case ... ... ");
        //grid.tileTable["A"][0].UpdateValue(100);
        answerTable["F0"] = 1;
        answerTable["F1"] = 1;
        answerTable["F2"] = 1;   
    }
}
