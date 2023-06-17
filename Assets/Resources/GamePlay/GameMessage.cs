using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameMessage : MonoBehaviour
{
    // Start is called before the first frame update
    static public UnityEvent OnArrowIsDeleted = new UnityEvent();
    static public UnityEvent OnToolReturnedToSlot = new UnityEvent();
    static public UnityEvent OnResetGridState = new UnityEvent(); // simply reset grid
    static public UnityEvent OnReset = new UnityEvent(); // return to the game state before testing
    static public UnityEvent OnPlay = new UnityEvent();
    static public UnityEvent OnPuzzleComplete = new UnityEvent();

    static public UnityEvent OnPassLogicTile = new UnityEvent();
    static public bool playingStory;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
