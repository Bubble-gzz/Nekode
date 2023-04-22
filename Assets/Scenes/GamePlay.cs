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
        Global.currentGameMode = Global.GameMode.Play;
        onNekoSubmit = new UnityEvent();
        
        Global.mouseOverUI = false;
        Global.mouseOverArrow = false;
        for (int i = 0; i < puzzlePresets.Count; i++)
            if (Global.currentPuzzleName == puzzlePresets[i].puzzleName) {
                InitWithPreset(puzzlePresets[i]);
                break;
            }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void InitWithPreset(PuzzlePreset preset)
    {
        puzzleSetting = preset;
        grid = GameObject.FindObjectOfType<MyGrid>();
        grid.SetTileCount(preset.tilePreset.tileCounts);
        grid.LoadFromFile(Application.dataPath + "/Resources/MapData/" + Global.currentPuzzleName + ".json");
        TilePreset tilePreset = preset.tilePreset;
        mainInventory.tileTypes = tilePreset.mainInventory;
        arithInventory.tileTypes = tilePreset.arithInventory;
        logicInventory.tileTypes = tilePreset.logicInventory;
        if (preset.inventory) mainInventory.gameObject.SetActive(true);
        Global.mainCam.orthographicSize = preset.cameraSize;
        Instantiate(preset.puzzleLogic); 
    }
    void Update()
    {
        
    }
}

[Serializable]
public class PuzzlePreset{
    public bool resetButton, inventory, CameraSwitcher, bubble;
    public string puzzleName;
    public TilePreset tilePreset;
    public GameObject puzzleLogic;
    public float cameraSize;
}