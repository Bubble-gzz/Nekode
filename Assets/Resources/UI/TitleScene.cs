using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Play()
    {
        SceneSwitcher.SwitchTo("SaveSelect");
    }

    public void Workshop()
    {
        SceneSwitcher.SwitchTo("Workshop");
    }
    public void Exit()
    {
        SceneSwitcher.SwitchTo("ExitGame");
    }
    public void Settings()
    {
        Debug.Log("jump to settings");
    }
}
