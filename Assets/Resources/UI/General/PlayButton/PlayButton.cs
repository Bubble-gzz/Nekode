using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MyButton
{
    // Start is called before the first frame update
    [SerializeField]
    Sprite[] textures = new Sprite[2];
    enum Icon{
        Pause,
        Play
    }
    Image icon;
    override protected void Awake()
    {
        base.Awake();
        Global.gameState = Global.GameState.Editing;
        icon = GetComponent<Image>();
    }
    override protected void Start()
    {
        GetComponent<Image>().sprite = textures[(int)Icon.Play];
        onClick.AddListener(OnClick);
        Global.onGameStateChanged.AddListener(OnGameStateChanged);
        //rect = GetComponent<RectTransform>();
        // if (Global.currentPuzzleName == "你好世界")
        // {
        //     rect.anchoredPosition = new Vector2(Screen.width * 0.46f, Screen.height * 0.2f);
        // }
    }

    void OnGameStateChanged()
    {
        if (Global.gameState == Global.GameState.Playing)
        {
            icon.sprite = textures[(int)Icon.Pause];
        }
        else
        {
            icon.sprite = textures[(int)Icon.Play];
        }
    }

    void OnClick()
    {
        if (Global.gameState == Global.GameState.Editing)
        {
            Global.grid.MapBackUp();
        }
        if (Global.gameState == Global.GameState.Playing)
        {
            Global.SetGameState(Global.GameState.Paused);
        }
        else {
            Global.SetGameState(Global.GameState.Playing);
        }
    }
}
