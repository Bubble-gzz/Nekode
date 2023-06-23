using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDiaryPage : MonoBehaviour
{
    // Start is called before the first frame update
    public int puzzleID;
    public List<GameObject> contents = new List<GameObject>();
    MyPanel panel;
    void Awake()
    {
        panel = GetComponent<MyPanel>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool Show()
    {
        int status;
        if (puzzleID == -1) status = 0;
        else status = PuzzleManager.PuzzleInfo(puzzleID);
        Debug.Log("puzzleID: " + puzzleID + "  status: " + status);
        //Debug.Log("diary show status:" + status + " contents[status]:" + contents[status]);
        if (status == -1) return false;
        foreach(var content in contents)
            content.SetActive(false);
        contents[status].SetActive(true);
        panel.Appear();
        return true;
    }
    public void Hide()
    {
        panel.Disappear();
    }
}
