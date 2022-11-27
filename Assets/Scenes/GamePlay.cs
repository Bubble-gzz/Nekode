using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Global.currentGameMode = Global.GameMode.Play;
    }
    void Start()
    {
        Global.mouseOverUI = false;
        Global.mouseOverArrow = false;
        MyGrid grid = GameObject.FindObjectOfType<MyGrid>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
