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
    }
    
    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
    }

    IEnumerator GameProcess()
    {
        if (Settings.language == "CH") SetTarget("读取A端口中的数，输出到B端口");
        else SetTarget("Read the data from port A and output it to port B");
        yield return null;
    }

    public override void GenerateTestCase()
    {
        base.GenerateTestCase();
        //Debug.Log("Generating Test Case ... ... ");
        grid.tileTable["A"][0].UpdateValue(100);
        answerTable["B"] = 100;
    }
}
