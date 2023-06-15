using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1_2 : PuzzleLogic
{
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        totalTestCase = 1;
        StartCoroutine(GameProcess());
        conditionStatus[0] = true;
        conditionStatus[1] = true;
        conditionStatus[2] = false;
        
        conditions[0] = "Navigate the kitten correctly";
        conditions[1] = "Use up all the arrows";
        conditions[2] = "Try to delete a placed arrow";

        GameMessage.OnArrowIsDeleted.AddListener(ArrowIsDeleted);
        //GameMessage.ToolReturnedToSlot.AddListener(ToolReturnedToSlot);
    }
    
    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
    }

    override protected IEnumerator GameProcess()
    {
        yield return base.GameProcess();

        dialogue.Open();
        
        if (Settings.language == "CH") dialogue.Play("第二堂课，我们来学习如何让小猫转向！");
        else dialogue.Play("Lesson 2, learn to navigate the kitten.", new Vector2(650, 150));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("这次，目标仍然是将输入端口的数字搬运到输出端口中。");
        else dialogue.Play("This time, we still want to copy the number from input port to output port.", new Vector2(650, 170));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("不过路径不再是直线了。");
        else dialogue.Play("Except that the path is no longer straight.", new Vector2(650, 150));
        while (dialogue.isPlaying) yield return null;

        if (Settings.language == "CH") dialogue.Play("关于转向的知识，看看这些卡片吧");
        else dialogue.Play("Feel free to check these cards for details.", new Vector2(650, 150));
        while (dialogue.isPlaying) yield return null;
        
        dialogue.Close(true);

        yield return new WaitForSeconds(0.2f);
        helpCardPanel = GameUIManager.PopOutPanel(helpCardPanelPrefab).GetComponentInChildren<MyPanel>();
        while (helpCardPanel.showing) yield return null;
        
        
        if (Settings.language == "CH") SetTarget("读取A端口中的数，输出到B端口");
        else SetTarget("Read the data from port A and output it to port B");
        GameUIManager.UnFoldUI();
        yield return null;
    }

    public override void GenerateTestCase()
    {
        base.GenerateTestCase();
        //Debug.Log("Generating Test Case ... ... ");
        grid.tileTable["A"][0].UpdateValue(100);
        answerTable["B"] = 100;
    }
    void ArrowIsDeleted()
    {
        conditionStatus[2] = true;
    }
}
