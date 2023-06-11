using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public static MyPanel resultPanel;
    public MyPanel resultPanelInTheScene;
    static Transform canvasRoot;
    void Awake()
    {
        canvasRoot = transform;
        resultPanel = resultPanelInTheScene;
    }
    static public GameObject PopOutPanel(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, canvasRoot);
        MyPanel panel = obj.GetComponent<MyPanel>();
        //panel.Appear();
        return obj;
    }
    static public MyPanel PopOutResultPanel()
    {
        resultPanel.Appear();
        return resultPanel;
    }
}
