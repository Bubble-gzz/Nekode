using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleLogic : MonoBehaviour
{
    protected MyGrid grid;
    
    int curTestCase;
    protected int totalTestCase;
    //public GameObject dialogueBoxPrefab;
    protected MyDialogueBox dialogue;
    public GameObject helpCardPanelPrefab;
    protected MyPanel helpCardPanel;
    protected Dictionary<string, int> answerTable;
    ResultPanel resultPanel;
    protected string[] conditions = new string[3];
    protected bool[] conditionStatus = new bool[3];
    bool isTestResultClean;
    virtual protected void Awake()
    {
        answerTable = new Dictionary<string, int>();
        totalTestCase = 1;
        curTestCase = 0;
        isTestResultClean = true;
    }
    virtual protected void Start()
    {
        Global.stepCount = 0;
        Global.puzzleComplete = false;
        Global.onTestStart.AddListener(OnTestStart);
        grid = Global.grid;
        GamePlay.onNekoSubmit.AddListener(CheckAnswers);
        GameMessage.OnResetGridState.AddListener(ClearResultOfTest);
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
        StartCoroutine(C_CheckAnswers());
    }
    public IEnumerator C_CheckAnswers()
    {
        bool accepted = true;
        foreach(string label in answerTable.Keys)
        {
            MyTile tile0 = grid.tileTable[label][0];

            if (tile0.value != answerTable[label])
            {
                accepted = false;
                foreach(MyTile tile in grid.tileTable[label])
                    StartCoroutine(tile.ResultWrong());
            }
            else
            {
                Debug.Log("[" + label + "] Value Correct.");
                foreach(MyTile tile in grid.tileTable[label])
                    StartCoroutine(tile.ResultCorrect());
            }
        }
        isTestResultClean = false;
        yield return new WaitForSeconds(1f);
        if (accepted)
        {
            if (curTestCase == totalTestCase)
            {
                PuzzleComplete();
                yield break;
            }
            NextTestCase();
        }
        else{
            TestCaseFail();
        }
    }
    void ClearResultOfTest()
    {
        if (isTestResultClean) return;
        foreach(string label in answerTable.Keys)
        {
            MyTile tile0 = grid.tileTable[label][0];
            foreach(MyTile tile in grid.tileTable[label])
                StartCoroutine(tile.ClearResultOfTest());
        }        
        isTestResultClean = true;
    }
    void TestCaseFail()
    {

    }
    void NextTestCase()
    {
        ClearResultOfTest();
        Global.isGeneratingTestData = true;
        curTestCase++;
        GenerateTestCase();
        Global.isGeneratingTestData = false;
    }

    virtual protected IEnumerator GameProcess()
    {
        yield return new WaitForEndOfFrame();
        dialogue = GameUIManager.PopOutDialogueBox().GetComponentInChildren<MyDialogueBox>();
        //dialogue = GameUIManager.PopOutPanel(dialogueBoxPrefab).GetComponentInChildren<MyDialogueBox>();
    }
    public void PuzzleComplete()
    {
        Debug.Log("puzzle complete");
        StartCoroutine(C_PuzzleComplete());
    }
    IEnumerator C_PuzzleComplete()
    {
        GameUIManager.FoldUI();
        resultPanel = GameUIManager.PopOutResultPanel().GetComponentInChildren<ResultPanel>();
        resultPanel.Appear();
        yield return new WaitForSeconds(0.3f);
        resultPanel.SetCondition(0, conditions[0]);
        yield return new WaitForSeconds(0.3f);
        resultPanel.SetCondition(1, conditions[1]);
        yield return new WaitForSeconds(0.3f);
        resultPanel.SetCondition(2, conditions[2]);

        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(CheckCondition0());
        yield return new WaitForSeconds(0.7f);
        yield return StartCoroutine(CheckCondition1());
        yield return new WaitForSeconds(0.7f);
        yield return StartCoroutine(CheckCondition2());
        yield return new WaitForSeconds(0.7f);
        
        resultPanel.PopStar(0, conditionStatus[0]);
        yield return new WaitForSeconds(0.3f);
        resultPanel.PopStar(1, conditionStatus[1]);
        yield return new WaitForSeconds(0.3f);
        resultPanel.PopStar(2, conditionStatus[2]);
        yield return new WaitForSeconds(0.3f);

        if (conditionStatus[0] && conditionStatus[1] && conditionStatus[2])
            resultPanel.PlayChord();
        //步数限制
        //空间限制
        //使用的方块限制
    }
    protected virtual IEnumerator CheckCondition0()
    {
        resultPanel.CheckCondition(0, conditionStatus[0]);
        yield return null;
    }
    protected virtual IEnumerator CheckCondition1()
    {

        resultPanel.CheckCondition(1, conditionStatus[1]);
        yield return null;
    }
    protected virtual IEnumerator CheckCondition2()
    {

        resultPanel.CheckCondition(2, conditionStatus[2]);
        yield return null;
    }
    protected int RemainingTiles(MyTile.Type type)
    {
        return Global.grid.tileCount[(int)type];
    }
}
