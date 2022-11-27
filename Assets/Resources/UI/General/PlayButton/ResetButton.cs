using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MyButton
{
    // Start is called before the first frame update
    override protected void Start()
    {
        if (Global.currentGameMode == Global.GameMode.Play)
        {
            if (!GamePlay.puzzleSetting.resetButton)
                gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    override protected void Update()
    {
        
    }
}
