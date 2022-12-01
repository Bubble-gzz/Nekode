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
}
