using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global
{
    static public Camera mainCam;
    static public bool mouseOverUI;
    static public bool mouseOverArrow;
    static public float nekoPlaySpeed;
    static public Neko currentNeko;
    public enum GameMode{
        Play,
        Workshop
    }
    static public GameMode currentGameMode;
}
