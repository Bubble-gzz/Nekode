using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

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
        for (int i = 0; i < puzzlePresets.Count; i++)
            if (Global.currentPuzzleName == puzzlePresets[i].puzzleName) {
                InitWithPreset(puzzlePresets[i]);
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
        Instantiate(preset.puzzleLogic); 
    }
    void Update()
    {
        
    }
}

[Serializable]
public class PuzzlePreset{
    public string puzzleName;
    public TilePreset tilePreset;
    public GameObject puzzleLogic;
    public float cameraSize;
}