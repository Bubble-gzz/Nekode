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
    static public int stepCount;
    static public Vector2 canvasReferenceResolution = new Vector2(1920, 1080);
    public enum GameMode{
        Test,
        Debug

    }
    static public bool inWorkshop;
    public enum GameState{
        Editing,
        Playing,
        Paused
    }
    static public GameState gameState;
    static public UnityEvent onGameStateChanged = new UnityEvent();
    static public UnityEvent onTestStart = new UnityEvent();
    static public bool isGeneratingTestData;
    static public void SetGameState(GameState newGameState)
    {
        if (gameMode == GameMode.Test)
        {
            if (gameState == GameState.Editing && newGameState == GameState.Playing)
            {
                isGeneratingTestData = true;
                onTestStart.Invoke();
            }
        }
        gameState = newGameState;
        onGameStateChanged.Invoke();
    }
    static public GameMode gameMode;
    static public Vector3 MoveToMouse(Vector3 pos)
    {
        Vector3 res = mainCam.ScreenToWorldPoint(Input.mousePosition);
        res.z = pos.z;
        return res;
    }
    static public Vector2 ScreenSpaceToCanvasSpace(Vector2 screenSpace)
    {
        float x = screenSpace.x / Screen.width * canvasReferenceResolution.x;
        float y = screenSpace.y / Screen.height * canvasReferenceResolution.y;
        return new Vector2(x, y);
    }
}
