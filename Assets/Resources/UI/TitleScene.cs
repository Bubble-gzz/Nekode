using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleScene : MonoBehaviour
{
    // Start is called before the first frame update
    public MyGrid grid;
    Transform mainCam;
    void Awake()
    {
        Global.gameState = Global.GameState.Playing;
        Global.gameMode = Global.GameMode.Test;
    }
    void Start()
    {
        mainCam = Global.mainCam.transform;
        grid.LoadFromFile("MapData/title_0");
        AudioManager.PlayMusicByName("Main_Menu");
        grid.InviteNeko(48,32,2); //E

        grid.InviteNeko(48,34,3); //N1
        grid.InviteNeko(52,38,1); //N2
        grid.InviteNeko(48,43,2); //E
        grid.InviteNeko(50,45,1); //K

        grid.InviteNeko(52,50,0); //O
        grid.InviteNeko(52,52,0); //O
        grid.InviteNeko(51,53,1); //O
        grid.InviteNeko(49,53,1); //O
        grid.InviteNeko(48,52,2); //O
        grid.InviteNeko(48,50,3); //O
        grid.InviteNeko(50,50,3); //O
        
        grid.InviteNeko(52,57,2); //D
        grid.InviteNeko(52,63,2); //E

        grid.InviteNeko(48,65,3); //N1
        grid.InviteNeko(52,69,1); //N2

        grid.InviteNeko(52,74,2); //E
        Vector2 startPos = grid.GetWorldPos(50, 36);
        Vector2 endPos = grid.GetWorldPos(50, 67);
        mainCam.transform.position = new Vector3(startPos.x, startPos.y, -10);
        mainCam.DOMove(new Vector3(endPos.x, endPos.y, -10) ,50).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Play()
    {
        SceneSwitcher.SwitchTo("SaveSelect");
    }

    public void Workshop()
    {
        SceneSwitcher.SwitchTo("Workshop");
    }
    public void Exit()
    {
        SceneSwitcher.SwitchTo("ExitGame");
    }
    public void Settings()
    {
        Debug.Log("jump to settings");
    }
}
