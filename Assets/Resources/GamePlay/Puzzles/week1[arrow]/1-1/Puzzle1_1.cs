using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1_1 : PuzzleLogic
{
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        totalTestCase = 1;
        StartCoroutine(GameProcess());
        conditions[0] = "Follow the instruction";
        conditions[1] = "Hit the play button";
        conditions[2] = "Complete the tutorial";
    }
    
    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
    }

    override protected IEnumerator GameProcess()
    {
        yield return base.GameProcess();
        DrBubble.instance.transform.position = new Vector3(33.15f, 31.5f, 0);
        yield return new WaitForSeconds(1f);
        dialogue.SetTailLR();
        dialogue.Open();
        
        if (Settings.language == "CH") dialogue.Play("首先让我们通过这个例子感受一下猫咪是如何完成任务的。");
        else dialogue.Play("Here's an example and let's see how the kitten gets the job done.", new Vector2(750, 150));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("这里，我们想让小猫将蓝色方块的数字搬运到绿色方块中。");
        else dialogue.Play("Here, we want the kitten to copy the number from the blue tile to the green tile.", new Vector2(850, 150));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("你肯定会问蓝色方块和绿色方块是什么意思 ^_^*");
        else dialogue.Play("You may be wondering,\n what does all this stuff mean? ^_^*", new Vector2(750, 150));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("看看这些卡片吧");
        else dialogue.Play("These cards should help you understand.", new Vector2(800, 120));
        while (dialogue.isPlaying) yield return null;
        
        dialogue.Close();

        yield return new WaitForSeconds(0.2f);
        helpCardPanel = GameUIManager.PopOutPanel(helpCardPanelPrefab).GetComponentInChildren<MyPanel>();
        yield return new WaitForSeconds(0.5f);
        while (helpCardPanel.showing) yield return null;
        
        dialogue.Open(new Vector2(250, 120));
        dialogue.Play("Let's go!", new Vector2(250, 120));
        while (dialogue.isPlaying) yield return null; 
        dialogue.Close(true);
        
        if (Settings.language == "CH") SetTarget("读取A端口中的数，输出到B端口");
        else SetTarget("Read the data from port A and output it to port B");
        GameUIManager.UnFoldUI();
        yield return null;
    }

    public override void GenerateTestCase()
    {
        base.GenerateTestCase();
        //Debug.Log("Generating Test Case ... ... ");
        //grid.tileTable["A"][0].UpdateValue(233);
        answerTable["B"] = 233;
    }
    protected override IEnumerator CheckCondition0()
    {
        conditionStatus[0] = true;
        return base.CheckCondition0();
    }
    protected override IEnumerator CheckCondition1()
    {
        conditionStatus[1] = true;
        return base.CheckCondition1();
    }    protected override IEnumerator CheckCondition2()
    {
        conditionStatus[2] = true;
        return base.CheckCondition2();
    }
}
