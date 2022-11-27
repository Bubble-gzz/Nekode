using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleLogic : MonoBehaviour
{

    virtual protected void Start()
    {
        Global.puzzleComplete = false;
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
