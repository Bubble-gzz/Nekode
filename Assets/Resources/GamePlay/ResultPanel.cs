using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ResultPanel : MonoBehaviour
{
    // Start is called before the first frame update
    MyPanel myPanel;
    Sequence starAnimSeq;
    static List<string>[] remarks = new List<string>[3]{
        new List<string>(){"Good", "Okay"},
        new List<string>(){"Great", "Nice"},
        new List<string>(){"Amazing", "Perfect", "Excellent"}
    };
    List<MyCondition> conditions = new List<MyCondition>();
    List<RatingStar> stars = new List<RatingStar>();
    void Awake()
    {
        myPanel = GetComponent<MyPanel>();
        foreach(Transform child in transform.Find("Content/Conditions"))
        {
            conditions.Add(child.GetComponentInChildren<MyCondition>());
        }
        foreach(Transform child in transform.Find("Content/Stars"))
        {
            stars.Add(child.GetComponentInChildren<RatingStar>());
        }
        Debug.Log("conditions.count:" + conditions.Count);
        Debug.Log("stars.count:" + stars.Count);
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCondition(int id, string content)
    {
        MyCondition condition = conditions[id];
        condition.SetCondition(content);
        //condition.GetComponentInChildren<TMP_Text>();
    }
    public void CheckCondition(int id, bool accepted)
    {
        MyCondition condition = conditions[id];
        if (accepted)
        {
            condition.CheckBox();
        }
    }
    public void PopStar(int id, bool accepted)
    {
        Debug.Log("Pop Star: " + id);
        if (accepted) stars[id].Pass();
        else stars[id].Fail();
    }
    public void PlayChord()
    {
        Debug.Log("Play Chord");
    }
    public void Appear()
    {
        myPanel.Appear();
    }
    public void Remark(int starCount)
    {
        string remark = remarks[starCount][Random.Range(0, remarks[starCount].Count)];
    }
}
