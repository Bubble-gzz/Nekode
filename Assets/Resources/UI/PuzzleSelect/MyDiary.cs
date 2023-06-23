using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDiary : MonoBehaviour
{
    // Start is called before the first frame update
    public List<MyDiaryPage> pages = new List<MyDiaryPage>();
    int currentPageID = -1;
    float blankTimelapse;
    public float blankTimeLimit = 0.2f;
    void Awake()
    {
        blankTimelapse = 0;
    }
    void Start()
    {
        ShowPage(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPageID == -1) blankTimelapse += Time.deltaTime;
        if (blankTimelapse > blankTimeLimit)
        {
            blankTimelapse = 0;
            ShowPage(0);
        }
    }
    public void ShowPage(int pageID)
    {
        if (pageID < 0 || pageID >= pages.Count) return;
        if (pageID == currentPageID) return;
        if (!pages[pageID].Show()) return;
        HidePage(currentPageID);
        
        currentPageID = pageID;
        Debug.Log("pageID: " + pageID);
        
    }
    public void HidePage(int pageID)
    {
        if (pageID < 0 || pageID >= pages.Count) return;
        currentPageID = -1;
        pages[pageID].Hide();
    }
}
