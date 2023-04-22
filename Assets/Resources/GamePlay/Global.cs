using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Global
{
    static public Camera mainCam;
    static public bool mouseOverUI;
    static public bool mouseOverArrow;
    static public float nekoPlaySpeed;
    static public MyGrid grid;
    static public Neko currentNeko;
    static public string currentPuzzleName;
    static public bool puzzleComplete;
    static public PuzzleTarget puzzleTarget;
    static public bool isTyping;
    public enum GameMode{
        Play,
        Workshop
    }
    public enum GameState{
        Editing,
        Playing,
        Paused
    }
    static public GameState gameState;
    static public UnityEvent onGameStateChanged = new UnityEvent();
    static public void SetGameState(GameState newGameState)
    {
        gameState = newGameState;
        onGameStateChanged.Invoke();
    }
    static public GameMode currentGameMode;
    static public Vector3 MoveToMouse(Vector3 pos)
    {
        Vector3 res = mainCam.ScreenToWorldPoint(Input.mousePosition);
        res.z = pos.z;
        return res;
    }
}
