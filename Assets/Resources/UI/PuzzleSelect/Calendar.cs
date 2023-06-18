using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calendar : MonoBehaviour
{
    // Start is called before the first frame update    // Start is called before the first frame update
    public int pageCount = 2;
    public GameObject indexCirclePrefab;
    public float indexCircleInterval = 20;
    Transform indexGroup;
    List<IndexCircle> indexCircles = new List<IndexCircle>();
    public List<MyPanel> pages = new List<MyPanel>();
    MyButtonController turnLeftButton, turnRightButton;
    int curPage;
    void Awake()
    {
        turnLeftButton = transform.Find("TurnLeft").GetComponentInChildren<MyButtonController>();
        turnRightButton = transform.Find("TurnRight").GetComponentInChildren<MyButtonController>();
    }
    void CreateIndex()
    {
        indexGroup = transform.Find("Index");
        for (int i = 0; i < pageCount; i++)
        {
            GameObject newIndexCircle = Instantiate(indexCirclePrefab, indexGroup);
            indexCircles.Add(newIndexCircle.GetComponent<IndexCircle>());
        }

    }
    void Start()
    {
        CreateIndex();
        curPage = PuzzleManager.calendarPage;
        for (int i = 0; i < pageCount; i++)
        {
            if (i == curPage) pages[i].Appear();
            else {
                pages[i].Disappear();
                indexCircles[i].SetState(false);
            }
        }
        indexCircles[0].SetState(true);
        float posX = - (pageCount - 1) * indexCircleInterval * 1.0f / 2;
        for (int i = 0; i < pageCount; i++)
        {
            indexCircles[i].GetComponent<RectTransform>().localPosition = new Vector3(posX, 0, 0);
            posX += indexCircleInterval;
        }
        
        turnLeftButton.SetActive(curPage > 0);
        turnRightButton.SetActive(curPage < pageCount - 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool ChangePage(int delta)
    {
        if (curPage + delta < 0 || curPage + delta >= pages.Count) return false;
        pages[curPage].Disappear();
        indexCircles[curPage].SetState(false);
        curPage += delta;
        PuzzleManager.calendarPage = curPage;
        
        pages[curPage].Appear();
        indexCircles[curPage].SetState(true);   
        return true;
    }
    public void NextPage()
    {
        if (!ChangePage(1)) return;
        if (curPage == 1) {
            turnLeftButton.SetActive(true);
        }
        if (curPage == pageCount - 1) {
            turnRightButton.SetActive(false);
        }
        else {
            turnRightButton.Clicked();
        }
    }
    public void LastPage()
    {
        if (!ChangePage(-1)) return;
        if (curPage == pageCount - 2) {
            turnRightButton.SetActive(true);
        }
        if (curPage == 0) {
            turnLeftButton.SetActive(false);
        }
        else {
            turnLeftButton.Clicked();
        }
    }
    public void SwitchToTitle()
    {
        SceneSwitcher.SwitchTo("Menu");
    }
}
