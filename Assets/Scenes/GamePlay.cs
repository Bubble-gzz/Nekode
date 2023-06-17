using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GamePlay : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    List<PuzzlePreset> puzzlePresets = new List<PuzzlePreset>();
    [SerializeField]
    List<GameObject> puzzleLogics = new List<GameObject>();
    [SerializeField]
    TileInventory mainInventory, arithInventory, logicInventory;
    MyGrid grid;
    static public PuzzlePreset puzzleSetting;
    static public UnityEvent onNekoSubmit;
    void Awake()
    {
        Global.inWorkshop = false;
        Global.gameMode = Global.GameMode.Test;
        Global.mouseOverUI = false;
        Global.mouseOverArrow = false;        
        onNekoSubmit = new UnityEvent();
    }
    void Start()
    {
        AudioManager.PlayMusicByName("Gameplay_bgm_relaxing");
        for (int i = 0; i < puzzlePresets.Count; i++)
            if (Global.currentPuzzleName == puzzlePresets[i].puzzleName) {
                InitWithPreset(puzzlePresets[i]);
                Global.currentPuzzleID = i;
                break;
            }
    }

    // Update is called once per frame
    void InitWithPreset(PuzzlePreset preset)
    {
        puzzleSetting = preset;
        grid = GameObject.FindObjectOfType<MyGrid>();
        grid.SetTileCount(preset.tilePreset.tileCounts);
        grid.LoadFromFile("MapData/" + Global.currentPuzzleName);
        //grid.LoadFromFile("MapData/" + Global.currentPuzzleName);//grid.LoadFromFile(Application.dataPath + "/Resources/MapData/" + Global.currentPuzzleName + ".json");
        TilePreset tilePreset = preset.tilePreset;
        mainInventory.tileTypes = tilePreset.mainInventory;
        arithInventory.tileTypes = tilePreset.arithInventory;
        logicInventory.tileTypes = tilePreset.logicInventory;
        mainInventory.gameObject.SetActive(true);
        Global.mainCam.orthographicSize = preset.cameraSize;
        Instantiate(preset.puzzleLogic, transform); 
    }
    void Update()
    {
        
    }
    public void BackToPuzzleSelect()
    {
        SwitchScene("PuzzleSelect");
    }
    public void NextPuzzle()
    {
        string targetSceneName = "";
        if (Global.currentPuzzleID >= puzzlePresets.Count - 1)
            targetSceneName = "PuzzleSelect";
        else {
            Global.currentPuzzleName = puzzlePresets[Global.currentPuzzleID + 1].puzzleName;
            targetSceneName = "GamePlay";
        }
        SwitchScene(targetSceneName);
    }
    public void Retry()
    {
        SwitchScene("GamePlay");
    }
    void SwitchScene(string targetSceneName)
    {
        SceneSwitcher.SwitchTo(targetSceneName);
    }
}

[Serializable]
public class PuzzlePreset{
    public string puzzleName;
    public TilePreset tilePreset;
    public GameObject puzzleLogic;
    public float cameraSize;
}