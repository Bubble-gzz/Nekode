using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    static GameUIManager instance;
    public MyPanel resultPanelInTheScene;
    public GameObject dialogueBoxPrefab;
    public Transform instructorCat;
    void Awake()
    {
        instance = this;
        instructorCat = GameObject.Find("Instructor")?.transform;
    }
    static public GameObject PopOutPanel(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, instance.transform);
        //panel.Appear();
        return obj;
    }
    static public GameObject PopOutDialogueBox()
    {
        GameObject obj = Instantiate(instance.dialogueBoxPrefab, instance.transform);
        MyPanel panel = obj.GetComponent<MyPanel>();
        //panel.Appear();
        return obj;        
    }
    static public MyPanel PopOutResultPanel()
    {
        instance.resultPanelInTheScene.Appear();
        return instance.resultPanelInTheScene;
    }
    static public void FoldUI()
    {

    }
    static public void UnFoldUI()
    {

    }
    static public void SetPuzzleTarget(string target)
    {

    }
}
