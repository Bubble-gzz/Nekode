using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public enum GameMode{
        Play,
        Workshop
    }
    static public GameMode currentGameMode;
    static public Vector3 MoveToMouse(Vector3 pos)
    {
        Vector3 res = mainCam.ScreenToWorldPoint(Input.mousePosition);
        res.z = pos.z;
        return res;
    }
}
