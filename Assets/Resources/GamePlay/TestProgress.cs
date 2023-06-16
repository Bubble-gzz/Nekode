using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestProgress : MonoBehaviour
{
    // Start is called before the first frame update
    MyPanel panel;
    public float panelWidth;
    Transform bulbsLayout;
    public GameObject bulbPrefab;
    List<TestResultBulb> bulbs = new List<TestResultBulb>();
    float spacing;
    int currentCaseID, caseN;
    void Awake()
    {
        PuzzleLogic.testProgress = this;
        panel = GetComponent<MyPanel>();
        bulbsLayout = transform.Find("Panel/Bulbs");
    }
    void Start()
    {
        
    }
    public void GenerateBulbs(int n)
    {
        caseN = n;
        foreach(Transform child in bulbsLayout)
            if (child.tag == "TestResultBulb") Destroy(child.gameObject);
        bulbs.Clear();
        if (n > 1) spacing = Mathf.Min(50, panelWidth / (n-1));
        else spacing = 0;
        float offsetX = - (n-1) * spacing / 2;
        for (int i = 0; i < n; i++)
        {
            TestResultBulb newBulb = Instantiate(bulbPrefab, bulbsLayout).GetComponent<TestResultBulb>();
            bulbs.Add(newBulb);
            newBulb.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetX, 0);
            offsetX += spacing;
        }
        bulbs[0].PopOut();
        currentCaseID = 0;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetCurrentResult(bool flag)
    {
        if (flag) bulbs[currentCaseID].SetPass();
        else bulbs[currentCaseID].SetFail();
    }
    public void NextCase()
    {
        bulbs[currentCaseID].PopBack();
        if (currentCaseID >= caseN - 1) return;
        currentCaseID++;
        bulbs[currentCaseID].PopOut();
    }
    public void Appear()
    {
        currentCaseID = 0;
        foreach (var bulb in bulbs) bulb.SetPending();
        panel.Appear();
    }
    public void Close()
    {
        foreach (var bulb in bulbs) bulb.SetPending();
        panel.Disappear();
    }
}
