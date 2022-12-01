using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleLogic : MonoBehaviour
{
    MyGrid grid;
    [SerializeField]
    public List<PuzzleCondition> conditions = new List<PuzzleCondition>();
    virtual protected void Start()
    {
        Global.puzzleComplete = false;
        grid = Global.grid;
        GamePlay.onNekoReset.AddListener(PuzzleReset);
        GamePlay.onNekoRun.AddListener(PuzzleRun);
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
    virtual public void PuzzleComplete()
    {
        Debug.Log("puzzle complete");
    }
    virtual public void PuzzleRun()
    {

    }
    virtual public void PuzzleReset()
    {

    }
    virtual public IEnumerator PuzzleInit()
    {
        yield return new WaitForEndOfFrame();
        
    }
}
