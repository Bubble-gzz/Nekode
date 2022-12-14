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
        Global.currentGameMode = Global.GameMode.Workshop;
        Global.mouseOverUI = false;
        Global.mouseOverArrow = false;
        
        GamePlay.onNekoReset = new UnityEvent();
        GamePlay.onNekoRun = new UnityEvent();
        GamePlay.hasNekoStart = false;
    }
    void Start()
    {
        MyGrid grid = GameObject.FindObjectOfType<MyGrid>();
        grid.Init();
    }
    
    // Update is called once per frame
    void Update()
    {
        title.text = puzzleName;
    }
}
