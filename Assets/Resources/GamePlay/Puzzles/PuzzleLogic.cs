using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleLogic : MonoBehaviour
{
    protected MyGrid grid;
    
    int curTestCase;
    protected int totalTestCase;

    protected Dictionary<string, int> answerTable;

    virtual protected void Awake()
    {
        answerTable = new Dictionary<string, int>();
        totalTestCase = 1;
        curTestCase = 0;
    }
    virtual protected void Start()
    {
        Global.puzzleComplete = false;
        Global.onTestStart.AddListener(OnTestStart);
        grid = Global.grid;
        GamePlay.onNekoSubmit.AddListener(CheckAnswers);
        StartCoroutine(PuzzleInit());
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        if (Global.puzzleComplete)
        {
            PuzzleComplete();
        }
    }
    protected void OnTestStart()
    {
        curTestCase = 1;
        GenerateTestCase();
        Global.isGeneratingTestData = false;
    }
    virtual public void GenerateTestCase()
    {
        
    }

    public void PuzzleComplete()
    {
        Debug.Log("puzzle complete");
    }
    public IEnumerator PuzzleInit()
    {
        yield return new WaitForEndOfFrame();
        
    }
    protected void SetTarget(string target)
    {
        Global.puzzleTarget.SetNewMessage(target);
    }
    public void CheckAnswers()
    {
        bool accpeted = true;
        foreach(string label in answerTable.Keys)
        {
            if (grid.tileTable[label][0].value != answerTable[label])
            {
                accpeted = false;
            }
            else
            {
                Debug.Log("[" + label + "] Value Correct.");
            }
        }
        if (accpeted)
        {
            if (curTestCase == totalTestCase)
            {
                PuzzleComplete();
                return;
            }
            NextTestCase();
        }
        else{
            TestCaseFail();
        }
    }
    void TestCaseFail()
    {

    }
    void NextTestCase()
    {
        Global.isGeneratingTestData = true;
        curTestCase++;
        GenerateTestCase();
        Global.isGeneratingTestData = false;
    }

}
