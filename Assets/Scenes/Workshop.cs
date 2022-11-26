using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workshop : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Global.currentGameMode = Global.GameMode.Workshop;
        Global.mouseOverUI = false;
        Global.mouseOverArrow = false;
        MyGrid grid = GameObject.FindObjectOfType<MyGrid>();
        grid.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
