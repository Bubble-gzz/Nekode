using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MyButton
{
    // Start is called before the first frame update
    [SerializeField]
    Sprite[] textures = new Sprite[2];
    enum State{
        Playing,
        Pause
    }
    State state;
    RectTransform rect;
    override protected void Start()
    {
        state = State.Pause;
        GetComponent<Image>().sprite = textures[(int)state];
        onClick.AddListener(SwitchState);
        GamePlay.onNekoReset.AddListener(Pause);
        rect = GetComponent<RectTransform>();
        if (Global.currentPuzzleName == "你好世界")
        {
            rect.anchoredPosition = new Vector2(Screen.width * 0.46f, Screen.height * 0.2f);
        }
    }

    void SwitchState()
    {
        if (state == State.Pause)
        {
            state = State.Playing;
            //Debug.Log("hasNekoStart: " + GamePlay.hasNekoStart);
            if (!GamePlay.hasNekoStart)
            {
                Global.grid.MapBackUp();
                GamePlay.onNekoRun.Invoke();
            }
            GamePlay.hasNekoStart = true;

            if (Global.currentNeko != null) Global.currentNeko.playMode = true;
        }
        else
        {
            state = State.Pause;
            if (Global.currentNeko != null) Global.currentNeko.playMode = false;
        }
        GetComponent<Image>().sprite = textures[(int)state];
    }
    // Update is called once per frame
    void Pause()
    {
        state = State.Pause;
        if (Global.currentNeko != null) Global.currentNeko.playMode = false;    
        GetComponent<Image>().sprite = textures[(int)state];
    }
}
