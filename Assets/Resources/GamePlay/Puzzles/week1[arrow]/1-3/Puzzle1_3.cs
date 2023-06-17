using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Puzzle1_3 : PuzzleLogic
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
        conditionStatus[1] = false;
        conditionStatus[2] = false;
        
        conditions[0] = "Set all 3 output ports to 1.";
        conditions[1] = "Finish the task in 21 steps";
        conditions[2] = "Use no more than 11 arrows";

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
        DrBubble.instance.transform.position = new Vector3(25f, 34f, 0);
        yield return new WaitForSeconds(1f);
        MyCamera camera = Global.mainCam.GetComponent<MyCamera>();
        Vector3 pos = Global.currentNeko.transform.position + new Vector3(-0.5f, -0.5f, 0);
        pos.z = -10;
        camera.transform.DOMove(pos, 1.5f).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(1.5f);
        dialogue.SetTailLR();
        dialogue.Open();
        
        if (Settings.language == "CH") dialogue.Play("聪明如你，想必你已经能够熟练使用箭头了。");
        else dialogue.Play("As a quick learner, you must have been able to use the arrows skilfully.", new Vector2(850, 150));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("看到散落在棋盘上的三个输出端口了吗？");
        else dialogue.Play("Here we have 3 output ports on the grid.", new Vector2(780, 120));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("你能将它们的值都设置为1吗？");
        else dialogue.Play("Try to set all of them to 1.", new Vector2(550, 120));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("如果你对转向还有些疑惑，随时点击右上角的卡片！");
        else dialogue.Play("You can check the manual in the upper-right corner anytime.", new Vector2(750, 160));
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

    protected override IEnumerator CheckCondition1()
    {
        conditionStatus[1] = (Global.stepCount <= 21);
        return base.CheckCondition1();
    }    
    protected override IEnumerator CheckCondition2()
    {
        conditionStatus[2] = (RemainingArrows() >= 4);
        return base.CheckCondition2();
    }
    int RemainingArrows()
    {
        int arrowID = (int)MyTile.Type.Arrow;
        return Global.grid.tileCount[arrowID];
    }
}
