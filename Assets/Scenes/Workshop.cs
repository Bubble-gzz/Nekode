using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
