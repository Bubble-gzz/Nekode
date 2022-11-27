using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GamePlay : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    List<PuzzlePresets> puzzlePresets = new List<PuzzlePresets>();
    [SerializeField]
    List<GameObject> puzzleLogics = new List<GameObject>();
    [SerializeField]
    TileInventory mainInventory, arithInventory, logicInventory;
    MyGrid grid;
    void Awake()
    {
        Global.currentGameMode = Global.GameMode.Play;
    }
    void Start()
    {
        Global.mouseOverUI = false;
        Global.mouseOverArrow = false;
        for (int i = 0; i < puzzlePresets.Count; i++)
            if (Global.currentPuzzleName == puzzlePresets[i].puzzleName) {
                InitWithPreset(puzzlePresets[i]);
                break;
            }
    }

    // Update is called once per frame
    void InitWithPreset(PuzzlePresets preset)
    {
        grid = GameObject.FindObjectOfType<MyGrid>();
        grid.SetTileCount(preset.tilePreset.tileCounts);
        grid.LoadFromFile(Application.dataPath + "/MapData/" + Global.currentPuzzleName + ".json");
        TilePreset tilePreset = preset.tilePreset;
        mainInventory.tileTypes = tilePreset.mainInventory;
        arithInventory.tileTypes = tilePreset.arithInventory;
        logicInventory.tileTypes = tilePreset.logicInventory;
        mainInventory.gameObject.SetActive(true);
        Instantiate(preset.puzzleLogic); 
    }
    void Update()
    {
        
    }
}

[Serializable]
public class PuzzlePresets{
    public string puzzleName;
    public TilePreset tilePreset;
    public GameObject puzzleLogic;
}