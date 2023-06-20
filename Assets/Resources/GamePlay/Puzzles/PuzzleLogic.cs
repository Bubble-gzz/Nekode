using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PuzzleLogic : MonoBehaviour
{
    protected MyGrid grid;
    static public int curTestCase;
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
    static public TestProgress testProgress;
    bool isTesting = false;
    Transform DrBubble;
    AudioSource sfx_accepted, sfx_error;
    virtual protected void Awake()
    {
        answerTable = new Dictionary<string, int>();
        totalTestCase = 1;
        curTestCase = 0;
        isTestResultClean = true;
        DrBubble = GameObject.Find("Dr.Bubble").transform;
        GameUIManager.HelpCardPrefab = helpCardPanelPrefab;
        sfx_accepted = GameObject.Find("sfx/accepted").GetComponent<AudioSource>();
        sfx_error = GameObject.Find("sfx/error").GetComponent<AudioSource>();
    }
    virtual protected void Start()
    {
        Global.stepCount = 0;
        Global.puzzleComplete = false;
        Global.onTestStart.AddListener(OnTestStart);
        grid = Global.grid;
        GamePlay.onNekoSubmit.AddListener(CheckAnswers);
        GameMessage.OnReset.AddListener(OnReset);
        GameMessage.OnResetGridState.AddListener(ClearResultOfTest);
        StartCoroutine(PuzzleInit());
        StartCoroutine(GameProcessWrapper());
    }

    // Update is called once per frame
    IEnumerator GameProcessWrapper()
    {
        GameMessage.playingStory = true;
        yield return GameProcess();
        GameMessage.playingStory = false;
    }
    virtual protected void Update()
    {
        if (Global.puzzleComplete)
        {
            PuzzleComplete();
        }
    }
    virtual protected void OnTestStart()
    {
        curTestCase = 1;
        testProgress?.GenerateBulbs(totalTestCase);
        testProgress?.Appear();
        GenerateTestCase();
        Global.isGeneratingTestData = false;
    }
    void OnReset()
    {
        testProgress?.Close();
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
    virtual public void CheckAnswers()
    {
        IEnumerator coroutine = C_CheckAnswers();
        ResetButton.coroutinesToBeKilledOnReset.Add(StartCoroutine(C_CheckAnswers()));
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
        if (accepted)
        {
            sfx_accepted.volume = AudioManager.sfxVolume;
            sfx_accepted.Play();
            testProgress?.SetCurrentResult(true);
            yield return new WaitForSeconds(1f);
            if (curTestCase == totalTestCase)
            {
                PuzzleComplete();
                yield break;
            }
            yield return NextTestCase();
        }
        else{
            sfx_error.volume = AudioManager.sfxVolume;
            sfx_error.Play();
            testProgress?.SetCurrentResult(false);
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
        ResetButton.Hint();
        PlayButton.StopPlaying();
    }
    IEnumerator NextTestCase()
    {
        //yield return new WaitForSeconds(0.7f);
        ClearResultOfTest();
        GameMessage.OnResetGridState.Invoke();
        testProgress?.NextCase();
        Global.isGeneratingTestData = true;
        //yield return new WaitForSeconds(0.5f);
        curTestCase++;
        GenerateTestCase();
        yield return new WaitForSeconds(0.7f);
        Global.isGeneratingTestData = false;
    }

    virtual protected IEnumerator GameProcess()
    {
        yield return new WaitForEndOfFrame();
        dialogue = GameUIManager.PopOutDialogueBox().GetComponentInChildren<MyDialogueBox>();
        dialogue.targetObject = DrBubble;
        //dialogue = GameUIManager.PopOutPanel(dialogueBoxPrefab).GetComponentInChildren<MyDialogueBox>();
    }
    public void PuzzleComplete()
    {
        Debug.Log("puzzle complete");
        StartCoroutine(C_PuzzleComplete());
    }
    IEnumerator C_PuzzleComplete()
    {
        GameMessage.OnPuzzleComplete.Invoke();
        testProgress?.Close();
        GameUIManager.FoldUI();
        resultPanel = GameUIManager.PopOutResultPanel().GetComponentInChildren<ResultPanel>();
        resultPanel.Appear();
        resultPanel.buttonPanel.Appear();

        /*update puzzle complete information*/
        yield return StartCoroutine(CheckCondition0());
        yield return StartCoroutine(CheckCondition1());
        yield return StartCoroutine(CheckCondition2());
        int starCount = 0;
        for (int i = 0; i < 3; i++)
            if (conditionStatus[i]) starCount++;
        PuzzleManager.UpdatePuzzleInfo(PuzzleManager.currentPuzzleID, starCount);
        /*update puzzle complete information*/

        yield return new WaitForSeconds(0.3f);
        resultPanel.SetCondition(0, conditions[0]);
        yield return new WaitForSeconds(0.3f);
        resultPanel.SetCondition(1, conditions[1]);
        yield return new WaitForSeconds(0.3f);
        resultPanel.SetCondition(2, conditions[2]);

        yield return new WaitForSeconds(0.5f);

        resultPanel.CheckCondition(0, conditionStatus[0]);
        yield return new WaitForSeconds(0.7f);

        resultPanel.CheckCondition(1, conditionStatus[1]);
        yield return new WaitForSeconds(0.7f);

        resultPanel.CheckCondition(2, conditionStatus[2]);
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
        //resultPanel.CheckCondition(0, conditionStatus[0]);
        yield return null;
    }
    protected virtual IEnumerator CheckCondition1()
    {

        //resultPanel.CheckCondition(1, conditionStatus[1]);
        yield return null;
    }
    protected virtual IEnumerator CheckCondition2()
    {
        //resultPanel.CheckCondition(2, conditionStatus[2]);
        yield return null;
    }
    protected int RemainingTiles(MyTile.Type type)
    {
        return Global.grid.tileCount[(int)type];
    }
}
