using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Workshop : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public string puzzleName;
    [SerializeField]
    TMP_Text title;
    void Awake()
    {
        Global.inWorkshop = true;
        Global.gameMode = Global.GameMode.Debug;
        Global.mouseOverUI = false;
        Global.mouseOverArrow = false;
        puzzleName = "";
    }
    void Start()
    {
        MyGrid grid = GameObject.FindObjectOfType<MyGrid>();
        bool presetExist = false;
        if (puzzleName != "") {
            presetExist |= grid.LoadFromFileWithReturnValue(Application.persistentDataPath+"/TempMapData/"+puzzleName+".json", false);
        }
        if (!presetExist) grid.Init();
        GameUIManager.UnFoldUI();
    }
    
    // Update is called once per frame
    void Update()
    {
        title.text = puzzleName;
    }
    public void UpdateName(string newName)
    {
        puzzleName = newName;
    }
}
