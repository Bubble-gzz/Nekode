using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ResultPanel : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Transform> stars;
    Sequence starAnimSeq;
    static List<string>[] remarks = new List<string>[3]{
        new List<string>(){"Good", "Okay"},
        new List<string>(){"Great", "Nice"},
        new List<string>(){"Amazing", "Perfect", "Excellent"}
    };
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Ponder()
    {
        Debug.Log("Pondering");
    }
    public void Pass(int starID)
    {
        Debug.Log("Pass");
        float unit = 0.2f;
        starAnimSeq = DOTween.Sequence();
        starAnimSeq.Append(stars[starID].DOScale(new Vector3(1.2f, 1.2f, 1.2f), unit));
        starAnimSeq.Append(stars[starID].DOScale(new Vector3(0.9f, 0.9f, 0.9f), unit));
        starAnimSeq.Append(stars[starID].DOScale(new Vector3(1f, 1f, 1f), unit));
    }
    public void Fail(int starID)
    {
        Debug.Log("Fail");
    }
    public void Remark(int starCount)
    {
        string remark = remarks[starCount][Random.Range(0, remarks[starCount].Count)];
    }
}
