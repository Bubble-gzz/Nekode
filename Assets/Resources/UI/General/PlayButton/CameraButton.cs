using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraButton : MyButton
{
    // Start is called before the first frame update
    [SerializeField]
    Sprite[] textures = new Sprite[2];
    MyCamera.Mode mode;
    override protected void Start()
    {
        base.Start();
        onClick.AddListener(SwitchState);
        mode = MyCamera.Mode.WSAD;
        GetComponent<Image>().sprite = textures[(int)mode];
        if (Global.currentGameMode == Global.GameMode.Play)
        {
            if (GamePlay.puzzleSetting != null)
            if (!GamePlay.puzzleSetting.CameraSwitcher)
                gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
    }
    void SwitchState()
    {
        if (mode == MyCamera.Mode.Follow)
        {
            Global.mainCam.GetComponent<MyCamera>().mode = mode = MyCamera.Mode.WSAD;
        }
        else
        {
            Global.mainCam.GetComponent<MyCamera>().mode = mode = MyCamera.Mode.Follow;
        }
        GetComponent<Image>().sprite = textures[(int)mode];
    }
}
