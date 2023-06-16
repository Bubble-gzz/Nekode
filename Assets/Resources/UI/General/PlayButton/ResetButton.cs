using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MyButton
{
    // Start is called before the first frame update
    Image icon;
    protected override void Awake()
    {
        base.Awake();
        icon = GetComponent<Image>();
    }
    override protected void Start()
    {
        icon.enabled = false;
        icon.material.color = new Color(1,1,1,1);
        onClick.AddListener(OnClick);
        Global.onGameStateChanged.AddListener(OnGameStateChanged);
    }

    void OnGameStateChanged()
    {
        if (Global.gameState == Global.GameState.Editing)
        {
            icon.enabled = false;
        }
        else {
            icon.enabled = true;
        }
    }
    public void OnClick()
    {
        Global.SetGameState(Global.GameState.Editing);
        GameUIManager.UnFoldEditUI();
        Global.grid.MapRecover();
        GameMessage.OnResetGridState.Invoke();
    }
}
