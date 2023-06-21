using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HelpCardGroup : MonoBehaviour
{
    // Start is called before the first frame update
    RectTransform rectTransform;
    public int pageCount = 0;
    public GameObject indexCirclePrefab;
    public float indexCircleInterval = 20;
    Transform indexGroup;
    List<IndexCircle> indexCircles;
    CanvasGroup canvasGroup;
    List<HelpCard> cards;
    MyButtonController turnLeftButton, turnRightButton;
    MyButtonController closeButton, gotIt;
    int curPage;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        cards = new List<HelpCard>();
        indexCircles = new List<IndexCircle>();
        canvasGroup = GetComponent<CanvasGroup>();
        indexGroup = transform.Find("Indexes");
        for (int i = 0; i < pageCount; i++)
        {
            GameObject newIndexCircle = Instantiate(indexCirclePrefab, indexGroup);
            indexCircles.Add(newIndexCircle.GetComponent<IndexCircle>());
        }
        foreach (Transform child in transform)
        {
            HelpCard card = child.GetComponent<HelpCard>();
            if (card != null) cards.Add(card);
        }
        turnLeftButton = transform.Find("TurnLeft").GetComponentInChildren<MyButtonController>();
        turnRightButton = transform.Find("TurnRight").GetComponentInChildren<MyButtonController>();
        closeButton = transform.Find("Close").GetComponentInChildren<MyButtonController>();
        gotIt = transform.Find("Got It!").GetComponentInChildren<MyButtonController>();
    }
    void Start()
    {
        curPage = 0;
        for (int i = 1; i < pageCount; i++)
        {
            cards[i].Disappear();
            indexCircles[i].SetState(false);
        }
        cards[0].Appear();
        indexCircles[0].SetState(true);
        float posX = - (pageCount - 1) * indexCircleInterval * 1.0f / 2;
        for (int i = 0; i < pageCount; i++)
        {
            indexCircles[i].GetComponent<RectTransform>().localPosition = new Vector3(posX, 0, 0);
            posX += indexCircleInterval;
        }
        turnRightButton.SetActive(pageCount > 1);
        turnLeftButton.SetActive(false);

        closeButton.SetActive(true);
        if (pageCount > 1) gotIt.SetActive(false);
        else gotIt.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool ChangePage(int delta)
    {
        if (curPage + delta < 0 || curPage + delta >= cards.Count) return false;
        cards[curPage].Disappear();
        indexCircles[curPage].SetState(false);
        curPage += delta;
        rectTransform.DOSizeDelta(cards[curPage].CardSize, 0.3f).SetEase(Ease.OutSine);
        
        cards[curPage].Appear();
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
            gotIt.SetActive(true);
        }
        else {
            turnRightButton.Clicked();
        }
    }
    public void LastPage()
    {
        if (!ChangePage(-1)) return;
        gotIt.SetActive(false);
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
    public void Appear()
    {
        canvasGroup.enabled = true;
    }
    public void Disappear()
    {
        canvasGroup.enabled = false;
    }
    public void Close()
    {
        GameUIManager.CloseHelpPanel();
    }
}
