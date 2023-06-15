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
    }
    void Start()
    {
        MyGrid grid = GameObject.FindObjectOfType<MyGrid>();
        if (puzzleName == "") grid.Init();
        else grid.LoadFromFile(Application.dataPath+"/TempMapData/"+puzzleName+".json", false);
    }
    
    // Update is called once per frame
    void Update()
    {
        title.text = puzzleName;
    }
}
